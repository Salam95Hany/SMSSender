using SMSSender.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Entities.Models.Messaging
{
    [Table(name: "Notifications", Schema = "sms")]
    public class Notification
    {
        public int NotificationId { get; set; }
        public Guid TransactionId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string? Provider { get; set; }
        public NotificationTypes NotificationType { get; set; }
        public NotificationReferenceTypes ReferenceType { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
