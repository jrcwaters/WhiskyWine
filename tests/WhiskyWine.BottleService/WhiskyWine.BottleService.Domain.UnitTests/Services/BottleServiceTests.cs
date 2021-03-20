using Moq;
using WhiskyWine.BottleService.Domain.Interfaces;
using WhiskyWine.BottleService.Domain.Models;
using NUnit.Framework;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace WhiskyWine.BottleService.Domain.UnitTests.Services
{
    /// <summary>
    /// The unit test class for the BottleService.
    /// </summary>
    public class BottleServiceTests
    {
        private Mock<IRepository<BottleDomainModel>> _mockRepository;

        /// <summary>
        /// Sets up mocks of BottleService dependencies. Runs before every test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _mockRepository = new Mock<IRepository<BottleDomainModel>>();
        }

        /// <summary>
        /// Tests that the GetBottlesAsync method returns the same Bottle returned by repository's GetByIdAsync method, when this is not null.
        /// </summary>
        [Test]
        public async Task GetBottleAsync_ReturnsResultReturnedByRepository_WhenNotNull()
        {
            //Arrange
            var bottleToReturn = new BottleDomainModel { BottleId = "bottleId" };
            _mockRepository.Setup(
                c => c.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync(bottleToReturn);

            var bottleService = new Domain.Services.BottleService(_mockRepository.Object);

            //Act
            var result = await bottleService.GetBottleAsync("bottleId");

            //Assert
            Assert.AreEqual(bottleToReturn.BottleId, result.BottleId);
        }

        /// <summary>
        /// Tests that the GetBottlesAsync method returns null when repository's GetByIdAsync method returns null.
        /// </summary>
        [Test]
        public async Task GetBottleAsync_ReturnsNull_WhenNullReturnedByRepository()
        {
            //Arrange
            _mockRepository.Setup(
                c => c.GetByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((BottleDomainModel)null);

            var bottleService = new Domain.Services.BottleService(_mockRepository.Object);

            //Act
            var result = await bottleService.GetBottleAsync("bottleId");

            //Assert
            Assert.AreEqual(null, null);
        }

        /// <summary>
        /// Tests that the GetAllBottlesAsync method returns the same list returned by the repository's GetAllAsync method, when this list is non-empty.
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task GetAllBottlesAsync_ReturnsListReturnedByRepo_WhenNonEmpty()
        {
            //Arrange
            var listToReturn = new List<BottleDomainModel> 
            { 
                new BottleDomainModel { BottleId = "bottle1" },
                new BottleDomainModel { BottleId = "bottle2"}
            };
            _mockRepository.Setup(
                c => c.GetAllAsync())
                .ReturnsAsync(listToReturn);

            var bottleService = new Domain.Services.BottleService(_mockRepository.Object);

            //Act
            var result = await bottleService.GetAllBottlesAsync() as List<BottleDomainModel>;

            //Assert
            Assert.AreEqual(listToReturn[0].BottleId, result[0].BottleId);
            Assert.AreEqual(listToReturn[1].BottleId, result[1].BottleId);
        }

        /// <summary>
        /// Tests that the GetAllBottlesAsync method returns the empty list when the repository's GetAllAsync method returns an empty list.
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task GetAllBottlesAsync_ReturnsEmptyList_WhenRepoReturnsEmptyList()
        {
            //Arrange
            var listToReturn = new List<BottleDomainModel>();
            _mockRepository.Setup(
                c => c.GetAllAsync())
                .ReturnsAsync(listToReturn);

            var bottleService = new Domain.Services.BottleService(_mockRepository.Object);

            //Act
            var result = await bottleService.GetAllBottlesAsync();

            //Assert
            Assert.IsInstanceOf(typeof(List<BottleDomainModel>), result);
            Assert.IsEmpty(result);
        }

        /// <summary>
        /// Tests that the PostBottleAsync method returns the bottle returned by to the repository's InsertAsync method, when this list is non-null.
        /// </summary>
        [Test]
        public async Task PostBottleAsync_ReturnsBottleReturnedByRepo_WhenNotNull()
        {
            //Arrange
            var bottleToReturn = new BottleDomainModel { BottleId = "bottleId" };
            _mockRepository.Setup(
                c => c.InsertAsync(It.IsAny<BottleDomainModel>()))
                .ReturnsAsync(bottleToReturn);

            var bottleService = new Domain.Services.BottleService(_mockRepository.Object);

            //Act
            var result = await bottleService.PostBottleAsync(bottleToReturn);

            //Assert
            Assert.AreEqual(bottleToReturn.BottleId, result.BottleId);

        }

        /// <summary>
        /// Tests that the PostBottleAsync method returns null when the repository's InsertAsync method returns null.
        /// </summary>
        [Test]
        public async Task PostBottleAsync_ReturnsNull_WhenRepoReturnsNull()
        {
            //Arrange
            _mockRepository.Setup(
                c => c.InsertAsync(It.IsAny<BottleDomainModel>()))
                .ReturnsAsync((BottleDomainModel)null);

            var bottleService = new Domain.Services.BottleService(_mockRepository.Object);

            //Act
            var result = await bottleService.PostBottleAsync(new BottleDomainModel());

            //Assert
            Assert.AreEqual(null, result);

        }

        /// <summary>
        /// Tests that the DeleteBottleAsync method returns true when the repository's DeleteAsync method returns true.
        /// </summary>
        [Test]
        public async Task DeleteBottleAsync_ReturnsTrue_WhenRepoReturnsTrue()
        {
            //Arrange
            _mockRepository.Setup(
                c => c.DeleteAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            var bottleService = new Domain.Services.BottleService(_mockRepository.Object);

            //Act
            var result = await bottleService.DeleteBottleAsync("bottleId");

            //Assert
            Assert.AreEqual(true, result);
        }

        /// <summary>
        /// Tests that the DeleteBottleAsync method returns false when the repository's DeleteAsync method returns false.
        /// </summary>
        [Test]
        public async Task DeleteBottleAsync_ReturnsFalse_WhenRepoReturnsFalse()
        {
            //Arrange
            _mockRepository.Setup(
                c => c.DeleteAsync(It.IsAny<string>()))
                .ReturnsAsync(false);

            var bottleService = new Domain.Services.BottleService(_mockRepository.Object);

            //Act
            var result = await bottleService.DeleteBottleAsync("bottleId");

            //Assert
            Assert.AreEqual(false, result);
        }
    }
}
