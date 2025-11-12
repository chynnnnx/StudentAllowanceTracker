using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentAllowanceTracker.Domain.Entities;

namespace StudentAllowanceTracker.Infrastructure.Persistence.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<CategoryEntity>
    {
        public void Configure(EntityTypeBuilder<CategoryEntity> builder)
        {
            builder.HasKey(c => c.CategoryID);

            builder.Property(c => c.UserID)
                   .IsRequired();

            builder.Property(c => c.CategoryName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(c => c.Type)
                   .HasConversion<int>()
                   .IsRequired();

            builder.Property(c => c.BudgetAmount)
                   .HasColumnType("decimal(18,2)")
                   .IsRequired(false);

            builder.HasOne(c => c.User)
                   .WithMany()
                   .HasForeignKey(c => c.UserID)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(c => c.Expenses)
                   .WithOne(e => e.Category)
                   .HasForeignKey(e => e.CategoryID)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.ToTable("Categories");
        }
    }
}
