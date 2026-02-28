namespace YatirimKoc.Application.Features.Users.Dtos;

public class UserDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Role { get; set; } = null!;
    public bool IsActive { get; set; }

    // AdminProfile Özeti
    public string? Title { get; set; }
    public string? Phone { get; set; }
    public string? ProfileImageUrl { get; set; }
}