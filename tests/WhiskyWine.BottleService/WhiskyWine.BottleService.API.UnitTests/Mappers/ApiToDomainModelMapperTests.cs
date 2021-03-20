using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiskyWine.BottleService.API.Mappers;
using WhiskyWine.BottleService.API.Models;
using WhiskyWine.BottleService.Domain.Enums;

namespace WhiskyWine.BottleService.API.UnitTests.Mappers
{
    /// <summary>
    /// The unit test class for the ApiToDomainModelMapper.
    /// </summary>
    public class ApiToDomainModelMapperTests
    {
        private ApiToDomainModelMapper _mapper;

        /// <summary>
        /// Sets up the class under test. Runs before every unit test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _mapper = new ApiToDomainModelMapper();
        }

        /// <summary>
        /// Test that the MapOne method returns a null BottleDomainModel if null BottleApiModel passed.
        /// </summary>
        [Test]
        public void MapOne_ReturnsNull_IfNullParamPassed()
        {
            //Arrange
            BottleApiModel fromBottle = null;

            //Act
            var result = _mapper.MapOne(fromBottle);

            //Assert
            Assert.IsNull(result);
        }

        /// <summary>
        /// Test that the MapOne method returns a BottleDomainModel with BottleId correctly mapped.
        /// </summary>
        [Test]
        public void MapOne_ReturnsBottleDomainModelWithCorrectId_WhenApiBottleMapped()
        {
            //Arrange
            var apiBottle = new BottleApiModel { BottleId = "507f1f77bcf86cd799439011" };

            //Act
            var result = _mapper.MapOne(apiBottle);

            //Assert
            Assert.AreEqual(apiBottle.BottleId, result.BottleId);
        }

        /// <summary>
        /// Test that the MapOne method sets the AlcoholCategory to Unknown when the AlcoholCategory string in the BottleApiModel can't be parsed
        /// </summary>
        [Test]
        public void MapOne_ReturnsBottleDomainModel_WithUnknownAlcoholCategory_WhenCategoryCantbeParsed()
        {
            //Arrange
            var apiBottle = new BottleApiModel { AlcoholCategory = "Unparseable Category" };

            //Act
            var result = _mapper.MapOne(apiBottle);

            //Assert
            Assert.AreEqual(AlcoholCategory.Unknown, result.AlcoholCategory);
        }

        /// <summary>
        /// Test the the MapOne method returns a BottleDomainModel with string correctly parsed into AlcoholCategory when parsing is possible.
        /// </summary>
        [Test]
        public void MapOne_ReturnsBottleDomainModel_WithParsedAlcoholEnum_WhenCategoryCanBeParsed()
        {
            //Arrange
            var apiBottle = new BottleApiModel { AlcoholCategory = "Whisky" };

            //Act
            var result = _mapper.MapOne(apiBottle);

            //Assert
            Assert.AreEqual(AlcoholCategory.Whisky, result.AlcoholCategory);
        }

        [Test]
        public void MapOne_ParsesCategoryInNonCaseSensitiveWay()
        {
            //Arrange
            var apiBottle = new BottleApiModel { AlcoholCategory = "whIsKy" };

            //Act
            var result = _mapper.MapOne(apiBottle);

            //Assert
            Assert.AreEqual(AlcoholCategory.Whisky, result.AlcoholCategory);
        }

        /// <summary>
        /// Test that the MapOne method returns a BottleDomainModel with Name correctly mapped.
        /// </summary>
        [Test]
        public void MapOne_ReturnsBottleDomainModelWithCorrectName_WhenApiBottleMapped()
        {
            //Arrange
            var apiBottle = new BottleApiModel { Name = "bottleName" };

            //Act
            var result = _mapper.MapOne(apiBottle);

            //Assert
            Assert.AreEqual(apiBottle.Name, result.Name);
        }

        /// <summary>
        /// Test that the MapOne method returns a BottleDomainModel with Region correctly mapped.
        /// </summary>
        [Test]
        public void MapOne_ReturnsBottleDomainModelWithCorrectRegion_WhenApiBottleMapped()
        {
            //Arrange
            var apiBottle = new BottleApiModel { Region = "bottleRegion" };

            //Act
            var result = _mapper.MapOne(apiBottle);

            //Assert
            Assert.AreEqual(apiBottle.Region, result.Region);
        }

        /// <summary>
        /// Test that the MapMany method returns an empty list of BottleDomainModel when an empty list of BottleApiModel is passed.
        /// </summary>
        [Test]
        public void MapMany_ReturnsEmptyList_WhenEmptyListPassed()
        {
            //Arrange 
            var apiBottleList = new List<BottleApiModel>();

            //Act
            var result = _mapper.MapMany(apiBottleList);

            //Assert
            Assert.IsEmpty(result);
        }

        [Test]
        public void MapMany_ReturnsCorrectlyMappedList_WhenNonEmptyListPassed()
        {
            //Arrange
            var apiBottleList = new List<BottleApiModel> 
            {
                new BottleApiModel{ BottleId = "bottleId1", AlcoholCategory = "Whisky" },
                new BottleApiModel{ BottleId = "bottleId2", AlcoholCategory = "redwine"}
            };

            //Act
            var result = _mapper.MapMany(apiBottleList);

            //Assert
            Assert.AreEqual("bottleId1", result.ToList()[0].BottleId);
            Assert.AreEqual(AlcoholCategory.Whisky, result.ToList()[0].AlcoholCategory);

            Assert.AreEqual("bottleId2", result.ToList()[1].BottleId);
            Assert.AreEqual(AlcoholCategory.RedWine, result.ToList()[1].AlcoholCategory);
        }
    }
}
