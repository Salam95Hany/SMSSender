using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Entities.Contracts.DTOs.Worker
{
    public class DeviceHealthDetailsRequest
    {
        public string DeviceId { get; set; }
        public Guid CustomerId { get; set; }
        public int BranchId { get; set; }
        public string DeviceName { get; set; }
        public int MessagesFailed { get; set; }
        public int ConnectionStatus { get; set; }
        public int ConnectionTransport { get; set; }
        public int BatteryLevel { get; set; }
        public bool BatteryCharging { get; set; }
        public DateTime LastSeen { get; set; }
        public DateTime LastSyncDate { get; set; }
    }
}
