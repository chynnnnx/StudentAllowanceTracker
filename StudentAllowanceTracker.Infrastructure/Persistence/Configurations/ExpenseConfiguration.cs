using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentAllowanceTracker.Domain.Entities;

namespace StudentAllowanceTracker.Infrastructure.Persistence.Configurations
{
    public class ExpenseConfiguration: IEntityTypeConfiguration<Expense>
    {
        public void Configure(EntityTypeBuilder<Expense> builder)
        {
            builder.HasKey(e => e.ExpenseID);
            builder.Property(e => e.Category)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(e =>e.Amount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(e => e.Date)
                .IsRequired();

            builder.HasOne(e => e.User)
                .WithMany(u => u.Expenses)
                .HasForeignKey(e => e.UserID)
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasOne(e => e.Allowance)
                .WithMany(a => a.Expenses)
                .HasForeignKey(e => e.AllowanceID)
                .OnDelete(DeleteBehavior.Cascade);
            builder.ToTable("Expenses");
        }

    }
}
