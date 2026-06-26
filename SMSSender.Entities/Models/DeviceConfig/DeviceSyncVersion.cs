using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Entities.Models.DeviceConfig
{
    [Table(name: "DeviceSyncVersions", Schema = "device")]
    public class DeviceSyncVersion
    {
        public int DeviceSyncVersionId { get; set; }
        public Guid CustomerId { get; set; }
        public int BranchId { get; set; }
        public int CurrentVersion { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
