using MediatR;
using Microsoft.EntityFrameworkCore;
using YatirimKoc.Application.Abstractions.Persistence;

namespace YatirimKoc.Application.Features.Settings.Commands.Update;

public class UpdateSiteSettingsCommandHandler
    : IRequestHandler<UpdateSiteSettingsCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateSiteSettingsCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        UpdateSiteSettingsCommand request,
        CancellationToken cancellationToken
    )
    {
        var setting = await _context.SiteSettings
            .FirstOrDefaultAsync(x => x.Key == request.Key, cancellationToken);

        if (setting is null)
            throw new KeyNotFoundException(
                $"Site setting bulunamadı: {request.Key}"
            );

        setting.Value = request.Value;
        setting.IsActive = request.IsActive;
        setting.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync(cancellationToken);
    }
}
