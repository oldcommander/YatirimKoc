using MediatR;
using YatirimKoc.Application.Abstractions.Persistence;

namespace YatirimKoc.Application.Features.MailTemplates.Commands;

public class DeleteMailTemplateCommand : IRequest<bool>
{
    public Guid Id { get; set; }
    public DeleteMailTemplateCommand(Guid id) { Id = id; }
}

public class DeleteMailTemplateCommandHandler : IRequestHandler<DeleteMailTemplateCommand, bool>
{
    private readonly IApplicationDbContext _context;
    public DeleteMailTemplateCommandHandler(IApplicationDbContext context) { _context = context; }

    public async Task<bool> Handle(DeleteMailTemplateCommand request, CancellationToken cancellationToken)
    {
        var template = await _context.MailTemplates.FindAsync(new object[] { request.Id }, cancellationToken);
        if (template == null) return false;

        _context.MailTemplates.Remove(template);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}