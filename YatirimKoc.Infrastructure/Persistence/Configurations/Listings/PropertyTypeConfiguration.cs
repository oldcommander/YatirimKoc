using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YatirimKoc.Domain.Entities.Listings;

namespace YatirimKoc.Infrastructure.Persistence.Configurations.Listings;

public class PropertyTypeConfiguration : IEntityTypeConfiguration<PropertyType>
{
    public void Configure(EntityTypeBuilder<PropertyType> builder)
    {
        builder.HasKey(x => x.Id);

        // Kendi kendine ilişki (Self-referencing) - Alt Kategori mantığı
        builder.HasOne(x => x.Parent)
               .WithMany(x => x.SubTypes)
               .HasForeignKey(x => x.ParentId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}