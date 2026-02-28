using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using YatirimKoc.Application.Abstractions.Persistence;
using YatirimKoc.Application.Interfaces; // FileUploadService için
using YatirimKoc.Domain.Entities.Admin;
using YatirimKoc.Domain.Entities.Identity;

namespace YatirimKoc.Application.Features.Users.Commands;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Guid>
{
    private readonly UserManager<User> _userManager;
    private readonly IApplicationDbContext _context;
    private readonly IFileUploadService _fileUploadService;

    public CreateUserCommandHandler(UserManager<User> userManager, IApplicationDbContext context, IFileUploadService fileUploadService)
    {
        _userManager = userManager;
        _context = context;
        _fileUploadService = fileUploadService;
    }

    public async Task<Guid> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        // 1. Profil Fotoğrafını Yükle
        string? profileImageUrl = null;
        if (request.ProfileImage != null && request.ProfileImage.Length > 0)
        {
            var uploadedUrls = await _fileUploadService.UploadAsync(new List<IFormFile> { request.ProfileImage }, "users");
            profileImageUrl = uploadedUrls.FirstOrDefault();
        }

        // 2. Identity User Oluştur
        var user = new User
        {
            Id = Guid.NewGuid(),
            UserName = request.Email,
            Email = request.Email,
            FirstName = request.FirstName,
            LastName = request.LastName,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        var result = await _userManager.CreateAsync(user, request.Password);
        if (!result.Succeeded)
            throw new Exception("Kullanıcı oluşturulamadı!");

        if (!string.IsNullOrWhiteSpace(request.Role))
            await _userManager.AddToRoleAsync(user, request.Role);

        // 3. AdminProfile Oluştur
        var adminProfile = new AdminProfile
        {
            Id = Guid.NewGuid(),
            UserId = user.Id,
            Title = request.Title,
            Phone = request.Phone,
            Biography = request.Biography,
            LicenseNumber = request.LicenseNumber,
            ExperienceYear = request.ExperienceYear,
            Specialties = request.Specialties,
            SpokenLanguages = request.SpokenLanguages,
            InstagramUrl = request.InstagramUrl,
            LinkedInUrl = request.LinkedInUrl,
            FacebookUrl = request.FacebookUrl,
            ProfileImageUrl = profileImageUrl, // BURASI EKLENDİ
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        _context.AdminProfiles.Add(adminProfile);
        await _context.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}