using NaviMente.WebApi.Domain.Shared.Entities;

namespace NaviMente.WebApi.Infrastructure.Persistence.Repositories
{
    public interface ILocationQueryRepository
    {
        Location? GetBySerialNumber(string serialNumber);
        Location? GetBySerialNumberAndTimestamp(string serialNumber, DateTime timestamp);
        Location? GetLastBySerialNumber(string serialNumber);
        List<Location>? GetRoute(string serialNumber, DateTime startDate, DateTime endDate);
    }
}
