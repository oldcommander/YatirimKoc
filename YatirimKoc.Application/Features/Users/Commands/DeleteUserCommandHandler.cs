using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using YatirimKoc.Application.Abstractions.Persistence;
using YatirimKoc.Domain.Entities.Identity;

namespace YatirimKoc.Application.Features.Users.Commands;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
{
    private readonly UserManager<User> _userManager;
    private readonly IApplicationDbContext _context;

    public DeleteUserCommandHandler(UserManager<User> userManager, IApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userManager.Users
            .Include(u => u.AdminProfile)
            .FirstOrDefaultAsync(u => u.Id == request.Id, cancellationToken);

        if (user == null) return false;

        // Soft delete uyguluyoruz
        user.IsActive = false;
        await _userManager.UpdateAsync(user);

        if (user.AdminProfile != null)
        {
            user.AdminProfile.IsActive = false;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}