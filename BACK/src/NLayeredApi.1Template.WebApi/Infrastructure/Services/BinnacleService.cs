using NaviMente.WebApi.Controllers;
using NaviMente.WebApi.Domain.Shared.Entities;
using NaviMente.WebApi.Dto.Binnacle;
using NaviMente.WebApi.Infrastructure.Persistence.Repositories;

namespace NaviMente.WebApi.Infrastructure.Services
{
    public class BinnacleService: IBinnacleService
    {
        private readonly ILogQueryRepository _logQueryRepository;
        private readonly ILogger<BinnacleController> _logger;

        public BinnacleService(ILogger<BinnacleController> logger, ILogQueryRepository logQueryRepository)
        {
            _logQueryRepository = logQueryRepository;
            _logger = logger;
        }

        public List<LogLineDTO> GetDeviceLogs(string serialNumber, int? severity)
        {
            _logger.LogInformation("Recuperando logs del dispositivo {serialNumber}", serialNumber);

            List<LogLine> logs = _logQueryRepository.GetBySerialNumberAndSeverity(serialNumber, severity);

            List<LogLineDTO> result = [];
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
