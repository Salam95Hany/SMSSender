using SMSSender.Entities.Common;
using SMSSender.Entities.Models.Global;
using SMSSender.Entities.Models.Messaging;
using SMSSender.Interfaces;
using SMSSender.Interfaces.CronJop;
using SMSSender.Interfaces.Hub;
using SMSSender.Interfaces.Repositories;
using SMSSender.Services.Common;

namespace SMSSender.CronJop
{
    public class WalletReminderService : IWalletReminderService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationService _notificationService;
        private readonly IHubNotificationService _hubNotificationService;
        private readonly ICurrentCustomerService _currentCustomerService;
        public WalletReminderService(IUnitOfWork unitOfWork, INotificationService notificationService, IHubNotificationService hubNotificationService, ICurrentCustomerService currentCustomerService)
        {
            _unitOfWork = unitOfWork;
            _notificationService = notificationService;
            _hubNotificationService = hubNotificationService;
            _currentCustomerService = currentCustomerService;
        }
        public async Task CheckRechargeReminders()
        {
            var now = DateTime.UtcNow.EgyptNow().Date;

            _currentCustomerService.IsSystemJob = true;

            var wallets = _unitOfWork.Repository<WalletDetail>().GetAllAsQueryable().AsEnumerable().Select(w =>
                {
                    var nextRecharge = w.LastRechargeDate.AddMonths(3).Date;
                    var daysLeft = (nextRecharge - now).Days;

                    return new
                    {
                        Wallet = w,
                        DaysLeft = daysLeft
                    };
                }).Where(x => x.DaysLeft is 30 or 15 or 10 or 5).ToList();

            if (!wallets.Any())
                return;

            var CustomerIds = wallets.Select(w => w.Wallet.CustomerId).Distinct().ToList();
            foreach (var item in wallets)
            {
                var message = item.DaysLeft switch
                {
                    30 => $"باقي 30 يوم على إعادة الشحن . المحفظة: {item.Wallet.PhoneNumber}",
                    15 => $"باقي 15 يوم على إعادة الشحن . المحفظة: {item.Wallet.PhoneNumber}",
                    10 => $"باقي 10 يوم على إعادة الشحن . المحفظة: {item.Wallet.PhoneNumber}",
                    5 => $"باقي 5 يوم على إعادة الشحن . المحفظة: {item.Wallet.PhoneNumber}",
                    _ => null
                };

                if (message != null)
                {
                    SendReminder(message, item.Wallet.CustomerId, item.Wallet.BranchId);
                }
            }

            await _unitOfWork.CompleteAsync();

            foreach (var custId in CustomerIds)
                await _hubNotificationService.SendMessageAddedAsync(7, custId);

        }

        public async Task ResetWalletDate()
        {
            var now = DateTime.UtcNow.EgyptNow().Date;
            _currentCustomerService.IsSystemJob = true;
            var wallets = _unitOfWork.Repository<WalletDetail>().GetAllAsQueryable();

            foreach (var entity in wallets)
            {
                if (entity.LastDailyResetDate.Date < now.Date)
                {
                    entity.UsedDailyDeposit = 0;
                    entity.UsedDailyWithdrawal = 0;
                    entity.LastDailyResetDate = now;
                }

                if (entity.LastMonthlyResetDate.Month != now.Month || entity.LastMonthlyResetDate.Year != now.Year)
                {
                    entity.UsedMonthlyDeposit = 0;
                    entity.UsedMonthlyWithdrawal = 0;
                    entity.LastMonthlyResetDate = now;
                }

            }

            await _unitOfWork.CompleteAsync();
        }

        private void SendReminder(string message, Guid CustomerId, int BranchId)
        {
            _notificationService.CreateSystemNotification("تنبيه إعادة شحن", message, CustomerId, BranchId);
        }
    }
}
