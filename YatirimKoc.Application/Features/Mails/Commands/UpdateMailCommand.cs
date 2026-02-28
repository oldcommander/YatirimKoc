using MediatR;
using YatirimKoc.Application.Abstractions.Persistence;

namespace YatirimKoc.Application.Features.Mails.Commands;

public class UpdateMailCommand : IRequest<bool>
{
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string Subject { get; set; } = null!;
    public string BodyHtml { get; set; } = null!;
    public bool IsActive { get; set; }
}

public class UpdateMailCommandHandler : IRequestHandler<UpdateMailCommand, bool>
{
    private readonly IApplicationDbContext _context;
    public UpdateMailCommandHandler(IApplicationDbContext context) { _context = context; }

    public async Task<bool> Handle(UpdateMailCommand request, CancellationToken cancellationToken)
    {
        var mail = await _context.Mails.FindAsync(new object[] { request.Id }, cancellationToken);
        if (mail == null) return false;

        mail.Code = request.Code;
        mail.Subject = request.Subject;
        mail.BodyHtml = request.BodyHtml;
        mail.IsActive = request.IsActive;

        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}