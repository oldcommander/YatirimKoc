using MediatR;
using Microsoft.EntityFrameworkCore;
using YatirimKoc.Application.Abstractions.Persistence;
using YatirimKoc.Application.Common.Models; // Yeni eklediğimiz model
using YatirimKoc.Domain.Entities.Listings;

namespace YatirimKoc.Application.Features.Listings.Queries;

public class GetAllListingsQuery : IRequest<PaginatedList<Listing>>
{
    public string? SearchTerm { get; set; }
    public Guid? TransactionTypeId { get; set; }
    public Guid? PropertyTypeId { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10; // Her sayfada 10 ilan
}

public class GetAllListingsQueryHandler : IRequestHandler<GetAllListingsQuery, PaginatedList<Listing>>
{
    private readonly IApplicationDbContext _context;

    public GetAllListingsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<Listing>> Handle(GetAllListingsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.Listings
            .Include(x => x.TransactionType)
            .Include(x => x.PropertyType)
            .Include(x => x.Images)
            .AsNoTracking() // Performans için
            .AsQueryable();

        // Filtreler
        if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            query = query.Where(x => x.Title.Contains(request.SearchTerm));

        if (request.TransactionTypeId.HasValue)
            query = query.Where(x => x.TransactionTypeId == request.TransactionTypeId);

        if (request.PropertyTypeId.HasValue)
            query = query.Where(x => x.PropertyTypeId == request.PropertyTypeId);

        var count = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken);

        return new PaginatedList<Listing>(items, count, request.PageNumber, request.PageSize);
    }
}