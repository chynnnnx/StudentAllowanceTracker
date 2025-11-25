using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudentAllowanceTracker.Domain.Entities;  
namespace StudentAllowanceTracker.Infrastructure.Persistence.Configurations
{
    public class HistoryConfiguration: IEntityTypeConfiguration<HistoryEntity>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<HistoryEntity> builder)
        {
            builder.HasKey(h => h.HistoryID);
            builder.Property(h => h.Type)
                .IsRequired()
                .HasMaxLength(100);
            builder.Property(h => h.Amount)
                .HasColumnType("decimal(18,2)")
                .IsRequired(false);
            builder.Property(h => h.CategoryName)
                .HasMaxLength(100)
                .IsRequired(false);
            builder.Property(h => h.Description)
                .HasMaxLength(500)
                .IsRequired(false);
            builder.Property(h => h.Date)
                .IsRequired();
            builder.HasOne(h => h.User)
                .WithMany(u => u.Histories)
                .HasForeignKey(h => h.UserID)
                .OnDelete(DeleteBehavior.Cascade);
            builder.ToTable("History");
        }
    }
}
