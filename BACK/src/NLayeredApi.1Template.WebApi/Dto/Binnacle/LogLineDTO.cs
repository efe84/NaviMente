namespace NaviMente.WebApi.Dto.Binnacle
{
    public class LogLineDTO
    {
        public string Timestamp { get; set; }
        public required long Severity { get; set; }
        public string? Message { get; set; }
    }
}
