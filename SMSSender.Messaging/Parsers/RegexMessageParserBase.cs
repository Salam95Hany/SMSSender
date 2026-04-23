using System.Globalization;
using SMSSender.Entities.Common;
using SMSSender.Interfaces.Common;
using SMSSender.Messaging;
using SMSSender.Messaging.Models;
using SMSSender.Messaging.Services;

namespace SMSSender.Messaging.Parsers
{
    public abstract class RegexMessageParserBase : IMessageParser
    {
        private const long MinUnixMilliseconds = -62135596800000;
        private const long MaxUnixMilliseconds = 253402300799999;
        private const long MinUnixSeconds = -62135596800;
        private const long MaxUnixSeconds = 253402300799;
        private static readonly OperationType[] DetectionOrder =
        {
            OperationType.Deposit,
            OperationType.Withdraw,
            OperationType.Transfer,
            OperationType.CashWithdrawal,
            OperationType.BalanceInquiry
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

            return new ParsedSmsMessage
            {
                Provider = Provider.ToString(),
                OperationType = DetectOperationType(normalizedMessage, settings),
                Amount = ParseDecimal(ExtractField(normalizedMessage, settings, nameof(FieldPatterns.Amount))),
                FromPhone = ExtractField(normalizedMessage, settings, nameof(FieldPatterns.FromPhone)),
                SenderName = ExtractField(normalizedMessage, settings, nameof(FieldPatterns.SenderName)),
                BalanceAfter = ParseDecimal(ExtractField(normalizedMessage, settings, nameof(FieldPatterns.BalanceAfter))),
                TransactionNumber = ExtractField(normalizedMessage, settings, nameof(FieldPatterns.TransactionNumber)),
                OperationDateTime = ParseStampDateTime(message.ReceivedStamp) ?? ExtractOperationDateTime(normalizedMessage, settings),
                SentDateTime = ParseStampDateTime(message.SentStamp)
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

        protected DateTime? ExtractOperationDateTime(string message, ProviderSettings settings)
        {
            var patterns = MessageParsingConfigService.GetFieldPatterns(settings, nameof(FieldPatterns.OperationDateTime));
            if (!_regexEngine.TryExtractGroups(message, patterns, out var groups, "value", "date", "time"))
            {
                return null;
            }

            var candidate = groups.TryGetValue("value", out var rawDateTime)
                ? rawDateTime
                : string.Join(" ", new[] { groups.GetValueOrDefault("date"), groups.GetValueOrDefault("time") }.Where(value => !string.IsNullOrWhiteSpace(value)));

            candidate = CleanStringValue(candidate);
            if (string.IsNullOrWhiteSpace(candidate))
            {
                return null;
            }

            var formats = settings.DateTimeFormats ?? Array.Empty<string>();
            foreach (var format in formats.Where(value => !string.IsNullOrWhiteSpace(value)))
            {
                if (DateTime.TryParseExact(candidate, format, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out var parsedDateTime))
                {
                    return parsedDateTime;
                }
            }

            return DateTime.TryParse(candidate, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out var fallbackDateTime)
                ? fallbackDateTime
                : null;
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

        protected DateTime? ParseStampDateTime(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            var normalizedValue = SmsTextNormalizer.Normalize(value);

            if (long.TryParse(normalizedValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out var numericStamp))
            {
                var digitsCount = normalizedValue.TrimStart('+', '-').Length;

                if (digitsCount >= 17 &&
                    numericStamp >= DateTime.MinValue.Ticks &&
                    numericStamp <= DateTime.MaxValue.Ticks)
                {
                    return new DateTime(numericStamp, DateTimeKind.Local);
                }

                if (digitsCount >= 11 &&
                    numericStamp >= MinUnixMilliseconds &&
                    numericStamp <= MaxUnixMilliseconds)
                {
                    return DateTimeOffset.FromUnixTimeMilliseconds(numericStamp).LocalDateTime;
                }

                if (digitsCount >= 10 &&
                    numericStamp >= MinUnixSeconds &&
                    numericStamp <= MaxUnixSeconds)
                {
                    return DateTimeOffset.FromUnixTimeSeconds(numericStamp).LocalDateTime;
                }

                if (numericStamp >= DateTime.MinValue.Ticks &&
                    numericStamp <= DateTime.MaxValue.Ticks)
                {
                    return new DateTime(numericStamp, DateTimeKind.Local);
                }
            }

            return DateTimeOffset.TryParse(normalizedValue, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out var parsedOffset)
                ? parsedOffset.LocalDateTime
                : DateTime.TryParse(normalizedValue, CultureInfo.InvariantCulture, DateTimeStyles.AllowWhiteSpaces, out var parsedDateTime)
                    ? parsedDateTime
                    : null;
        }
    }
}
