using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Entities.Contracts.DTOs
{
    public class ProfitClosingsDto
    {
        public DateTime From { get; set; }
        public DateTime To { get; set; }
    }
}
