using MediatR;
using YatirimKoc.Application.Abstractions.Persistence;
using YatirimKoc.Domain.Entities.Mails;

namespace YatirimKoc.Application.Features.Mails.Commands;

public class CreateMailCommand : IRequest<Guid>
{
    public string Code { get; set; } = null!;
    public string Subject { get; set; } = null!;
    public string BodyHtml { get; set; } = null!;
    public bool IsActive { get; set; }
}

public class CreateMailCommandHandler : IRequestHandler<CreateMailCommand, Guid>
{
    private readonly IApplicationDbContext _context;
    public CreateMailCommandHandler(IApplicationDbContext context) { _context = context; }

    public async Task<Guid> Handle(CreateMailCommand request, CancellationToken cancellationToken)
    {
        var mail = new YatirimKoc.Domain.Entities.Mails.Mails
        {
            Code = request.Code,
            Subject = request.Subject,
            BodyHtml = request.BodyHtml,
            IsActive = request.IsActive,
            CreatedAt = DateTime.UtcNow
        };

        _context.Mails.Add(mail);
        await _context.SaveChangesAsync(cancellationToken);
        return mail.Id;
    }
}