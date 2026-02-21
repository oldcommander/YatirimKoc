using YatirimKoc.Domain.Common;

namespace YatirimKoc.Domain.Entities.Listings;

public class PropertyType : BaseEntity
{
    public string Name { get; set; } = null!; // Konut, Arsa, İşyeri
    public string Slug { get; set; } = null!;

    // Alt kategoriler için (Örn: Konut -> Daire)
    public Guid? ParentId { get; set; }
    public PropertyType? Parent { get; set; }
    public ICollection<PropertyType> SubTypes { get; set; } = new List<PropertyType>();

    public bool IsActive { get; set; } = true;

    public ICollection<Listing> Listings { get; set; } = new List<Listing>();
    public ICollection<PropertyFeature> Features { get; set; } = new List<PropertyFeature>(); // Bu tipe ait özellikler
}