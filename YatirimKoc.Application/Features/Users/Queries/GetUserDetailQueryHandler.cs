using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using YatirimKoc.Application.Features.Users.Dtos;
using YatirimKoc.Domain.Entities.Identity;

namespace YatirimKoc.Application.Features.Users.Queries;

public class GetUserDetailQueryHandler : IRequestHandler<GetUserDetailQuery, UserDetailDto>
{
    private readonly UserManager<User> _userManager;

    public GetUserDetailQueryHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<UserDetailDto> Handle(GetUserDetailQuery request, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users
            .Include(u => u.AdminProfile)
            .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

        if (user == null)
            throw new Exception("Kullanıcı bulunamadı.");

        var roles = await _userManager.GetRolesAsync(user);
        var role = roles.FirstOrDefault() ?? "Bilinmiyor";

        return new UserDetailDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email!,
            Role = role,
            IsActive = user.IsActive,
            Title = user.AdminProfile?.Title,
            Phone = user.AdminProfile?.Phone,
            ProfileImageUrl = user.AdminProfile?.ProfileImageUrl,
            Biography = user.AdminProfile?.Biography,
            LicenseNumber = user.AdminProfile?.LicenseNumber,
            ExperienceYear = user.AdminProfile?.ExperienceYear,
            Specialties = user.AdminProfile?.Specialties,
            SpokenLanguages = user.AdminProfile?.SpokenLanguages,
            InstagramUrl = user.AdminProfile?.InstagramUrl,
            LinkedInUrl = user.AdminProfile?.LinkedInUrl,
            FacebookUrl = user.AdminProfile?.FacebookUrl,
            CreatedAt = user.CreatedAt
        };
    }
}