using MediatR;
using YatirimKoc.Application.Abstractions.Persistence;

namespace YatirimKoc.Application.Features.MailTemplates.Commands;

public class UpdateMailTemplateCommand : IRequest<bool>
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string Subject { get; set; } = null!;
    public string BodyHtml { get; set; } = null!;
    public bool IsActive { get; set; }
}

public class UpdateMailTemplateCommandHandler : IRequestHandler<UpdateMailTemplateCommand, bool>
{
    private readonly IApplicationDbContext _context;
    public UpdateMailTemplateCommandHandler(IApplicationDbContext context) { _context = context; }

    public async Task<bool> Handle(UpdateMailTemplateCommand request, CancellationToken cancellationToken)
    {
        var template = await _context.MailTemplates.FindAsync(new object[] { request.Id }, cancellationToken);
        if (template == null) return false;

        template.Code = request.Code;
        template.Subject = request.Subject;
        template.BodyHtml = request.BodyHtml;
        template.IsActive = request.IsActive;

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}