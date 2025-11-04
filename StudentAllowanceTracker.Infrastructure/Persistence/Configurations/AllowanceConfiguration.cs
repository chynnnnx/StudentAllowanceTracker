
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentAllowanceTracker.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentAllowanceTracker.Infrastructure.Persistence.Configurations
{
    public class AllowanceConfiguration: IEntityTypeConfiguration<Allowance>
    {
        public void Configure(EntityTypeBuilder<Allowance> builder)
        {
            builder.HasKey(a => a.AllowanceID);

            builder.Property(a => a.Amount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");


            builder.Property(a => a.Description)
                .HasMaxLength(500);

            builder.Property(a => a.StartDate)
                .IsRequired();

            builder.Property(a => a.EndDate)
                .IsRequired(false);
            builder.Property(a => a.Type)
                  .HasConversion<string>()
                  .HasMaxLength(50);
            builder.HasOne(a => a.User)
             .WithMany(u => u.Allowances)
             .HasForeignKey(a => a.UserId)
             .OnDelete(DeleteBehavior.Cascade);
            builder.ToTable("Allowances");

        }
    }
}
