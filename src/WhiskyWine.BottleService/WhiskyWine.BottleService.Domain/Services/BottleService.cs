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

        public BottleService(IRepository<Bottle> repository)
        {
            this._repository = repository;
        }

        public async Task<Bottle> GetBottleAsync(string bottleId)
        {
            return await this._repository.GetByIdAsync(bottleId);
        }

        public async Task<IEnumerable<Bottle>> GetAllBottlesAsync()
        {
            return await this._repository.GetAllAsync();
        }

        public async Task<Bottle> PostBottleAsync(Bottle bottle)
        {
            return await this._repository.InsertAsync(bottle);
        }

        public async Task UpdateBottleAsync(string bottleId, Bottle bottle)
        {
            await this._repository.UpdateAsync(bottleId, bottle);
        }

        public async Task<bool> DeleteBottleAsync(string bottleId)
        {
            return await this._repository.DeleteAsync(bottleId);
        }
    }
}
