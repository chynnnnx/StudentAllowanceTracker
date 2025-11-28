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
    public class UserSubscriptionConfiguration: IEntityTypeConfiguration<UserSubscription>
    {
        public void Configure(EntityTypeBuilder<UserSubscription> builder)
        {
            builder.HasKey(builder => builder.Id);
            builder.Property(us => us.ReceiveEmail)
                .IsRequired()
                .HasDefaultValue(true);
            builder.Property(us => us.Frequency)
                .IsRequired()
                .HasConversion<string>()
                .HasMaxLength(50);
            builder.Property(us => us.LastReminderSentAt)
                .IsRequired();
            builder.HasOne(us => us.User)
                .WithMany(u => u.UserSubscriptions)
                .HasForeignKey(us => us.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
