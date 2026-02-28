using MediatR;
using Microsoft.EntityFrameworkCore;
using YatirimKoc.Application.Abstractions.Persistence;
using YatirimKoc.Application.Common.Models;
using YatirimKoc.Application.Features.Mails.Dtos;

namespace YatirimKoc.Application.Features.Mails.Queries;

public class GetAllMailsQuery : IRequest<PaginatedList<MailDto>>
{
    public string? SearchTerm { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class GetAllMailsQueryHandler : IRequestHandler<GetAllMailsQuery, PaginatedList<MailDto>>
{
    private readonly IApplicationDbContext _context;

    public GetAllMailsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<MailDto>> Handle(GetAllMailsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Mails.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
        {
            var search = request.SearchTerm.ToLower();
            // Entity'deki alanınız Email veya EmailAddress olabilir. Gerekirse burayı ona göre güncelleyin.
            query = query.Where(x => x.Email.ToLower().Contains(search));
        }

        var count = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(x => new MailDto
            {
                Id = x.Id,
                Email = x.Email,
                IsActive = x.IsActive,
                CreatedAt = x.CreatedAt
            }).ToListAsync(cancellationToken);

        return new PaginatedList<MailDto>(items, count, request.PageNumber, request.PageSize);
    }
}