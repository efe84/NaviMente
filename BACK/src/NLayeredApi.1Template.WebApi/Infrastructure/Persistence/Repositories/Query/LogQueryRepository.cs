using MongoDB.Driver;
using NaviMente.WebApi.Domain.Shared.Entities;
using System.Diagnostics.CodeAnalysis;

namespace NaviMente.WebApi.Infrastructure.Persistence.Repositories.Query
{
    public class LogQueryRepository: ILogQueryRepository
    {
        private readonly IMongoCollection<LogLine> _logsCollection;

        public LogQueryRepository(IApplicationContext dbContext)
        {
            _logsCollection = dbContext.Logs;
        }

        public List<LogLine> GetBySerialNumberAndSeverity(string serialNumber, int? severity)
        {
            var filter = Builders<LogLine>.Filter.Eq(l => l.SerialNumber, serialNumber);

            if (severity.HasValue)
            {
                var severityFilter = Builders<LogLine>.Filter.Eq(l => l.Severity, severity.Value);
                filter = Builders<LogLine>.Filter.And(filter, severityFilter);
            }

            return _logsCollection
                .Find(filter)
                .SortByDescending(l => l.Timestamp)
                .ToList();
        }
    }
}
