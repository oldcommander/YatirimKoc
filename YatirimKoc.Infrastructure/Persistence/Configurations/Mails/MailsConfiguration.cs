using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace YatirimKoc.Infrastructure.Persistence.Configurations.Mails;

public class MailsConfiguration : IEntityTypeConfiguration<YatirimKoc.Domain.Entities.Mails.Mails>
{
    public void Configure(EntityTypeBuilder<YatirimKoc.Domain.Entities.Mails.Mails> builder)
    {
        builder.ToTable("Mails");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Code)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Subject)
            .IsRequired()
            .HasMaxLength(250);

        builder.Property(x => x.BodyHtml)
            .IsRequired();

        builder.Property(x => x.IsActive)
            .HasDefaultValue(true);
    }
}