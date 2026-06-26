using SMSSender.Entities.Models.DeviceConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Entities.Contracts.DTOs.Worker
{
    public class DeviceWorkrtDto
    {
        public int LatestVersion { get; set; }
        public List<Device> Devices { get; set; }
        public List<DeviceChangeLog> Changes { get; set; }
    }
}
