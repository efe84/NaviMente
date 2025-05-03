namespace NaviMente.WebApi.Dto.Device
{ 
    public class DeviceDTO
    {
        public required string SerialNumber { get; set; }
        public string? Name { get; set; }
        public DateTime? LastUpdate { get; set; }
    }
}
