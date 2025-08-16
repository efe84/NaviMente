using MongoDB.Bson.Serialization.Attributes;

namespace NaviMente.WebApi.Dto.Device
{
    public class DeviceRegisterDTO
    {
        public required string DeviceName { get; set; }
        public required string SerialNumber { get; set; }
        public required long UserId { get; set; }
    }
}
