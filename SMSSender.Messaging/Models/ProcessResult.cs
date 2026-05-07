using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Messaging.Models
{
    public class ProcessResult
    {
        public bool Success { get; set; }
        public Guid? TransactionId { get; set; }
    }
}
