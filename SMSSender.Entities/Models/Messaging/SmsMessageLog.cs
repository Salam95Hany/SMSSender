using SMSSender.Entities.Models.Global;
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
    public class SmsMessageLog: ICustomerEntity
    {
        [Key]
        public int SmsMessageLogId { get; set; }
        public Guid TransactionId { get; set; }
        public string SmsGateId { get; set; }
        public Guid CustomerId { get; set; }
        public int BranchId { get; set; }
        public string Message { get; set; }
        public string ErrorMessage { get; set; }
        public string MsgStatus { get; set; }
        public string Provider { get; set; }
        public string ProviderName { get; set; }
        public string ProviderPhone { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime CreatedDate { get; set; }
        public string Sim { get; set; }

    }
}
