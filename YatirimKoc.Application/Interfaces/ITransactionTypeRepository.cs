using YatirimKoc.Application.Abstractions.Persistence;
using YatirimKoc.Domain.Entities.Listings;

namespace YatirimKoc.Application.Interfaces
{
    public interface ITransactionTypeRepository : IRepository<TransactionType>
    {
    }
}