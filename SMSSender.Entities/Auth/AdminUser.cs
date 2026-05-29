using Microsoft.AspNetCore.Identity;
using SMSSender.Entities.Models.Global;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Entities.Auth
{
    public class AdminUser: IdentityUser
    {
        public Guid CustomerId { get; set; }
        public int BranchId { get; set; }
        public string Address { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LoginDate { get; set; }
    }
}
