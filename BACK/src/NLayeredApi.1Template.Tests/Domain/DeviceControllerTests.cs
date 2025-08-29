using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using Moq;
using NaviMente.WebApi.Controllers;
using NaviMente.WebApi.Domain.Shared.Entities;
using NaviMente.WebApi.Dto.Device;
using NaviMente.WebApi.Infrastructure.Services;

namespace NaviMente.Tests.Domain
{
    public class DeviceControllerTests
    {
        private readonly Mock<IDeviceService> _deviceServiceMock;
        private readonly Mock<ILogger<DeviceController>> _loggerMock;
        private readonly DeviceController _controller;

        public DeviceControllerTests()
        {
            _deviceServiceMock = new Mock<IDeviceService>();
            _loggerMock = new Mock<ILogger<DeviceController>>();
            _controller = new DeviceController(_loggerMock.Object, _deviceServiceMock.Object);
        }

        [Fact]
        public void Register_ReturnsOk_WhenDeviceRegistered()
        {
            // Arrange
            var deviceDto = new DeviceRegisterDTO { SerialNumber = "1234", UserId = 1, DeviceName = "Device1" };
            var fakeId = ObjectId.GenerateNewId();

            _deviceServiceMock.Setup(s => s.RegisterDevice(deviceDto)).Returns(fakeId);

            // Act
            var result = _controller.Register(deviceDto) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(fakeId, result.Value);
        }

        [Fact]
        public void Register_ReturnsBadRequest_WhenDeviceIdNull()
        {
            // Arrange
            var deviceDto = new DeviceRegisterDTO { SerialNumber = "1234", UserId = 1, DeviceName = "Device1" };

            _deviceServiceMock.Setup(s => s.RegisterDevice(deviceDto)).Returns((ObjectId?)null);

            // Act
            var result = _controller.Register(deviceDto) as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
            Assert.Equal("Error al registrar un dispositivo nuevo", result.Value);
        }

        [Fact]
        public void Register_ReturnsBadRequest_WhenExceptionThrown()
        {
            // Arrange
            var deviceDto = new DeviceRegisterDTO { SerialNumber = "1234", UserId = 1, DeviceName = "Device1" };

            _deviceServiceMock.Setup(s => s.RegisterDevice(deviceDto)).Throws(new Exception("fail"));

            // Act
            var result = _controller.Register(deviceDto) as BadRequestResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public void GetUserDevices_ReturnsOk_WithDevices()
        {
            // Arrange
            var userId = "123";
            var devices = new List<DeviceDTO> { new DeviceDTO { SerialNumber = "SN1" } };

            _deviceServiceMock.Setup(s => s.GetUserDevicesAsync(123)).Returns(devices);

            // Act
            var result = _controller.GetUserDevices(userId) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(devices, result.Value);
        }

        [Fact]
        public void GetUserDevices_ReturnsBadRequest_WhenExceptionThrown()
        {
            // Arrange
            var userId = "123";

            _deviceServiceMock.Setup(s => s.GetUserDevicesAsync(It.IsAny<long>())).Throws(new Exception());

            // Act
            var result = _controller.GetUserDevices(userId) as BadRequestResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public void UnassignDevice_ReturnsOk_WithUser()
        {
            // Arrange
            long userId = 1;
            string serial = "SN123";
            var user = new User { UserId = 1, Username = "user" };

            _deviceServiceMock.Setup(s => s.UnassignDeviceAsync(userId, serial)).Returns(user);

            // Act
            var result = _controller.UnassignDevice(userId, serial) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(user, result.Value);
        }

        [Fact]
        public void UnassignDevice_ReturnsBadRequest_WhenExceptionThrown()
        {
            // Arrange
            long userId = 1;
            string serial = "SN123";

            _deviceServiceMock.Setup(s => s.UnassignDeviceAsync(userId, serial)).Throws(new Exception());

            // Act
            var result = _controller.UnassignDevice(userId, serial) as BadRequestResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public void RegisterBlockedZone_ReturnsOk_WithMessage()
        {
            // Arrange
            var zoneDto = new ZoneDTO { SerialNumber = "SN1", Shapes = [] };

            // Act
            var result = _controller.RegisterBlockedZone(zoneDto) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.NotNull(result.Value);
            Assert.Contains("Zona registrada satisfactoriamente", result.Value.ToString());
        }

        [Fact]
        public void RegisterBlockedZone_ReturnsBadRequest_WhenExceptionThrown()
        {
            // Arrange
            var zoneDto = new ZoneDTO { SerialNumber = "SN1", Shapes = [] };
            _deviceServiceMock.Setup(s => s.AddRestrictedZone(zoneDto)).Throws(new Exception());

            // Act
            var result = _controller.RegisterBlockedZone(zoneDto) as BadRequestResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public void GetZones_ReturnsOk_WithZones()
        {
            // Arrange
            string serialNumber = "SN123";

            var shape = new ShapeOutputDTO
            {
                Type = "Circle",
                Center = new List<double> { 2.0, 3.0 },
                Radius = 100,
                Coordinates = null,
                Bounds = null
            };

            var zones = new List<ZoneOutputDTO>
            {
                new ZoneOutputDTO
                {
                    ZoneId = 1,
                    SerialNumber = serialNumber,
                    CreatedAt = DateTime.UtcNow,
                    Shape = shape
                }
            };

            _deviceServiceMock.Setup(s => s.GetRestrictedZones(serialNumber)).Returns(zones);

            // Act
            var result = _controller.GetZones(serialNumber) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(zones, result.Value);
        }

        [Fact]
        public void GetZones_ReturnsBadRequest_WhenExceptionThrown()
        {
            // Arrange
            string serialNumber = "SN123";

            _deviceServiceMock.Setup(s => s.GetRestrictedZones(serialNumber)).Throws(new Exception());

            // Act
            var result = _controller.GetZones(serialNumber) as BadRequestResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }

        [Fact]
        public void DeleteZone_ReturnsOk()
        {
            // Arrange
            long zoneId = 42;

            // Act
            var result = _controller.DeleteZone(zoneId) as OkResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
        }

        [Fact]
        public void DeleteZone_ReturnsBadRequest_WhenExceptionThrown()
        {
            // Arrange
            long zoneId = 42;

            _deviceServiceMock.Setup(s => s.DeleteZone(zoneId)).Throws(new Exception());

            // Act
            var result = _controller.DeleteZone(zoneId) as BadRequestResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }
    }
}
