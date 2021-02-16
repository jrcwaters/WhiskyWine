using System.Collections.Generic;
using System.Threading.Tasks;

namespace WhiskyWine.BottleService.Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T> InsertAsync(T entity);
        Task<T> GetByIdAsync(string id);
        Task<IEnumerable<T>> GetAllAsync();
        Task UpdateAsync(string id, T entity);
        Task DeleteAsync(string id);
    }
}
