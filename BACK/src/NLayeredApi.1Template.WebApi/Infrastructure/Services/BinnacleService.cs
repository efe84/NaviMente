using MongoDB.Driver;
using NaviMente.WebApi.Controllers;
using NaviMente.WebApi.Domain.Shared.Entities;
using NaviMente.WebApi.Dto.Binnacle;
using NaviMente.WebApi.Infrastructure.Persistence;

namespace NaviMente.WebApi.Infrastructure.Services
{
    public class BinnacleService
    {
        private readonly IMongoCollection<LogLine> _logsCollection;
        private readonly ILogger<BinnacleController> _logger;

        public BinnacleService(ApplicationContext dbContext, ILogger<BinnacleController> logger)
        {
            _logsCollection = dbContext.Logs;
            _logger = logger;
        }

        public async Task<List<LogLineDTO>> GetDeviceLogsAsync(string serialNumber, int? severity)
        {
            var filter = Builders<LogLine>.Filter.Eq(l => l.SerialNumber, serialNumber);

            if (severity.HasValue)
            {
                var severityFilter = Builders<LogLine>.Filter.Eq(l => l.Severity, severity.Value);
                filter = Builders<LogLine>.Filter.And(filter, severityFilter);
            }

            List<LogLine> logs = await _logsCollection
                .Find(filter)
                .SortBy(l => l.Timestamp)
                .ToListAsync();

            List<LogLineDTO> result = new List<LogLineDTO>();
            foreach (LogLine log in logs)
            {
                result.Add(new LogLineDTO
                {
                    Severity = log.Severity,
                    Timestamp = log.Timestamp.ToString("dd/MM/yyyy HH:mm:ss"),
                    Message = log.Message
                });
            }

            return result;
        }
    }
}
