using MongoDB.Driver;
using NaviMente.WebApi.Domain.Shared.Entities;
using System.Diagnostics.CodeAnalysis;

namespace NaviMente.WebApi.Infrastructure.Persistence.Repositories.Query
{ 
    public class RestrictedZoneQueryRepository: IRestrictedZoneQueryRepository
    {
        private readonly IMongoCollection<RestrictedZone> _restrictedZonesCollection;

        public RestrictedZoneQueryRepository(IApplicationContext dbContext)
        {
            _restrictedZonesCollection = dbContext.Zone;
        }

        public void Insert(RestrictedZone zone)
        {
            _restrictedZonesCollection.InsertOne(zone);
        }

        public void Delete(long zoneId)
        {
            var filter = Builders<RestrictedZone>.Filter.Eq(z => z.ZoneId, zoneId);
            var result = _restrictedZonesCollection.DeleteOne(filter);
        }

        public List<RestrictedZone> GetBySerialNumber(string serialNumber)
        {
            return _restrictedZonesCollection
                .Find(x => x.SerialNumber == serialNumber)
                .ToList();
        }
    }
}
