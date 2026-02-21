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
    public class MailTemplateConfiguration
        : IEntityTypeConfiguration<MailTemplate>
    {
        public void Configure(EntityTypeBuilder<MailTemplate> builder)
        {
            builder.ToTable("MailTemplates");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Code)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Subject)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.BodyHtml)
                .IsRequired()
                .HasColumnType("longtext");

            builder.HasIndex(x => x.Code)
                .IsUnique();
        }
    }

}
