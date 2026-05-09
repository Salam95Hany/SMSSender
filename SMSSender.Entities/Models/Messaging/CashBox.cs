using SMSSender.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Entities.Models.Messaging
{
    [Table(name: "CashBoxes", Schema = "sms")]
    public class CashBox
    {
        public int CashBoxId { get; set; }
        public int? MessageTransactionId { get; set; }
        public CashBoxTransactionType TransactionType { get; set; }
        public string? Reason { get; set; }
        public string? CashBoxNumber { get; set; }
        public double? TransactionAmount { get; set; }
        public double BalanceBefore { get; set; }
        public double BalanceAfter { get; set; }
        public bool IsDeleted { get; set; }
        public string InsertUser { get; set; }
        public DateTime InsertDate { get; set; }
        public string? UpdateUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        [NotMapped]
        public string? ProviderPhone { get; set; }
        [NotMapped]
        public double? Commission { get; set; }
        [NotMapped]
        public bool? IsIncludeCommission { get; set; }
    }
}
