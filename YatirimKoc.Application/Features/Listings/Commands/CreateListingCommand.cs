using MediatR;
using System.ComponentModel.DataAnnotations;

namespace YatirimKoc.Application.Features.Listings.Commands;

public class CreateListingCommand : IRequest<Guid>
{
    [Required]
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;

    [Required]
    public decimal Price { get; set; }
    public string Currency { get; set; } = "TL";

    [Required]
    public string City { get; set; } = null!;

    [Required]
    public string District { get; set; } = null!;
    public string? Neighborhood { get; set; }

    [Required]
    public Guid? TransactionTypeId { get; set; }

    [Required]
    public Guid? PropertyTypeId { get; set; }

    public bool IsPublished { get; set; }

    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public List<string>? ImageUrls { get; set; }
    public Dictionary<Guid, string>? FeatureValues { get; set; }

    // EKLENEN KISIM: İlanı ekleyen kullanıcının ID'si
    public Guid UserId { get; set; }
}