using MediatR;
using Microsoft.EntityFrameworkCore;
using YatirimKoc.Application.Abstractions.Persistence;
using YatirimKoc.Application.Features.SiteSettings.Dtos;
using YatirimKoc.Application.Features.SiteSettings.Queries;

namespace YatirimKoc.Application.Features.SiteSettings.Queries;

public class GetSiteSettingsQueryHandler
    : IRequestHandler<GetSiteSettingsQuery, SiteSettingsDto>
{
    private readonly IApplicationDbContext _context;

    public GetSiteSettingsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SiteSettingsDto> Handle(
        GetSiteSettingsQuery request,
        CancellationToken cancellationToken)
    {
        var settings = await _context.SiteSettings
            .AsNoTracking()
            .Where(x => x.IsActive)
            .ToListAsync(cancellationToken);

        var dto = new SiteSettingsDto();

        foreach (var setting in settings)
        {
            switch (setting.Key)
            {
                case "SiteName":
                    dto.SiteName = setting.Value;
                    break;

                case "LogoUrl":
                    dto.LogoUrl = setting.Value;
                    break;

                case "MaintenanceMode":
                    dto.MaintenanceMode =
                        bool.TryParse(setting.Value, out var value) && value;
                    break;
            }
        }

        return dto;
    }
}
