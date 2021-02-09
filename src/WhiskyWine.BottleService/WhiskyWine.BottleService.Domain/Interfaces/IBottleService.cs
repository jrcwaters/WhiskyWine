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
        Task<Bottle> GetBottle(int bottleId);

        Task<IEnumerable<Bottle>> GetAllBottles();

        Task<Bottle> PostBottle(Bottle bottle);

        Task<Bottle> UpdateBottle(int bottleId, Bottle bottle);

        Task<bool> DeleteBottle();
    }
}
