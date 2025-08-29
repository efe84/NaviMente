using MongoDB.Bson;
using NaviMente.WebApi.Domain.Shared.Entities;
using NaviMente.WebApi.Dto.Device;

namespace NaviMente.WebApi.Infrastructure.Services
{
    public interface IDeviceService
    {
        ObjectId? RegisterDevice(DeviceRegisterDTO deviceRegister);
        List<DeviceDTO> GetUserDevicesAsync(long userId);
        User? UnassignDeviceAsync(long userId, string serialNumber);
        void AddRestrictedZone(ZoneDTO zoneDto);
        List<ZoneOutputDTO> GetRestrictedZones(string serialNumber);
        bool DeleteZone(long zoneId);
    }
}
