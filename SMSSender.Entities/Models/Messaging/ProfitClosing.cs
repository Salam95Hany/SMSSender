using SMSSender.Entities.Models.Global;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMSSender.Entities.Models.Messaging
{
    [Table(name: "ProfitClosings", Schema = "sms")]
    public class ProfitClosing: ICustomerEntity
    {
        public int ProfitClosingId { get; set; }
        public Guid CustomerId { get; set; }
        public int BranchId { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public double TotalProfit { get; set; }
        public double NetProfit { get; set; }
        public double? CashBalanceBefore { get; set; }
        public double? CashBalanceAfter { get; set; }
        public string ClosedBy { get; set; }
        public DateTime? ClosedDate { get; set; }
    }
}
