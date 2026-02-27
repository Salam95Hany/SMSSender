using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Messaging.Models
{
    public class ParsedOperationMessage
    {
        public Guid TransactionId { get; set; }
        public string Provider { get; set; } // مزوّد الخدمة: Vodafone Cash / InstaPay
        public string ProviderName { get; set; } // اسم مزوّد الخدمة
        public string ProviderPhone { get; set; } // رقم مزوّد الخدمة
        public OperationType OperationType { get; set; } // نوع العملية: إيداع / سحب / كاش
        public double? Amount { get; set; } // مبلغ العملية
        public string FromPhone { get; set; } // رقم هاتف المرسل
        public string SenderName { get; set; } // اسم المرسل
        public double? BalanceAfter { get; set; } // الرصيد بعد تنفيذ العملية
        public string TransactionNumber { get; set; } // رقم العملية
        public DateTime OperationServerDateTime { get; set; } // تاريخ ووقت العملية في السيرفر
        public DateTime? OperationMsgDateTime { get; set; } // تاريخ ووقت العملية في الرسالة
    }
}
