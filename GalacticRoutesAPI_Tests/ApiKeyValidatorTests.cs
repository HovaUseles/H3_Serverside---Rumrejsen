using GalacticRoutesAPI.Exceptions;
using GalacticRoutesAPI.Models;
using GalacticRoutesAPI.Repositories;
using GalacticRoutesAPI.Services;
using System;
using System.Linq;

namespace GalacticRoutesAPI_Tests
{
    public class ApiKeyValidatorTests
    {
        [Fact]
        public void ValidateApiKey_NullApiKey_ThrowsArgumentNullException()
        {
            // Arrange
            var validator = new ApiKeyValidator(Mock.Of<IGenericRepository<Request>>());

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => validator.ValidateApiKey(null));
        }

        [Fact]
        public void ValidateApiKey_NullSpaceTraveler_ThrowsArgumentNullExceptionWithMessage()
        {
            // Arrange
            var apiKey = new ApiKey("keyValue", DateTime.Now.AddMinutes(10), null);
            var validator = new ApiKeyValidator(Mock.Of<IGenericRepository<Request>>());

            // Act & Assert
            var exception = Assert.Throws<ArgumentNullException>(() => validator.ValidateApiKey(apiKey));
            Assert.Equal("SpaceTraveler", exception.ParamName);
        }

        [Fact]
        public void ValidateApiKey_ExpiredApiKey_ReturnsFalse()
        {
            // Arrange
            var apiKey = new ApiKey("keyValue", DateTime.Now.AddMinutes(-1), new SpaceTraveler("John Doe", false, new List<Request>()));
            var validator = new ApiKeyValidator(Mock.Of<IGenericRepository<Request>>());

            // Act
            var result = validator.ValidateApiKey(apiKey);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ValidateApiKey_CadetWithExceededRateLimit_ThrowsRateLimitExceededException()
        {
            // Arrange
            var spaceTraveler = new SpaceTraveler("Cadet1", true, new List<Request>());
            var apiKey = new ApiKey("keyValue", DateTime.Now.AddMinutes(10), spaceTraveler);

            // Mock the repository to return recent requests count
            var requestRepositoryMock = new Mock<IGenericRepository<Request>>();
            requestRepositoryMock.Setup(repo => repo.GetAll())
                .Returns(new[]
                {
                new Request(spaceTraveler) { RequestTime = DateTime.Now.AddMinutes(-25) },
                new Request(spaceTraveler) { RequestTime = DateTime.Now.AddMinutes(-20) },
                new Request(spaceTraveler) { RequestTime = DateTime.Now.AddMinutes(-15) },
                new Request(spaceTraveler) { RequestTime = DateTime.Now.AddMinutes(-10) },
                new Request(spaceTraveler) { RequestTime = DateTime.Now.AddMinutes(-5) }
            });

            var validator = new ApiKeyValidator(requestRepositoryMock.Object);

            // Act & Assert
            var exception = Assert.Throws<RateLimitExceededException>(() => validator.ValidateApiKey(apiKey));
            Assert.Equal(5, exception.Rate);
        }

        [Fact]
        public void ValidateApiKey_CadetWithinRateLimit_ReturnsTrue()
        {
            // Arrange
            var spaceTraveler = new SpaceTraveler("Cadet2", true, new List<Request>());
            var apiKey = new ApiKey("keyValue", DateTime.Now.AddMinutes(10), spaceTraveler);

            // Mock the repository to return recent requests count
            var requestRepositoryMock = new Mock<IGenericRepository<Request>>();
            requestRepositoryMock.Setup(repo => repo.GetAll())
                .Returns(new Request[]
                {
                    new Request(spaceTraveler) { RequestTime = DateTime.Now.AddMinutes(-15) },
                    new Request(spaceTraveler) { RequestTime = DateTime.Now.AddMinutes(-10) },
                    new Request(spaceTraveler) { RequestTime = DateTime.Now.AddMinutes(-5) }
                });

            var validator = new ApiKeyValidator(requestRepositoryMock.Object);

            // Act
            var result = validator.ValidateApiKey(apiKey);

            // Assert
            Assert.True(result);
        }
    }

}