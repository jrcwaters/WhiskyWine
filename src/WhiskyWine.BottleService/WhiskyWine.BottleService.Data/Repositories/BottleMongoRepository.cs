using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WhiskyWine.BottleService.Data.Models;
using WhiskyWine.BottleService.Domain.Interfaces;
using WhiskyWine.BottleService.Domain.Models;

namespace WhiskyWine.BottleService.Data.Repositories
{
    public class BottleMongoRepository : IRepository<Bottle>
    {
        private readonly IMongoCollection<BottleMongoModel> _bottles;
        private readonly IMapper<Bottle, BottleMongoModel> _toMongoMapper;
        private readonly IMapper<BottleMongoModel, Bottle> _toDomainMapper;

        public BottleMongoRepository(IBottleServiceDatabaseSettings settings,
            IMapper<Bottle, BottleMongoModel> toMongoMapper,
            IMapper<BottleMongoModel, Bottle> toDomainMapper)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);
            
            this._bottles = database.GetCollection<BottleMongoModel>(settings.BottlesCollectionName);
            this._toMongoMapper = toMongoMapper;
            this._toDomainMapper = toDomainMapper;
        }

        public async Task<Bottle> InsertAsync(Bottle entity)
        {
            var mongoModel = _toMongoMapper.Map(entity);
           
            await _bottles.InsertOneAsync(mongoModel);
            return _toDomainMapper.Map(mongoModel);
        }

        public async Task<Bottle> GetByIdAsync(string id)
        {
            var idValid = ObjectId.TryParse(id, out var objectId);
            if (!idValid) return null;
            
            var mongoModel = (await _bottles.FindAsync(bottle => bottle.BottleId == objectId)).FirstOrDefault();
            return _toDomainMapper.Map(mongoModel);
        }

        public async Task<IEnumerable<Bottle>> GetAllAsync()
        {
            var mongoModelList = (await _bottles.FindAsync(c => true)).ToList();

            var domainModelList = new List<Bottle>();
            foreach (var mongoModel in mongoModelList)
            {
                domainModelList.Add(_toDomainMapper.Map(mongoModel));
            }
            return domainModelList;
        }

        public async Task UpdateAsync(string id, Bottle entity)
        {
            var idValid = ObjectId.TryParse(id, out var objectId);
            if (!idValid) return;

            var mongoModel = _toMongoMapper.Map(entity);

            await _bottles.ReplaceOneAsync(bottle => bottle.BottleId == objectId, mongoModel);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var idValid = ObjectId.TryParse(id, out var objectId);
            if (!idValid) return false;
            return (await _bottles.DeleteOneAsync(c => c.BottleId == objectId)).IsAcknowledged;
        }
    }
}
