using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SMSSender.Entities.Auth;
using SMSSender.Entities.Models.Config;
using SMSSender.Entities.Models.Global;
using SMSSender.Entities.Models.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SMSSender.Entities.Models
{
    public partial class SMSDbContext : IdentityDbContext<AdminUser>
    {
        private readonly ICurrentCustomerService _currentCustomerService;
        public SMSDbContext(DbContextOptions<SMSDbContext> options, ICurrentCustomerService currentCustomerService) : base(options)
        {
            _currentCustomerService = currentCustomerService;
        }

        public DbSet<SmsMessageLog> SmsMessageLogs { get; set; }
        public DbSet<MessageTransaction> MessageTransactions { get; set; }
        public DbSet<CashBox> CashBoxes { get; set; }
        public DbSet<WalletDetail> WalletDetails { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<ProfitClosing> ProfitClosings { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Plan> Plans { get; set; }
        public DbSet<CustomerSubscription> CustomerSubscriptions { get; set; }
        public DbSet<Branch> Branches { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(ICustomerEntity).IsAssignableFrom(entityType.ClrType))
                {
                    var method = typeof(SMSDbContext).GetMethod(nameof(SetGlobalQueryFilter), BindingFlags.NonPublic | BindingFlags.Static)?.MakeGenericMethod(entityType.ClrType);
                    method?.Invoke(null, new object[] { modelBuilder, this });
                }
            }
        }


        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            if (_currentCustomerService.IsSystemJob)
                return await base.SaveChangesAsync(cancellationToken);

            var customerId = _currentCustomerService.CustomerId;
            var branchId = _currentCustomerService.BranchId;
            var isAdmin = _currentCustomerService.IsAdmin;

            foreach (var entry in ChangeTracker.Entries<ICustomerEntity>())
            {
                // CustomerId
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CustomerId = customerId;
                }

                if (entry.State == EntityState.Modified)
                {
                    if (entry.Entity.CustomerId != customerId)
                    {
                        throw new Exception("Cross-tenant update detected");
                    }
                }

                // BranchId
                if (entry.Entity is ICustomerEntity branchEntity)
                {
                    if (entry.State == EntityState.Added)
                        if (!isAdmin)
                            branchEntity.BranchId = branchId;

                    if (entry.State == EntityState.Modified)
                        if (!isAdmin && branchEntity.BranchId != branchId)
                            throw new Exception("Cross-branch update detected");
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }

        private static void SetGlobalQueryFilter<TEntity>(ModelBuilder modelBuilder, SMSDbContext context) where TEntity : class, ICustomerEntity
        {
            modelBuilder.Entity<TEntity>().HasQueryFilter(e =>
                context._currentCustomerService.IsSystemJob ||
                (
                    e.CustomerId == context._currentCustomerService.CustomerId &&
                    (context._currentCustomerService.IsAdmin || e.BranchId == context._currentCustomerService.BranchId)
                )
            );
        }
    }
}
