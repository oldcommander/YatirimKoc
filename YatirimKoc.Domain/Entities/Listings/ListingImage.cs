namespace YatirimKoc.Domain.Entities.Listings;

public class ListingImage
{
    public Guid Id { get; set; }

    public Guid ListingId { get; set; }
    public Listing Listing { get; set; } = null!;

    public string ImageUrl { get; set; } = null!;
    public bool IsCover { get; set; } = false;
    public int Order { get; set; } = 0;
    public bool IsDeleted { get; set; } = false;

}
