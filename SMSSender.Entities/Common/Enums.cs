using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Entities.Common
{
    public enum OperationType
    {
        Deposit = 1,  // إيداع
        Withdraw = 2, // سحب
        CashWithdrawal = 3, // سحب نقدي
        Transfer = 4, // تحويل
        BalanceInquiry = 5 // استعلام رصيد
    }
}
