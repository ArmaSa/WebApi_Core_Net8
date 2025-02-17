using InvoiceAppWebApi.Domain;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using InvoiceAppWebApi.Data.Mappings;
using InvoiceAppWebApi.Domain.BaseEntity;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Security.AccessControl;
using System.Text.Json;

namespace InvoiceAppWebApi.Data
{
    public class InvoiceDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<InvoiceItem> InvoiceItems { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        public InvoiceDbContext(DbContextOptions<InvoiceDbContext> options, IHttpContextAccessor httpContextAccessor, IServiceProvider serviceProvider) : base(options)
        {
            LoadData();
            _serviceProvider = serviceProvider;
            _httpContextAccessor = httpContextAccessor;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityUserLogin<string>>()
                .HasKey(login => new { login.LoginProvider, login.ProviderKey });

            modelBuilder.Entity<IdentityUserClaim<string>>()
                .HasKey(claim => claim.Id);

            modelBuilder.Entity<IdentityUserRole<string>>()
                .HasKey(userRole => new { userRole.UserId, userRole.RoleId });

            modelBuilder.Entity<IdentityRoleClaim<string>>()
                .HasKey(roleClaim => roleClaim.Id);

            modelBuilder.Entity<IdentityUserToken<string>>()
                .HasKey(userToken => new { userToken.UserId, userToken.LoginProvider, userToken.Name });

            new CustomerMapping(modelBuilder.Entity<Customer>());
            new PaymentMapping(modelBuilder.Entity<Payment>());
            new InvoiceMapping(modelBuilder.Entity<Invoice>());
            new InvoiceItemMapping(modelBuilder.Entity<InvoiceItem>());
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            //add Entry Tracking
            var userId = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var userName = _httpContextAccessor.HttpContext?.User.Identity?.Name;

            foreach (var entry in ChangeTracker.Entries<AuditEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.UserId = userId;
                    entry.Entity.CreatedDate = DateTime.UtcNow;
                    entry.Entity.CreatedBy = !string.IsNullOrEmpty(userName) ? userName : null;
                }
                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedDate = DateTime.UtcNow;
                    entry.Entity.UpdatedBy = !string.IsNullOrEmpty(userName) ? userName : null;
                }
            }

            //add Audit log
            var auditEntries = new List<AuditLog>();
            var changes = ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Modified || e.State == EntityState.Added || e.State == EntityState.Deleted);

            foreach (var entry in changes)
            {
                string tableName = entry.Entity.GetType().Name;
                string? changedUserId = userId;

                AuditLog auditLog = new()
                {
                    EntityName = tableName,
                    ChangedUserId = changedUserId,
                    ChangeDate = DateTime.UtcNow,
                };

                var oldValues = new Dictionary<string, object?>();
                var newValues = new Dictionary<string, object?>();
                var changedColumns = new List<string>();

                switch (entry.State)
                {
                    case EntityState.Added:
                        foreach (var prop in entry.Properties)
                        {
                            if (prop.IsTemporary) continue;

                            auditLog.AuditType = Common.Enums.TrailType.Create;
                            newValues[prop.Metadata.Name] = prop.CurrentValue;
                        }
                        break;
                    case EntityState.Deleted:
                        foreach (var prop in entry.Properties)
                        {
                            auditLog.AuditType = Common.Enums.TrailType.Delete;
                            oldValues[prop.Metadata.Name] = prop.OriginalValue;
                            changedColumns.Add(prop.Metadata.Name);
                        }
                        break;
                    case EntityState.Modified:
                        foreach (var prop in entry.Properties)
                        {
                            if (prop.IsTemporary || Equals(prop.CurrentValue, prop.OriginalValue)) continue;

                            auditLog.AuditType = Common.Enums.TrailType.Update;
                            oldValues[prop.Metadata.Name] = prop.OriginalValue;
                            newValues[prop.Metadata.Name] = prop.CurrentValue;
                            changedColumns.Add(prop.Metadata.Name); ;
                        }
                        break;
                }

                auditLog.OldValues = JsonSerializer.Serialize(oldValues);
                auditLog.NewValues = JsonSerializer.Serialize(newValues);
                auditLog.ChangedColumns = changedColumns;

                auditEntries.Add(auditLog);
            }

            await AuditLogs.AddRangeAsync(auditEntries, cancellationToken);
            return await base.SaveChangesAsync(cancellationToken);
        }

        public void LoadData()
        {
            //    Customers.AddAsync(new Customer()
            //    {
            //        FirstName = "Ali",
            //        LastName = "Arena",
            //        PhoneNumber = "09377554254",
            //        Email = "itcenter167@gmail.com",
            //        Address = "Valiasr"
            //    });

            //    Invoices.AddAsync(new Invoice()
            //    {
            //        Id= 1001,
            //        CustomerId = 1000,
            //        InvoiceNumber = "A15200",
            //        PaidAmount = 10000,
            //        SarResidDate = DateTime.UtcNow.AddDays(15),
            //        SodorDate = DateTime.UtcNow.AddDays(-15),
            //        Status = Common.Enums.PaymentStatus.Pending,
            //        TotalAmount = 20000,
            //    });


            //    InvoiceItems.AddAsync(new InvoiceItem()
            //    {
            //        Id = 100,
            //        InvoiceId = 1001,
            //        Description = "Bag Model Cat25",
            //        Quantity = 1,
            //        UnitPrice = 20000
            //    });

            //    Payments.AddAsync(new Payment()
            //    {
            //        Amount = 10000,
            //        InvoiceId= 1001,
            //        PaymentDate = DateTime.UtcNow,
            //        PaymentMethod = Common.Enums.PaymentMethods.Cash
            //    });
        }
    }

    public static class SeedData
    {
        public static async Task SeedAdminUserAsync(UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager)
        {
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new ApplicationRole { Name = "Admin", Description = "Administrator role with full permissions" });
            }

            var adminUser = await userManager.FindByEmailAsync("admin@example.com");
            if (adminUser == null)
            {
                var newAdmin = new ApplicationUser
                {
                    UserName = "admin",
                    Email = "admin@example.com",
                    EmailConfirmed = true,
                    FullName = "admin",
                };

                var createResult = await userManager.CreateAsync(newAdmin, "Admin@123");
                if (createResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(newAdmin, "Admin");
                }
            }
        }
    }
}
