using SMSSender.Entities.Common;
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
        public WalletReminderService(IUnitOfWork unitOfWork, INotificationService notificationService, IHubNotificationService hubNotificationService)
        {
            _unitOfWork = unitOfWork;
            _notificationService = notificationService;
            _hubNotificationService = hubNotificationService;
        }
        public async Task CheckRechargeReminders()
        {
            var now = DateTime.UtcNow.EgyptNow();
            bool hasNotifications = false;
            var wallets = await _unitOfWork.Repository<WalletDetail>().GetAllAsync();

            foreach (var entity in wallets)
            {
                var nextRecharge = entity.LastRechargeDate.AddMonths(3);
                var daysLeft = (nextRecharge - now.Date).Days;

                if (daysLeft == 30)
                {
                    SendReminder(entity, $"باقي 30 يوم على إعادة الشحن . المحفظة: {entity.PhoneNumber}");
                    hasNotifications = true;
                }  

                if (daysLeft == 15)
                {
                    SendReminder(entity, $"باقي 15 يوم على إعادة الشحن . المحفظة: {entity.PhoneNumber}");
                    hasNotifications = true;
                }
                    
                if (daysLeft == 10)
                {
                    SendReminder(entity, $"باقي 10 يوم على إعادة الشحن . المحفظة: {entity.PhoneNumber}");
                    hasNotifications = true;
                }
                    
                if (daysLeft == 5)
                {
                    SendReminder(entity, $"باقي 5 يوم على إعادة الشحن . المحفظة: {entity.PhoneNumber}");
                    hasNotifications = true;
                }
                    
            }

            await _unitOfWork.CompleteAsync();

            if (hasNotifications)
                await _hubNotificationService.SendMessageAddedAsync();
        }

        private void SendReminder(WalletDetail entity, string message)
        {
            _notificationService.CreateNotification("تنبيه إعادة شحن", message, null, NotificationTypes.System, NotificationReferenceTypes.WalletDetail);
        }
    }
}
