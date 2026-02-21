using MediatR;
using System.ComponentModel.DataAnnotations;

namespace YatirimKoc.Application.Features.Listings.Commands;

public class CreateListingCommand : IRequest<Guid>
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }
    public string Currency { get; set; } = "TL";

    public string City { get; set; } = null!;
    public string District { get; set; } = null!;
    public string? Neighborhood { get; set; }

    [Required]
    public Guid? ListingTypeId { get; set; }

    [Required]
    public Guid? ListingCategoryId { get; set; }

    public bool IsPublished { get; set; }

    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public List<string>? ImageUrls { get; set; }

}
