using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using YatirimKoc.Application.Abstractions.Persistence;
using YatirimKoc.Domain.Entities.Admin;
using YatirimKoc.Domain.Entities.Identity;
using YatirimKoc.Domain.Entities.Listings;
using YatirimKoc.Domain.Entities.Logs;
using YatirimKoc.Domain.Entities.Mails;
using YatirimKoc.Domain.Entities.Messages;
using YatirimKoc.Domain.Entities.Reservations;
using YatirimKoc.Domain.Entities.Security;
using YatirimKoc.Domain.Entities.Settings;
using User = YatirimKoc.Domain.Entities.Identity.User;

namespace YatirimKoc.Infrastructure.Persistence;

public class ApplicationDbContext
    : IdentityDbContext<User, Role, Guid>,
      IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    // --------------------
    // ADMIN & SECURITY
    // --------------------
    public DbSet<AdminProfile> AdminProfiles => Set<AdminProfile>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();
    public DbSet<OtpVerification> OtpVerifications => Set<OtpVerification>();

    // --------------------
    // SITE SETTINGS
    // --------------------
    public DbSet<SiteSetting> SiteSettings => Set<SiteSetting>();

    // --------------------
    // MAIL
    // --------------------
    public DbSet<Mails> Mails => Set<Mails>();
    public DbSet<MailLog> MailLogs => Set<MailLog>();

    public DbSet<NewsletterSubscriber> NewsletterSubscribers { get; set; }
    // --------------------
    // RESERVATION & CONTACT
    // --------------------
    public DbSet<Reservation> Reservations => Set<Reservation>();
    public DbSet<AdminAvailability> AdminAvailabilities => Set<AdminAvailability>();
    public DbSet<ContactMessage> ContactMessages => Set<ContactMessage>();

    // --------------------
    // LISTINGS
    // --------------------
    public DbSet<Listing> Listings => Set<Listing>();
    public DbSet<TransactionType> TransactionTypes { get; set; }
    public DbSet<PropertyType> PropertyTypes { get; set; }
    public DbSet<Feature> Features { get; set; }
    public DbSet<PropertyFeature> PropertyFeatures { get; set; }
    public DbSet<ListingFeatureValue> ListingFeatureValues { get; set; }

    public DbSet<ListingImage> ListingImages => Set<ListingImage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ApplicationDbContext).Assembly
        );
    }
}
