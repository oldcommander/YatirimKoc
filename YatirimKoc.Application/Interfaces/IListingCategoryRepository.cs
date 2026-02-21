using YatirimKoc.Domain.Entities;
using YatirimKoc.Domain.Entities.Listings;

namespace YatirimKoc.Application.Interfaces;

public interface IListingCategoryRepository
{
    Task<List<ListingCategory>> GetAllAsync();
    Task<ListingCategory> GetByIdAsync(Guid id);
}
