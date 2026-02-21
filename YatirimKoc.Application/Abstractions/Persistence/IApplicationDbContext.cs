using Microsoft.EntityFrameworkCore;
using YatirimKoc.Domain.Entities.Admin;
using YatirimKoc.Domain.Entities.Listings;
using YatirimKoc.Domain.Entities.Logs;
using YatirimKoc.Domain.Entities.Mails;
using YatirimKoc.Domain.Entities.Messages;
using YatirimKoc.Domain.Entities.Reservations;
using YatirimKoc.Domain.Entities.Security;
using YatirimKoc.Domain.Entities.Settings;

namespace YatirimKoc.Application.Abstractions.Persistence;

public interface IApplicationDbContext
{
    // --------------------
    // CORE
    // --------------------
    DbSet<AdminProfile> AdminProfiles { get; }
    DbSet<AuditLog> AuditLogs { get; }

    // --------------------
    // SITE SETTINGS
    // --------------------
    DbSet<SiteSetting> SiteSettings { get; }

    // --------------------
    // MAIL
    // --------------------
    DbSet<MailTemplate> MailTemplates { get; }
    DbSet<MailLog> MailLogs { get; }

    // --------------------
    // SECURITY
    // --------------------
    DbSet<OtpVerification> OtpVerifications { get; }

    // --------------------
    // RESERVATION & CONTACT
    // --------------------
    DbSet<Reservation> Reservations { get; }
    DbSet<AdminAvailability> AdminAvailabilities { get; }
    DbSet<ContactMessage> ContactMessages { get; }

    // --------------------
    // LISTINGS
    // --------------------
    DbSet<Listing> Listings { get; }
    DbSet<ListingType> ListingTypes { get; }
    DbSet<ListingCategory> ListingCategories { get; }

    // --------------------
    // SAVE
    // --------------------
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
