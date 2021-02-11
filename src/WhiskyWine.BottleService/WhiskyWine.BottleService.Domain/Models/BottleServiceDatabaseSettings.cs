using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiskyWine.BottleService.Domain.Interfaces;

namespace WhiskyWine.BottleService.Domain.Models
{
    public class BottleServiceDatabaseSettings : IBottleServiceDatabaseSettings
    {
        public string BottlesCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}
