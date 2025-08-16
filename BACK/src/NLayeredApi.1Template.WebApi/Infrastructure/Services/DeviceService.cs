using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;
using NaviMente.WebApi.Controllers;
using NaviMente.WebApi.Domain.Shared.Entities;
using NaviMente.WebApi.Dto.Device;
using NaviMente.WebApi.Infrastructure.Persistence;
using NaviMente.WebApi.Infrastructure.Persistence.Repositories;

namespace NaviMente.WebApi.Infrastructure.Services
{
    public class DeviceService: IDeviceService
    {
        private readonly IDeviceQueryRepository _deviceQueryRepository;
        private readonly ILocationQueryRepository _locationQueryRepository;
        private readonly IUserQueryRepository _userQueryRepository;
        private readonly IRestrictedZoneQueryRepository _restrictedZoneQueryRepository;
        private readonly ICounterQueryRepository _counterQeryRepository;
        private readonly ILogger<DeviceController> _logger; 

        public DeviceService(ILogger<DeviceController> logger, IDeviceQueryRepository deviceQueryRepository, IUserQueryRepository userQueryRepository, ILocationQueryRepository locationQueryRepository, IRestrictedZoneQueryRepository restrictedZoneQueryRepository, ICounterQueryRepository counterQueryRepository)
        {
            _locationQueryRepository = locationQueryRepository;
            _deviceQueryRepository = deviceQueryRepository;
            _userQueryRepository = userQueryRepository;
            _restrictedZoneQueryRepository = restrictedZoneQueryRepository;
            _counterQeryRepository = counterQueryRepository;
            _logger = logger;
        }

        public ObjectId? RegisterDevice(DeviceRegisterDTO deviceRegister)
        {
            var existingDevice = _deviceQueryRepository.GetBySerialNumber(deviceRegister.SerialNumber) ?? throw new Exception("This NaviBand is not recognized, get in contact with NaviMente");
            if (existingDevice.UserId != null)
                throw new Exception("NaviBand already in use");

            var duplicateDeviceName = _deviceQueryRepository.GetByNameAndUser(deviceRegister.DeviceName, deviceRegister.UserId);
            if(duplicateDeviceName != null)
                throw new Exception("You have another NaviBand with that name");

            _deviceQueryRepository.RegisterDevice(deviceRegister.SerialNumber, deviceRegister.DeviceName, deviceRegister.UserId);

            return _deviceQueryRepository.GetBySerialNumber(deviceRegister.SerialNumber).DeviceId.Value;
        }

        public List<DeviceDTO> GetUserDevicesAsync(long userId)
        {
            var user = _userQueryRepository.GetByUserId(userId) ?? throw new Exception($"User '{userId}' not found.");
            var devices = _deviceQueryRepository.GetByUserId(userId);

            List<DeviceDTO> devicesList = new List<DeviceDTO>();
            foreach (var device in devices)
            {
                Location? location = _locationQueryRepository.GetBySerialNumber(device.SerialNumber);

                var deviceDTO = new DeviceDTO()
                {
                    SerialNumber = device.SerialNumber,
                    Name = device.DeviceName,
                    LastUpdate = location?.Timestamp
                };
                devicesList.Add(deviceDTO);
            }

            return devicesList;
        }

        public User? UnassignDeviceAsync(long userId, string serialNumber)
        {

            var existingDevice = _deviceQueryRepository.GetBySerialNumber(serialNumber) ?? throw new Exception($"NaviBand with SerialNumber {serialNumber} not found.");
            if ( existingDevice.UserId != userId )
                throw new Exception("This naviBand is not assigned to you");

            _deviceQueryRepository.UnassignDevice(serialNumber);

            return _userQueryRepository.GetByUserId(userId);
        }

        public void AddRestrictedZone(ZoneDTO zoneDto)
        {
            foreach (var shapeDto in zoneDto.Shapes)
            {
                long newZoneId = _counterQeryRepository.GetNextSequenceValue("zoneId");

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

                _restrictedZoneQueryRepository.Insert(zone);
            }
        }

        public List<ZoneOutputDTO> GetRestrictedZones(string serialNumber)
        {
            var zones = _restrictedZoneQueryRepository.GetBySerialNumber(serialNumber);
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

        public bool DeleteZone(long zoneId)
        {
            _restrictedZoneQueryRepository.Delete(zoneId);
            return true;
        }
    }
}
