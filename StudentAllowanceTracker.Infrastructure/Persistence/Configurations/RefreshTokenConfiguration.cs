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
    public class RefreshTokenConfiguration: IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasKey(rt => rt.Id);
            builder.Property(rt => rt.TokenHash)
                .IsRequired()
                .HasMaxLength(500);
            builder.Property(rt => rt.UserID)
                .IsRequired()
                .HasMaxLength(450);
            builder.Property(rt => rt.Expiration)
                .IsRequired();
            builder.Property(rt => rt.IsRevoked)
                .HasDefaultValue(false);
            builder.Property(rt => rt.CreatedAt)
                .HasDefaultValueSql("GETUTCDATE()");
        }
    }
}
