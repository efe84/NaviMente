using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using NaviMente.WebApi.Domain.Shared.Entities;

namespace NaviMente.WebApi.Infrastructure.Persistence
{
    public class ApplicationContext : DbContext
    {
        private readonly IMongoDatabase _database;

        public ApplicationContext()
        {
            var connectionString = "mongodb://localhost:27017";
            var client = new MongoClient(connectionString);

            _database = client.GetDatabase("NaviMenteDev");
        }

        public IMongoCollection<User> Users => _database.GetCollection<User>("Users");
        //public IMongoCollection<Device> Devices => _database.GetCollection<Device>("Devices");
        public IMongoCollection<Location> Locations => _database.GetCollection<Location>("Locations");
    }
}
