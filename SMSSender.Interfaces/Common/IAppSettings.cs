using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Interfaces.Common
{
    public interface IAppSettings
    {
        string[] URLList { get; set; }
        public string SecretKey { get; set; }
        ConnectionStrings ConnectionStrings { get; set; }
        JWT Jwt { get; set; }
        MessageParsingSettings MessageParsing { get; set; }
    }

    public class ConnectionStrings
    {
        public string DBConnection { get; set; }
    }

    public class JWT
    {
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpiryMinutes { get; set; }
    }

    public class MessageParsingSettings
    {
        public Dictionary<string, ProviderSettings> Providers { get; set; } = new();
    }

    public class ProviderSettings
    {
        public OperationKeywords OperationKeywords { get; set; } = new();
        public FieldPatterns FieldPatterns { get; set; } = new();
    }

    public class OperationKeywords
    {
        public string[] Deposit { get; set; } = Array.Empty<string>();
        public string[] Withdraw { get; set; } = Array.Empty<string>();
        public string[] Cash { get; set; } = Array.Empty<string>();
    }

    public class FieldPatterns
    {
        public string Amount { get; set; }
        public string FromPhone { get; set; }
        public string SenderName { get; set; }
        public string BalanceAfter { get; set; }
        public string TransactionNumber { get; set; }
        public string OperationDateTime { get; set; }
        //public string ToAccountNumber { get; set; } // اختياري لبعض Providers
    }
}
