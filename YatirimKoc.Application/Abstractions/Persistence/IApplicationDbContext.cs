using Microsoft.EntityFrameworkCore;
using YatirimKoc.Domain.Entities.Listings;
using System.Threading;
using System.Threading.Tasks;
using YatirimKoc.Domain.Entities.Settings;
using YatirimKoc.Domain.Entities.Admin;
using YatirimKoc.Domain.Entities.Messages;
using YatirimKoc.Domain.Entities.Reservations;
using YatirimKoc.Domain.Entities.Mails; // YENİ: AdminProfile için eklendi

namespace YatirimKoc.Application.Abstractions.Persistence;

public interface IApplicationDbContext
{
    DbSet<Listing> Listings { get; }
    DbSet<ListingImage> ListingImages { get; }
    DbSet<TransactionType> TransactionTypes { get; }
    DbSet<PropertyType> PropertyTypes { get; }
    DbSet<Feature> Features { get; }
    DbSet<PropertyFeature> PropertyFeatures { get; }
    DbSet<ListingFeatureValue> ListingFeatureValues { get; }
    DbSet<SiteSetting> SiteSettings { get; }
    DbSet<ContactMessage> ContactMessages { get; }
    DbSet<Reservation> Reservations { get; }
    DbSet<Mails> Mails { get; }

    // YENİ EKLENEN KISIM:
    DbSet<AdminProfile> AdminProfiles { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}