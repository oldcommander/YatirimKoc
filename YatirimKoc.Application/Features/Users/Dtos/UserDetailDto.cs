namespace YatirimKoc.Application.Features.Users.Dtos;

public class UserDetailDto : UserDto
{
    public string? Biography { get; set; }
    public string? LicenseNumber { get; set; }
    public int? ExperienceYear { get; set; }
    public string? Specialties { get; set; }
    public string? SpokenLanguages { get; set; }
    public string? InstagramUrl { get; set; }
    public string? LinkedInUrl { get; set; }
    public string? FacebookUrl { get; set; }
    public DateTime CreatedAt { get; set; }
}