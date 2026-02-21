using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YatirimKoc.Domain.Entities.Listings;

namespace YatirimKoc.Infrastructure.Persistence.Configurations.Listings;

public class ListingConfiguration : IEntityTypeConfiguration<Listing>
{
    public void Configure(EntityTypeBuilder<Listing> builder)
    {
        builder.HasKey(x => x.Id);

        // Satılık/Kiralık İlişkisi
        builder.HasOne(x => x.TransactionType)
               .WithMany(x => x.Listings)
               .HasForeignKey(x => x.TransactionTypeId)
               .OnDelete(DeleteBehavior.Restrict);

        // Konut/Arsa İlişkisi
        builder.HasOne(x => x.PropertyType)
               .WithMany(x => x.Listings)
               .HasForeignKey(x => x.PropertyTypeId)
               .OnDelete(DeleteBehavior.Restrict);

        // Kullanıcı Profili (Admin) İlişkisi
        builder.HasOne(x => x.AdminProfile)
               .WithMany()
               .HasForeignKey(x => x.AdminProfileId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}