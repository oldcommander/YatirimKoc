using Microsoft.EntityFrameworkCore;
using YatirimKoc.Domain.Entities.Listings;
using System.Threading;
using System.Threading.Tasks;
using YatirimKoc.Domain.Entities.Settings;

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

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}