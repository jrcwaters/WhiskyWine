using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiskyWine.BottleService.Domain.Interfaces;
using WhiskyWine.BottleService.Domain.Models;

namespace WhiskyWine.BottleService.Data.Caches
{
    public class BottleMemoryCache : IReadThroughCache<Bottle>
    {
        private MemoryCache _memoryCache = new MemoryCache(
            new MemoryCacheOptions
            {
                SizeLimit = 1024
            });

        public async Task<Bottle> GetOrCreateCacheEntry(int id, Func<Task<Bottle>> retrieveNewEntry)
        {
            Bottle cacheEntry;
            var isInCache = _memoryCache.TryGetValue(id, out cacheEntry);

            if (!isInCache)
            {
                cacheEntry = await retrieveNewEntry();
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    //Set the size of the entry, relative to the MemoryCacheOptions.SizeLimit
                    .SetSize(1)
                    //Set time after entry removed from cache
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(60));

                _memoryCache.Set(id, cacheEntry, cacheEntryOptions);
            }

            return cacheEntry;
        }
    }
}
