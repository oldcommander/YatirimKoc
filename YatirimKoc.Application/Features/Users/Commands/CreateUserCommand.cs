using MediatR;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace YatirimKoc.Application.Features.Users.Commands;

public class CreateUserCommand : IRequest<Guid>
{
    // Identity User Bilgileri
    [Required]
    public string FirstName { get; set; } = null!;

    [Required]
    public string LastName { get; set; } = null!;

    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;

    [Required]
    public string Role { get; set; } = null!; // SuperAdmin veya Admin

    // AdminProfile (Danışman) Bilgileri
    [Required]
    public string Title { get; set; } = null!;
    public string? Phone { get; set; }
    public string? Biography { get; set; }
    public string? LicenseNumber { get; set; }
    public int? ExperienceYear { get; set; }
    public string? Specialties { get; set; }
    public string? SpokenLanguages { get; set; }
    public string? InstagramUrl { get; set; }
    public string? LinkedInUrl { get; set; }
    public string? FacebookUrl { get; set; }
    public IFormFile? ProfileImage { get; set; }
}