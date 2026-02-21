using YatirimKoc.Domain.Common;

namespace YatirimKoc.Domain.Entities.Listings;

public class Feature : BaseEntity
{
    public string Name { get; set; } = null!; // "Oda Sayısı", "Isıtma Tipi", "Takasa Uygun", "Balkon"

    // Formda nasıl gösterilecek? (Text, Number, Select, Checkbox vb.)
    public string InputType { get; set; } = null!;

    // Eğer InputType "Select" ise virgülle ayrılmış seçenekler ("Kombi,Merkezi,Doğalgaz Sobası")
    public string? Options { get; set; }

    public bool IsActive { get; set; } = true;

    // Bu özelliğin atandığı Emlak Tipleri (Örn: Oda sayısı -> Konut ve İşyeri'ne atanır)
    public ICollection<PropertyFeature> PropertyFeatures { get; set; } = new List<PropertyFeature>();
}