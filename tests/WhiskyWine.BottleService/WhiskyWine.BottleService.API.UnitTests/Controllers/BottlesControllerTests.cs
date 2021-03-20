using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using WhiskyWine.BottleService.API.Controllers;
using WhiskyWine.BottleService.API.Mappers;
using WhiskyWine.BottleService.API.Models;
using WhiskyWine.BottleService.Domain.Interfaces;
using WhiskyWine.BottleService.Domain.Models;

namespace WhiskyWine.BottleService.API.UnitTests.Controllers
{
    /// <summary>
    /// The unit test class for the BottlesController.
    /// </summary>
    public class BottlesControllerTests
    {
        /// <summary>
        /// Mocked out BottleService.
        /// </summary>
        private Mock<IBottleService> _mockBottleService;

        /// <summary>
        /// Mocked out Validator.
        /// </summary>
        private Mock<IValidator<BottleApiModel>> _mockValidator;

        /// <summary>
        /// Domain to API model mapper.
        /// Not mocked since setting up the mock would require basically the same code as the mapper, since mapper just parses and transfers properties between objects.
        /// </summary>
        private IMapper<BottleDomainModel, BottleApiModel> _toApiModelMapper;

        /// <summary>
        /// API to Domain model mapper.
        /// Not mocked since setting up the mock would require basically the same code as the mapper, since mapper just parses and transfers properties between objects.
        /// </summary>
        private IMapper<BottleApiModel, BottleDomainModel> _toDomainModelMapper;

        /// <summary>
        /// Set up the dependencies. Runs before every unit test in this class.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            _mockBottleService = new Mock<IBottleService>();
            _mockValidator = new Mock<IValidator<BottleApiModel>>();
            _toApiModelMapper = new DomainToApiModelMapper();
            _toDomainModelMapper = new ApiToDomainModelMapper();
        }

        /// <summary>
        /// Test that the GetBottlesAsync method returns a NotFound result when the IBottleService implementation's GetBottleAsync method returns null.
        /// </summary>
        [Test]
        public async Task GetBottleAsync_Returns404_WhenServiceReturnsNull()
        {
            //Arrange
            _mockBottleService.Setup(
                c => c.GetBottleAsync(It.IsAny<string>()))
                .ReturnsAsync((BottleDomainModel)null);

            var bottlesController = new BottlesController(_mockBottleService.Object,
                _mockValidator.Object,
                _toApiModelMapper,
                _toDomainModelMapper);

            //Act
            var actionResult = await bottlesController.GetBottleAsync("bottleId");

            //Assert
            Assert.IsInstanceOf(typeof(NotFoundObjectResult), actionResult);
        }

        /// <summary>
        /// Test that the GetBottleAsync method returns an Ok result when the IBottleService implementation's GetBottleAsync method returns a non-null Bottle object.
        /// </summary>
        [Test]
        public async Task GetBottleAsync_Returns200ContainingBottleData_WhenServiceFindsBottle()
        {
            //Arrange
            var domainBottleReturnedByService = new BottleDomainModel
            {
                BottleId = "bottleId"
            };

            var expectedApiBottle = new BottleApiModel
            {
                BottleId = "bottleId"
            };

            _mockBottleService.Setup(
                c => c.GetBottleAsync(It.IsAny<string>()))
                .ReturnsAsync(domainBottleReturnedByService);

            var bottlesController = new BottlesController(_mockBottleService.Object,
                _mockValidator.Object,
                _toApiModelMapper,
                _toDomainModelMapper);

            //Act
            var actionResult = await bottlesController.GetBottleAsync("bottleId");
            var objectResult = actionResult as OkObjectResult;
            var actualBottle = objectResult.Value as BottleApiModel;

            //Assert
            Assert.IsInstanceOf(typeof(OkObjectResult), actionResult);
            Assert.IsNotNull(actualBottle);
            Assert.AreEqual(expectedApiBottle.BottleId, actualBottle.BottleId);
        }

        /// <summary>
        /// Test that the GetAllBottlesAsync method returns an Ok result when the IBottleService implementation's GetAllBottlesAsync method returns a non-empty list of Bottles.
        /// </summary>
        [Test]
        public async Task GetAllBottlesAsync_Returns200ContainingListOfAllBottles_WhenServiceFindsBottles()
        {
            //Arrange
            var domainBottleListReturnedByService = new List<BottleDomainModel>
            {
                new BottleDomainModel { BottleId = "bottle1" },
                new BottleDomainModel { BottleId = "bottle2" }
            };

            var expectedApiBottleList = new List<BottleApiModel>
            {
            new BottleApiModel{ BottleId = "bottle1" },
            new BottleApiModel{ BottleId = "bottle2" }
            };

            _mockBottleService.Setup(
                c => c.GetAllBottlesAsync())
                .ReturnsAsync(domainBottleListReturnedByService);

            var bottlesController = new BottlesController(_mockBottleService.Object,
                _mockValidator.Object,
                _toApiModelMapper,
                _toDomainModelMapper);

            //Act
            var actionResult = await bottlesController.GetAllBottlesAsync();
            var objectResult = actionResult as OkObjectResult;
            var actualList = objectResult.Value as List<BottleApiModel>;

            //Assert
            Assert.IsInstanceOf(typeof(OkObjectResult), actionResult);
            Assert.AreEqual(expectedApiBottleList[0].BottleId, actualList[0].BottleId);
            Assert.AreEqual(expectedApiBottleList[1].BottleId, actualList[1].BottleId);
        }

        /// <summary>
        /// Test that the GetAllBottlesAsync method returns an Ok result when the IBottleService implementation's GetAllBottlesAsync method returns an empty list of Bottles.
        /// </summary>
        [Test]
        public async Task GetAllBottlesAsync_Returns200ContainingEmptyList_WhenServiceDoesntFindBottles()
        {
            //Arrange
            _mockBottleService.Setup(
                c => c.GetAllBottlesAsync())
                .ReturnsAsync(new List<BottleDomainModel>());

            var bottlesController = new BottlesController(_mockBottleService.Object,
                _mockValidator.Object,
                _toApiModelMapper,
                _toDomainModelMapper);

            //Act
            var actionResult = await bottlesController.GetAllBottlesAsync();
            var objectResult = actionResult as OkObjectResult;
            var actualList = objectResult.Value as List<BottleApiModel>;

            //Assert
            Assert.IsInstanceOf(typeof(OkObjectResult), actionResult);
            Assert.IsInstanceOf(typeof(List<BottleApiModel>), actualList);
            Assert.IsEmpty(actualList);
        }

        /// <summary>
        /// Test that the PostBottleAsync method returns a Created result when the IBottleService implementation's PostBottleAsync method returns a Bottle.
        /// </summary>
        [Test]
        public async Task PostBottleAsync_Returns201ContainingPostedBottle_WhenValidBottlePosted()
        {
            //Arrange
            var domainBottleReturnedByService = new BottleDomainModel { BottleId = "bottleId", Name = "myBottle" };
            _mockBottleService.Setup(
                c => c.PostBottleAsync(It.IsAny<BottleDomainModel>()))
                .ReturnsAsync(domainBottleReturnedByService);

            var bottlesController = new BottlesController(_mockBottleService.Object,
                _mockValidator.Object,
                _toApiModelMapper,
                _toDomainModelMapper);

            var apiBottleToPost = new BottleApiModel { BottleId = "bottleId", Name = "myBottle" };

            //Act
            var actionResult = await bottlesController.PostBottleAsync(apiBottleToPost);
            var createdResult = actionResult as CreatedResult;
            var createdLocation = createdResult.Location;
            var createdBottle = createdResult.Value as BottleApiModel;

            //Assert
            Assert.IsInstanceOf(typeof(CreatedResult), actionResult);
            Assert.AreEqual($"api/bottles/{apiBottleToPost.BottleId}", createdLocation);
            Assert.AreEqual(apiBottleToPost.Name, createdBottle.Name);
        }

        /// <summary>
        /// Test that the PostBottleAsync method returns a BadRequest result when the IBottleService implementation's PostBottleAsync method returns null.
        /// </summary>
        [Test]
        public async Task PostBottleAsync_Returns400_WhenServiceReturnsNull()
        {
            //Arrange
            _mockBottleService.Setup(
                c => c.PostBottleAsync(It.IsAny<BottleDomainModel>()))
                .ReturnsAsync((BottleDomainModel)null);

            var bottlesController = new BottlesController(_mockBottleService.Object,
                _mockValidator.Object,
                _toApiModelMapper,
                _toDomainModelMapper);

            //Act
            var actionResult = await bottlesController.PostBottleAsync(new BottleApiModel());

            //Assert
            Assert.IsInstanceOf(typeof(BadRequestObjectResult), actionResult);
        }

        /// <summary>
        /// Test that the UpdateBottleAsync method returns an Ok result when the IBottleService implementation's GetBottleAsync method returns a Bottle.
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task UpdateBottleAsync_Returns200ContainingBottleData_WhenServiceFindsAndUpdatesBottle()
        {
            //Arrange
            var apiBottleToPassController = new BottleApiModel { BottleId = "bottleId", Name = "bottleName" };
            var domainBottleReturnedByService = new BottleDomainModel { BottleId = "bottleId", Name = "bottleName" };

            _mockBottleService.Setup(
                c => c.GetBottleAsync(It.IsAny<string>()))
                .ReturnsAsync(domainBottleReturnedByService);

            var bottlesController = new BottlesController(_mockBottleService.Object,
                _mockValidator.Object,
                _toApiModelMapper,
                _toDomainModelMapper);

            //Act
            var actionResult = await bottlesController.UpdateBottleAsync("bottleId", apiBottleToPassController);
            var okResult = actionResult as OkObjectResult;
            var actualBottle = okResult.Value as BottleApiModel;

            //Assert
            Assert.IsInstanceOf(typeof(OkObjectResult), actionResult);
            Assert.IsInstanceOf(typeof(BottleApiModel), actualBottle);
            Assert.AreEqual(apiBottleToPassController.Name, actualBottle.Name);
        }

        /// <summary>
        /// Test that the UpdateBottleAsync method returns a NotFound result when the IBottleService implementation's GetBottleAsync method returns null.
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task UpdateBottleAsync_Returns404_WhenServiceCantGetBottleMatchingId()
        {
            //Arrange
            _mockBottleService.Setup(
                c => c.GetBottleAsync(It.IsAny<string>()))
                .ReturnsAsync((BottleDomainModel)null);

            var bottlesController = new BottlesController(_mockBottleService.Object,
                _mockValidator.Object,
                _toApiModelMapper,
                _toDomainModelMapper);

            //Act
            var actionResult = await bottlesController.UpdateBottleAsync("bottleId", new BottleApiModel());
            var notFoundResult = actionResult as NotFoundObjectResult;
            var notFoundValue = notFoundResult.Value as string;

            //Assert
            Assert.IsInstanceOf(typeof(NotFoundObjectResult), actionResult);
            Assert.AreEqual("bottleId", notFoundValue);
        }

        /// <summary>
        /// Test that the DeleteBottleAsync method returns a NotFound result when the IBottleService implementation's DeleteBottleAsync method returns false.
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task DeleteBottleAsync_Returns404_WhenServiceCantGetBottleMatchingId()
        {
            //Arrange
            _mockBottleService.Setup(
                c => c.DeleteBottleAsync(It.IsAny<string>()))
                .ReturnsAsync(false);

            var bottlesController = new BottlesController(_mockBottleService.Object,
                _mockValidator.Object,
                _toApiModelMapper,
                _toDomainModelMapper);

            //Act
            var actionResult = await bottlesController.DeleteBottleAsync("bottleId");
            var notFoundResult = actionResult as NotFoundObjectResult;
            var notFoundValue = notFoundResult.Value as string;

            //Assert
            Assert.IsInstanceOf(typeof(NotFoundObjectResult), actionResult);
            Assert.AreEqual("bottleId", notFoundValue);
        }

        /// <summary>
        /// Test that the DeleteBottleAsync method returns a NoContent result when the IBottleService implementation's DeleteBottleAsync method returns true.
        /// </summary>
        /// <returns></returns>
        [Test]
        public async Task DeleteBottleAsync_Returns204_WhenServiceDeletesBottleAndReturnsTrue()
        {
            //Arrange
            _mockBottleService.Setup(
                c => c.DeleteBottleAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            var bottlesController = new BottlesController(_mockBottleService.Object,
                _mockValidator.Object,
                _toApiModelMapper,
                _toDomainModelMapper);

            //Act
            var actionResult = await bottlesController.DeleteBottleAsync("bottleId");

            //Assert
            Assert.IsInstanceOf(typeof(NoContentResult), actionResult);
        }
    }
}
