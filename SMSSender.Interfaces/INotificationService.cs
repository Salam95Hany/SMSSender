using SMSSender.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Interfaces
{
    public interface INotificationService
    {
        void CreateSystemNotification(string Title, string Body, Guid CustomerId, int BranchId);
        void CreateNotification(string Title, string Body, string? Provider = null, NotificationTypes NotificationType = NotificationTypes.System, NotificationReferenceTypes ReferenceType = NotificationReferenceTypes.System, Guid TransactionId = default(Guid));
        Task MarkAsRead(int NotificationId);
        Task MarkAllAsRead();
    }
}
