using MediatR;

namespace YatirimKoc.Application.Features.Settings.Commands.Update;

public record UpdateSiteSettingsCommand(
    string Key,
    string Value,
    bool IsActive
) : IRequest;
