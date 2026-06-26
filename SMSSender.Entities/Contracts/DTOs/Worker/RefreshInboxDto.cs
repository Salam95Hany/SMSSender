using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Entities.Contracts.DTOs.Worker
{
    public class RefreshInboxDto
    {
        public string DeviceId { get; set; }
        public DateTime? From { get; set; }
        public DateTime? To { get; set; }
    }
}
