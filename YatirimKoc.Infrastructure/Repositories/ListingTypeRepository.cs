using Microsoft.EntityFrameworkCore;
using YatirimKoc.Application.Interfaces;
using YatirimKoc.Domain.Entities;
using YatirimKoc.Domain.Entities.Listings;
using YatirimKoc.Infrastructure.Persistence;

namespace YatirimKoc.Infrastructure.Repositories;

public class ListingTypeRepository : IListingTypeRepository
{
    private readonly ApplicationDbContext _context;

    public ListingTypeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<ListingType>> GetAllAsync()
    {
        return await _context.ListingTypes.AsNoTracking().ToListAsync();
    }

    public async Task<ListingType> GetByIdAsync(Guid id)
    {
        return await _context.ListingTypes.FindAsync(id);
    }
}
