using Moq;
using WhiskyWine.BottleService.Domain.Interfaces;
using WhiskyWine.BottleService.Domain.Models;
using NUnit.Framework;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace WhiskyWine.BottleService.Domain.UnitTests.Services
{
    public class BottleServiceTests
    {
        private Mock<IRepository<Bottle>> _mockRepository;

        [SetUp]
        public void SetUp()
        {
            _mockRepository = new Mock<IRepository<Bottle>>();
        }

        [Test]
        public async Task GetBottleAsync_ReturnsResultReturnedByRepository_WhenNonNull()
        {
            //Arrange
            var bottleToReturn = new Bottle { BottleId = "bottleId" };
            _mockRepository.Setup(
                c => c.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(bottleToReturn);

            var bottleService = new Domain.Services.BottleService(_mockRepository.Object);

            //Act
            var result = await bottleService.GetBottleAsync("bottleId");

            //Assert
            Assert.AreEqual(bottleToReturn.BottleId, result.BottleId);
        }

        [Test]
        public async Task GetBottleAsync_ReturnsNull_WhenNullReturnedByRepository()
        {
            //Arrange
            _mockRepository.Setup(
                c => c.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((Bottle)null);

            var bottleService = new Domain.Services.BottleService(_mockRepository.Object);

            //Act
            var result = await bottleService.GetBottleAsync("bottleId");

            //Assert
            Assert.AreEqual(null, null);
        }

        [Test]
        public async Task GetAllBottlesAsync_ReturnsListReturnedByRepo_WhenNonEmpty()
        {
            //Arrange
            var listToReturn = new List<Bottle> 
            { 
                new Bottle { BottleId = "bottle1" },
                new Bottle { BottleId = "bottle2"}
            };
            _mockRepository.Setup(
                c => c.GetAllAsync())
                .ReturnsAsync(listToReturn);

            var bottleService = new Domain.Services.BottleService(_mockRepository.Object);

            //Act
            var result = await bottleService.GetAllBottlesAsync() as List<Bottle>;

            //Assert
            Assert.AreEqual(listToReturn[0].BottleId, result[0].BottleId);
            Assert.AreEqual(listToReturn[1].BottleId, result[1].BottleId);
        }

        [Test]
        public async Task GetAllBottlesAsync_ReturnsEmptyList_WhenRepoReturnsEmptyList()
        {
            //Arrange
            var listToReturn = new List<Bottle>();
            _mockRepository.Setup(
                c => c.GetAllAsync())
                .ReturnsAsync(listToReturn);

            var bottleService = new Domain.Services.BottleService(_mockRepository.Object);

            //Act
            var result = await bottleService.GetAllBottlesAsync();

            //Assert
            Assert.IsInstanceOf(typeof(List<Bottle>), result);
            Assert.IsEmpty(result);
        }
    }
}
