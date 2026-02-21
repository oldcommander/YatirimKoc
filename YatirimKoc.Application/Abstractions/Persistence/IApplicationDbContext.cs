using Microsoft.EntityFrameworkCore;
using YatirimKoc.Domain.Entities.Listings;
using System.Threading;
using System.Threading.Tasks;
using YatirimKoc.Domain.Entities.Settings;
using YatirimKoc.Domain.Entities.Admin; // YENİ: AdminProfile için eklendi

namespace YatirimKoc.Application.Abstractions.Persistence;

public interface IApplicationDbContext
{
    DbSet<Listing> Listings { get; }
    DbSet<TransactionType> TransactionTypes { get; }
    DbSet<PropertyType> PropertyTypes { get; }
    DbSet<Feature> Features { get; }
    DbSet<PropertyFeature> PropertyFeatures { get; }
    DbSet<ListingFeatureValue> ListingFeatureValues { get; }
    DbSet<SiteSetting> SiteSettings { get; }

    // YENİ EKLENEN KISIM:
    DbSet<AdminProfile> AdminProfiles { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}