using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YatirimKoc.Domain.Entities.Mails;

namespace YatirimKoc.Infrastructure.Persistence.Configurations.Mails
{
    public class MailLogConfiguration
        : IEntityTypeConfiguration<MailLog>
    {
        public void Configure(EntityTypeBuilder<MailLog> builder)
        {
            builder.ToTable("MailLogs");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.ToEmail)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Subject)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.TemplateCode)
                .HasMaxLength(100);

            builder.HasIndex(x => x.CreatedAt);
        }
    }

}
