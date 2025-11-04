using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using StudentAllowanceTracker.Domain.Entities;
using StudentAllowanceTracker.Infrastructure.Persistence.Configurations;


namespace StudentAllowanceTracker.Infrastructure.Persistence.Data
{
    public class AppDbContext:  IdentityDbContext<AppIdentityUser>
    {

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Allowance> Allowances { get; set; }
        public DbSet<EmailVerificationCode> EmailVerificationCodes { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Allowance>().ToTable("Allowances");
            modelBuilder.Entity< EmailVerificationCode>().ToTable("EmailVerificationCodes");
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
