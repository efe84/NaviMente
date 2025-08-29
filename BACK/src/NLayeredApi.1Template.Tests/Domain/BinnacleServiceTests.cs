using Microsoft.Extensions.Logging;
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
    public class BinnacleServiceTests
    {
        private readonly Mock<ILogQueryRepository> _logQueryRepositoryMock;
        private readonly Mock<ILogger<BinnacleController>> _loggerMock;
        private readonly BinnacleService _service;

        public BinnacleServiceTests()
        {
            _logQueryRepositoryMock = new Mock<ILogQueryRepository>();
            _loggerMock = new Mock<ILogger<BinnacleController>>();
            _service = new BinnacleService(_loggerMock.Object, _logQueryRepositoryMock.Object);
        }

        [Fact]
        public void GetDeviceLogs_ReturnsMappedLogs_WhenLogsExist()
        {
            // Arrange
            var serialNumber = "SN123";
            int? severity = 2;

            var logs = new List<LogLine>
        {
            new LogLine
            {
                SerialNumber = serialNumber,
                Timestamp = new DateTime(2023, 1, 1, 10, 20, 30),
                Severity = 2,
                Message = "Log 1"
            },
            new LogLine
            {
                SerialNumber = serialNumber,
                Timestamp = new DateTime(2023, 1, 1, 11, 15, 0),
                Severity = 2,
                Message = "Log 2"
            }
        };

            _logQueryRepositoryMock
                .Setup(r => r.GetBySerialNumberAndSeverity(serialNumber, severity))
                .Returns(logs);

            // Act
            var result = _service.GetDeviceLogs(serialNumber, severity);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(logs.Count, result.Count);

            for (int i = 0; i < logs.Count; i++)
            {
                Assert.Equal(logs[i].Severity, result[i].Severity);
                Assert.Equal(logs[i].Timestamp.ToString("dd/MM/yyyy HH:mm:ss"), result[i].Timestamp);
                Assert.Equal(logs[i].Message, result[i].Message);
            }

            // Verify logging
            _loggerMock.Verify(
                x => x.Log(
                    LogLevel.Information,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString().Contains(serialNumber)),
                    null,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()),
                Times.Once);
        }

        [Fact]
        public void GetDeviceLogs_ReturnsEmptyList_WhenNoLogsFound()
        {
            // Arrange
            var serialNumber = "SN999";
            int? severity = null;

            _logQueryRepositoryMock
                .Setup(r => r.GetBySerialNumberAndSeverity(serialNumber, severity))
                .Returns(new List<LogLine>());

            // Act
            var result = _service.GetDeviceLogs(serialNumber, severity);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }
    }
}
