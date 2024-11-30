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
            const int TimeGapMinutes = 30;
            var routes = new List<Dto.Location.Route>();
            var currentCoordinates = new List<List<double>>();

            var filter = Builders<Location>.Filter.And(
                Builders<Location>.Filter.Eq(l => l.DeviceId, deviceId),
                Builders<Location>.Filter.Gte(l => l.Timestamp, startDate),
                Builders<Location>.Filter.Lte(l => l.Timestamp, endDate)
            );

            var locations = _locationCollection
                .Find(filter)
                .SortBy(l => l.Timestamp)
                .ToList();

            var previousTimestamp = locations.First().Timestamp;

            foreach (var location in locations)
            {
                if (location.LocationData is GeoJsonPoint<GeoJson2DCoordinates> point)
                {
                    var currentTimestamp = location.Timestamp;

                    if ((currentTimestamp - previousTimestamp).TotalMinutes > TimeGapMinutes && currentCoordinates.Any())
                    {
                        routes.Add(new Dto.Location.Route
                        {
                            Type = "LineString",
                            Coordinates = new List<List<double>>(currentCoordinates)
                        });
                        currentCoordinates.Clear();
                    }

                    currentCoordinates.Add(new List<double> { point.Coordinates.X, point.Coordinates.Y });
                    previousTimestamp = currentTimestamp;
                }
            }

            if (currentCoordinates.Any())
            {
                routes.Add(new Dto.Location.Route
                {
                    Type = "LineString",
                    Coordinates = currentCoordinates
                });
            }

            return new LocationLineDTO
            {
                DeviceId = deviceId,
                StartDate = startDate,
                EndDate = endDate,
                Routes = routes
            };
        }
    }
}
