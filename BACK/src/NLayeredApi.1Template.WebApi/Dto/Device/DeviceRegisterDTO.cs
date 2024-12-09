using MongoDB.Bson.Serialization.Attributes;

namespace NaviMente.WebApi.Dto.Device
{
    public class DeviceRegisterDTO
    {
        public string? DeviceName { get; set; }
        public string? SerialNumber { get; set; }
        public long? UserId { get; set; }
    }
}
