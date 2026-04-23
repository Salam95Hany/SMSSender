using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SMSSender.Entities.Models.Messaging
{
    [Table(name: "FailedSmsLogs", Schema = "sms")]
    public class FailedSmsLog
    {
        [Key]
        public int Id { get; set; }
        public string RawMessage { get; set; } = string.Empty;
        public string Provider { get; set; } = string.Empty;
        public string ErrorReason { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
