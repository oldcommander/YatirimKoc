namespace YatirimKoc.Domain.Entities.Listings;

public class PropertyFeature
{
    public Guid PropertyTypeId { get; set; }
    public PropertyType PropertyType { get; set; } = null!;

    public Guid FeatureId { get; set; }
    public Feature Feature { get; set; } = null!;

    public bool IsRequired { get; set; } = false; // İlan girerken bu alan zorunlu mu?
    public int Order { get; set; } // Formda kaçıncı sırada çıkacak?
}