using MediatR;
using Microsoft.EntityFrameworkCore;
using YatirimKoc.Application.Abstractions.Persistence;
using YatirimKoc.Application.Features.Mails.Dtos;

namespace YatirimKoc.Application.Features.Mails.Queries;

public class GetMailByIdQuery : IRequest<MailDto?>
{
    public Guid Id { get; set; }
    public GetMailByIdQuery(Guid id) { Id = id; }
}

public class GetMailByIdQueryHandler : IRequestHandler<GetMailByIdQuery, MailDto?>
{
    private readonly IApplicationDbContext _context;

    public GetMailByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<MailDto?> Handle(GetMailByIdQuery request, CancellationToken cancellationToken)
    {
        var mail = await _context.Mails
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (mail == null) return null;

        return new MailDto
        {
            Id = mail.Id,
            Code = mail.Code,
            Subject = mail.Subject,
            BodyHtml = mail.BodyHtml,
            IsActive = mail.IsActive,
            CreatedAt = mail.CreatedAt
        };
    }
}   