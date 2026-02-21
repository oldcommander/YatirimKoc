using MediatR;

namespace YatirimKoc.Application.Features.Listings.Commands;

public class UpdateListingCommand : IRequest<bool>
{
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; }
    public string City { get; set; } = null!;
    public string District { get; set; } = null!;
    public Guid TransactionTypeId { get; set; }
    public Guid PropertyTypeId { get; set; }
    public bool IsPublished { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public List<string>? NewImageUrls { get; set; }
    public List<Guid> DeletedImageIds { get; set; } = new();
    public Dictionary<Guid, string>? FeatureValues { get; set; }
}