using MediatR;
using Microsoft.EntityFrameworkCore;
using YatirimKoc.Application.Abstractions.Persistence; // YENİ: Doğru referans
using YatirimKoc.Domain.Entities.Listings;

namespace YatirimKoc.Application.Features.Listings.Queries;

public class GetAllListingsQuery : IRequest<List<Listing>>
{
}

public class GetAllListingsQueryHandler : IRequestHandler<GetAllListingsQuery, List<Listing>>
{
    private readonly IApplicationDbContext _context; // YENİ: Interface kullanıyoruz

    public GetAllListingsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Listing>> Handle(GetAllListingsQuery request, CancellationToken cancellationToken)
    {
        return await _context.Listings
            .Include(x => x.TransactionType)
            .Include(x => x.PropertyType)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}