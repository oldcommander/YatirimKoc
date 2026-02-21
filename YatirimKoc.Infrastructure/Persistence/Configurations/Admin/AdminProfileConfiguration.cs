using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YatirimKoc.Domain.Entities.Admin;

namespace YatirimKoc.Infrastructure.Persistence.Configurations.Admin;

public class AdminProfileConfiguration
    : IEntityTypeConfiguration<AdminProfile>
{
    public void Configure(EntityTypeBuilder<AdminProfile> builder)
    {
        builder.ToTable("AdminProfiles");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Title)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(x => x.Biography)
            .HasColumnType("longtext");

        builder.Property(x => x.Phone)
            .HasMaxLength(20);

        builder.Property(x => x.ProfileImageUrl)
            .HasMaxLength(500);

        builder.Property(x => x.LicenseNumber)
            .HasMaxLength(50);

        builder.HasOne(x => x.User)
            .WithOne(u => u.AdminProfile)
            .HasForeignKey<AdminProfile>(x => x.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.UserId)
            .IsUnique();
    }
}
