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
        public FailedSmsSettings FailedSms { get; set; } = new();
        public Dictionary<string, ProviderSettings> Providers { get; set; } = new();
    }

    public class FailedSmsSettings
    {
        public string RelativeDirectory { get; set; } = "sms-failures";
        public string DateFolderFormat { get; set; } = "yyyy-MM-dd";
    }

    public class ProviderSettings
    {
        public string DisplayName { get; set; } = string.Empty;
        public int Priority { get; set; }
        public string[] SenderAliases { get; set; } = Array.Empty<string>();
        public string[] DetectionPatterns { get; set; } = Array.Empty<string>();
        public string[] DateTimeFormats { get; set; } = Array.Empty<string>();
        public OperationKeywords OperationKeywords { get; set; } = new();
        public FieldPatterns FieldPatterns { get; set; } = new();
    }

    public class OperationKeywords
    {
        public string[] Deposit { get; set; } = Array.Empty<string>();
        public string[] Withdraw { get; set; } = Array.Empty<string>();
        public string[] Transfer { get; set; } = Array.Empty<string>();
        public string[] CashWithdrawal { get; set; } = Array.Empty<string>();
        public string[] BalanceInquiry { get; set; } = Array.Empty<string>();

        public IEnumerable<string> All()
        {
            return Deposit
                .Concat(Withdraw)
                .Concat(Transfer)
                .Concat(CashWithdrawal)
                .Concat(BalanceInquiry)
                .Where(value => !string.IsNullOrWhiteSpace(value));
        }
    }

    public class FieldPatterns
    {
        public string[] Amount { get; set; } = Array.Empty<string>();
        public string[] FromPhone { get; set; } = Array.Empty<string>();
        public string[] SenderName { get; set; } = Array.Empty<string>();
        public string[] BalanceAfter { get; set; } = Array.Empty<string>();
        public string[] TransactionNumber { get; set; } = Array.Empty<string>();
        public string[] OperationDateTime { get; set; } = Array.Empty<string>();

        public IReadOnlyList<string> GetPatterns(string fieldName)
        {
            return fieldName switch
            {
                nameof(Amount) => Amount,
                nameof(FromPhone) => FromPhone,
                nameof(SenderName) => SenderName,
                nameof(BalanceAfter) => BalanceAfter,
                nameof(TransactionNumber) => TransactionNumber,
                nameof(OperationDateTime) => OperationDateTime,
                _ => Array.Empty<string>()
            };
        }
    }
}
