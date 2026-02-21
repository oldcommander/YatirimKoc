using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using YatirimKoc.Domain.Entities.Listings;

namespace YatirimKoc.Infrastructure.Persistence.Configurations.Listings
{
    public class ListingConfiguration : IEntityTypeConfiguration<Listing>
    {
        public void Configure(EntityTypeBuilder<Listing> builder)
        {
            builder.ToTable("Listings");

            builder.HasKey(x => x.Id);

            // --------------------
            // BASIC INFO
            // --------------------
            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(x => x.Slug)
                .IsRequired()
                .HasMaxLength(300);

            builder.HasIndex(x => x.Slug)
                .IsUnique();

            builder.Property(x => x.Description)
                .HasColumnType("longtext");

            builder.Property(x => x.Price)
                .HasPrecision(18, 2);

            builder.Property(x => x.Currency)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(x => x.IsPublished)
                .IsRequired();

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            // --------------------
            // LOCATION
            // --------------------
            builder.Property(x => x.City)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.District)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(x => x.Neighborhood)
                .HasMaxLength(150);

            // --------------------
            // RELATIONS
            // --------------------
            builder.HasOne(x => x.ListingType)
                .WithMany()
                .HasForeignKey(x => x.ListingTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.ListingCategory)
                .WithMany()
                .HasForeignKey(x => x.ListingCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.AdminProfile)
                .WithMany()
                .HasForeignKey(x => x.AdminProfileId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Images)
                .WithOne(x => x.Listing)
                .HasForeignKey(x => x.ListingId)
                .OnDelete(DeleteBehavior.Cascade);

            // --------------------
            // INDEXES
            // --------------------
            builder.HasIndex(x => x.Price);
            builder.HasIndex(x => x.IsPublished);
            builder.HasIndex(x => new { x.City, x.District });
            builder.HasIndex(x => x.CreatedByUserId);

            builder.Property(x => x.Latitude)
    .HasPrecision(10, 6);

            builder.Property(x => x.Longitude)
                .HasPrecision(10, 6);

            builder.Property(x => x.IsDeleted)
                .HasDefaultValue(false);

            builder.HasQueryFilter(x => !x.IsDeleted);

        }
    }
}
