using SMSSender.Entities.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Entities.Models.DeviceConfig
{
    [Table(name: "DeviceChangeLogs", Schema = "device")]
    public class DeviceChangeLog
    {
        public int DeviceChangeLogId { get; set; }
        public string DeviceId { get; set; }
        public Guid CustomerId { get; set; }
        public int BranchId { get; set; }
        public int Version { get; set; }
        public ChangeLogAction Action { get; set; }
        public string? FieldKey { get; set; }
        public string? FieldValue { get; set; }
        public DateTime CreatedAt { get; set; }

        [NotMapped]
        public string DeviceName { get; set; }
        [NotMapped]
        public int LastVersion { get; set; }
    }
}
