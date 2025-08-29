using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver.GeoJsonObjectModel;
using Moq;
using NaviMente.WebApi.Controllers;
using NaviMente.WebApi.Domain.Shared.Entities;
using NaviMente.WebApi.Dto.Device;
using NaviMente.WebApi.Infrastructure.Persistence.Repositories;
using NaviMente.WebApi.Infrastructure.Services;

namespace NaviMente.Tests.Domain
{
    public class DeviceServiceTests
    {
        private readonly Mock<IDeviceQueryRepository> _deviceRepoMock;
        private readonly Mock<ILocationQueryRepository> _locationRepoMock;
        private readonly Mock<IUserQueryRepository> _userRepoMock;
        private readonly Mock<IRestrictedZoneQueryRepository> _restrictedZoneRepoMock;
        private readonly Mock<ICounterQueryRepository> _counterRepoMock;
        private readonly Mock<ILogger<DeviceController>> _loggerMock;
        private readonly DeviceService _service;

        public DeviceServiceTests()
        {
            _deviceRepoMock = new Mock<IDeviceQueryRepository>();
            _locationRepoMock = new Mock<ILocationQueryRepository>();
            _userRepoMock = new Mock<IUserQueryRepository>();
            _restrictedZoneRepoMock = new Mock<IRestrictedZoneQueryRepository>();
            _counterRepoMock = new Mock<ICounterQueryRepository>();
            _loggerMock = new Mock<ILogger<DeviceController>>();

            _service = new DeviceService(
                _loggerMock.Object,
                _deviceRepoMock.Object,
                _userRepoMock.Object,
                _locationRepoMock.Object,
                _restrictedZoneRepoMock.Object,
                _counterRepoMock.Object);
        }

        [Fact]
        public void RegisterDevice_ReturnsDeviceId_WhenSuccessful()
        {
            var dto = new DeviceRegisterDTO
            {
                SerialNumber = "SN123",
                DeviceName = "Device1",
                UserId = 1
            };
            var deviceId = ObjectId.GenerateNewId();

            _deviceRepoMock.Setup(r => r.GetBySerialNumber(dto.SerialNumber)).Returns(new Device { UserId = null });
            _deviceRepoMock.Setup(r => r.GetByNameAndUser(dto.DeviceName, dto.UserId)).Returns((Device)null);
            _deviceRepoMock.Setup(r => r.GetBySerialNumber(dto.SerialNumber)).Returns(new Device { DeviceId = deviceId, AssignedDate = DateTime.Now, isActive = true });

            // Act
            var result = _service.RegisterDevice(dto);

            // Assert
            Assert.Equal(deviceId, result);
            _deviceRepoMock.Verify(r => r.RegisterDevice(dto.SerialNumber, dto.DeviceName, dto.UserId), Times.Once);
        }

        [Fact]
        public void RegisterDevice_ThrowsException_WhenDeviceNotRecognized()
        {
            var dto = new DeviceRegisterDTO { SerialNumber = "SN123", DeviceName = "Device 1", UserId = 1 };
            _deviceRepoMock.Setup(r => r.GetBySerialNumber(dto.SerialNumber)).Returns((Device)null);

            var ex = Assert.Throws<Exception>(() => _service.RegisterDevice(dto));
            Assert.Equal("This NaviBand is not recognized, get in contact with NaviMente", ex.Message);
        }

        [Fact]
        public void RegisterDevice_ThrowsException_WhenDeviceAlreadyInUse()
        {
            var dto = new DeviceRegisterDTO { SerialNumber = "SN123", DeviceName = "Device 1", UserId = 1 };
            _deviceRepoMock.Setup(r => r.GetBySerialNumber(dto.SerialNumber)).Returns(new Device { UserId = 1 });

            var ex = Assert.Throws<Exception>(() => _service.RegisterDevice(dto));
            Assert.Equal("NaviBand already in use", ex.Message);
        }

        [Fact]
        public void RegisterDevice_ThrowsException_WhenDuplicateDeviceName()
        {
            var dto = new DeviceRegisterDTO { SerialNumber = "SN123", DeviceName = "Device1", UserId = 1 };
            _deviceRepoMock.Setup(r => r.GetBySerialNumber(dto.SerialNumber)).Returns(new Device { UserId = null });
            _deviceRepoMock.Setup(r => r.GetByNameAndUser(dto.DeviceName, dto.UserId)).Returns(new Device());

            var ex = Assert.Throws<Exception>(() => _service.RegisterDevice(dto));
            Assert.Equal("You have another NaviBand with that name", ex.Message);
        }

        [Fact]
        public void GetUserDevicesAsync_ReturnsDeviceList_WhenUserExists()
        {
            long userId = 1;
            var user = new User { UserId = userId };
            var devices = new List<Device>
        {
            new Device { SerialNumber = "SN1", DeviceName = "D1" },
            new Device { SerialNumber = "SN2", DeviceName = "D2" }
        };
            var location1 = new Location { Timestamp = DateTime.UtcNow.AddDays(-1) };
            var location2 = new Location { Timestamp = DateTime.UtcNow };

            _userRepoMock.Setup(r => r.GetByUserId(userId)).Returns(user);
            _deviceRepoMock.Setup(r => r.GetByUserId(userId)).Returns(devices);
            _locationRepoMock.Setup(r => r.GetBySerialNumber("SN1")).Returns(location1);
            _locationRepoMock.Setup(r => r.GetBySerialNumber("SN2")).Returns(location2);

            var result = _service.GetUserDevicesAsync(userId);

            Assert.Equal(2, result.Count);
            Assert.Contains(result, d => d.SerialNumber == "SN1" && d.LastUpdate == location1.Timestamp);
            Assert.Contains(result, d => d.SerialNumber == "SN2" && d.LastUpdate == location2.Timestamp);
        }

        [Fact]
        public void GetUserDevicesAsync_ThrowsException_WhenUserNotFound()
        {
            long userId = 1;
            _userRepoMock.Setup(r => r.GetByUserId(userId)).Returns((User)null);

            var ex = Assert.Throws<Exception>(() => _service.GetUserDevicesAsync(userId));
            Assert.Equal($"User '{userId}' not found.", ex.Message);
        }

        [Fact]
        public void UnassignDeviceAsync_ReturnsUser_WhenSuccessful()
        {
            long userId = 1;
            string serialNumber = "SN123";
            var device = new Device { SerialNumber = serialNumber, UserId = userId };
            var user = new User { UserId = userId };

            _deviceRepoMock.Setup(r => r.GetBySerialNumber(serialNumber)).Returns(device);
            _userRepoMock.Setup(r => r.GetByUserId(userId)).Returns(user);

            var result = _service.UnassignDeviceAsync(userId, serialNumber);

            Assert.Equal(user, result);
            _deviceRepoMock.Verify(r => r.UnassignDevice(serialNumber), Times.Once);
        }

        [Fact]
        public void UnassignDeviceAsync_ThrowsException_WhenDeviceNotFound()
        {
            _deviceRepoMock.Setup(r => r.GetBySerialNumber("SN123")).Returns((Device)null);

            var ex = Assert.Throws<Exception>(() => _service.UnassignDeviceAsync(1, "SN123"));
            Assert.Equal("NaviBand with SerialNumber SN123 not found.", ex.Message);
        }

        [Fact]
        public void UnassignDeviceAsync_ThrowsException_WhenDeviceNotAssignedToUser()
        {
            var device = new Device { SerialNumber = "SN123", UserId = 2 };
            _deviceRepoMock.Setup(r => r.GetBySerialNumber("SN123")).Returns(device);

            var ex = Assert.Throws<Exception>(() => _service.UnassignDeviceAsync(1, "SN123"));
            Assert.Equal("This naviBand is not assigned to you", ex.Message);
        }

        [Fact]
        public void AddRestrictedZone_InsertsZonesCorrectly()
        {
            var zoneDto = new ZoneDTO
            {
                SerialNumber = "SN123",
                Shapes = new List<ShapeDTO>
                {
                    new ShapeDTO { Type = "circle", Center = new CoordinateDTO { Lat = 1, Lng = 2 }, Radius = 10 },
                    new ShapeDTO { Type = "rectangle", Bounds = new BoundsDTO{North = 1, South = 0, East = 1, West = 0} },
                    new ShapeDTO { Type = "polygon", Coordinates = new List<List<double>> { new List<double>{0,0}, new List<double>{1,0}, new List<double>{1,1}, new List<double> { 0, 0 } } }
                }
            };

            _counterRepoMock.SetupSequence(c => c.GetNextSequenceValue("zoneId"))
                .Returns(1)
                .Returns(2)
                .Returns(3);

            _restrictedZoneRepoMock.Setup(r => r.Insert(It.IsAny<RestrictedZone>()));

            _service.AddRestrictedZone(zoneDto);

            _restrictedZoneRepoMock.Verify(r => r.Insert(It.IsAny<RestrictedZone>()), Times.Exactly(3));
        }

        [Fact]
        public void GetRestrictedZones_ReturnsZonesTransformed()
        {
            var zones = new List<RestrictedZone>
            {
                new RestrictedZone
                {
                    ZoneId = 1,
                    SerialNumber = "SN123",
                    CreatedAt = DateTime.UtcNow,
                    Type = "circle",
                    Center = new GeoJson2DGeographicCoordinates(10,20),
                    Radius = 15
                },
                new RestrictedZone
                {
                    ZoneId = 2,
                    SerialNumber = "SN123",
                    CreatedAt = DateTime.UtcNow,
                    Type = "rectangle",
                    Bounds = new BoundsDTO
                    {
                        North = 1,
                        South = 0,
                        East = 1,
                        West = 0
                    }
                },
                new RestrictedZone
                {
                    ZoneId = 3,
                    SerialNumber = "SN123",
                    CreatedAt = DateTime.UtcNow,
                    Type = "polygon",
                    Shape = new GeoJsonPolygon<GeoJson2DGeographicCoordinates>(
                        new GeoJsonPolygonCoordinates<GeoJson2DGeographicCoordinates>(
                            new GeoJsonLinearRingCoordinates<GeoJson2DGeographicCoordinates>(
                                new List<GeoJson2DGeographicCoordinates>
                                {
                                    new GeoJson2DGeographicCoordinates(0,0),
                                    new GeoJson2DGeographicCoordinates(1,0),
                                    new GeoJson2DGeographicCoordinates(1,1),
                                    new GeoJson2DGeographicCoordinates(0,1),
                                    new GeoJson2DGeographicCoordinates(0,0)
                                }
                            )
                        )
                    )
                }
            };

            _restrictedZoneRepoMock.Setup(r => r.GetBySerialNumber("SN123")).Returns(zones);

            var result = _service.GetRestrictedZones("SN123");

            Assert.Equal(3, result.Count);

            // Circle
            Assert.Equal(1, result[0].ZoneId);
            Assert.Equal("circle", result[0].Shape.Type);
            Assert.NotNull(result[0].Shape.Center);
            Assert.Equal(15, result[0].Shape.Radius);

            // Rectangle
            Assert.Equal(2, result[1].ZoneId);
            Assert.Equal("rectangle", result[1].Shape.Type);
            Assert.NotNull(result[1].Shape.Bounds);

            // Polygon
            Assert.Equal(3, result[2].ZoneId);
            Assert.Equal("polygon", result[2].Shape.Type);
            Assert.NotNull(result[2].Shape.Coordinates);
            Assert.True(result[2].Shape.Coordinates.Count > 0);
        }

        [Fact]
        public void DeleteZone_CallsRepositoryAndReturnsTrue()
        {
            long zoneId = 1;

            var result = _service.DeleteZone(zoneId);

            _restrictedZoneRepoMock.Verify(r => r.Delete(zoneId), Times.Once);
            Assert.True(result);
        }
    }
}
