using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using NaviMente.WebApi.Controllers;
using NaviMente.WebApi.Domain.Services;
using NaviMente.WebApi.Domain.Shared.Entities;
using NaviMente.WebApi.Dto.Device;
using NaviMente.WebApi.Dto.Login;
using NaviMente.WebApi.Infrastructure.Persistence;

namespace NaviMente.WebApi.Infrastructure.Services
{
    public class DeviceService
    {
        private readonly IMongoCollection<Device> _devicesCollection;
        private readonly IMongoCollection<Location> _locationsCollection;
        private readonly IMongoCollection<User> _usersCollection;
        private readonly ILogger<DeviceController> _logger; 

        public DeviceService(ApplicationContext dbContext, ILogger<DeviceController> logger)
        {
            _locationsCollection = dbContext.Locations;
            _devicesCollection = dbContext.Devices;
            _usersCollection = dbContext.Users;
            _logger = logger;
        }

        public async Task RegisterDeviceAsync(DeviceRegisterDTO deviceRegister)
        {
            var existingDevice = await _devicesCollection
                .Find(d => d.SerialNumber == deviceRegister.SerialNumber)
                .FirstOrDefaultAsync();

            if (existingDevice == null)
                throw new Exception("This NaviBand is not recognized, get in contact with NaviMente");

            if (existingDevice.UserId != null)
                throw new Exception("NaviBand already in use");

            var duplicateDeviceName = await _devicesCollection
                .Find(d => d.DeviceName == deviceRegister.DeviceName && d.UserId == deviceRegister.UserId)
                .FirstOrDefaultAsync();

            if (duplicateDeviceName != null)
                throw new Exception("You have another NaviBand with that name");

            var updateDefinition = Builders<Device>.Update
                .Set(d => d.DeviceName, deviceRegister.DeviceName)
                .Set(d => d.AssignedDate, DateTime.UtcNow)
                .Set(d => d.UserId, deviceRegister.UserId);

            var updateResult = await _devicesCollection
                .UpdateOneAsync(
                    d => d.SerialNumber == deviceRegister.SerialNumber,
                    updateDefinition
                );

            if (updateResult.ModifiedCount == 0)
                throw new Exception("Failed to update the device. Please try again.");
        }

        public async Task<List<DeviceDTO>> GetUserDevicesAsync(string username)
        {
            var user = await _usersCollection
                .Find(u => u.Username == username)
                .FirstOrDefaultAsync();

            if (user == null)
                throw new Exception($"User with username '{username}' not found.");

            var devices = await _devicesCollection
                .Find(d => d.UserId == user.UserId)
                .ToListAsync();

            List<DeviceDTO> devicesList = new List<DeviceDTO>();
            foreach (var device in devices)
            {
                Location? location = _locationsCollection
                                    .Find(l => l.SerialNumber == device.SerialNumber)
                                    .SortByDescending(l => l.Timestamp)
                                    .FirstOrDefault();

                var deviceDTO = new DeviceDTO()
                {
                    DeviceName = device.DeviceName,
                    LastUpdate = (location != null ? location.Timestamp : null)
                };
                devicesList.Add(deviceDTO);
            }

            return devicesList;
        }

        public async Task UnassignDeviceAsync(long userId, string serialNumber)
        {

            var existingDevice = await _devicesCollection
                .Find(d => d.SerialNumber == serialNumber)
                .FirstOrDefaultAsync();

            if (existingDevice == null)
                throw new Exception($"NaviBand with SerialNumber {serialNumber} not found.");

            if( existingDevice.UserId != userId )
                throw new Exception("This naviBand is not assigned to you");

            var updateDefinition = Builders<Device>.Update
                .Set(d => d.UserId, null)
                .Set(d => d.DeviceName, null)
                .Set(d => d.AssignedDate, null);

            var updateResult = await _devicesCollection
                .UpdateOneAsync(
                    d => d.SerialNumber == serialNumber,
                    updateDefinition
                );

            if (updateResult.ModifiedCount == 0)
                throw new Exception("Failed to unassign the naviBand. Please try again.");
        }

    }
}
