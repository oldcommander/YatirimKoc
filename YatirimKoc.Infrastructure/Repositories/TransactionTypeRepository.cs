using YatirimKoc.Application.Abstractions.Persistence;
using YatirimKoc.Application.Interfaces;
using YatirimKoc.Domain.Entities.Listings;
using YatirimKoc.Infrastructure.Persistence;

namespace YatirimKoc.Infrastructure.Repositories
{
    public class TransactionTypeRepository : Repository<TransactionType>, ITransactionTypeRepository
    {
        public TransactionTypeRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}