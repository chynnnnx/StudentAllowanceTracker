using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using StudentAllowanceTracker.Domain.Entities;

namespace StudentAllowanceTracker.Infrastructure.Persistence.Configurations
{
    public class EmailVerificationCodeConfiguration : IEntityTypeConfiguration<EmailVerificationCode>
    {
        public void Configure(EntityTypeBuilder<EmailVerificationCode> builder)
        {
            builder.ToTable("EmailVerificationCodes");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Code)
                   .IsRequired()
                   .HasMaxLength(10);

            builder.Property(e => e.Expiration)
                   .IsRequired();

            builder.HasOne(e => e.User)
                   .WithMany()
                   .HasForeignKey(e => e.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
