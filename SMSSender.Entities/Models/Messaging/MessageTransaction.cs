using SMSSender.Entities.Common;
using SMSSender.Entities.Models.Config;
using SMSSender.Entities.Models.Global;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Entities.Models.Messaging
{
    [Table(name: "MessageTransactions", Schema = "sms")]
    public class MessageTransaction: ICustomerEntity
    {
        [Key]
        public int MessageTransactionId { get; set; }
        public Guid TransactionId { get; set; }
        public string SmsGateId { get; set; }
        public Guid CustomerId { get; set; }
        public int BranchId { get; set; }
        [ForeignKey(nameof(BranchId))]
        public Branch Branch { get; set; }
        public string Provider { get; set; } // مزوّد الخدمة: Vodafone Cash / InstaPay
        public string ProviderName { get; set; } // اسم مزوّد الخدمة
        public string ProviderPhone { get; set; } // رقم مزوّد الخدمة
        public OperationType OperationType { get; set; } // نوع العملية: إيداع / سحب / كاش
        public double? Amount { get; set; } // مبلغ العملية
        public decimal? Commission { get; set; }
        public string? FromPhone { get; set; } // رقم هاتف المرسل
        public string? SenderName { get; set; } // اسم المرسل
        public double? BalanceAfter { get; set; } // الرصيد بعد تنفيذ العملية
        public string? TransactionNumber { get; set; } // رقم العملية
        public TransactionStatus TransactionStatus { get; set; } // حالة العملية : مؤجل \ مكتمل
        public DateTime OperationServerDateTime { get; set; } // تاريخ ووقت العملية في السيرفر
        public DateTime CreatedAt { get; set; } // تاريخ ووقت الاستلام
        public bool? IsCalculated { get; set; }
    }
}
