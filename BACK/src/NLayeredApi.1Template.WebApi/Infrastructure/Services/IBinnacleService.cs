using NaviMente.WebApi.Dto.Binnacle;

namespace NaviMente.WebApi.Infrastructure.Services
{
    public interface IBinnacleService
    {
        List<LogLineDTO> GetDeviceLogs(string serialNumber, int? severity);
    }
}
