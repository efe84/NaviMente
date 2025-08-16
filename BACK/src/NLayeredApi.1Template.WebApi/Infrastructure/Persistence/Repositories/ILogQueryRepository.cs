using NaviMente.WebApi.Domain.Shared.Entities;

namespace NaviMente.WebApi.Infrastructure.Persistence.Repositories
{
    public interface ILogQueryRepository
    {
        List<LogLine> GetBySerialNumberAndSeverity(string serialNumber, int? severity);
    }
}
