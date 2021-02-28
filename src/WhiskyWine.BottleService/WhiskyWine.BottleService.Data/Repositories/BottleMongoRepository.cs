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
        private readonly IMongoDbContext<BottleMongoModel> _dbContext;
        private readonly IMapper<Bottle, BottleMongoModel> _toMongoMapper;
        private readonly IMapper<BottleMongoModel, Bottle> _toDomainMapper;

        public BottleMongoRepository(IMongoDbContext<BottleMongoModel> dbContext,
            IMapper<Bottle, BottleMongoModel> toMongoMapper,
            IMapper<BottleMongoModel, Bottle> toDomainMapper)
        {
            this._dbContext = dbContext;
            this._toMongoMapper = toMongoMapper;
            this._toDomainMapper = toDomainMapper;
        }

        public async Task<Bottle> InsertAsync(Bottle entity)
        {
            var mongoModel = _toMongoMapper.Map(entity);
           
            await _dbContext.Collection.InsertOneAsync(mongoModel);
            return _toDomainMapper.Map(mongoModel);
        }

        public async Task<Bottle> GetByIdAsync(string id)
        {
            var idValid = ObjectId.TryParse(id, out var objectId);
            if (!idValid) return null;
            
            var mongoModel = (await _dbContext.Collection.FindAsync(bottle => bottle.BottleId == objectId)).FirstOrDefault();
            return _toDomainMapper.Map(mongoModel);
        }

        public async Task<IEnumerable<Bottle>> GetAllAsync()
        {
            var mongoModelList = (await _dbContext.Collection.FindAsync(c => true)).ToList();

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

            await _dbContext.Collection.ReplaceOneAsync(bottle => bottle.BottleId == objectId, mongoModel);
        }

        public async Task<bool> DeleteAsync(string id)
        {
            var idValid = ObjectId.TryParse(id, out var objectId);
            if (!idValid) return false;

            var deleteResult = await _dbContext.Collection.DeleteOneAsync(c => c.BottleId == objectId);
            if (deleteResult.IsAcknowledged == false) return false;
            if (deleteResult.DeletedCount == 0) return false;
            return true;
        }
    }
}
