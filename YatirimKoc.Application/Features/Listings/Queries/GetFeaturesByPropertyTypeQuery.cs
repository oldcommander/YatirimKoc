using MediatR;
using Microsoft.EntityFrameworkCore;
using YatirimKoc.Application.Abstractions.Persistence;

namespace YatirimKoc.Application.Features.Listings.Queries;

// API'ye döndüreceğimiz hafif DTO modeli
public class FeatureDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string InputType { get; set; } = null!;
    public string? Options { get; set; }
    public bool IsRequired { get; set; }
}

public class GetFeaturesByPropertyTypeQuery : IRequest<List<FeatureDto>>
{
    public Guid PropertyTypeId { get; set; }
}

public class GetFeaturesByPropertyTypeQueryHandler : IRequestHandler<GetFeaturesByPropertyTypeQuery, List<FeatureDto>>
{
    private readonly IApplicationDbContext _context;

    public GetFeaturesByPropertyTypeQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<FeatureDto>> Handle(GetFeaturesByPropertyTypeQuery request, CancellationToken cancellationToken)
    {
        return await _context.PropertyFeatures
            .Include(pf => pf.Feature)
            .Where(pf => pf.PropertyTypeId == request.PropertyTypeId && pf.Feature.IsActive)
            .OrderBy(pf => pf.Order)
            .Select(pf => new FeatureDto
            {
                Id = pf.FeatureId,
                Name = pf.Feature.Name,
                InputType = pf.Feature.InputType, // text, number, select
                Options = pf.Feature.Options,
                IsRequired = pf.IsRequired
            })
            .ToListAsync(cancellationToken);
    }
}