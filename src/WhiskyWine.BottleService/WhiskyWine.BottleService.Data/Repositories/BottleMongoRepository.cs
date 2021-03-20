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
    /// <summary>
    /// The repository class used to perform CRUD operations with bottle data to mongodb.
    /// </summary>
    public class BottleMongoRepository : IRepository<BottleDomainModel>
    {
        private readonly IMongoDbContext<BottleMongoModel> _dbContext;
        private readonly IMapper<BottleDomainModel, BottleMongoModel> _toMongoMapper;
        private readonly IMapper<BottleMongoModel, BottleDomainModel> _toDomainMapper;

        /// <summary>
        /// Constructs an instance of the BottleMongoRepository class.
        /// </summary>
        /// <param name="dbContext">An instance of class implementing the IMongoDbContext interface with generic type parameter BottleMongoBottle.</param>
        /// <param name="toMongoMapper">An instance of class implementing the IMapper interface with generic type parameters Bottle, BottleMongoModel.</param>
        /// <param name="toDomainMapper">An instance of class implementing the IMapper interface with generic type parameters BottleMongoModel, Bottle.</param>
        public BottleMongoRepository(IMongoDbContext<BottleMongoModel> dbContext,
            IMapper<BottleDomainModel, BottleMongoModel> toMongoMapper,
            IMapper<BottleMongoModel, BottleDomainModel> toDomainMapper)
        {
            this._dbContext = dbContext;
            this._toMongoMapper = toMongoMapper;
            this._toDomainMapper = toDomainMapper;
        }

        /// <summary>
        /// Inserts a bottle into the dbcontext mongo collection after mapping it to a BottleMongoModel.
        /// </summary>
        /// <param name="entity">The Bottle entity to insert.</param>
        /// <returns>Task of Bottle, containing the Bottle that has been inserted.</returns>
        public async Task<BottleDomainModel> InsertAsync(BottleDomainModel entity)
        {
            var mongoModel = _toMongoMapper.MapOne(entity);
           
            await _dbContext.Collection.InsertOneAsync(mongoModel);
            return _toDomainMapper.MapOne(mongoModel);
        }

        /// <summary>
        /// Gets a bottle from the mongodb collection.
        /// </summary>
        /// <param name="id">The id of the bottle to get.</param>
        /// <returns>Task of Bottle containing the Bottle that has been returned from the collection, or null if bottle not returned.</returns>
        public async Task<BottleDomainModel> GetByIdAsync(string id)
        {
            var idValid = ObjectId.TryParse(id, out var objectId);
            //If id passed is not a valid ObjectId the retrieval cannot proceed since no matching record will be found.
            if (!idValid) return null;
            
            var mongoModel = (await _dbContext.Collection.FindAsync(bottle => bottle.BottleId == objectId)).FirstOrDefault();
            return _toDomainMapper.MapOne(mongoModel);
        }

        /// <summary>
        /// Gets all bottles from the mongodb collection.
        /// </summary>
        /// <returns>Task of IEnumerable containing the Bottles returned from the collection.</returns>
        public async Task<IEnumerable<BottleDomainModel>> GetAllAsync()
        {
            var mongoModelList = (await _dbContext.Collection.FindAsync(c => true)).ToList();

            var domainModelList = _toDomainMapper.MapMany(mongoModelList);
            return domainModelList;
        }

        /// <summary>
        /// Update-Replaces an existing bottle in the mongodb collection.
        /// </summary>
        /// <param name="id">The id of the bottle to update.</param>
        /// <param name="entity">The new bottle to associate to the id.</param>
        public async Task UpdateAsync(string id, BottleDomainModel entity)
        {
            var idValid = ObjectId.TryParse(id, out var objectId);
            //If id passed is not a valid ObjectId the update cannot proceed since no matching record will be found.
            if (!idValid) return;

            var mongoModel = _toMongoMapper.MapOne(entity);

            await _dbContext.Collection.ReplaceOneAsync(bottle => bottle.BottleId == objectId, mongoModel);
        }

        /// <summary>
        /// Deletes a bottle in the mongodb collection.
        /// </summary>
        /// <param name="id">The id of the bottle to delete.</param>
        /// <returns>Task of boolean, true if deletion successful, false if not.</returns>
        public async Task<bool> DeleteAsync(string id)
        {
            var idValid = ObjectId.TryParse(id, out var objectId);
            //If id passed is not a valid ObjectId the deletion cannot proceed since no matching record will be found.
            if (!idValid) return false;

            var deleteResult = await _dbContext.Collection.DeleteOneAsync(c => c.BottleId == objectId);
            
            //Check that mongo has acknowledged the request, and an item has actually been deleted. Return false if not.
            if (deleteResult.IsAcknowledged == false || deleteResult.DeletedCount == 0) return false;
            return true;
        }
    }
}
