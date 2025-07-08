using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;
using NaviMente.WebApi.Controllers;
using NaviMente.WebApi.Domain.Shared.Entities;
using NaviMente.WebApi.Dto.Device;
using NaviMente.WebApi.Infrastructure.Persistence;

namespace NaviMente.WebApi.Infrastructure.Services
{
    public class DeviceService
    {
        private readonly IMongoCollection<Device> _devicesCollection;
        private readonly IMongoCollection<Location> _locationsCollection;
        private readonly IMongoCollection<User> _usersCollection;
        private readonly IMongoCollection<RestrictedZone> _restrictedZonesCollection;
        private readonly IMongoCollection<Counter> _countersCollection;
        private readonly ILogger<DeviceController> _logger; 

        public DeviceService(ApplicationContext dbContext, ILogger<DeviceController> logger)
        {
            _locationsCollection = dbContext.Locations;
            _devicesCollection = dbContext.Devices;
            _usersCollection = dbContext.Users;
            _restrictedZonesCollection = dbContext.Zone;
            _countersCollection = dbContext.Counters;
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

        public async Task<List<DeviceDTO>> GetUserDevicesAsync(long userId)
        {
            var user = await _usersCollection
                .Find(u => u.UserId == userId)
                .FirstOrDefaultAsync();

            if (user == null)
                throw new Exception($"User '{userId}' not found.");

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
                    SerialNumber = device.SerialNumber ?? "",
                    Name = device.DeviceName,
                    LastUpdate = (location != null ? location.Timestamp : null)
                };
                devicesList.Add(deviceDTO);
            }

            return devicesList;
        }

        public async Task<User> UnassignDeviceAsync(long userId, string serialNumber)
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

            return _usersCollection.Find(u => u.UserId == userId).FirstOrDefault();
        }

        public async Task AddRestrictedZone(ZoneDTO zoneDto)
        {
            var tasks = new List<Task>();

            foreach (var shapeDto in zoneDto.Shapes)
            {
                long newZoneId = await GetNextSequenceValue("zoneId");

                RestrictedZone zone = new RestrictedZone
                {
                    ZoneId = newZoneId,
                    SerialNumber = zoneDto.SerialNumber,
                    CreatedAt = DateTime.UtcNow,
                    Type = shapeDto.Type.ToLower()
                };

                switch (shapeDto.Type.ToLower())
                {
                    case "circle":
                        zone.Center = new GeoJson2DGeographicCoordinates(shapeDto.Center.Lng, shapeDto.Center.Lat);
                        zone.Radius = shapeDto.Radius;
                        break;

                    case "rectangle":
                        zone.Bounds = shapeDto.Bounds;
                        break;

                    case "polygon":
                        var coordinates = new List<GeoJson2DGeographicCoordinates>();
                        foreach (var coord in shapeDto.Coordinates)
                        {
                            coordinates.Add(new GeoJson2DGeographicCoordinates(coord[0], coord[1]));
                        }
                        var linearRing = new GeoJsonLinearRingCoordinates<GeoJson2DGeographicCoordinates>(coordinates);
                        var polygonCoordinates = new GeoJsonPolygonCoordinates<GeoJson2DGeographicCoordinates>(linearRing);
                        zone.Shape = new GeoJsonPolygon<GeoJson2DGeographicCoordinates>(polygonCoordinates);
                        break;
                }

                tasks.Add(_restrictedZonesCollection.InsertOneAsync(zone));
            }

            await Task.WhenAll(tasks);
        }

        public async Task<List<ZoneOutputDTO>> GetRestrictedZones(string serialNumber)
        {
            var filter = Builders<RestrictedZone>.Filter.Eq(z => z.SerialNumber, serialNumber);
            var zones = await _restrictedZonesCollection.Find(filter).ToListAsync();

            var output = zones.Select(z =>
            {
                var shape = new ShapeOutputDTO
                {
                    Type = z.Type
                };

                switch (z.Type.ToLower())
                {
                    case "circle":
                        if (z.Center != null)
                        {
                            shape.Center = new List<double> { z.Center.Longitude, z.Center.Latitude };
                        }
                        shape.Radius = z.Radius;
                        break;

                    case "rectangle":
                        shape.Bounds = z.Bounds;
                        break;

                    case "polygon":
                        if (z.Shape != null)
                        {
                            shape.Coordinates = z.Shape.Coordinates.Exterior.Positions
                                .Select(p => new List<double> { p.Longitude, p.Latitude })
                                .ToList();
                        }
                        break;
                }

                return new ZoneOutputDTO
                {
                    ZoneId = z.ZoneId,
                    SerialNumber = z.SerialNumber,
                    CreatedAt = z.CreatedAt,
                    Shape = shape
                };
            })
            .ToList();

            return output;
        }

        public async Task<bool> DeleteZone(long zoneId)
        {
            var filter = Builders<RestrictedZone>.Filter.Eq(z => z.ZoneId, zoneId);
            var result = await _restrictedZonesCollection.DeleteOneAsync(filter);

            if (result.DeletedCount == 0)
                throw new Exception("Zona no encontrada");

            return true;
        }

        private async Task<long> GetNextSequenceValue(string sequenceName)
        {
            var filter = Builders<Counter>.Filter.Eq(c => c.Id, sequenceName);
            var update = Builders<Counter>.Update.Inc(c => c.SequenceValue, 1);

            var options = new FindOneAndUpdateOptions<Counter>
            {
                ReturnDocument = ReturnDocument.After,
                IsUpsert = true
            };

            var updatedCounter = await _countersCollection.FindOneAndUpdateAsync(filter, update, options);
            return updatedCounter.SequenceValue;
        }
    }
}
