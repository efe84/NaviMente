using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;
using NaviMente.WebApi.Controllers;
using NaviMente.WebApi.Domain.Shared.Entities;
using NaviMente.WebApi.Dto.Location;
using NaviMente.WebApi.Infrastructure.Persistence;

namespace NaviMente.WebApi.Infrastructure.Services
{
    public class LocationService
    {
        private readonly IMongoCollection<Location> _locationCollection;
        private readonly ILogger<LocationService> _logger;

        public LocationService(ApplicationContext dbContext)
        {
            _locationCollection = dbContext.Locations;
        }

        public LocationPointDTO GetLocation(long deviceId, DateTime timestamp)
        {
            Location location = _locationCollection
                                    .Find(l => l.DeviceId == deviceId && l.Timestamp == timestamp)
                                    .FirstOrDefault() ?? throw new Exception($"Location not found {deviceId}");

            if (location.LocationData is GeoJsonPoint<GeoJson2DCoordinates> point)
            {
                return new LocationPointDTO
                {
                    DeviceId = location.DeviceId,
                    Timestamp = location.Timestamp,
                    Longitude = point.Coordinates.X,
                    Latitude = point.Coordinates.Y
                };
            }
            throw new Exception("Tipo de location no correspondiente a Point");
        }

        public LocationPointDTO GetLastLocation(long deviceId)
        {
            try
            {
                Location location = _locationCollection
                                    .Find(l => l.DeviceId == deviceId)
                                    .SortByDescending(l => l.Timestamp)
                                    .FirstOrDefault() ?? throw new Exception($"Location not found {deviceId}");

                if (location.LocationData is GeoJsonPoint<GeoJson2DCoordinates> point)
                {
                    return new LocationPointDTO
                    {
                        DeviceId = location.DeviceId,
                        Timestamp = location.Timestamp,
                        Longitude = point.Coordinates.X,
                        Latitude = point.Coordinates.Y
                    };
                }
                throw new Exception("Tipo de location no correspondiente a Point");

            }
            catch (Exception e)
            {
                _logger.LogError(e,"No se encuentran registros para el naviBand seleccionado");
                throw;
            }
        }
    }
}
