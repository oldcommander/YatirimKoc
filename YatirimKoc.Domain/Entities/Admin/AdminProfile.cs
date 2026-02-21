using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using YatirimKoc.Domain.Common;
using YatirimKoc.Domain.Entities.Identity;

namespace YatirimKoc.Domain.Entities.Admin;

public class AdminProfile
{
    public Guid Id { get; set; }

    // Identity User
    public Guid UserId { get; set; }

    public string Title { get; set; } = null!; // Gayrimenkul Danışmanı
    public string? Biography { get; set; }

    public string? Phone { get; set; }
    public string? ProfileImageUrl { get; set; }

    public string? LicenseNumber { get; set; } // Yetki belgesi
    public int? ExperienceYear { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // Navigation
    public User User { get; set; } = null!;
}


