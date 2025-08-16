using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NaviMente.WebApi.Controllers;
using NaviMente.WebApi.Dto.Binnacle;
using NaviMente.WebApi.Infrastructure.Services;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NaviMente.Tests.Domain
{
    public class BinnacleControllerTests
    {
        private readonly Mock<ILogger<BinnacleController>> _loggerMock;
        private readonly Mock<IBinnacleService> _binnacleServiceMock;
        private readonly BinnacleController _controller;

        public BinnacleControllerTests()
        {
            _loggerMock = new Mock<ILogger<BinnacleController>>();
            _binnacleServiceMock = new Mock<IBinnacleService>();
            _controller = new BinnacleController(_loggerMock.Object, _binnacleServiceMock.Object);
        }

        [Fact]
        public void GetDeviceLogs_ReturnsOk_WhenLogsFound()
        {
            // Arrange
            var serial = "SN123";
            int? severity = 2;
            var expectedLogs = new List<LogLineDTO>
            {
                new LogLineDTO
                {
                    Timestamp = DateTime.UtcNow.ToString("o"),
                    Severity = 2,
                    Message = "Primer log de prueba"
                },
                new LogLineDTO
                {
                    Timestamp = DateTime.UtcNow.AddMinutes(-1).ToString("o"),
                    Severity = 1,
                    Message = "Segundo log de prueba"
                }
            };

            _binnacleServiceMock.Setup(s => s.GetDeviceLogs(serial, severity))
                                .Returns(expectedLogs);

            // Act
            var result = _controller.GetDeviceLogs(serial, severity) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(expectedLogs, result.Value);
        }

        [Fact]
        public void GetDeviceLogs_ReturnsBadRequest_WhenServiceThrows()
        {
            // Arrange
            var serial = "SN999";
            int? severity = null;

            _binnacleServiceMock.Setup(s => s.GetDeviceLogs(serial, severity))
                                .Throws(new Exception("DB error"));

            // Act
            var result = _controller.GetDeviceLogs(serial, severity) as BadRequestResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(400, result.StatusCode);
        }
    }
}
