using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Entities.Models.Global
{
    public class CurrentCustomerService : ICurrentCustomerService
    {
        public Guid CustomerId { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsSystemJob { get; set; }
        public int BranchId { get; set; }
    }
}
