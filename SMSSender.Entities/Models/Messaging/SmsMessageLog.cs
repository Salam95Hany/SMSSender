using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SMSSender.Entities.Models.Messaging
{
    [Table(name: "SmsMessageLogs", Schema = "sms")]
    public class SmsMessageLog
    {
        [Key]
        public int SmsMessageLogId { get; set; }
        public Guid TransactionId { get; set; }
        public string Message { get; set; }
        public string ErrorMessage { get; set; }
        public string MsgStatus { get; set; }
        public string Provider { get; set; }
        public string ProviderName { get; set; }
        public string ProviderPhone { get; set; }
        public string SentStamp { get; set; }
        public string ReceivedStamp { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Sim { get; set; }

    }
}
