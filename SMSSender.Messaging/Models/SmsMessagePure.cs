using SMSSender.Messaging.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SMSSender.Messaging
{
    public class SmsMessagePure
    {
        public int? MessageTransactionId { get; set; }
        public Guid? TransactionId { get; set; }
        public string DeviceName { get; set; }
        public string PhoneNumber { get; set; }
        public string Message { get; set; }
        public ProviderType Provider { get; set; }
        public string ProviderStr { get; set; }
        public string SentStamp { get; set; }
        public string ReceivedStamp { get; set; }
        public string Sim { get; set; }
    }
}
