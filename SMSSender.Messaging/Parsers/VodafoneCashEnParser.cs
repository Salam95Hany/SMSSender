using SMSSender.Entities.Models.Messaging;
using SMSSender.Interfaces.Common;
using SMSSender.Messaging.Models;
using SMSSender.Messaging.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SMSSender.Messaging.Parsers
{
    public class VodafoneCashEnParser : IProviderMessageParser
    {
        private readonly IAppSettings _appSettings;

        public VodafoneCashEnParser(IAppSettings appSettings)
        {
            _appSettings = appSettings;
        }
        public ProviderType Provider => ProviderType.VodafoneCashEnglish;

        public MessageTransaction Parse(string message)
        {
            message = message.Normalize();
            var parsed = new MessageTransaction { Provider = Provider.ToString() };

            if (TryExtractField(message, "Amount", out var amountStr) && double.TryParse(amountStr, out var amount))
                parsed.Amount = amount;

            if (TryExtractField(message, "FromPhone", out var fromPhone))
                parsed.FromPhone = fromPhone;

            if (TryExtractField(message, "BalanceAfter", out var balanceStr) && double.TryParse(balanceStr, out var balance))
                parsed.BalanceAfter = balance;

            if (TryExtractField(message, "TransactionNumber", out var txnNumber))
                parsed.TransactionNumber = txnNumber;

            if (TryExtractField(message, "OperationDateTime", out var datetimeStr, "date") &&
                TryExtractField(message, "OperationDateTime", out var timeStr, "time"))
            {
                var dateTimeText = $"{datetimeStr} {timeStr}";
                string[] formats =
                {
                    "yy-MM-dd HH:mm",
                    "dd-MM-yy HH:mm",
                    "dd-MM-yyyy HH:mm",
                    "yy/MM/dd HH:mm",
                    "dd/MM/yy HH:mm",
                    "dd/MM/yyyy HH:mm",
                    "MMM dd, yyyy h:mm:ss tt",
                    "MMM d, yyyy h:mm:ss tt"
                };

                if (DateTime.TryParseExact(dateTimeText, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var parsedDateTime))
                    parsed.OperationMsgDateTime = parsedDateTime;
            }

            parsed.OperationServerDateTime = DateTime.Now;

            return parsed;
        }

        private bool TryExtractField(string message, string field, out string value, string group = "value")
        {
            value = null;
            var pattern = MessageParsingConfigService.GetFieldPattern(_appSettings, Provider.ToString(), field);
            if (string.IsNullOrWhiteSpace(pattern))
                throw new Exception($"Regex Field {field} Not Found In Settings File");

            var match = Regex.Match(message, pattern);
            if (!match.Success)
                return false;

            value = match.Groups[group].Value;
            return true;
        }
    }
}
