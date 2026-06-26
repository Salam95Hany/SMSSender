using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Entities.Models.DeviceConfig
{
    [Table(name: "Devices", Schema = "device")]
    public class Device
    {
        public int DeviceId { get; set; }
        public string DeviceUniqueId { get; set; }
        public Guid? CustomerId { get; set; }
        public int? BranchId { get; set; }
        public string DeviceName { get; set; }
        public string BaseUrl { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string? Sim1Number { get; set; }
        public string? Sim2Number { get; set; }
        public string? Sim1Name { get; set; }
        public string? Sim2Name { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        [NotMapped]
        public string? BranchName { get; set; }
    }
}
