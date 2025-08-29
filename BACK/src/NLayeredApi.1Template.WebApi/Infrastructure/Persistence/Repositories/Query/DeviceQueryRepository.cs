using MongoDB.Driver;
using NaviMente.WebApi.Domain.Shared.Entities;
using System.Diagnostics.CodeAnalysis;

namespace NaviMente.WebApi.Infrastructure.Persistence.Repositories.Query
{
    public class DeviceQueryRepository: IDeviceQueryRepository
    {
        private readonly IMongoCollection<Device> _devicesCollection;

        public DeviceQueryRepository(IApplicationContext dbContext)
        {
            _devicesCollection = dbContext.Devices;
        }

        public Device? GetBySerialNumber(string serialNumber)
        {
            return _devicesCollection
                .Find(x => x.SerialNumber == serialNumber)
                .FirstOrDefault();
        }

        public List<Device> GetByUserId(long userId)
        {
            return _devicesCollection
                .Find(x => x.UserId == userId)
                .ToList();
        }

        public Device? GetByNameAndUser(string deviceName, long userId)
        {
            return _devicesCollection
                .Find(x => x.DeviceName == deviceName && x.UserId == userId)
                .FirstOrDefault();
        }

        public void AssignDevice(string serialNumber, string deviceName)
        {
            var filter = Builders<Device>.Filter.Eq(u => u.SerialNumber, serialNumber);

            var update = Builders<Device>.Update
                .Set(u => u.DeviceName, deviceName)
                .Set(u => u.AssignedDate, DateTime.UtcNow)
                .Set(u => u.isActive, true);

            _devicesCollection.UpdateOne(filter, update);
        }

        public void UnassignDevice(string serialNumber)
        {
            var updateDefinition = Builders<Device>.Update
                .Set(d => d.UserId, null)
                .Set(d => d.DeviceName, null)
                .Set(d => d.AssignedDate, null);

            var updateResult = _devicesCollection
                .UpdateOne(
                    d => d.SerialNumber == serialNumber,
                    updateDefinition
                );
        }

        public void RegisterDevice(string serialNumber, string deviceName, long userId)
        {
            var updateDefinition = Builders<Device>.Update
                .Set(d => d.DeviceName, deviceName)
                .Set(d => d.AssignedDate, DateTime.UtcNow)
                .Set(d => d.UserId, userId);

            var updateResult = _devicesCollection
                .UpdateOne(
                    d => d.SerialNumber == serialNumber,
                    updateDefinition
                );
        }
    }
}
