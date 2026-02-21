using YatirimKoc.Application.Abstractions.Persistence;
using YatirimKoc.Application.Interfaces;
using YatirimKoc.Domain.Entities.Listings;
using YatirimKoc.Infrastructure.Persistence;

namespace YatirimKoc.Infrastructure.Repositories
{
    public class PropertyTypeRepository : Repository<PropertyType>, IPropertyTypeRepository
    {
        public PropertyTypeRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}