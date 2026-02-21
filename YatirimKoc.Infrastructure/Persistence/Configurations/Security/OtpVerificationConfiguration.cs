using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YatirimKoc.Domain.Entities.Security;

namespace YatirimKoc.Infrastructure.Persistence.Configurations.Security
{
    public class OtpVerificationConfiguration
        : IEntityTypeConfiguration<OtpVerification>
    {
        public void Configure(EntityTypeBuilder<OtpVerification> builder)
        {
            builder.ToTable("OtpVerifications");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Code)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(x => x.Purpose)
                .IsRequired()
                .HasMaxLength(50);

            builder.HasIndex(x => new { x.Email, x.Purpose });
            builder.HasIndex(x => x.ExpiresAt);
        }
    }

}
