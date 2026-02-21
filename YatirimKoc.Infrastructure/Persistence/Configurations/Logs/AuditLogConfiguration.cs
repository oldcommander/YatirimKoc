using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YatirimKoc.Domain.Entities.Logs;

namespace YatirimKoc.Infrastructure.Persistence.Configurations.Logs;

public class AuditLogConfiguration : IEntityTypeConfiguration<AuditLog>
{
    public void Configure(EntityTypeBuilder<AuditLog> builder)
    {
        builder.ToTable("AuditLogs");

        // PK
        builder.HasKey(x => x.Id);

        // Columns
        builder.Property(x => x.TableName)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(x => x.Action)
            .IsRequired()
            .HasMaxLength(50); // Insert / Update / Delete

        builder.Property(x => x.OldValues)
            .HasColumnType("longtext");

        builder.Property(x => x.NewValues)
            .HasColumnType("longtext");

        builder.Property(x => x.UserEmail)
            .HasMaxLength(256);

        builder.Property(x => x.IpAddress)
            .HasMaxLength(45); // IPv6 uyumlu

        builder.Property(x => x.CreatedAt)
            .IsRequired();

        // Indexler (performans kritik)
        builder.HasIndex(x => x.UserId);
        builder.HasIndex(x => x.TableName);
        builder.HasIndex(x => x.Action);
        builder.HasIndex(x => x.CreatedAt);
    }
}
