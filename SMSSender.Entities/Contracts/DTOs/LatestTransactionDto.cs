using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Entities.Contracts.DTOs
{
    public class LatestTransactionDto
    {
        public Guid TransactionId { get; set; }
        public string Provider { get; set; }
        public string ProviderName { get; set; }
        public string ProviderPhone { get; set; }
        public string OperationTypeName { get; set; }
        public int OperationType { get; set; }
        public double? Amount { get; set; }
        public string FromPhone { get; set; }
        public string SenderName { get; set; }
        public double? BalanceAfter { get; set; }
        public string TransactionNumber { get; set; }
        public DateTime OperationServerDateTime { get; set; }
        public decimal? Commission { get; set; }
    }
}
