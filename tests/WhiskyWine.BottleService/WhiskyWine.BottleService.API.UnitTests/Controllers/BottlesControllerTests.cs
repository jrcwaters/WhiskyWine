using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http.Results;
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
        public async Task GetBottleAsync_Returns404_WhenRepoReturnsNull()
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
        public async Task GetBottleAsync_Returns200ContainingBottleData_WhenRepoFindsBottle()
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
        public async Task GetAllBottlesAsync_Returns200ContainingListOfAllBottles_WhenRepoFindsBottles()
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

    }
}
