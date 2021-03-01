using System;
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

namespace WhiskyWine.BottleService.Data.UnitTests.Repositories
{
    public class BottleMongoRepositoryTests
    {
        private Mock<IMapper<Bottle, BottleMongoModel>> _mockToMongoMapper;
        private Mock<IMapper<BottleMongoModel, Bottle>> _mockToDomainMapper;
        private Mock<IMongoDbContext<BottleMongoModel>> _mockDbContext;

        /// <summary>
        /// SetUp mocks of all dependencies for BottleMongoRepository. Will be run before every test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            //Create mocks of the BottleMongoRepository dependencies and store in fields
            _mockToMongoMapper = new Mock<IMapper<Bottle, BottleMongoModel>>();
            _mockToDomainMapper = new Mock<IMapper<BottleMongoModel, Bottle>>();
            _mockDbContext = new Mock<IMongoDbContext<BottleMongoModel>>();

            
        }

        /// <summary>
        /// Test that the GetByIdAsync method returns null if the Id string passed cannot be parsed into a MongoDB.Bson.ObjectId object.
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task GetByIdAsync_ReturnsNull_IfInvalidIdStringPassed()
        {
            //Arrange
            var repo = new BottleMongoRepository(_mockDbContext.Object, _mockToMongoMapper.Object, _mockToDomainMapper.Object);

            //Act
            var result = await repo.GetByIdAsync("1234");

            //Assert
            Assert.AreEqual(null, result);
        }

        [Test]
        public async Task GetByIdAsync_ReturnsDomainBottle_WhenBottleReturnedFromCollection()
        {
            //Arrange

            //Create the Id of the object to retrieve from the mock collection
            var idString = "507f1f77bcf86cd799439011";
            var objectId = new ObjectId(idString);

            var mongoBottle = new BottleMongoModel { BottleId = objectId, Name = "bottleName" };

            var mockCursor = CreateAndSetUpMockCursor(new List<BottleMongoModel> { mongoBottle });
            var mockCollection = CreateAndSetupMockMongoCollection(mockCursor.Object);

            _mockDbContext.SetupGet(c => c.Collection).Returns(mockCollection.Object);


            //Setup the Map method of the mock Mongo to Domain model Mapper 
            var mappedToDomainBottle = new Bottle();
            _mockToDomainMapper.Setup(
                    c => c.Map(It.IsAny<BottleMongoModel>()))
                .Callback<BottleMongoModel>((passedMongoBottle) =>
                {
                    mappedToDomainBottle = passedMongoBottle == null ? null : new Bottle
                    {
                        BottleId = passedMongoBottle.BottleId.ToString(),
                        Name = passedMongoBottle.Name,
                        AlcoholCategory = passedMongoBottle.AlcoholCategory,
                        Region = passedMongoBottle.Region
                    };
                })
                .Returns(() => mappedToDomainBottle);

            var repo = new BottleMongoRepository(_mockDbContext.Object, _mockToMongoMapper.Object, _mockToDomainMapper.Object);

            //Act
            var result = await repo.GetByIdAsync("507f1f77bcf86cd799439010");

            //Assert
            Assert.AreEqual(mappedToDomainBottle.BottleId, result.BottleId);
            Assert.AreEqual(mappedToDomainBottle.Name, result.Name);

        }

        public async Task GetByIdAsync_ReturnsNull_WhenNoBottleIsReturnedFromCollection()
        {
            //Arrange

            //Act

            //Assert
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
