using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Threading.Tasks;
using WhiskyWine.BottleService.Domain.Interfaces;
using WhiskyWine.BottleService.Domain.Models;

namespace WhiskyWine.BottleService.Domain.Services
{
    public class BottleService : IBottleService
    {
        private readonly IRepository<Bottle> _repository;
        private readonly IReadThroughCache<Bottle> _cache;

        public BottleService(IReadThroughCache<Bottle> cache, IRepository<Bottle> repository)
        {
            this._repository = repository;
            this._cache = cache;
        }

        public async Task<Bottle> GetBottle(int bottleId)
        {
            return await _cache.GetOrCreateCacheEntry(bottleId, () => _repository.GetById(bottleId));
        }

        public async Task<IEnumerable<Bottle>> GetAllBottles()
        {
            return await _repository.GetAll();
        }

        public async Task<Bottle> PostBottle(Bottle bottle)
        {
            return await _repository.Insert(bottle);
        }

        public async Task<Bottle> UpdateBottle(int bottleId, Bottle bottle)
        {
            //Add method in cache to delete from cache, will be celled here and in delete method
            bottle.BottleId = bottleId;
            return await _repository.Update(bottleId, bottle);
        }

        public async Task<bool> DeleteBottle()
        {
            throw new System.NotImplementedException();
        }
    }
}
