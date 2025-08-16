using Microsoft.Extensions.Logging;
using MongoDB.Driver.GeoJsonObjectModel;
using Moq;
using NaviMente.WebApi.Controllers;
using NaviMente.WebApi.Domain.Shared.Entities;
using NaviMente.WebApi.Infrastructure.Persistence.Repositories;
using NaviMente.WebApi.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaviMente.Tests.Domain
{
    public class LocationServiceTests
    {
        private readonly Mock<ILocationQueryRepository> _locationRepoMock;
        private readonly Mock<ILogger<LocationController>> _loggerMock;
        private readonly LocationService _service;

        public LocationServiceTests()
        {
            _locationRepoMock = new Mock<ILocationQueryRepository>();
            _loggerMock = new Mock<ILogger<LocationController>>();
            _service = new LocationService(_loggerMock.Object, _locationRepoMock.Object);
        }

        [Fact]
        public void GetLocation_ReturnsLocationPointDTO_WhenPointExists()
        {
            // Arrange
            var serial = "SN123";
            var timestamp = DateTime.UtcNow;
            var point = new GeoJsonPoint<GeoJson2DCoordinates>(new GeoJson2DCoordinates(12.34, 56.78));
            var location = new Location
            {
                SerialNumber = serial,
                Timestamp = timestamp,
                LocationData = point
            };

            _locationRepoMock.Setup(r => r.GetBySerialNumberAndTimestamp(serial, timestamp))
                             .Returns(location);

            // Act
            var result = _service.GetLocation(serial, timestamp);

            // Assert
            Assert.Equal(serial, result.SerialNumber);
            Assert.Equal(timestamp, result.Timestamp);
            Assert.Equal(12.34, result.Longitude);
            Assert.Equal(56.78, result.Latitude);
        }

        [Fact]
        public void GetLocation_ThrowsException_WhenLocationNotFound()
        {
            // Arrange
            var serial = "SN404";
            var timestamp = DateTime.UtcNow;

            _locationRepoMock.Setup(r => r.GetBySerialNumberAndTimestamp(serial, timestamp))
                             .Returns((Location)null);

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => _service.GetLocation(serial, timestamp));
            Assert.Equal("Ubicacion no encontrada", ex.Message);
        }

        [Fact]
        public void GetLocation_ThrowsException_WhenLocationDataIsNotPoint()
        {
            // Arrange
            var serial = "SN123";
            var timestamp = DateTime.UtcNow;
            var coordinates = new[]{new GeoJson2DCoordinates(1.0, 2.0),new GeoJson2DCoordinates(3.0, 4.0)};
            var lineStringCoordinates = new GeoJsonLineStringCoordinates<GeoJson2DCoordinates>(coordinates);
            var invalidData = new GeoJsonLineString<GeoJson2DCoordinates>(lineStringCoordinates);

            var location = new Location
            {
                SerialNumber = serial,
                Timestamp = timestamp,
                LocationData = invalidData
            };

            _locationRepoMock.Setup(r => r.GetBySerialNumberAndTimestamp(serial, timestamp))
                             .Returns(location);

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => _service.GetLocation(serial, timestamp));
            Assert.Equal("Tipo de location no correspondiente a Point", ex.Message);
        }

        [Fact]
        public void GetLastLocation_ReturnsNull_WhenNotFound()
        {
            // Arrange
            var serial = "SN000";

            _locationRepoMock.Setup(r => r.GetLastBySerialNumber(serial))
                             .Returns((Location)null);

            // Act
            var result = _service.GetLastLocation(serial);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void GetLastLocation_ReturnsLocationPointDTO_WhenFound()
        {
            // Arrange
            var serial = "SN123";
            var timestamp = DateTime.UtcNow;
            var point = new GeoJsonPoint<GeoJson2DCoordinates>(new GeoJson2DCoordinates(98.76, 54.32));

            var location = new Location
            {
                SerialNumber = serial,
                Timestamp = timestamp,
                LocationData = point
            };

            _locationRepoMock.Setup(r => r.GetLastBySerialNumber(serial))
                             .Returns(location);

            // Act
            var result = _service.GetLastLocation(serial);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(serial, result.SerialNumber);
            Assert.Equal(timestamp, result.Timestamp);
            Assert.Equal(98.76, result.Longitude);
            Assert.Equal(54.32, result.Latitude);
        }

        [Fact]
        public void GetLastLocation_ThrowsException_WhenLocationDataIsNotPoint()
        {
            // Arrange
            var serial = "SN123";
            var timestamp = DateTime.UtcNow;

            var coordinates = new[]{ new GeoJson2DCoordinates(1.0, 2.0), new GeoJson2DCoordinates(3.0, 4.0)};
            var lineStringCoordinates = new GeoJsonLineStringCoordinates<GeoJson2DCoordinates>(coordinates);
            var invalidData = new GeoJsonLineString<GeoJson2DCoordinates>(lineStringCoordinates);

            var location = new Location
            {
                SerialNumber = serial,
                Timestamp = timestamp,
                LocationData = invalidData
            };

            _locationRepoMock.Setup(r => r.GetLastBySerialNumber(serial))
                             .Returns(location);

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => _service.GetLastLocation(serial));
            Assert.Equal("Tipo de location no correspondiente a Point", ex.Message);
        }

        [Fact]
        public void GetLastLocation_ThrowsException_WhenRepositoryFails()
        {
            // Arrange
            var serial = "SN123";

            _locationRepoMock.Setup(r => r.GetLastBySerialNumber(serial))
                             .Throws(new InvalidOperationException("DB error"));

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() => _service.GetLastLocation(serial));
            Assert.Equal("DB error", ex.Message);
        }

        [Fact]
        public void GetRoute_ReturnsSingleLineString_WhenNoGaps()
        {
            // Arrange
            var serial = "SN123";
            var startDate = DateTime.UtcNow.AddHours(-1);
            var endDate = DateTime.UtcNow;

            var locations = new List<Location>
        {
            new Location
            {
                SerialNumber = serial,
                Timestamp = startDate,
                LocationData = new GeoJsonPoint<GeoJson2DCoordinates>(new GeoJson2DCoordinates(1, 2))
            },
            new Location
            {
                SerialNumber = serial,
                Timestamp = startDate.AddMinutes(10),
                LocationData = new GeoJsonPoint<GeoJson2DCoordinates>(new GeoJson2DCoordinates(3, 4))
            }
        };

            _locationRepoMock.Setup(r => r.GetRoute(serial, startDate, endDate))
                             .Returns(locations);

            // Act
            var result = _service.GetRoute(serial, startDate, endDate);

            // Assert
            Assert.Single(result.Routes);
            Assert.Equal("LineString", result.Routes[0].Type);
            Assert.Equal(new List<double> { 1, 2 }, result.Routes[0].Coordinates[0]);
            Assert.Equal(new List<double> { 3, 4 }, result.Routes[0].Coordinates[1]);
        }

        [Fact]
        public void GetRoute_SplitsIntoMultipleRoutes_WhenGapGreaterThan30Minutes()
        {
            // Arrange
            var serial = "SN123";
            var startDate = DateTime.UtcNow.AddHours(-2);
            var endDate = DateTime.UtcNow;

            var locations = new List<Location>
        {
            new Location
            {
                SerialNumber = serial,
                Timestamp = startDate,
                LocationData = new GeoJsonPoint<GeoJson2DCoordinates>(new GeoJson2DCoordinates(1, 2))
            },
            new Location
            {
                SerialNumber = serial,
                Timestamp = startDate.AddMinutes(10),
                LocationData = new GeoJsonPoint<GeoJson2DCoordinates>(new GeoJson2DCoordinates(3, 4))
            },
            // Gap > 30 min
            new Location
            {
                SerialNumber = serial,
                Timestamp = startDate.AddMinutes(50),
                LocationData = new GeoJsonPoint<GeoJson2DCoordinates>(new GeoJson2DCoordinates(5, 6))
            }
        };

            _locationRepoMock.Setup(r => r.GetRoute(serial, startDate, endDate))
                             .Returns(locations);

            // Act
            var result = _service.GetRoute(serial, startDate, endDate);

            // Assert
            Assert.Equal(2, result.Routes.Count);
            Assert.Equal("LineString", result.Routes[0].Type);
            Assert.Equal("LineString", result.Routes[1].Type);
        }
    }
}
