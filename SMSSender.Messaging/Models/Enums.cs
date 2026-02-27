using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Messaging.Models
{
    public enum ProviderType
    {
        VodafoneCash = 1,
        InstaPay = 2
    }
    public enum OperationType
    {
        Deposit = 1,  // إيداع
        Withdraw = 2, // سحب
        Cash = 3      // سيولة نقدية
    }

    public enum MsgStatus
    {
        Success = 200,
        Failure = 400,
    }
}
