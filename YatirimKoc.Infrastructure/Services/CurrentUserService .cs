using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore; // Include için gerekli
using System.Security.Claims;
using YatirimKoc.Application.Interfaces;
using YatirimKoc.Domain.Entities.Identity; // User için gerekli

namespace YatirimKoc.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<User> _userManager;

        public Guid UserId { get; private set; }
        public Guid AdminProfileId { get; private set; }

        public CurrentUserService(
            IHttpContextAccessor httpContextAccessor,
            UserManager<User> userManager)
        {
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;

            var userIdClaim = _httpContextAccessor.HttpContext?
                .User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!string.IsNullOrEmpty(userIdClaim))
            {
                UserId = Guid.Parse(userIdClaim);

                var user = _userManager.Users
                    .Include(x => x.AdminProfile)
                    .FirstOrDefault(x => x.Id == UserId);

                if (user?.AdminProfile != null)
                    AdminProfileId = user.AdminProfile.Id;
            }
        }
    }

}
