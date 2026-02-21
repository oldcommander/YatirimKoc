using YatirimKoc.Domain.Common;
using YatirimKoc.Domain.Entities.Admin;

namespace YatirimKoc.Domain.Entities.Listings;

public class Listing : AuditableEntity // Varsa auditable entity'nizden türetin
{
    public string Title { get; set; } = null!;
    public string Slug { get; set; } = null!;
    public string Description { get; set; } = null!;

    public decimal Price { get; set; }
    public string Currency { get; set; } = "TL";

    // Konum Verileri
    public string City { get; set; } = null!;
    public string District { get; set; } = null!;
    public string? Neighborhood { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public bool IsPublished { get; set; } = false;

    // İlişkiler
    public Guid TransactionTypeId { get; set; } // Satılık mı Kiralık mı?
    public TransactionType TransactionType { get; set; } = null!;

    public Guid PropertyTypeId { get; set; } // Konut mu Arsa mı?
    public PropertyType PropertyType { get; set; } = null!;

    // Dinamik Değerler (Bu ilana girilen özel değerler)
    public ICollection<ListingFeatureValue> FeatureValues { get; set; } = new List<ListingFeatureValue>();

    public ICollection<ListingImage> Images { get; set; } = new List<ListingImage>();

    // ... Diğer özellikler (Fiyat, Başlık vs.) ...

    // --------------------
    // AUTHORIZATION (Geri Eklenenler)
    // --------------------
    public Guid CreatedByUserId { get; set; }
    public Guid AdminProfileId { get; set; }
    public AdminProfile AdminProfile { get; set; } = null!;
}