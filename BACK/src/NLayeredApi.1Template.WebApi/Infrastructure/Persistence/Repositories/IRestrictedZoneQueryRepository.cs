using NaviMente.WebApi.Domain.Shared.Entities;

namespace NaviMente.WebApi.Infrastructure.Persistence.Repositories
{
    public interface IRestrictedZoneQueryRepository
    {
        void Insert(RestrictedZone zone);
        void Delete(long zoneId);
        List<RestrictedZone> GetBySerialNumber(string serialNumber);
    }
}
