using NaviMente.WebApi.Dto.Location;

namespace NaviMente.WebApi.Infrastructure.Services
{
    public interface ILocationService
    {
        LocationPointDTO GetLocation(string serialNumber, DateTime timestamp);
        LocationPointDTO? GetLastLocation(string serialNumber);
        LocationLineDTO GetRoute(string serialNumber, DateTime startDate, DateTime endDate);
    }
}
