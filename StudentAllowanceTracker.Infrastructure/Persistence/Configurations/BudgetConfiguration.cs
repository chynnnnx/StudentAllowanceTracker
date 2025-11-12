using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentAllowanceTracker.Domain.Entities;

namespace StudentAllowanceTracker.Infrastructure.Persistence.Configurations
{
    public class BudgetConfiguration : IEntityTypeConfiguration<BudgetEntity>
    {
        public void Configure(EntityTypeBuilder<BudgetEntity> builder)
        {
            builder.HasKey(b => b.BudgetID);

            builder.Property(b => b.UserID)
                   .IsRequired();

            builder.Property(b => b.TotalAllowance)
                   .IsRequired()
                   .HasColumnType("decimal(18,2)");

            builder.Property(b => b.NeedsPercentage)
                   .IsRequired()
                   .HasColumnType("decimal(5,2)");

            builder.Property(b => b.WantsPercentage)
                   .IsRequired()
                   .HasColumnType("decimal(5,2)");

            builder.Property(b => b.SavingsPercentage)
                   .IsRequired()
                   .HasColumnType("decimal(5,2)");

            builder.Property(b => b.StartDate)
                   .IsRequired();

            builder.HasOne(b => b.User)
                   .WithMany()
                   .HasForeignKey(b => b.UserID)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Budgets");
        }
    }
}
