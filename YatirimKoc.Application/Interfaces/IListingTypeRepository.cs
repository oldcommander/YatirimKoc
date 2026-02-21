using YatirimKoc.Domain.Entities;
using YatirimKoc.Domain.Entities.Listings;

namespace YatirimKoc.Application.Interfaces;

public interface IListingTypeRepository
{
    Task<List<ListingType>> GetAllAsync();
    Task<ListingType> GetByIdAsync(Guid id);
}
