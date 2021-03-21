using System.Collections.Generic;
using System.Threading;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using WhiskyWine.BottleService.Data.Models;
using WhiskyWine.BottleService.Data.Repositories;
using WhiskyWine.BottleService.Domain.Interfaces;
using WhiskyWine.BottleService.Domain.Models;
using WhiskyWine.BottleService.Data.Mappers;
using System.Linq;
using WhiskyWine.BottleService.Domain.Enums;

namespace WhiskyWine.BottleService.Data.UnitTests.Repositories
{
    /// <summary>
    /// The unit test class for the BottleMongoRepository.
    /// </summary>
    public class BottleMongoRepositoryTests
    {
        /// <summary>
        /// Mocked out DB context.
        /// </summary>
        private Mock<IMongoDbContext<BottleMongoModel>> _mockDbContext;

        /// <summary>
        /// Domain to Mongo model mapper.
        /// Not mocked since setting up the mock would require basically the same code as the mapper, since mapper just parses and transfers properties between objects.
        /// </summary>
        private IMapper<BottleDomainModel, BottleMongoModel> _toMongoModelMapper;

        /// <summary>
        /// Mongo to Domain model mapper.
        /// Not mocked since setting up the mock would require basically the same code as the mapper, since mapper just parses and transfers properties between objects.
        /// </summary>
        private IMapper<BottleMongoModel, BottleDomainModel> _toDomainModelMapper;

        /// <summary>
        /// SetUp mocks of all dependencies for BottleMongoRepository. Will be run before every test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            //Create mocks/ instances of the BottleMongoRepository dependencies and store in fields
            _mockDbContext = new Mock<IMongoDbContext<BottleMongoModel>>();
            _toMongoModelMapper = new DomainToMongoModelMapper();
            _toDomainModelMapper = new MongoToDomainModelMapper();
        }

        /// <summary>
        /// Test that the GetByIdAsync method returns null if the Id string passed cannot be parsed into a MongoDB.Bson.ObjectId object.
        /// </summary>
        [Test]
        public async Task GetByIdAsync_ReturnsNull_IfInvalidIdStringPassed()
        {
            //Arrange
            var repo = new BottleMongoRepository(_mockDbContext.Object, _toMongoModelMapper, _toDomainModelMapper);

            //Act
            var result = await repo.GetByIdAsync("1234");

            //Assert
            Assert.AreEqual(null, result);
        }

        /// <summary>
        /// 
        /// </summary>
        [Test]
        public async Task GetByIdAsync_ReturnsDomainBottle_WhenBottleReturnedFromCollection()
        {
            //Arrange

            //Create the Id of the object to retrieve from the mock collection
            var idString = "507f1f77bcf86cd799439011";
            var objectId = new ObjectId(idString);

            var mongoBottleReturnedByCollection = new BottleMongoModel { BottleId = objectId, Name = "bottleName" };

            var mockCursor = CreateAndSetUpMockCursor(new List<BottleMongoModel> { mongoBottleReturnedByCollection });
            var mockCollection = CreateAndSetupMockMongoCollection(mockCursor.Object);

            _mockDbContext.SetupGet(c => c.Collection).Returns(mockCollection.Object);


            //Setup the domain Bottle we expect to be returned by the method
            var expectedDomainBottle = new BottleDomainModel { BottleId = idString, Name = "bottleName" };

            var repo = new BottleMongoRepository(_mockDbContext.Object, _toMongoModelMapper, _toDomainModelMapper);

            //Act
            var result = await repo.GetByIdAsync("507f1f77bcf86cd799439010");

            //Assert
            Assert.AreEqual(expectedDomainBottle.BottleId, result.BottleId);
            Assert.AreEqual(expectedDomainBottle.Name, result.Name);

        }



        [Test]
        public async Task GetByIdAsync_ReturnsNull_WhenNoBottleIsReturnedFromCollection()
        {
            //Arrange
            var mockCursor = CreateAndSetUpMockCursor(new List<BottleMongoModel>());
            var mockCollection = CreateAndSetupMockMongoCollection(mockCursor.Object);
            _mockDbContext.SetupGet(c => c.Collection).Returns(mockCollection.Object);

            var repo = new BottleMongoRepository(_mockDbContext.Object, _toMongoModelMapper, _toDomainModelMapper);

            //Act
            var result = await repo.GetByIdAsync("507f1f77bcf86cd799439010");

            //Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetByIdAsync_RetursNull_WhenInvalidIdPassed()
        {
            //Arrange
            var repo = new BottleMongoRepository(_mockDbContext.Object, _toMongoModelMapper, _toDomainModelMapper);

            //Act
            var result = await repo.GetByIdAsync("invalidId");

            //Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetAllAsync_ReturnsListContainingAllBottles_WhenBottlesReturnedFromCollection()
        {
            //Arrange
            var mongoBottleListReturnedByCollection = new List<BottleMongoModel>
            {
                new BottleMongoModel{ BottleId = new ObjectId("507f1f77bcf86cd799439010"), Name = "bottle1"},
                new BottleMongoModel{ BottleId = new ObjectId("507f1f77bcf86cd799439011"), Name = "bottle2"}
            };

            var mockCursor = CreateAndSetUpMockCursor(mongoBottleListReturnedByCollection);
            var mockCollection = CreateAndSetupMockMongoCollection(mockCursor.Object);
            _mockDbContext.SetupGet(c => c.Collection).Returns(mockCollection.Object);

            var expectedDomainBottleList = new List<BottleDomainModel>
            {
                new BottleDomainModel{BottleId = "507f1f77bcf86cd799439010", Name = "bottle1"},
                new BottleDomainModel{BottleId = "507f1f77bcf86cd799439011", Name = "bottle2"}
            };

            var repo = new BottleMongoRepository(_mockDbContext.Object, _toMongoModelMapper, _toDomainModelMapper);

            //Act
            var result = await repo.GetAllAsync();

            //Assert
            Assert.AreEqual(expectedDomainBottleList[0].BottleId, result.ToList()[0].BottleId);
            Assert.AreEqual(expectedDomainBottleList[0].Name, result.ToList()[0].Name);

            Assert.AreEqual(expectedDomainBottleList[1].BottleId, result.ToList()[1].BottleId);
            Assert.AreEqual(expectedDomainBottleList[1].Name, result.ToList()[1].Name);

        }

        [Test]
        public async Task GetAllAsync_ReturnsEmptyList_WhenNoBottlesReturnedFromCollection()
        {
            //Arrange
            var mockCursor = CreateAndSetUpMockCursor(new List<BottleMongoModel>());
            var mockCollection = CreateAndSetupMockMongoCollection(mockCursor.Object);
            _mockDbContext.SetupGet(c => c.Collection).Returns(mockCollection.Object);

            var repo = new BottleMongoRepository(_mockDbContext.Object, _toMongoModelMapper, _toDomainModelMapper);

            //Act
            var result = await repo.GetAllAsync();

            //Assert
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task InsertAsync_ReturnsNullWithoutTryingToInsertIntoCollection_WhenNullBottleDomainModelPassed()
        {
            //Arrange
            //Set up the db context to throw if the Collection property is accessed. In this way we test that the InsertAsync method returns null without ever accessing the collection.
            _mockDbContext.Setup(c => c.Collection).Throws(new System.Exception("Test failed. MongoDbContext Collection property was accessed. InsertAsync method should return null without accessing the Collection property."));
            var repo = new BottleMongoRepository(_mockDbContext.Object, _toMongoModelMapper, _toDomainModelMapper);

            //Act
            var result = await repo.InsertAsync(null);

            //Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task InsertAsync_CallsInsertOneAsyncOnCollectionOnce_ThenReturnsEntityThatWasPassed()
        {
            //Arrange
            //Create the BottleDomainModel that we will insert into the repo. This same entity should be returned by the method on a successful run.
            var bottle = new BottleDomainModel { BottleId = "507f1f77bcf86cd799439010", AlcoholCategory = AlcoholCategory.Whisky };

            var mockCollection = new Mock<IMongoCollection<BottleMongoModel>>();
            _mockDbContext.SetupGet(c => c.Collection).Returns(mockCollection.Object);

            var repo = new BottleMongoRepository(_mockDbContext.Object, _toMongoModelMapper, _toDomainModelMapper);

            //Act
            var result = await repo.InsertAsync(bottle);

            //Assert
            //Verify that the db context Collection's InsertoneAsync method is called exactly once. 
            //Any more or less will throw meaning the test will fail.
            mockCollection.Verify(c => c.InsertOneAsync(
                   It.IsAny<BottleMongoModel>(),
                   It.IsAny<InsertOneOptions>(),
                   It.IsAny<CancellationToken>()), Times.Once());

            Assert.AreEqual(bottle.BottleId, result.BottleId);
            Assert.AreEqual(bottle.AlcoholCategory, result.AlcoholCategory);
        }

        [Test]
        public async Task UpdateAsync_ReturnsWithoutAccessingCollection_WhenInvalidIdPassed()
        {
            //Arrange
            //Set up the db context to throw if the Collection property is accessed. In this way we test that the InsertAsync method returns without ever accessing the collection.
            _mockDbContext.Setup(c => c.Collection).Throws(new System.Exception("Test failed. MongoDbContext Collection property was accessed. UpdateAsync method should return without accessing the Collection property."));

            var repo = new BottleMongoRepository(_mockDbContext.Object, _toMongoModelMapper, _toDomainModelMapper);

            //Act
            await repo.UpdateAsync("Invalid Id", new BottleDomainModel());

            //Assert - assertion is just that the method returns without accessing the db context Collection property. Test will fail if the property is accessed.
        }

        [Test]
        public async Task UpdateAsync_ReturnsWithoutAccessingCollection_WhenNullBottleDomainModelPassed()
        {
            //Arrange
            //Set up the db context to throw if the Collection property is accessed. In this way we test that the InsertAsync method returns without ever accessing the collection.
            _mockDbContext.Setup(c => c.Collection).Throws(new System.Exception("Test failed. MongoDbContext Collection property was accessed. UpdateAsync method should return without accessing the Collection property."));

            var repo = new BottleMongoRepository(_mockDbContext.Object, _toMongoModelMapper, _toDomainModelMapper);

            //Act
            await repo.UpdateAsync("507f1f77bcf86cd799439010", null);

            //Assert - assertion is just that the method returns without accessing the db context Collection property. Test will fail if the property is accessed.  
        }

        [Test]
        public async Task UpdateAsync_CallsReplaceOneAsyncOnCollectionOnce_WhenValidIdAndNonNullEntityPassed()
        {
            //Arrange
            //Create the BottleDomainModel that we will insert into the repo. This same entity should be returned by the method on a successful run.
            var bottle = new BottleDomainModel { BottleId = "507f1f77bcf86cd799439010", AlcoholCategory = AlcoholCategory.Whisky };

            var mockCollection = new Mock<IMongoCollection<BottleMongoModel>>();
            _mockDbContext.SetupGet(c => c.Collection).Returns(mockCollection.Object);

            var repo = new BottleMongoRepository(_mockDbContext.Object, _toMongoModelMapper, _toDomainModelMapper);

            //Act
            await repo.UpdateAsync("507f1f77bcf86cd799439010", bottle);

            //Assert
            //Verify that the db context Collection's InsertoneAsync method is called exactly once. 
            //Any more or less will throw meaning the test will fail.
            mockCollection.Verify(c => c.ReplaceOneAsync(
                It.IsAny<FilterDefinition<BottleMongoModel>>(),
                It.IsAny<BottleMongoModel>(),
                It.IsAny<ReplaceOptions>(),
                It.IsAny<CancellationToken>()), Times.Once());
        }

        [Test]
        public async Task DeleteAsync_ReturnsFalse_WithoutAccessingCollection_IfInvalidIdPassed()
        {
            //Arrange
            //Set up the db context to throw if the Collection property is accessed. In this way we test that the InsertAsync method returns without ever accessing the collection.
            _mockDbContext.Setup(c => c.Collection).Throws(new System.Exception("Test failed. MongoDbContext Collection property was accessed. DeleteAsync method should return without accessing the Collection property."));

            var repo = new BottleMongoRepository(_mockDbContext.Object, _toMongoModelMapper, _toDomainModelMapper);

            //Act
            var result = await repo.DeleteAsync("Invalid Id");

            //Assert - assertion is just that the method returns false without accessing the db context Collection property. Test will fail if the property is accessed.
            Assert.IsFalse(result);
        }

        private Mock<IAsyncCursor<BottleMongoModel>> CreateAndSetUpMockCursor(IEnumerable<BottleMongoModel> collectionToReturnFromCurrentProperty)
        {
            var mockCursor = new Mock<IAsyncCursor<BottleMongoModel>>();
            mockCursor.Setup(_ => _.Current).Returns(collectionToReturnFromCurrentProperty);
            mockCursor
                .SetupSequence(_ => _.MoveNext(It.IsAny<CancellationToken>()))
                .Returns(true)
                .Returns(false);
            return mockCursor;
        }

        private Mock<IMongoCollection<BottleMongoModel>> CreateAndSetupMockMongoCollection(IAsyncCursor<BottleMongoModel> cursorToReturnFromFindAsync)
        {
            var mockCollection = new Mock<IMongoCollection<BottleMongoModel>>();
            //Setup mock collection to return 
            mockCollection.Setup(
                    c => c.FindAsync(
                        It.IsAny<FilterDefinition<BottleMongoModel>>(), It.IsAny<FindOptions<BottleMongoModel, BottleMongoModel>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(cursorToReturnFromFindAsync);

            return mockCollection;
        }
    }
}
