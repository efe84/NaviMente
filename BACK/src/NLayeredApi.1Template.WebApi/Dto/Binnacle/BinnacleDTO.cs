using NaviMente.WebApi.Domain.Shared.Entities;

namespace NaviMente.WebApi.Dto.Binnacle
{
    public class BinnacleDTO
    {
        public DateTime? Date { get; set; }
        public List<LogEvent>? DayEvents { get; set; }
    }
}
