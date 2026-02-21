using Microsoft.AspNetCore.Identity;

namespace YatirimKoc.Domain.Entities.Identity;

public class Role : IdentityRole<Guid>
{
    public string? Description { get; set; }
}
