using Microsoft.Extensions.Configuration;
using SMSSender.Interfaces.Common;
using SMSSender.Messaging.Models;
using SMSSender.Messaging.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SMSSender.Messaging.Parsers
{
    public class InstaPayParser : IProviderMessageParser
    {
        private readonly IAppSettings _appSettings;

        public InstaPayParser(IAppSettings appSettings)
        {
            _appSettings = appSettings;
        }
        public ProviderType Provider => ProviderType.VodafoneCash;

        public ParsedOperationMessage Parse(string message)
        {
            var parsed = new ParsedOperationMessage { Provider = Provider.ToString() };

            parsed.Amount = decimal.Parse(ExtractField(message, "Amount"));
            parsed.FromPhone = ExtractField(message, "FromPhone");
            parsed.SenderName = ExtractField(message, "SenderName");
            parsed.BalanceAfter = decimal.Parse(ExtractField(message, "BalanceAfter"));
            parsed.TransactionNumber = ExtractField(message, "TransactionNumber");

            var date = ExtractField(message, "OperationDateTime", "date");
            var time = ExtractField(message, "OperationDateTime", "time");
            parsed.OperationMsgDateTime = DateTime.ParseExact($"{date} {time}", "dd-MM-yy HH:mm", null);
            parsed.OperationServerDateTime = DateTime.Now.AddHours(2);

            return parsed;
        }

        private string ExtractField(string message, string field, string group = "value")
        {
            var pattern = MessageParsingConfigService.GetFieldPattern(_appSettings, Provider.ToString(), field);
            var match = Regex.Match(message, pattern);
            if (!match.Success) throw new Exception($"Field {field} not found");
            return match.Groups[group].Value;
        }
    }
}
