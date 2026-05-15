using ICU4N.Impl;
using SMSSender.Entities.Common;
using SMSSender.Entities.Models.Messaging;
using SMSSender.Interfaces;
using SMSSender.Interfaces.Repositories;
using SMSSender.Services.Common;

namespace SMSSender.Services
{
    public class NotificationService : INotificationService
    {
        private readonly IUnitOfWork _unitOfWork;
        public NotificationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public void CreateNotification(string Title, string Body, string? Provider = null, NotificationTypes NotificationType = NotificationTypes.System, NotificationReferenceTypes ReferenceType = NotificationReferenceTypes.System, Guid TransactionId = default(Guid))
        {
            var notification = new Notification
            {
                TransactionId = TransactionId,
                Title = Title,
                Body = Body,
                Provider = Provider,
                NotificationType = NotificationType,
                ReferenceType = ReferenceType,
                IsRead = false,
                CreatedAt = DateTime.UtcNow.EgyptNow()
            };

            _unitOfWork.Repository<Notification>().AddAsync(notification);
        }

        public async Task MarkAsRead(int NotificationId)
        {
            var notification = await _unitOfWork.Repository<Notification>().GetByIdAsync(NotificationId);

            if (notification is null || notification.IsRead)
                return;

            notification.IsRead = true;
            await _unitOfWork.CompleteAsync();
        }

        public async Task MarkAllAsRead()
        {
            string sql = @"UPDATE sms.Notifications SET IsRead = 1 WHERE IsRead = 0";

            await _unitOfWork.ExecuteSqlAsync(sql);
        }
    }
}
