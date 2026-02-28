using MediatR;
using Microsoft.EntityFrameworkCore;
using YatirimKoc.Application.Abstractions.Persistence;
using YatirimKoc.Application.Features.MailTemplates.Dtos;

namespace YatirimKoc.Application.Features.MailTemplates.Queries;

public class GetMailTemplateByIdQuery : IRequest<MailTemplateDto?>
{
    public Guid Id { get; set; }
    public GetMailTemplateByIdQuery(Guid id) { Id = id; }
}

public class GetMailTemplateByIdQueryHandler : IRequestHandler<GetMailTemplateByIdQuery, MailTemplateDto?>
{
    private readonly IApplicationDbContext _context;

    public GetMailTemplateByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<MailTemplateDto?> Handle(GetMailTemplateByIdQuery request, CancellationToken cancellationToken)
    {
        var template = await _context.MailTemplates
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (template == null) return null;

        return new MailTemplateDto
        {
            Id = template.Id,
            Code = template.Code,
            Subject = template.Subject,
            BodyHtml = template.BodyHtml,
            IsActive = template.IsActive,
            CreatedAt = template.CreatedAt
        };
    }
}