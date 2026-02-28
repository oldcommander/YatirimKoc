using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using YatirimKoc.Application.Abstractions.Persistence;
using YatirimKoc.Application.Interfaces;
using YatirimKoc.Domain.Entities.Identity;

namespace YatirimKoc.Application.Features.Users.Commands;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Guid>
{
    private readonly UserManager<User> _userManager;
    private readonly IApplicationDbContext _context;
    private readonly IFileUploadService _fileUploadService;

    public UpdateUserCommandHandler(
        UserManager<User> userManager,
        IApplicationDbContext context,
        IFileUploadService fileUploadService)
    {
        _userManager = userManager;
        _context = context;
        _fileUploadService = fileUploadService;
    }

    public async Task<Guid> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users
            .Include(u => u.AdminProfile)
            .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

        if (user == null)
            throw new Exception("Kullanıcı bulunamadı.");

        // Identity Güncellemesi
        user.FirstName = request.FirstName;
        user.LastName = request.LastName;
        user.Email = request.Email;
        user.UserName = request.Email;
        user.IsActive = request.IsActive;

        await _userManager.UpdateAsync(user);

        // Rol Güncellemesi
        if (!string.IsNullOrWhiteSpace(request.Role))
        {
            var currentRoles = await _userManager.GetRolesAsync(user);
            if (!currentRoles.Contains(request.Role))
            {
                await _userManager.RemoveFromRolesAsync(user, currentRoles);
                await _userManager.AddToRoleAsync(user, request.Role);
            }
        }

        // 1. Yeni Fotoğraf Yüklendiyse Kaydet
        string? newProfileImageUrl = null;
        if (request.ProfileImage != null && request.ProfileImage.Length > 0)
        {
            var uploadedUrls = await _fileUploadService.UploadAsync(new List<IFormFile> { request.ProfileImage }, "users");
            newProfileImageUrl = uploadedUrls.FirstOrDefault();
        }

        // Admin Profile Güncellemesi
        if (user.AdminProfile != null)
        {
            user.AdminProfile.Title = request.Title;
            user.AdminProfile.Phone = request.Phone;
            user.AdminProfile.Biography = request.Biography;
            user.AdminProfile.LicenseNumber = request.LicenseNumber;
            user.AdminProfile.ExperienceYear = request.ExperienceYear;
            user.AdminProfile.Specialties = request.Specialties;
            user.AdminProfile.SpokenLanguages = request.SpokenLanguages;
            user.AdminProfile.InstagramUrl = request.InstagramUrl;
            user.AdminProfile.LinkedInUrl = request.LinkedInUrl;
            user.AdminProfile.FacebookUrl = request.FacebookUrl;
            user.AdminProfile.IsActive = request.IsActive;

            // Eğer yeni bir resim url'si oluştuysa sadece o zaman güncelle (eski resim kaybolmasın)
            if (!string.IsNullOrEmpty(newProfileImageUrl))
            {
                user.AdminProfile.ProfileImageUrl = newProfileImageUrl;
            }
        }

        await _context.SaveChangesAsync(cancellationToken);

        return user.Id;
    }
}