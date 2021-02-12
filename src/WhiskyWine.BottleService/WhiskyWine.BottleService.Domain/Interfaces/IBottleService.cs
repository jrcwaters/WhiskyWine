using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiskyWine.BottleService.Domain.Models;

namespace WhiskyWine.BottleService.Domain.Interfaces
{
    public interface IBottleService
    {
        Task<Bottle> GetBottleAsync(string bottleId);

        Task<IEnumerable<Bottle>> GetAllBottlesAsync();

        Task<Bottle> PostBottleAsync(Bottle bottle);

        Task UpdateBottleAsync(string bottleId, Bottle bottle);

        Task<bool> DeleteBottleAsync();
    }
}
