using NaviMente.WebApi.Domain.Shared.Entities;

namespace NaviMente.WebApi.Infrastructure.Persistence.Repositories
{
    public interface IDeviceQueryRepository
    {
        Device? GetBySerialNumber(string serialNumber);
        List<Device> GetByUserId(long userId);
        Device? GetByNameAndUser(string deviceName, long userId);
        void AssignDevice(string serialNumber, string deviceName);
        void UnassignDevice(string serialNumber);
        void RegisterDevice(string serialNumber, string deviceName, long userId);
    }
}
