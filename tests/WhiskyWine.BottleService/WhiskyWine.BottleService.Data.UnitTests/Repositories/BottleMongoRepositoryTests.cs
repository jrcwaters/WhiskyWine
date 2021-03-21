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
