using SMSSender.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Entities.Contracts.DTOs
{
    public class FinishMessageTransactionRequest
    {
        public int MessageTransactionId { get; set; }
        public OperationType OperationType { get; set; }
        public TransactionStatus TransactionStatus { get; set; }
        public double Amount { get; set; }
        public int Commission { get; set; }
    }
}
