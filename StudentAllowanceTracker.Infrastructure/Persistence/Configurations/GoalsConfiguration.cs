using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentAllowanceTracker.Domain.Entities;

namespace StudentAllowanceTracker.Infrastructure.Persistence.Configurations
{
    public class GoalsConfiguration: IEntityTypeConfiguration<GoalsEntity>
    {
        public void Configure(EntityTypeBuilder<GoalsEntity> builder) 
        {
        builder.HasKey(g => g.GoalID);
            builder.Property(g => g.GoalName)
            .IsRequired()
            .HasMaxLength(200);

            builder.Property(g => g.TargetAmount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(g => g.CurrentAmount)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(g => g.TargetDate)
                .IsRequired();

            builder.Property(g => g.Description)
                .HasMaxLength(1000);
            builder.Property(g => g.IsCompleted)
                   .IsRequired()
                   .HasDefaultValue(false);


            builder.HasOne(g => g.User)
                .WithMany(u => u.Goals)
                .HasForeignKey(g => g.UserID)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Goals");

        }

    }
}
