using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using SMSSender.Entities.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Entities.Models
{
    public partial class SMSDbContext : IdentityDbContext<AdminUser>
    {
        public SMSDbContext(DbContextOptions<SMSDbContext> options) : base(options)
        {
        }

        //public DbSet<Activity> Activities { get; set; }
    }
}
