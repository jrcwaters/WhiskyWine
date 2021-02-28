using MongoDB.Driver;
using System.Collections.Generic;

namespace WhiskyWine.BottleService.Domain.Interfaces
{
    public interface IMongoDbContext<T>
    {
        public IMongoCollection<T> Collection { get; }
    }
}
