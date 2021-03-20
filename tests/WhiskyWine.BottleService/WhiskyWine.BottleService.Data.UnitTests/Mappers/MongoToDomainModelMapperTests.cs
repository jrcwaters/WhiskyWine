using MongoDB.Bson;
using NUnit.Framework;
using WhiskyWine.BottleService.Data.Mappers;
using WhiskyWine.BottleService.Data.Models;
using WhiskyWine.BottleService.Domain.Enums;

namespace WhiskyWine.BottleService.Data.UnitTests.Mappers
{
    /// <summary>
    /// The unit test class for the MongoToDomainModelMapper.
    /// </summary>
    public class MongoToDomainModelMapperTests
    {
        private MongoToDomainModelMapper _mapper;

        /// <summary>
        /// Sets up the class under test. Runs before every unit test.
        /// </summary>
        [SetUp]
        public void SetUp()
        {
            _mapper = new MongoToDomainModelMapper();
        }

        /// <summary>
        /// Test that the Map method returns a null Bottleif null BottleMongoModel passed.
        /// </summary>
        [Test]
        public void Map_ReturnsNull_IfNullParamPassed()
        {
            //Arrange
            BottleMongoModel fromBottle = null;

            //Act
            var result = _mapper.MapOne(fromBottle);

            //Assert
            Assert.IsNull(result);
        }

        /// <summary>
        /// Test that the Map method returns a Bottle with BottleId correctly mapped to string.
        /// </summary>
        [Test]
        public void Map_ReturnsBottleWithIdAsString_WhenMongoBottleMapped()
        {
            //Arrange
            var expectedIdString = "507f1f77bcf86cd799439011";
            var mongoBottle = new BottleMongoModel { BottleId = new ObjectId(expectedIdString) };

            //Act
            var result = _mapper.MapOne(mongoBottle);

            //Assert
            Assert.AreEqual(expectedIdString, result.BottleId);
        }

        /// <summary>
        /// Test that the Map method returns a Bottle with correct Name property when a BottleMongoModel with non-null Name is passed.
        /// </summary>
        [Test]
        public void Map_ReturnsBottleWithNamePropertySet_WhenMongoBottleWithNonNullNamePassed()
        {
            //Arrange
            var expectedName = "bottleName";
            var mongoBottle = new BottleMongoModel { Name = expectedName };

            //Act
            var result = _mapper.MapOne(mongoBottle);

            //Assert
            Assert.AreEqual(expectedName, result.Name);
        }

        /// <summary>
        /// Test that the Map method returns a Bottle with correct Region property when a BottleMongoModel with non-null Region is passed.
        /// </summary>
        [Test]
        public void Map_ReturnsBottleWithRegionPropertySet_WhenMongoBottleWithNonNullRegionPassed()
        {
            //Arrange
            var expectedRegion = "region";
            var mongoBottle = new BottleMongoModel { Region = expectedRegion };

            //Act
            var result = _mapper.MapOne(mongoBottle);

            //Assert
            Assert.AreEqual(expectedRegion, result.Region);
        }

        /// <summary>
        /// Test that the Map method returns a Bottle with correct AlcoholCategory property when a BottleMongoModel with non-null AlcoholCategory is passed.
        /// </summary>
        [Test]
        public void Map_ReturnsBottleWithAlcoholPropertySet_WhenMongoBottleWithNonNullCategoryPassed()
        {
            //Arrange
            var expectedCategory = AlcoholCategory.Whisky;
            var mongoBottle = new BottleMongoModel { AlcoholCategory = expectedCategory };

            //Act
            var result = _mapper.MapOne(mongoBottle);

            //Assert
            Assert.AreEqual(expectedCategory, result.AlcoholCategory);
        }
    }
}
