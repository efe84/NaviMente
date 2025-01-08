using MongoDB.Bson;
using MongoDB.Driver;
using NaviMente.WebApi.Controllers;
using NaviMente.WebApi.Domain.Shared.Entities;
using NaviMente.WebApi.Dto.Binnacle;
using NaviMente.WebApi.Dto.Device;
using NaviMente.WebApi.Infrastructure.Persistence;
using Serilog.Events;
using LogEvent = NaviMente.WebApi.Domain.Shared.Entities.LogEvent;

namespace NaviMente.WebApi.Infrastructure.Services
{
    public class BinnacleService
    {
        private readonly IMongoCollection<LogRegister> _logsCollection;
        private readonly ILogger<BinnacleController> _logger;

        public BinnacleService(ApplicationContext dbContext, ILogger<BinnacleController> logger)
        {
            _logsCollection = dbContext.Logs;
            _logger = logger;
        }

        public async Task<List<BinnacleDTO>> GetDeviceLogsAsync(string serialNumber)
        {
            var filtro = Builders<LogRegister>.Filter.
                Regex(log => log.FileName, new BsonRegularExpression($@"_\d*{serialNumber}\.log$"));

            List<LogRegister> logs = await _logsCollection.Find(filtro).ToListAsync();

            List<LogEvent> logEvents = logs.SelectMany(log => log.Content ?? new List<LogEvent>()).ToList();

            List<BinnacleDTO> groupedByDate = logEvents.GroupBy(logEvent => logEvent.Date?.Date)
                                 .Where(group => group.Key.HasValue)
                                 .Select(group => new BinnacleDTO
                                 {
                                     Date = group.Key.Value,
                                     DayEvents = group.ToList()
                                 })
                                 .OrderBy(binnacle => binnacle.Date)
                                 .ToList();

            return groupedByDate;
        }
    }
}
