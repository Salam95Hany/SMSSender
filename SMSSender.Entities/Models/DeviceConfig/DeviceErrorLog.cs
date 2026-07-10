using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Entities.Models.DeviceConfig
{
    [Table(name: "DeviceErrorLogs", Schema = "device")]
    public class DeviceErrorLog
    {
        public int DeviceErrorLogId { get; set; }
        public string? DeviceId { get; set; }
        public Guid CustomerId { get; set; }
        public int BranchId { get; set; }
        public string? Service { get; set; }
        public string? Method { get; set; }
        public string? Message { get; set; }
        public string? StackTrace { get; set; }
    }
}
