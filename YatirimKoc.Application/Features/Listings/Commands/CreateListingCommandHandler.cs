using MediatR;
using Microsoft.EntityFrameworkCore;
using YatirimKoc.Application.Abstractions.Persistence;
using YatirimKoc.Domain.Entities.Admin;
using YatirimKoc.Domain.Entities.Listings;

namespace YatirimKoc.Application.Features.Listings.Commands;

public class CreateListingCommandHandler : IRequestHandler<CreateListingCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateListingCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateListingCommand request, CancellationToken cancellationToken)
    {
        // 1. KULLANICI VE ADMIN PROFİLİ KONTROLÜ (HATA DÜZELTİLDİ)
        var adminProfile = await _context.AdminProfiles // Set<> yerine doğrudan tabloyu çağırıyoruz
            .FirstOrDefaultAsync(x => x.UserId == request.UserId, cancellationToken);

        // Eğer kullanıcının henüz bir Admin kaydı yoksa, anında oluştur.
        if (adminProfile == null)
        {
            adminProfile = new AdminProfile
            {
                Id = Guid.NewGuid(),
                UserId = request.UserId,
                Title = "Sistem Yöneticisi", // HATA BURADAN KAYNAKLANIYORDU, EKLENDİ!
                Phone = "Belirtilmemiş",     // Eğer Phone alanı da zorunluysa diye garantiye alalım
                CreatedAt = DateTime.UtcNow
            };
            _context.AdminProfiles.Add(adminProfile);
            await _context.SaveChangesAsync(cancellationToken);
        }

        // 2. İLANI OLUŞTUR
        var listing = new Listing
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Slug = request.Title.ToLower().Replace(" ", "-").Replace("ı", "i").Replace("ğ", "g").Replace("ü", "u").Replace("ş", "s").Replace("ö", "o").Replace("ç", "c"),
            Description = request.Description ?? "",
            Price = request.Price,
            Currency = request.Currency,
            City = request.City,
            District = request.District,
            Neighborhood = request.Neighborhood,

            TransactionTypeId = request.TransactionTypeId!.Value,
            PropertyTypeId = request.PropertyTypeId!.Value,

            IsPublished = request.IsPublished,
            Latitude = request.Latitude,
            Longitude = request.Longitude,

            CreatedByUserId = request.UserId,
            AdminProfileId = adminProfile.Id,

            CreatedAt = DateTime.UtcNow
        };

        // 3. MEDYA (RESİM/VİDEO) EKLENMESİ
        if (request.ImageUrls != null && request.ImageUrls.Any())
        {
            foreach (var url in request.ImageUrls)
            {
                listing.Images.Add(new ListingImage
                {
                    Id = Guid.NewGuid(),
                    ImageUrl = url,
                    IsCover = listing.Images.Count == 0
                });
            }
        }

        // 4. DİNAMİK ÖZELLİKLERİN (SÖZLÜK) EKLENMESİ
        if (request.FeatureValues != null && request.FeatureValues.Any())
        {
            foreach (var feature in request.FeatureValues)
            {
                if (!string.IsNullOrWhiteSpace(feature.Value))
                {
                    listing.FeatureValues.Add(new ListingFeatureValue
                    {
                        FeatureId = feature.Key,
                        Value = feature.Value
                    });
                }
            }
        }

        _context.Listings.Add(listing);
        await _context.SaveChangesAsync(cancellationToken);

        return listing.Id;
    }
}