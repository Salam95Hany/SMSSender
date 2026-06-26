using System.Globalization;
using SMSSender.Entities.Common;
using SMSSender.Interfaces.Common;
using SMSSender.Messaging.Models;
using SMSSender.Messaging.Services;

namespace SMSSender.Messaging.Parsers
{
    public abstract class RegexMessageParserBase : IMessageParser
    {
        private static readonly OperationType[] DetectionOrder =
        {
            OperationType.Deposit,
            OperationType.Withdraw,
            OperationType.Transfer,
            OperationType.CashWithdrawal,
            OperationType.BalanceInquiry,
            OperationType.ChargeWallet
        };

        private readonly IRegexEngine _regexEngine;
        private readonly IMessageProviderRegistry _providerRegistry;

        protected RegexMessageParserBase(IRegexEngine regexEngine, IMessageProviderRegistry providerRegistry)
        {
            _regexEngine = regexEngine;
            _providerRegistry = providerRegistry;
        }

        public abstract ProviderType Provider { get; }

        public ParsedSmsMessage Parse(SmsMessagePure message)
        {
            var normalizedMessage = SmsTextNormalizer.Normalize(message.Message);
            if (!_providerRegistry.TryGet(Provider, out var providerDefinition))
            {
                return new ParsedSmsMessage { Provider = Provider.ToString() };
            }

            var settings = providerDefinition.Settings;
            var amount = ParseDecimal(ExtractField(normalizedMessage, settings, nameof(FieldPatterns.Amount)));
            var operationType = DetectOperationType(normalizedMessage, settings);

            return new ParsedSmsMessage
            {
                Provider = Provider.ToString(),
                OperationType = DetectOperationType(normalizedMessage, settings),
                Amount = amount,
                Commission = amount.HasValue ? CalculateFee(amount.Value, operationType.Value, message.Provider) : null,
                FromPhone = ExtractField(normalizedMessage, settings, nameof(FieldPatterns.FromPhone)),
                SenderName = ExtractField(normalizedMessage, settings, nameof(FieldPatterns.SenderName)),
                BalanceAfter = ParseDecimal(ExtractField(normalizedMessage, settings, nameof(FieldPatterns.BalanceAfter))),
                TransactionNumber = ExtractField(normalizedMessage, settings, nameof(FieldPatterns.TransactionNumber)),
                CreatedAt = message.CreatedAt,
            };
        }

        protected virtual OperationType? DetectOperationType(string message, ProviderSettings settings)
        {
            var keywordMap = MessageParsingConfigService.GetOperationKeywords(settings);

            foreach (var operationType in DetectionOrder)
            {
                if (!keywordMap.TryGetValue(operationType, out var keywords))
                {
                    continue;
                }

                if (keywords.Any(keyword =>
                    !string.IsNullOrWhiteSpace(keyword) &&
                    message.Contains(keyword, StringComparison.OrdinalIgnoreCase)))
                {
                    return operationType;
                }
            }

            return null;
        }

        protected string? ExtractField(string message, ProviderSettings settings, string fieldName)
        {
            var patterns = MessageParsingConfigService.GetFieldPatterns(settings, fieldName);
            if (!_regexEngine.TryExtract(message, patterns, out var value))
            {
                return null;
            }

            return CleanStringValue(value);
        }

        protected decimal? ParseDecimal(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            var normalizedValue = value.Replace(",", string.Empty, StringComparison.Ordinal).Trim();
            return decimal.TryParse(normalizedValue, NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out var parsed)
                ? parsed
                : null;
        }

        protected string? CleanStringValue(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            return value.Trim().Trim('.', ',', ';', ':', '،', '؛');
        }

        public decimal CalculateFee(decimal amount, OperationType type, ProviderType Provider)
        {
            decimal fee = 0;

            if (Provider == ProviderType.VodafoneCash)
            {
                switch (type)
                {
                    case OperationType.Withdraw:

                        if (amount < 400)
                            return 5;

                        while (amount > 0)
                        {
                            if (amount <= 1000)
                            {
                                if (amount == 100)
                                    break;

                                if (amount >= 700)
                                    fee += 15;
                                else if (amount >= 400)
                                    fee += 10;
                                else if (amount > 100)
                                    fee += 5;

                                break;
                            }

                            fee += 15;
                            amount -= 1000;
                        }

                        break;

                    case OperationType.Deposit:

                        while (amount > 0)
                        {
                            decimal chunk = Math.Min(amount, 1000);

                            if (chunk >= 500)
                                fee += 10;
                            else
                                fee += 5;

                            amount -= chunk;
                        }

                        break;

                    case OperationType.CashWithdrawal:

                        decimal chunks = Math.Ceiling(amount / 1000m);
                        fee = chunks * 8.5m;

                        break;
                }
            }
            else
            {
                while (amount > 0)
                {
                    decimal chunk = Math.Min(amount, 1000);

                    if (chunk >= 500)
                        fee += 10;
                    else
                        fee += 5;

                    amount -= chunk;
                }
            }


            return fee;
        }
    }
}
