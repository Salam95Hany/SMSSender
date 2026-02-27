using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Messaging.Models
{
    public class MessageStatusLog
    {
        public Guid TransactionId { get; set; }
        public string RawMessage { get; set; }
        public string ErrorMessage { get; set; }
        public string MsgStatus { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
