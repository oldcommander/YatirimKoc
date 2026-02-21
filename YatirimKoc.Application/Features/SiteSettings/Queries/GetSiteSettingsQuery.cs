using MediatR;
using YatirimKoc.Application.Features.SiteSettings.Dtos;

namespace YatirimKoc.Application.Features.SiteSettings.Queries
{
    public record GetSiteSettingsQuery : IRequest<SiteSettingsDto>;

}
