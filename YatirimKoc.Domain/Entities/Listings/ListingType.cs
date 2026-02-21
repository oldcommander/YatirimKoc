namespace YatirimKoc.Domain.Entities.Listings;

public class ListingType
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!; // Satılık, Kiralık
    public bool IsActive { get; set; } = true;
}
