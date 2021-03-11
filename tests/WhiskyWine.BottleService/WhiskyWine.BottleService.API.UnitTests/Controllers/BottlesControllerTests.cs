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
        /// Set up the dependencies. Runs before every unit test in this class.
        /// </summary>
        [SetUp]
        public void Setup()
        {
            _mockBottleService = new Mock<IBottleService>();
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
                .ReturnsAsync((Bottle)null);

            var bottlesController = new BottlesController(_mockBottleService.Object);

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

        /// <summary>
        /// Test that the GetAllBottlesAsync method returns an Ok result when the IBottleService implementation's GetAllBottlesAsync method returns a non-empty list of Bottles.
        /// </summary>
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

        /// <summary>
        /// Test that the GetAllBottlesAsync method returns an Ok result when the IBottleService implementation's GetAllBottlesAsync method returns an empty list of Bottles.
        /// </summary>
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

        /// <summary>
        /// Test that the PostBottleAsync method returns a Created result when the IBottleService implementation's PostBottleAsync method returns a Bottle.
        /// </summary>
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

        /// <summary>
        /// Test that the PostBottleAsync method returns a BadRequest result when the IBottleService implementation's PostBottleAsync method returns null.
        /// </summary>
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

        /// <summary>
        /// Test that the UpdateBottleAsync method returns an Ok result when the IBottleService implementation's GetBottleAsync method returns a Bottle.
        /// </summary>
        /// <returns></returns>
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

            var bottlesController = new BottlesController(_mockBottleService.Object);

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

            var bottlesController = new BottlesController(_mockBottleService.Object);

            //Act
            var actionResult = await bottlesController.DeleteBottleAsync("bottleId");

            //Assert
            Assert.IsInstanceOf(typeof(NoContentResult), actionResult);
        }
    }
}
