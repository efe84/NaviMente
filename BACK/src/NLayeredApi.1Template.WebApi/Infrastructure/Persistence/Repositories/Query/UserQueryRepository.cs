using MongoDB.Driver;
using NaviMente.WebApi.Domain.Shared.Entities;
using System.Diagnostics.CodeAnalysis;

namespace NaviMente.WebApi.Infrastructure.Persistence.Repositories.Query
{
    public class UserQueryRepository: IUserQueryRepository
    {
        private readonly IMongoCollection<User> _usersCollection;

        public UserQueryRepository(IApplicationContext dbContext)
        {
            _usersCollection = dbContext.Users;
        }

        public User? GetByUsername(string username)
        {
            return _usersCollection
                .Find(u => u.Username == username)
                .FirstOrDefault();
        }

        public User? GetByUserId(long userId)
        {
            return _usersCollection
                .Find(u => u.UserId == userId)
                .FirstOrDefault();
        }

        public void InsertUser(User user)
        {
            _usersCollection.InsertOne(user);
        }

        public void UpdateEmail(string username, string newEmail) 
        {
            var updateDefinition = Builders<User>.Update.Set(u => u.Email, newEmail);

            var result = _usersCollection.UpdateOne(
                u => u.Username == username,
                updateDefinition
            );
        }

        public void UpdateMainPhone(string username, string newMainPhone)
        {
            var updateDefinition = Builders<User>.Update.Set(u => u.MainPhone, newMainPhone);

            var result = _usersCollection.UpdateOne(
                u => u.Username == username,
                updateDefinition
            );
        }

        public void UnlinkTelegram(string userId)
        {
            var filter = Builders<User>.Filter.Eq(u => u.UserId, long.Parse(userId));
            var update = Builders<User>.Update.Set(u => u.TelegramChatId, null);

            _usersCollection.UpdateOne(filter, update);
        }
    }
}
