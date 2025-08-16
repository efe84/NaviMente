using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NaviMente.WebApi.Controllers;
using NaviMente.WebApi.Dto.Location;
using NaviMente.WebApi.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaviMente.Tests.Domain
{
    public class LocationControllerTests
    {
        private readonly Mock<ILogger<LocationController>> _loggerMock;
        private readonly Mock<ILocationService> _locationServiceMock;
        private readonly LocationController _controller;

        public LocationControllerTests()
        {
            _loggerMock = new Mock<ILogger<LocationController>>();
            _locationServiceMock = new Mock<ILocationService>();
            _controller = new LocationController(_loggerMock.Object, _locationServiceMock.Object);
        }

        [Fact]
        public void GetLocation_ReturnsOk_WhenLocationFound()
        {
            // Arrange
            var dto = new LocationDTO { SerialNumber = "SN123", TimeStamp = DateTime.UtcNow };
            var expectedLocation = new LocationPointDTO { Latitude = 1.23, Longitude = 4.56 };

            _locationServiceMock.Setup(s => s.GetLocation(dto.SerialNumber, dto.TimeStamp))
                                .Returns(expectedLocation);

            // Act
            var result = _controller.GetLocation(dto) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(expectedLocation, result.Value);
        }

        [Fact]
        public void GetLocation_ReturnsBadRequest_WhenExceptionThrown()
        {
            // Arrange
            var dto = new LocationDTO { SerialNumber = "SN123", TimeStamp = DateTime.UtcNow };

            _locationServiceMock.Setup(s => s.GetLocation(dto.SerialNumber, dto.TimeStamp))
                                .Throws(new Exception("Test error"));

            // Act
            var result = _controller.GetLocation(dto);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void GetLastLocation_ReturnsOk_WhenLocationFound()
        {
            // Arrange
            var serialNumber = "SN123";
            var expectedLocation = new LocationPointDTO { Latitude = 9.87, Longitude = 6.54, SerialNumber = "SN123", Timestamp = DateTime.UtcNow };

            _locationServiceMock.Setup(s => s.GetLastLocation(serialNumber))
                                .Returns(expectedLocation);

            // Act
            var result = _controller.GetLastLocation(serialNumber) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(expectedLocation, result.Value);
            var actual = Assert.IsType<LocationPointDTO>(result.Value);
            Assert.Equal(expectedLocation.Latitude, actual.Latitude);
            Assert.Equal(expectedLocation.Longitude, actual.Longitude);
            Assert.Equal(expectedLocation.SerialNumber, actual.SerialNumber);
            Assert.Equal(expectedLocation.Timestamp, actual.Timestamp);
        }

        [Fact]
        public void GetLastLocation_ReturnsBadRequest_WhenExceptionThrown()
        {
            // Arrange
            var serialNumber = "SN123";

            _locationServiceMock.Setup(s => s.GetLastLocation(serialNumber))
                                .Throws(new Exception("Test error"));

            // Act
            var result = _controller.GetLastLocation(serialNumber);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void GetRoute_ReturnsOk_WhenRouteFound()
        {
            // Arrange
            var dto = new RouteDTO
            {
                SerialNumber = "SN123",
                StartDate = DateTime.UtcNow.AddDays(-1),
                EndDate = DateTime.UtcNow
            };
            var expectedRoute = new LocationLineDTO();

            _locationServiceMock.Setup(s => s.GetRoute(dto.SerialNumber, dto.StartDate, dto.EndDate))
                                .Returns(expectedRoute);

            // Act
            var result = _controller.GetRoute(dto) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(expectedRoute, result.Value);
        }

        [Fact]
        public void GetRoute_ReturnsBadRequest_WhenExceptionThrown()
        {
            // Arrange
            var dto = new RouteDTO
            {
                SerialNumber = "SN123",
                StartDate = DateTime.UtcNow.AddDays(-1),
                EndDate = DateTime.UtcNow
            };

            _locationServiceMock.Setup(s => s.GetRoute(dto.SerialNumber, dto.StartDate, dto.EndDate))
                                .Throws(new Exception("Test error"));

            // Act
            var result = _controller.GetRoute(dto);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }
    }
}
