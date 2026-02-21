using YatirimKoc.Domain.Common;

namespace YatirimKoc.Domain.Entities.Listings;

public class TransactionType : BaseEntity // BaseEntity id vs tutuyordur
{
    public string Name { get; set; } = null!; // Satılık, Kiralık, Kat Karşılığı vb.
    public string Slug { get; set; } = null!;
    public bool IsActive { get; set; } = true;

    public ICollection<Listing> Listings { get; set; } = new List<Listing>();
}