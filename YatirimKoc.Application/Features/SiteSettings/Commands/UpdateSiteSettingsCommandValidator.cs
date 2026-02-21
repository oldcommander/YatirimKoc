using FluentValidation;

namespace YatirimKoc.Application.Features.Settings.Commands.Update;

public class UpdateSiteSettingsCommandValidator
    : AbstractValidator<UpdateSiteSettingsCommand>
{
    public UpdateSiteSettingsCommandValidator()
    {
        RuleFor(x => x.Key)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.Value)
            .NotEmpty()
            .MaximumLength(1000);
    }
}
