using MediatR;
using Microsoft.EntityFrameworkCore;
using YatirimKoc.Application.Abstractions.Persistence;
using YatirimKoc.Domain.Entities.Listings;

public class GetAllListingsQuery : IRequest<List<Listing>>
{
}

public class GetAllListingsQueryHandler
    : IRequestHandler<GetAllListingsQuery, List<Listing>>
{
    private readonly IApplicationDbContext _context;

    public GetAllListingsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Listing>> Handle(
            GetAllListingsQuery request,
            CancellationToken cancellationToken)
    {
        return await _context.Listings
            .Include(x => x.TransactionType) // Değişti
            .Include(x => x.PropertyType)    // Değişti
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}
