using Microsoft.AspNetCore.Identity;
using YatirimKoc.Domain.Entities.Admin;

namespace YatirimKoc.Domain.Entities.Identity;

public class User : IdentityUser<Guid>
{
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public AdminProfile? AdminProfile { get; set; }
}

