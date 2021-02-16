using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using WhiskyWine.BottleService.API.Controllers;
using WhiskyWine.BottleService.Domain.Interfaces;
using WhiskyWine.BottleService.Domain.Models;

namespace WhiskyWine.BottleService.API.UnitTests.Controllers
{
    public class BottlesControllerTests
    {
        private Mock<IBottleService> _mockBottleService;

        [SetUp]
        public void Setup()
        {
            _mockBottleService = new Mock<IBottleService>();
        }

        [Test]
        public async Task GetBottleAsync_Returns404_WhenServiceReturnsNull()
        {
            //Arrange
            _mockBottleService.Setup(
                c => c.GetBottleAsync(It.IsAny<string>()))
                .ReturnsAsync((Bottle)null);

            var bottlesController = new BottlesController(_mockBottleService.Object);

            //Act
            var actionResult = await bottlesController.GetBottleAsync("bottleId");

            //Assert
            Assert.IsInstanceOf(typeof(NotFoundObjectResult), actionResult);
        }

        [Test]
        public async Task GetBottleAsync_Returns200ContainingBottleData_WhenServiceFindsBottle()
        {
            //Arrange
            var expectedBottle = new Bottle 
            { 
                BottleId = "bottleId"
            };
            
            _mockBottleService.Setup(
                c => c.GetBottleAsync(It.IsAny<string>()))
                .ReturnsAsync(expectedBottle);

            var bottlesController = new BottlesController(_mockBottleService.Object);

            //Act
            var actionResult = await bottlesController.GetBottleAsync("bottleId");
            var objectResult = actionResult as OkObjectResult;
            var actualBottle = objectResult.Value as Bottle;

            //Assert
            Assert.IsInstanceOf(typeof(OkObjectResult), actionResult);
            Assert.IsNotNull(actualBottle);
            Assert.AreEqual(actualBottle.BottleId, expectedBottle.BottleId);
        }

        [Test]
        public async Task GetAllBottlesAsync_Returns200ContainingListOfAllBottles_WhenServiceFindsBottles()
        {
            //Arrange
            var expectedList = new List<Bottle>
            {
                new Bottle { BottleId = "bottle1" },
                new Bottle { BottleId = "bottle2" }
            };

            _mockBottleService.Setup(
                c => c.GetAllBottlesAsync())
                .ReturnsAsync(expectedList);

            var bottlesController = new BottlesController(_mockBottleService.Object);

            //Act
            var actionResult = await bottlesController.GetAllBottlesAsync();
            var objectResult = actionResult as OkObjectResult;
            var actualList = objectResult.Value as List<Bottle>;

            //Assert
            Assert.IsInstanceOf(typeof(OkObjectResult), actionResult);
            Assert.AreEqual(actualList[0].BottleId, expectedList[0].BottleId);
            Assert.AreEqual(actualList[1].BottleId, expectedList[1].BottleId);

        }

        [Test]
        public async Task GetAllBottlesAsync_Returns200ContainingEmptyList_WhenServiceDoesntFindBottles()
        {
            //Arrange
            _mockBottleService.Setup(
                c => c.GetAllBottlesAsync())
                .ReturnsAsync(new List<Bottle>());

            var bottlesController = new BottlesController(_mockBottleService.Object);

            //Act
            var actionResult = await bottlesController.GetAllBottlesAsync();
            var objectResult = actionResult as OkObjectResult;
            var actualList = objectResult.Value as List<Bottle>;

            //Assert
            Assert.IsInstanceOf(typeof(OkObjectResult), actionResult);
            Assert.IsInstanceOf(typeof(List<Bottle>), actualList);
            Assert.IsEmpty(actualList);          
        }

        [Test]
        public async Task PostBottleAsync_Returns201ContainingPostedBottle_WhenValidBottlePosted()
        {
            //Arrange
            var postedBottle = new Bottle { BottleId = "bottleId", Name = "myBottle" };
            _mockBottleService.Setup(
                c => c.PostBottleAsync(It.IsAny<Bottle>()))
                .ReturnsAsync(postedBottle);

            var bottlesController = new BottlesController(_mockBottleService.Object);

            //Act
            var actionResult = await bottlesController.PostBottleAsync(postedBottle);
            var createdResult = actionResult as CreatedResult;
            var createdLocation = createdResult.Location;
            var createdBottle = createdResult.Value as Bottle;

            //Assert
            Assert.IsInstanceOf(typeof(CreatedResult), actionResult);
            Assert.AreEqual($"api/bottles/{postedBottle.BottleId}", createdLocation);
            Assert.AreEqual(postedBottle.Name, createdBottle.Name);
        }

        [Test]
        public async Task PostBottleAsync_Returns400_WhenServiceReturnsNull()
        {
            //Arrange
            _mockBottleService.Setup(
                c => c.PostBottleAsync(It.IsAny<Bottle>()))
                .ReturnsAsync((Bottle)null);

            var bottlesController = new BottlesController(_mockBottleService.Object);

            //Act
            var actionResult = await bottlesController.PostBottleAsync(new Bottle());

            //Assert
            Assert.IsInstanceOf(typeof(BadRequestObjectResult), actionResult);
        }

        [Test]
        public async Task UpdateBottleAsync_Returns200ContainingBottleData_WhenServiceFindsAndUpdatesBottle()
        {
            //Arrange
            _mockBottleService.Setup(
                c => c.GetBottleAsync(It.IsAny<string>()))
                .ReturnsAsync(new Bottle());

            var newBottleData = new Bottle { BottleId = "bottleId", Name = "bottleName" };

            var bottlesController = new BottlesController(_mockBottleService.Object);

            //Act
            var actionResult = await bottlesController.UpdateBottleAsync("bottleId", newBottleData);
            var okResult = actionResult as OkObjectResult;
            var actualBottle = okResult.Value as Bottle;

            //Assert
            Assert.IsInstanceOf(typeof(OkObjectResult), actionResult);
            Assert.IsInstanceOf(typeof(Bottle), actualBottle);
            Assert.AreEqual(newBottleData.Name, actualBottle.Name);
        }

        [Test]
        public async Task UpdateBottleAsync_Returns404_WhenServiceCantGetBottleMatchingId()
        {
            //Arrange
            _mockBottleService.Setup(
                c => c.GetBottleAsync(It.IsAny<string>()))
                .ReturnsAsync((Bottle)null);

            var bottlesController = new BottlesController(_mockBottleService.Object);

            //Act
            var actionResult = await bottlesController.UpdateBottleAsync("bottleId", new Bottle());
            var notFoundResult = actionResult as NotFoundObjectResult;
            var notFoundValue = notFoundResult.Value as string;

            //Assert
            Assert.IsInstanceOf(typeof(NotFoundObjectResult), actionResult);
            Assert.AreEqual("bottleId", notFoundValue);
        }

        [Test]
        public async Task DeleteBottleAsync_Returns404_WhenServiceCantGetBottleMatchingId()
        {
            //Arrange
            _mockBottleService.Setup(
                c => c.GetBottleAsync(It.IsAny<string>()))
                .ReturnsAsync((Bottle)null);

            var bottlesController = new BottlesController(_mockBottleService.Object);

            //Act
            var actionResult = await bottlesController.DeleteBottleAsync("bottleId");
            var notFoundResult = actionResult as NotFoundObjectResult;
            var notFoundValue = notFoundResult.Value as string;

            //Assert
            Assert.IsInstanceOf(typeof(NotFoundObjectResult), actionResult);
            Assert.AreEqual("bottleId", notFoundValue);
        }

        [Test]
        public async Task DeleteBottleAsync_Returns204_WhenServiceDeletesBottleAndReturnsTrue()
        {
            //Arrange
            _mockBottleService.Setup(
                c => c.GetBottleAsync(It.IsAny<string>()))
                .ReturnsAsync(new Bottle());

            var bottlesController = new BottlesController(_mockBottleService.Object);

            //Act
            var actionResult = await bottlesController.DeleteBottleAsync("bottleId");

            //Assert
            Assert.IsInstanceOf(typeof(NoContentResult), actionResult);
        }
    }
}
