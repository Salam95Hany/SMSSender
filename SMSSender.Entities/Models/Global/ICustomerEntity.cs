using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Entities.Models.Global
{
    public interface ICustomerEntity
    {
        Guid CustomerId { get; set; }
        int BranchId { get; set; }
    }
}
