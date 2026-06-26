using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Entities.Contracts.DTOs
{
    public class MessageDetailsDto
    {
        public Guid TransactionId { get; set; }
        public string Provider { get; set; }
        public string ProviderName { get; set; }
        public string ProviderPhone { get; set; }
        public string Sim { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Message { get; set; }
    }
}
