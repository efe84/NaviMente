using MongoDB.Driver;
using Moq;
using NaviMente.WebApi.Domain.Shared.Entities;
using NaviMente.WebApi.Infrastructure.Persistence;
using NaviMente.WebApi.Infrastructure.Persistence.Repositories.Query;

namespace NaviMente.Tests.Domain
{
    public class CounterQueryRepositoryTests
    {
        [Fact]
        public void GetNextSequenceValue_ReturnsIncrementedValue()
        {
            // Arrange
            var mockCollection = new Mock<IMongoCollection<Counter>>();
            var dbContextMock = new Mock<IApplicationContext>();

            var expectedCounter = new Counter
            {
                Id = "zoneId",
                SequenceValue = 42
            };

            dbContextMock
                .Setup(c => c.Counters)
                .Returns(mockCollection.Object);

            mockCollection
                .Setup(c => c.FindOneAndUpdate(
                    It.IsAny<FilterDefinition<Counter>>(),
                    It.IsAny<UpdateDefinition<Counter>>(),
                    It.IsAny<FindOneAndUpdateOptions<Counter, Counter>>(),
                    default
                ))
                .Returns(expectedCounter);

            var repository = new CounterQueryRepository(dbContextMock.Object);

            // Act
            var result = repository.GetNextSequenceValue("zoneId");

            // Assert
            Assert.Equal(42, result);
            mockCollection.Verify(c => c.FindOneAndUpdate(
                It.IsAny<FilterDefinition<Counter>>(),
                It.IsAny<UpdateDefinition<Counter>>(),
                It.IsAny<FindOneAndUpdateOptions<Counter, Counter>>(),
                default
            ), Times.Once);
        }
    }
}
