namespace YatirimKoc.Domain.Entities.Listings;

public class ListingCategory
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!; // Konut, Arsa
    public bool IsActive { get; set; } = true;
}
