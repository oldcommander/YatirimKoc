using System.Collections.Generic;
using System.Threading.Tasks;

namespace YatirimKoc.Application.Abstractions.Persistence
{
    public interface IRepository<T> where T : class
    {
        Task<List<T>> GetAllAsync();
    }
}