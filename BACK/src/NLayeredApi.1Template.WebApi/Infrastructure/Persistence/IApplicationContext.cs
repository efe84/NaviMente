using MongoDB.Driver;
using NaviMente.WebApi.Domain.Shared.Entities;

namespace NaviMente.WebApi.Infrastructure.Persistence
{
    public interface IApplicationContext
    {
        IMongoCollection<User> Users { get; }
        IMongoCollection<Device> Devices { get; }
        IMongoCollection<Location> Locations { get; }
        IMongoCollection<Counter> Counters { get; }
        IMongoCollection<RestrictedZone> Zone { get; }
        IMongoCollection<LogLine> Logs { get; }
        IMongoCollection<TelegramLinkCode> Codes { get; }
    }
}
