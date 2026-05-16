using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Interfaces.CronJop
{
    public interface IWalletReminderService
    {
        Task CheckRechargeReminders();
    }
}
