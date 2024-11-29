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
        private readonly ILogger<LocationController> _logger;

        public LocationService(ApplicationContext dbContext, ILogger<LocationController> logger)
        {
            _locationCollection = dbContext.Locations;
            _logger = logger;
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

        public LocationLineDTO GetRoute(long deviceId, DateTime startDate, DateTime endDate)
        {

            var filter = Builders<Location>.Filter.And(
                Builders<Location>.Filter.Eq(l => l.DeviceId, deviceId),
                Builders<Location>.Filter.Gte(l => l.Timestamp, startDate),
                Builders<Location>.Filter.Lte(l => l.Timestamp, endDate)
            );

            var locations = _locationCollection
                .Find(filter)
                .SortBy(l => l.Timestamp)
                .ToList();

            var coordinates = locations
                .Where(l => l.LocationData is GeoJsonPoint<GeoJson2DCoordinates>)
                .Select(l =>
                {
                    var point = l.LocationData as GeoJsonPoint<GeoJson2DCoordinates>;
                    if( point != null )
                        return new List<double> { point.Coordinates.X, point.Coordinates.Y };
                    else return null;
                })
                .ToList();

            var route = new Dto.Location.Route
            {
                Type = "LineString",
                Coordinates = coordinates
            };

            return new LocationLineDTO
            {
                DeviceId = deviceId,
                StartDate = startDate,
                EndDate = endDate,
                Route = route
            };
        }
    }
}
