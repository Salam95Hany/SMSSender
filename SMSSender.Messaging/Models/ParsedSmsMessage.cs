using SMSSender.Entities.Common;

namespace SMSSender.Messaging.Models
{
    public class ParsedSmsMessage
    {
        public string Provider { get; set; } = string.Empty;
        public OperationType? OperationType { get; set; }
        public decimal? Amount { get; set; }
        public string? FromPhone { get; set; }
        public string? SenderName { get; set; }
        public decimal? BalanceAfter { get; set; }
        public string? TransactionNumber { get; set; }
        public DateTime? OperationDateTime { get; set; }
        public DateTime? SentDateTime { get; set; }
    }
}
