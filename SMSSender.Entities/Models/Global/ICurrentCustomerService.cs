using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Entities.Models.Global
{
    public interface ICurrentCustomerService
    {
        Guid CustomerId { get; set; }
        int BranchId { get; set; }
        bool IsAdmin { get; set; }
        public bool IsSystemJob { get; set; }
    }
}
