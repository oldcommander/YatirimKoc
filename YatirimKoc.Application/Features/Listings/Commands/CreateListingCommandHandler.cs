using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using YatirimKoc.Application.Abstractions.Persistence;
using YatirimKoc.Application.Interfaces;
using YatirimKoc.Domain.Entities.Listings;


namespace YatirimKoc.Application.Features.Listings.Commands;

public class CreateListingCommandHandler
    : IRequestHandler<CreateListingCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUser;

    public CreateListingCommandHandler(IApplicationDbContext context, ICurrentUserService currentUser)
    {
        _context = context;
        _currentUser = currentUser;
    }

    public async Task<Guid> Handle(CreateListingCommand request, CancellationToken cancellationToken)
    {
        var listing = new Listing
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Slug = await GenerateSlugAsync(request.Title, cancellationToken),
            Description = request.Description,
            Price = request.Price,
            Currency = request.Currency,
            City = request.City,
            District = request.District,
            Neighborhood = request.Neighborhood,
            ListingTypeId = (Guid)request.ListingTypeId,
            ListingCategoryId = (Guid)request.ListingCategoryId,
            IsPublished = request.IsPublished,
            CreatedAt = DateTime.UtcNow,
            CreatedByUserId = _currentUser.UserId,
            AdminProfileId = _currentUser.AdminProfileId,
            Latitude = request.Latitude,
            Longitude = request.Longitude
        };

        if (request.ImageUrls != null)
        {
            listing.Images = request.ImageUrls
                .Select((url, index) => new ListingImage
                {
                    Id = Guid.NewGuid(),
                    ImageUrl = url,
                    Order = index,
                    IsCover = index == 0
                }).ToList();
        }

        await _context.Listings.AddAsync(listing);
        await _context.SaveChangesAsync(cancellationToken);

        return listing.Id;
    }
    private async Task<string> GenerateSlugAsync(string title, CancellationToken cancellationToken)
    {
        var slug = title.ToLowerInvariant();

        slug = slug
            .Replace("ı", "i")
            .Replace("ğ", "g")
            .Replace("ü", "u")
            .Replace("ş", "s")
            .Replace("ö", "o")
            .Replace("ç", "c");

        slug = Regex.Replace(slug, @"[^a-z0-9\s-]", "");
        slug = Regex.Replace(slug, @"\s+", "-").Trim('-');

        // unique kontrol
        var exists = await _context.Listings
            .AnyAsync(x => x.Slug == slug, cancellationToken);

        if (exists)
            slug = $"{slug}-{Guid.NewGuid().ToString().Substring(0, 6)}";

        return slug;
    }

}
