using SMSSender.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Entities.Models.DeviceConfig
{
    [Table(name: "DeviceRefreshInboxes", Schema = "device")]
    public class DeviceRefreshInbox
    {
        public int DeviceRefreshInboxId { get; set; }
        public string? DeviceId { get; set; }
        public Guid? CustomerId { get; set; }
        public int? BranchId { get; set; }
        public InboxRefreshStatus? RefreshStatus { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? LastUpdate { get; set; }
    }
}
