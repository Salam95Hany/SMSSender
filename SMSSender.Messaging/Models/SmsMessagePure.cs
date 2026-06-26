using Newtonsoft.Json;
using SMSSender.Messaging.Models;
using System.Text;

namespace SMSSender.Messaging
{
    public class SmsMessagePure
    {
        public int? MessageTransactionId { get; set; }
        public Guid? TransactionId { get; set; }
        public string SmsGateId { get; set; }
        public Guid CustomerId { get; set; }
        public int BranchId { get; set; }
        public string DeviceName { get; set; }
        public string PhoneNumber { get; set; }
        public string Message { get; set; }
        public ProviderType Provider { get; set; }
        public string ProviderStr { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Sim { get; set; }
    }
}
