using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using YatirimKoc.Application.Features.Users.Dtos;
using YatirimKoc.Domain.Entities.Identity;

namespace YatirimKoc.Application.Features.Users.Queries;

public class GetAllUsersQueryHandler : IRequestHandler<GetAllUsersQuery, List<UserDto>>
{
    private readonly UserManager<User> _userManager;

    public GetAllUsersQueryHandler(UserManager<User> userManager)
    {
        _userManager = userManager;
    }

    public async Task<List<UserDto>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _userManager.Users
            .Include(u => u.AdminProfile)
            .OrderByDescending(u => u.CreatedAt)
            .ToListAsync(cancellationToken);

        var userDtos = new List<UserDto>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? "Bilinmiyor";

            userDtos.Add(new UserDto
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email!,
                Role = role,
                IsActive = user.IsActive,
                Title = user.AdminProfile?.Title,
                Phone = user.AdminProfile?.Phone,
                ProfileImageUrl = user.AdminProfile?.ProfileImageUrl
            });
        }

        return userDtos;
    }
}