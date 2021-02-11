using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhiskyWine.BottleService.Domain.Interfaces
{
    public interface IReadThroughCache<T>
    {
        Task<T> GetOrCreateCacheEntry(int id, Func<Task<T>> createCacheEntry);
    }
}
