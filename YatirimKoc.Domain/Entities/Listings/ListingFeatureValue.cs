namespace YatirimKoc.Domain.Entities.Listings;

public class ListingFeatureValue
{
    public Guid ListingId { get; set; }
    public Listing Listing { get; set; } = null!;

    public Guid FeatureId { get; set; }
    public Feature Feature { get; set; } = null!;

    // Kullanıcının girdiği veya seçtiği değer (Text olarak tutarız, her veri tipini kapsar)
    // Örn: "3+1", "Doğalgaz", "True" (Checkbox için), "15" (Bina yaşı)
    public string Value { get; set; } = null!;
}