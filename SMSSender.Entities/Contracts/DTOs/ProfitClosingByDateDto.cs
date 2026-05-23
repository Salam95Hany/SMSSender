using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Entities.Contracts.DTOs
{
    public class ProfitClosingByDateDto
    {
        public double TotalProfit { get; set; }
        public double NetProfit { get; set; }
    }
}
