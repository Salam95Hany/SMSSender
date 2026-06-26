using SMSSender.Entities.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Entities.Contracts.DTOs
{
    public class DeviceRefreshInboxDto
    {
        public int? DeviceRefreshInboxId { get; set; }
        public string DeviceId { get; set; }
        public string DeviceName { get; set; }
        public string BranchName { get; set; }
        public int BranchId { get; set; }
        public bool IsActive { get; set; }
        public InboxRefreshStatus? RefreshStatus { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
        public DateTime? LastUpdate { get; set; }
    }
}
