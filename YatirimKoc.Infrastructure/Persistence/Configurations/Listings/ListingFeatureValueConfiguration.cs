using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YatirimKoc.Domain.Entities.Listings;

namespace YatirimKoc.Infrastructure.Persistence.Configurations.Listings;

public class ListingFeatureValueConfiguration : IEntityTypeConfiguration<ListingFeatureValue>
{
    public void Configure(EntityTypeBuilder<ListingFeatureValue> builder)
    {
        // Composite Primary Key
        builder.HasKey(lfv => new { lfv.ListingId, lfv.FeatureId });

        builder.HasOne(lfv => lfv.Listing)
               .WithMany(l => l.FeatureValues)
               .HasForeignKey(lfv => lfv.ListingId)
               .OnDelete(DeleteBehavior.Cascade);

        // Feature ile ilişki
        builder.HasOne(lfv => lfv.Feature)
               .WithMany() // Feature üzerinden ListingFeatureValue'lara gitmeye gerek yok
               .HasForeignKey(lfv => lfv.FeatureId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}