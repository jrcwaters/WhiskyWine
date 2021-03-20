using MongoDB.Bson;
using NUnit.Framework;
using WhiskyWine.BottleService.Data.Mappers;
using WhiskyWine.BottleService.Domain.Enums;
using WhiskyWine.BottleService.Domain.Models;

namespace WhiskyWine.BottleService.Data.UnitTests.Mappers
{
    /// <summary>
    /// The unit test class for the DomainToMongoModelMapper.
    /// </summary>
    public class DomainToMongoModelMapperTests
    {
        private DomainToMongoModelMapper _mapper;

        /// <summary>
        /// Sets up the class under test. Runs before every unit test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _mapper = new DomainToMongoModelMapper();
        }

        /// <summary>
        /// Test that the Map method returns a null BottleMongoModel if null Bottle passed.
        /// </summary>
        [Test]
        public void Map_ReturnsNull_IfNullParamPassed()
        {
            //Arrange
            BottleDomainModel fromBottle = null;

            //Act
            var result = _mapper.MapOne(fromBottle);

            //Assert
            Assert.IsNull(result);
        }

        /// <summary>
        /// Test that the Map method returns a BottleMongoModel with an empty ObjectId when a Bottle with unparseable BottleId is passed.
        /// </summary>
        [Test]
        public void Map_ReturnsMongoBottleWithEmptyObjectId_WhenDomainIdNotParseable()
        {
            //Arrange
            var fromBottle = new BottleDomainModel{ BottleId = "bottleId" };

            //Act
            var result = _mapper.MapOne(fromBottle);

            //Assert
            Assert.AreEqual(ObjectId.Empty, result.BottleId);
        }

        /// <summary>
        /// Test that the Map method returns a BottleMongoModel with a valid ObjectId when a Bottle with parseable BottleId is passed.
        /// </summary>
        [Test]
        public void Map_ReturnsMongoBottleWithValidObjectId_WhenDomainIdParseable()
        {
            //Arrange
            var idAsString = "507f1f77bcf86cd799439011";
            var fromBottle = new BottleDomainModel{ BottleId = idAsString };

            var expectedObjectId = new ObjectId(idAsString);

            //Act
            var result = _mapper.MapOne(fromBottle);

            //Assert
            Assert.AreEqual(expectedObjectId, result.BottleId);
        }

        /// <summary>
        /// Test that the Map method returns a BottleMongoModel with correct Name property when a Bottle with non-null Name is passed.
        /// </summary>
        [Test]
        public void Map_ReturnsMongoBottleWithNamePropertySet_WhenBottleWithNonNullNamePassed()
        {
            //Arrange
            var expectedBottleName = "bottleName";
            var fromBottle = new BottleDomainModel{ Name = expectedBottleName };

            //Act
            var result = _mapper.MapOne(fromBottle);

            //Assert
            Assert.AreEqual(expectedBottleName, result.Name);
        }

        /// <summary>
        /// Test that the Map method returns a BottleMongoModel with correct Region property when a Bottle with non-null Region is passed.
        /// </summary>
        [Test]
        public void Map_ReturnsMongoBottleWithRegionPropertySet_WhenBottleWithNonNullRegionPassed()
        {
            //Arrange
            var expectedBottleRegion = "bottleRegion";
            var fromBottle = new BottleDomainModel{ Region = expectedBottleRegion };

            //Act
            var result = _mapper.MapOne(fromBottle);

            //Assert
            Assert.AreEqual(expectedBottleRegion, result.Region);
        }

        /// <summary>
        /// Test that the Map method returns a BottleMongoModel with correct AlcoholCategory property when a Bottle with non-null AlcoholProperty is passed.
        /// </summary>
        [Test]
        public void Map_ReturnsMongoBottleWithAlcoholCategorySet_WhenBottleWithNonNullCategoryPassed()
        {
            //Arrange
            var expectedAlcoholCategory = AlcoholCategory.Whisky;
            var fromBottle = new BottleDomainModel{ AlcoholCategory = expectedAlcoholCategory };

            //Act
            var result = _mapper.MapOne(fromBottle);

            //Assert
            Assert.AreEqual(expectedAlcoholCategory, result.AlcoholCategory);
        }
    }
}
