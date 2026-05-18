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
        Transfer = 4, // دخلت في المحفظة ولم تخصم من الصندوق
        BalanceInquiry = 5, // استعلام رصيد
        ChargeWallet = 6
    }

    public enum TransactionStatus
    {
        Completed = 1,
        Delayed = 2
    }

    public enum CashBoxTransactionType
    {
        Deposit = 1,
        Withdraw = 2,
        CashWithdrawal = 3
    }

    public enum NotificationTypes
    {
        Deposit = 1,  // إيداع
        Withdraw = 2, // سحب
        CashWithdrawal = 3, // سحب نقدي
        Transfer = 4, // دخلت في المحفظة ولم تخصم من الصندوق
        BalanceInquiry = 5, // استعلام رصيد
        ChargeWallet = 6, // شحن رصيد المحفظة
        System = 7
    }

    public enum NotificationReferenceTypes
    {
        MessageTransaction = 1,
        WalletDetail = 2,
        System = 3
    }
}
