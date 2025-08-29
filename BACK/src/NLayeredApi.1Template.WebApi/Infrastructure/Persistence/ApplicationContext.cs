using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using NaviMente.WebApi.Domain.Shared.Entities;

namespace NaviMente.WebApi.Infrastructure.Persistence
{
    public class ApplicationContext: IApplicationContext
    {
        private readonly IMongoDatabase _database;

        public ApplicationContext(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("Default") ?? "mongodb://localhost:27017";
            var client = new MongoClient(connectionString);

            _database = client.GetDatabase("NaviMenteDev");
        }

        public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
        public IMongoCollection<Device> Devices => _database.GetCollection<Device>("Devices");
        public IMongoCollection<Location> Locations => _database.GetCollection<Location>("Locations");
        public IMongoCollection<Counter> Counters => _database.GetCollection<Counter>("Counters");
        public IMongoCollection<RestrictedZone> Zone => _database.GetCollection<RestrictedZone>("RestrictedZones");
        public IMongoCollection<LogLine> Logs => _database.GetCollection<LogLine>("Logs");
        public IMongoCollection<TelegramLinkCode> Codes => _database.GetCollection<TelegramLinkCode>("Codes");
    }
}
