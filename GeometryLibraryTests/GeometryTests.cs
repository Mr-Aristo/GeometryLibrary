using System;
using System.Threading.Tasks;
using Xunit;
using Moq;
using Serilog;
using GeometryLibrary.Concretes;
using GeometryLibrary;
using StackExchange.Redis;

namespace GeometryLibraryTests
{
    public class GeometryTests
    {


        [Fact]
        public async Task CalculateAreaAsync_Circle_CalculatesAndCachesArea()
        {
            // Arrange
            var loggerMock = new Mock<ILogger>();
            var redisCacheMock = new Mock<IDatabase>();
            var circle = new Circle(5);
            var calculator = new ShapeAreaCalculator<Circle>(loggerMock.Object, redisCacheMock.Object);

            // Act
            double area = await calculator.CalculateAreaAsync(circle);

            // Assert
            Assert.Equal(Math.PI * 25, area, precision: 5);
            redisCacheMock.Verify(db => db.StringSetAsync(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), It.IsAny<TimeSpan?>(), It.IsAny<bool>(), It.IsAny<When>(), It.IsAny<CommandFlags>()), Times.Once);
            loggerMock.Verify(l => l.Information(It.IsAny<string>(), It.IsAny<object[]>()), Times.Exactly(2));
        }

        [Fact]
        public async Task CalculateAreaAsync_Circle_ReturnsCachedArea()
        {
            // Arrange
            var loggerMock = new Mock<ILogger>();
            var redisCacheMock = new Mock<IDatabase>();
            var circle = new Circle(5);
            var cachedArea = (Math.PI * 25).ToString();
            redisCacheMock.Setup(db => db.StringGetAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>())).ReturnsAsync(cachedArea);
            var calculator = new ShapeAreaCalculator<Circle>(loggerMock.Object, redisCacheMock.Object);

            // Act
            double area = await calculator.CalculateAreaAsync(circle);

            // Assert
            Assert.Equal(Math.PI * 25, area, precision: 5);
            redisCacheMock.Verify(db => db.StringGetAsync(It.IsAny<RedisKey>(), It.IsAny<CommandFlags>()), Times.Once);
            redisCacheMock.Verify(db => db.StringSetAsync(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), It.IsAny<TimeSpan?>(), It.IsAny<bool>(), It.IsAny<When>(), It.IsAny<CommandFlags>()), Times.Never);
            loggerMock.Verify(l => l.Information(It.IsAny<string>(), It.IsAny<object[]>()), Times.Once);
        }

        [Fact]
        public async Task CalculateAreaAsync_Triangle_CalculatesAndCachesArea()
        {
            // Arrange
            var loggerMock = new Mock<ILogger>();
            var redisCacheMock = new Mock<IDatabase>();
            var triangle = new Triangle(3, 4, 5);
            var calculator = new ShapeAreaCalculator<Triangle>(loggerMock.Object, redisCacheMock.Object);

            // Act
            double area = await calculator.CalculateAreaAsync(triangle);

            // Assert
            Assert.Equal(6, area, precision: 5);
            redisCacheMock.Verify(db => db.StringSetAsync(It.IsAny<RedisKey>(), It.IsAny<RedisValue>(), It.IsAny<TimeSpan?>(), It.IsAny<bool>(), It.IsAny<When>(), It.IsAny<CommandFlags>()), Times.Once);
            loggerMock.Verify(l => l.Information(It.IsAny<string>(), It.IsAny<object[]>()), Times.Exactly(2));
        }

        [Fact]
        public void Triangle_IsRightAngled_ReturnsTrueForRightTriangle()
        {
            // Arrange
            var triangle = new Triangle(3, 4, 5);

            // Act
            bool isRightAngled = triangle.IsRightAngled();

            // Assert
            Assert.True(isRightAngled);
        }

        [Fact]
        public void Triangle_IsRightAngled_ReturnsFalseForNonRightTriangle()
        {
            // Arrange
            var triangle = new Triangle(3, 4, 6);

            // Act
            bool isRightAngled = triangle.IsRightAngled();

            // Assert
            Assert.False(isRightAngled);
        }
    }
}
