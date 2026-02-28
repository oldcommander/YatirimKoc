using MediatR;
using Microsoft.EntityFrameworkCore;
using YatirimKoc.Application.Abstractions.Persistence;
using YatirimKoc.Application.Common.Models;
using YatirimKoc.Application.Features.MailTemplates.Dtos;

namespace YatirimKoc.Application.Features.MailTemplates.Queries;

public class GetAllMailTemplatesQuery : IRequest<PaginatedList<MailTemplateDto>>
{
    public string? SearchTerm { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetAllMailTemplatesQueryHandler : IRequestHandler<GetAllMailTemplatesQuery, PaginatedList<MailTemplateDto>>
{
    private readonly IApplicationDbContext _context;

    public GetAllMailTemplatesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<MailTemplateDto>> Handle(GetAllMailTemplatesQuery request, CancellationToken cancellationToken)
    {
        var query = _context.MailTemplates.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var search = request.SearchTerm.ToLower();
            query = query.Where(x => x.Code.ToLower().Contains(search) || x.Subject.ToLower().Contains(search));
        }

        var count = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(x => x.Code)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(x => new MailTemplateDto
            {
                Id = x.Id,
                Code = x.Code,
                Subject = x.Subject,
                IsActive = x.IsActive,
                CreatedAt = x.CreatedAt
            }).ToListAsync(cancellationToken);

        return new PaginatedList<MailTemplateDto>(items, count, request.PageNumber, request.PageSize);
    }
}