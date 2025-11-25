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
        public DbSet<ExpenseEntity> Expenses { get; set; }
        public DbSet<GoalsEntity> StudentGoals { get; set; }
       public DbSet<CategoryEntity> Category { get; set; }
        public DbSet<BudgetEntity> Budgets { get; set; }
        public DbSet<HistoryEntity> History { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Allowance>().ToTable("Allowances");
            modelBuilder.Entity< EmailVerificationCode>().ToTable("EmailVerificationCodes");
            modelBuilder.Entity<ExpenseEntity>().ToTable("Expenses");
            modelBuilder.Entity<GoalsEntity>().ToTable("StudentGoals");
            modelBuilder.Entity<CategoryEntity>().ToTable("Categories");
            modelBuilder.Entity<BudgetEntity>().ToTable("Budgets");
            modelBuilder.Entity<HistoryEntity>().ToTable("History");

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
        }
    }
}
