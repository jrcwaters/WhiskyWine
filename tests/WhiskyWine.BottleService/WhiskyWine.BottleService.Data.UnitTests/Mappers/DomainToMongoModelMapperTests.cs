using MongoDB.Bson;
using NUnit.Framework;
using WhiskyWine.BottleService.Data.Mappers;
using WhiskyWine.BottleService.Domain.Models;

namespace WhiskyWine.BottleService.Data.UnitTests.Mappers
{
    public class DomainToMongoModelMapperTests
    {
        private DomainToMongoModelMapper _mapper;

        [SetUp]
        public void SetUp()
        {
            _mapper = new DomainToMongoModelMapper();
        }

        [Test]
        public void Map_ReturnsNull_IfNullParamPassed()
        {
            //Arrange
            Bottle fromBottle = null;

            //Act
            var result = _mapper.Map(fromBottle);

            //Assert
            Assert.IsNull(result);
        }

        [Test]
        public void Map_ReturnsMongoBottleWithEmptyObjectId_WhenDomainIdNotParseable()
        {
            //Arrange
            var fromBottle = new Bottle{ BottleId = "bottleId" };

            //Act
            var result = _mapper.Map(fromBottle);

            //Assert
            Assert.AreEqual(ObjectId.Empty, result.BottleId);
        }

        [Test]
        public void Map_ReturnsMongoBottleWithValidObjectId_WhenDomainIdParseable()
        {
            //Arrange
            var idAsString = "507f1f77bcf86cd799439011";
            var fromBottle = new Bottle{ BottleId = idAsString };

            var expectedObjectId = new ObjectId(idAsString);

            //Act
            var result = _mapper.Map(fromBottle);

            //Assert
            Assert.AreEqual(expectedObjectId, result.BottleId);
        }

        [Test]
        public void Map_ReturnsMongoBottleWithNamePropertySet_WhenBottleWithNonNullNamePassed()
        {
            //Arrange
            var expectedBottleName = "bottleName";
            var fromBottle = new Bottle{ Name = expectedBottleName };

            //Act
            var result = _mapper.Map(fromBottle);

            //Assert
            Assert.AreEqual(expectedBottleName, result.Name);
        }

        [Test]
        public void Map_ReturnsMongoBottleWithRegionPropertySet_WhenBottleWithNonNullRegionPassed()
        {
            //Arrange
            var expectedBottleRegion = "bottleRegion";
            var fromBottle = new Bottle{ Region = expectedBottleRegion };

            //Act
            var result = _mapper.Map(fromBottle);

            //Assert
            Assert.AreEqual(expectedBottleRegion, result.Region);
        }

        [Test]
        public void Map_ReturnsMongoBottleWithAlcoholCategorySet_WhenBottleWithNonNullCategoryPassed()
        {
            //Arrange
            var expectedAlcoholCategory = "whisky";
            var fromBottle = new Bottle{ AlcoholCategory = expectedAlcoholCategory };

            //Act
            var result = _mapper.Map(fromBottle);

            //Assert
            Assert.AreEqual(expectedAlcoholCategory, result.AlcoholCategory);
        }
    }
}
