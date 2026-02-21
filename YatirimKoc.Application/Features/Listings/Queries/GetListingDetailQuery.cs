using MediatR;
using Microsoft.EntityFrameworkCore;
using YatirimKoc.Application.Abstractions.Persistence;
using YatirimKoc.Domain.Entities.Listings;

namespace YatirimKoc.Application.Features.Listings.Queries;

public class GetListingDetailQuery : IRequest<Listing?>
{
    public Guid Id { get; set; }
}

public class GetListingDetailQueryHandler : IRequestHandler<GetListingDetailQuery, Listing?>
{
    private readonly IApplicationDbContext _context;

    public GetListingDetailQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Listing?> Handle(GetListingDetailQuery request, CancellationToken cancellationToken)
    {
        return await _context.Listings
            .Include(x => x.TransactionType)
            .Include(x => x.PropertyType)
            .Include(x => x.Images)
            .Include(x => x.FeatureValues)
                .ThenInclude(fv => fv.Feature) // Özelliklerin isimlerini (Oda Sayısı vb.) almak için
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
    }
}