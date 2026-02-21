using Microsoft.EntityFrameworkCore;
using YatirimKoc.Application.Interfaces;
using YatirimKoc.Domain.Entities;
using YatirimKoc.Domain.Entities.Listings;
using YatirimKoc.Infrastructure.Persistence;

namespace YatirimKoc.Infrastructure.Repositories;

public class ListingCategoryRepository : IListingCategoryRepository
{
    private readonly ApplicationDbContext _context;

    public ListingCategoryRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<ListingCategory>> GetAllAsync()
    {
        return await _context.ListingCategories.AsNoTracking().ToListAsync();
    }

    public async Task<ListingCategory> GetByIdAsync(Guid id)
    {
        return await _context.ListingCategories.FindAsync(id);
    }
}
