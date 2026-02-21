using MediatR;
using YatirimKoc.Application.Abstractions.Persistence; // YENİ: Doğru referans
using YatirimKoc.Domain.Entities.Listings;

namespace YatirimKoc.Application.Features.Listings.Commands;

public class CreateListingCommandHandler : IRequestHandler<CreateListingCommand, Guid>
{
    private readonly IApplicationDbContext _context; // YENİ: Interface kullanıyoruz

    public CreateListingCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateListingCommand request, CancellationToken cancellationToken)
    {
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

            // TODO: Authentication/Identity eklendiğinde CurrentUser'dan alınacak
            CreatedByUserId = Guid.Empty,
            CreatedAt = DateTime.UtcNow
        };

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

        // DİNAMİK ÖZELLİKLERİ İLANA EKLE
        if (request.FeatureValues != null && request.FeatureValues.Any())
        {
            foreach (var feature in request.FeatureValues)
            {
                // Boş gönderilmeyen alanları kaydet
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