using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhiskyWine.BottleService.Domain.Interfaces
{
    public interface IBottleServiceDatabaseSettings
    {
        string BottlesCollectionName { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}
