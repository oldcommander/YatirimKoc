using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YatirimKoc.Domain.Entities.Listings;

namespace YatirimKoc.Infrastructure.Persistence.Configurations.Listings;

public class PropertyFeatureConfiguration : IEntityTypeConfiguration<PropertyFeature>
{
    public void Configure(EntityTypeBuilder<PropertyFeature> builder)
    {
        // Composite Primary Key (İki anahtarın birleşimi)
        builder.HasKey(pf => new { pf.PropertyTypeId, pf.FeatureId });

        builder.HasOne(pf => pf.PropertyType)
               .WithMany(pt => pt.Features)
               .HasForeignKey(pf => pf.PropertyTypeId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(pf => pf.Feature)
               .WithMany(f => f.PropertyFeatures)
               .HasForeignKey(pf => pf.FeatureId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}