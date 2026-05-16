using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Entities.Models.Messaging
{
    [Table(name: "WalletDetails", Schema = "sms")]
    public class WalletDetail
    {
        public int WalletDetailId { get; set; }
        public string PhoneNumber { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public double Amount { get; set; }
        public double UsedMonthlyDeposit { get; set; }
        public double UsedMonthlyWithdrawal { get; set; }
        public double UsedDailyDeposit { get; set; }
        public double UsedDailyWithdrawal { get; set; }
        public DateTime LastRechargeDate { get; set; }
        public DateTime LastDailyResetDate { get; set; }
        public DateTime LastMonthlyResetDate { get; set; }
        public DateTime LastDailyDepositLimitNotificationDate { get; set; }
        public DateTime LastMonthlyDepositLimitNotificationDate { get; set; }
        public DateTime LastDailyWithdrawalLimitNotificationDate { get; set; }
        public DateTime LastMonthlyWithdrawalLimitNotificationDate { get; set; }
    }
}
