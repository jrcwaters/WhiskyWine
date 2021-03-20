using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using WhiskyWine.BottleService.API.Mappers;
using WhiskyWine.BottleService.Domain.Enums;
using WhiskyWine.BottleService.Domain.Models;

namespace WhiskyWine.BottleService.API.UnitTests.Mappers
{

    /// <summary>
    /// The unit test class for the DomainToApiModelMapper.
    /// </summary>
    public class DomainToApiModelMapperTests
    {
        /// <summary>
        /// The class under test.
        /// </summary>
        private DomainToApiModelMapper _mapper;

        /// <summary>
        /// Sets up the class under test. Runs before every unit test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _mapper = new DomainToApiModelMapper();
        }

        /// <summary>
        /// Test that the MapOne method returns a null BottleAoiModel if null BottleDomainModel passed.
        /// </summary>
        [Test]
        public void MapOne_ReturnsNull_IfNullParamPassed()
        {
            //Arrange
            BottleDomainModel fromBottle = null;

            //Act
            var result = _mapper.MapOne(fromBottle);

            //Assert
            Assert.IsNull(result);
        }

        /// <summary>
        /// Test that the MapOne method converts the BottleDomainModel AlcoholCategory enum to the expected string.
        /// </summary>
        [Test]
        public void MapOne_CorrectlyTranslatesAlcoholCategoryToString()
        {
            //Arrange
            BottleDomainModel fromBottle = new BottleDomainModel { AlcoholCategory = AlcoholCategory.Whisky };

            //Act
            var result = _mapper.MapOne(fromBottle);

            //Assert
            Assert.AreEqual("Whisky", result.AlcoholCategory);
        }

        /// <summary>
        /// Test that the MapOne method maps the BottleDomainModel Name correctly.
        /// </summary>
        [Test]
        public void MapOne_ReturnsBottleApiModelWithCorrectName_WhenDomainBottleMapped()
        {
            //Arrange
            BottleDomainModel fromBottle = new BottleDomainModel { Name = "bottleName" };

            //Act
            var result = _mapper.MapOne(fromBottle);

            //Assert
            Assert.AreEqual("bottleName", result.Name);
        }

        /// <summary>
        /// Test that the MapOne method maps the BottleDomainModel Region correctly.
        /// </summary>
        [Test]
        public void MapOne_ReturnsBottleApiModelWithCorrectRegion_WhenDomainBottleMapped()
        {
            //Arrange
            BottleDomainModel fromBottle = new BottleDomainModel { Region = "bottleRegion" };

            //Act
            var result = _mapper.MapOne(fromBottle);

            //Assert
            Assert.AreEqual("bottleRegion", result.Region);
        }

        /// <summary>
        /// Test that the MapOne method maps the BottleDomainModel Id correctly.
        /// </summary>
        [Test]
        public void MapOne_ReturnsBottleApiModelWithCorrectId_WhenDomainBottleMapped()
        {
            //Arrange
            BottleDomainModel fromBottle = new BottleDomainModel { BottleId = "bottleId" };

            //Act
            var result = _mapper.MapOne(fromBottle);

            //Assert
            Assert.AreEqual("bottleId", result.BottleId);
        }

        /// <summary>
        /// Test that the MapMany method returns an empty list of BottleDomainModel when an empty list of BottleApiModel is passed.
        /// </summary>
        [Test]
        public void MapMany_ReturnsEmptyList_WhenEmptyListPassed()
        {
            //Arrange 
            var apiBottleList = new List<BottleDomainModel>();

            //Act
            var result = _mapper.MapMany(apiBottleList);

            //Assert
            Assert.IsEmpty(result);
        }

        /// <summary>
        /// Test that the MapMany method can correctly map a list of BottleDomainModels to BottleApiModels.
        /// </summary>
        [Test]
        public void MapMany_ReturnsCorrectlyMappedList_WhenNonEmptyListPassed()
        {
            //Arrange
            var apiBottleList = new List<BottleDomainModel>
            {
                new BottleDomainModel{ BottleId = "bottleId1", AlcoholCategory = AlcoholCategory.Whisky },
                new BottleDomainModel{ BottleId = "bottleId2", AlcoholCategory = AlcoholCategory.RedWine }
            };

            //Act
            var result = _mapper.MapMany(apiBottleList);

            //Assert
            Assert.AreEqual("bottleId1", result.ToList()[0].BottleId);
            Assert.AreEqual("Whisky", result.ToList()[0].AlcoholCategory);

            Assert.AreEqual("bottleId2", result.ToList()[1].BottleId);
            Assert.AreEqual("RedWine", result.ToList()[1].AlcoholCategory);
        }

    }
}
