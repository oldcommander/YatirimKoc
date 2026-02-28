using YatirimKoc.Domain.Entities.Identity;

namespace YatirimKoc.Domain.Entities.Admin;

public class AdminProfile
{
    public Guid Id { get; set; }

    // Identity User
    public Guid UserId { get; set; }

    public string Title { get; set; } = null!; // Örn: Lüks Konut Uzmanı, Broker
    public string? Biography { get; set; }

    public string? Phone { get; set; }
    public string? ProfileImageUrl { get; set; }

    public string? LicenseNumber { get; set; } // Taşınmaz Ticareti Yetki Belgesi
    public int? ExperienceYear { get; set; }

    // Yeni Eklenen Kurumsal Alanlar
    public string? Specialties { get; set; } // Örn: "Ticari, Arsa, Lüks Konut" (Virgülle ayrılmış veya JSON)
    public string? SpokenLanguages { get; set; } // Örn: "İngilizce, Rusça"
    public string? InstagramUrl { get; set; }
    public string? LinkedInUrl { get; set; }
    public string? FacebookUrl { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public User User { get; set; } = null!;
}