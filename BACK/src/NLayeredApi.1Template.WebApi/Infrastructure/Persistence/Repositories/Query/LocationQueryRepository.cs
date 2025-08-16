using MongoDB.Driver;
using NaviMente.WebApi.Domain.Shared.Entities;
using System.Diagnostics.CodeAnalysis;

namespace NaviMente.WebApi.Infrastructure.Persistence.Repositories.Query
{
    public class LocationQueryRepository: ILocationQueryRepository
    {
        private readonly IMongoCollection<Location> _locationsCollection;

        public LocationQueryRepository(IApplicationContext dbContext)
        {
            _locationsCollection = dbContext.Locations;
        }

        public Location? GetBySerialNumber(string serialNumber)
        {
            return _locationsCollection
                .Find(x => x.SerialNumber == serialNumber)
                .SortByDescending(l => l.Timestamp)
                .FirstOrDefault();
        }

        public Location? GetBySerialNumberAndTimestamp(string serialNumber, DateTime timestamp)
        {
            return _locationsCollection
                   .Find(l => l.SerialNumber == serialNumber && l.Timestamp == timestamp)
                   .FirstOrDefault();
        }

        public Location? GetLastBySerialNumber(string serialNumber)
        {
            return _locationsCollection
                   .Find(l => l.SerialNumber == serialNumber)
                   .SortByDescending(l => l.Timestamp)
                   .FirstOrDefault();
        }

        public List<Location>? GetRoute(string serialNumber, DateTime startDate, DateTime endDate)
        {
            var filter = Builders<Location>.Filter.And(
                Builders<Location>.Filter.Eq(l => l.SerialNumber, serialNumber),
                Builders<Location>.Filter.Gte(l => l.Timestamp, startDate),
                Builders<Location>.Filter.Lte(l => l.Timestamp, endDate)
            );

            var locations = _locationsCollection
                                .Find(filter)
                                .SortBy(l => l.Timestamp)
                                .ToList();

            return locations;
        }
    }
}
