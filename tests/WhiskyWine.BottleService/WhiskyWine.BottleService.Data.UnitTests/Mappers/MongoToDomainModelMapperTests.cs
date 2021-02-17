using MongoDB.Bson;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiskyWine.BottleService.Data.Mappers;
using WhiskyWine.BottleService.Data.Models;

namespace WhiskyWine.BottleService.Data.UnitTests.Mappers
{
    public class MongoToDomainModelMapperTests
    {
        private MongoToDomainModelMapper _mapper;

        [SetUp]
        public void SetUp()
        {
            _mapper = new MongoToDomainModelMapper();
        }

        [Test]
        public void Map_ReturnsNull_IfNullParamPassed()
        {
            //Arrange
            BottleMongoModel fromBottle = null;

            //Act
            var result = _mapper.Map(fromBottle);

            //Assert
            Assert.IsNull(result);
        }

        [Test]
        public void Map_ReturnsBottleWithIdAsString_WhenMongoBottleMapped()
        {
            //Arrange
            var expectedIdString = "507f1f77bcf86cd799439011";
            var mongoBottle = new BottleMongoModel { BottleId = new ObjectId(expectedIdString) };

            //Act
            var result = _mapper.Map(mongoBottle);

            //Assert
            Assert.AreEqual(expectedIdString, result.BottleId);
        }

        [Test]
        public void Map_ReturnsBottleWithNamePropertySet_WhenMongoBottleWithNonNullNamePassed()
        {
            //Arrange
            var expectedName = "bottleName";
            var mongoBottle = new BottleMongoModel { Name = expectedName };

            //Act
            var result = _mapper.Map(mongoBottle);

            //Assert
            Assert.AreEqual(expectedName, result.Name);
        }

        [Test]
        public void Map_ReturnsBottleWithRegionPropertySet_WhenMongoBottleWithNonNullRegionPassed()
        {
            //Arrange
            var expectedRegion = "region";
            var mongoBottle = new BottleMongoModel { Region = expectedRegion };

            //Act
            var result = _mapper.Map(mongoBottle);

            //Assert
            Assert.AreEqual(expectedRegion, result.Region);
        }

        [Test]
        public void Map_ReturnsBottleWithAlcoholPropertySet_WhenMongoBottleWithNonNullCategoryPassed()
        {
            //Arrange
            var expectedCategory = "whisky";
            var mongoBottle = new BottleMongoModel { AlcoholCategory = expectedCategory };

            //Act
            var result = _mapper.Map(mongoBottle);

            //Assert
            Assert.AreEqual(expectedCategory, result.AlcoholCategory);
        }
    }
}
