using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;
using NaviMente.WebApi.Controllers;
using NaviMente.WebApi.Domain.Shared.Entities;
using NaviMente.WebApi.Dto.Location;
using NaviMente.WebApi.Infrastructure.Persistence.Repositories;

namespace NaviMente.WebApi.Infrastructure.Services
{
    public class LocationService: ILocationService
    {
        private readonly ILocationQueryRepository _locationQueryRepository;
        private readonly ILogger<LocationController> _logger;

        public LocationService(ILogger<LocationController> logger, ILocationQueryRepository locationQueryRepository)
        {
            _locationQueryRepository = locationQueryRepository;
            _logger = logger;
        }

        public LocationPointDTO GetLocation(string serialNumber, DateTime timestamp)
        {
            Location location = _locationQueryRepository.GetBySerialNumberAndTimestamp(serialNumber, timestamp) ?? throw new Exception("Ubicacion no encontrada");

            if (location.LocationData is GeoJsonPoint<GeoJson2DCoordinates> point)
            {
                return new LocationPointDTO
                {
                    SerialNumber = location.SerialNumber,
                    Timestamp = location.Timestamp,
                    Longitude = point.Coordinates.X,
                    Latitude = point.Coordinates.Y
                };
            }
            throw new Exception("Tipo de location no correspondiente a Point");
        }

        public LocationPointDTO? GetLastLocation(string serialNumber)
        {
            try
            {
                Location? location = _locationQueryRepository.GetLastBySerialNumber(serialNumber);
                if (location == null)
                    return null;

                if (location.LocationData is GeoJsonPoint<GeoJson2DCoordinates> point)
                {
                    return new LocationPointDTO
                    {
                        SerialNumber = location.SerialNumber,
                        Timestamp = location.Timestamp,
                        Longitude = point.Coordinates.X,
                        Latitude = point.Coordinates.Y
                    };
                }
                throw new Exception("Tipo de location no correspondiente a Point");

            }
            catch (Exception e)
            {
                throw;
            }
        }

        public LocationLineDTO GetRoute(string serialNumber, DateTime startDate, DateTime endDate)
        {
            const int TimeGapMinutes = 30;
            var routes = new List<Dto.Location.Route>();
            var currentCoordinates = new List<List<double>>();
            var locations = _locationQueryRepository.GetRoute(serialNumber,startDate,endDate);

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
                            Coordinates = [.. currentCoordinates]
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
                SerialNumber = serialNumber,
                Routes = routes
            };
        }
    }
}
