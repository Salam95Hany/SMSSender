using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Entities.Contracts.DTOs
{
    public class UpdateTransactionMessage
    {
        public Guid TransactionId { get; set; }
        public double? Amount { get; set; }
        public string? FromPhone { get; set; }
        public string? SenderName { get; set; }
        public double? BalanceAfter { get; set; }
        public decimal? Commission { get; set; }
    }
}
