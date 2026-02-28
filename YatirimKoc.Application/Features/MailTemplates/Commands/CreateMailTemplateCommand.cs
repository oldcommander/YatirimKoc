using MediatR;
using YatirimKoc.Application.Abstractions.Persistence;
using YatirimKoc.Domain.Entities.Mails;

namespace YatirimKoc.Application.Features.MailTemplates.Commands;

public class CreateMailTemplateCommand : IRequest<Guid>
{
    public string Code { get; set; } = null!;
    public string Subject { get; set; } = null!;
    public string BodyHtml { get; set; } = null!;
    public bool IsActive { get; set; }
}

public class CreateMailTemplateCommandHandler : IRequestHandler<CreateMailTemplateCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    public CreateMailTemplateCommandHandler(IApplicationDbContext context) { _context = context; }

    public async Task<Guid> Handle(CreateMailTemplateCommand request, CancellationToken cancellationToken)
    {
        var template = new MailTemplate
        {
            Code = request.Code,
            Subject = request.Subject,
            BodyHtml = request.BodyHtml,
            IsActive = request.IsActive,
            CreatedAt = DateTime.UtcNow
        };

        _context.MailTemplates.Add(template);
        await _context.SaveChangesAsync(cancellationToken);
        return template.Id;
    }
}