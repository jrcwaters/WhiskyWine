using System.Collections.Generic;
using System.Threading.Tasks;

namespace WhiskyWine.BottleService.Domain.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<T> Insert(T entity);
        Task<T> GetById(int id);
        Task<IEnumerable<T>> GetAll();
        Task<T> Update(int id, T entity);
        Task<bool> Delete(int id);
    }
}
