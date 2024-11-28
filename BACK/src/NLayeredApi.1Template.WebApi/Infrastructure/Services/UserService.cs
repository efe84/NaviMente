using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using NaviMente.WebApi.Controllers;
using NaviMente.WebApi.Domain.Shared.Entities;
using NaviMente.WebApi.Dto.Login;
using NaviMente.WebApi.Dto.User;
using NaviMente.WebApi.Infrastructure.Persistence;
using NLayeredApi._1Template.WebApi.Model.Login;

namespace NaviMente.WebApi.Domain.Services
{
    public class UserService
    {
        private readonly IMongoCollection<User> _usersCollection;
        private readonly ILogger<UserService> _logger;

        public UserService(ApplicationContext dbContext)
        {
            _usersCollection = dbContext.Users;
        }

        public async Task CreateUserAsync(UserRegisterDTO userRegister)
        {
            if (await _usersCollection.Find(u => u.Username == userRegister.Username).FirstOrDefaultAsync() != null)
                throw new Exception("That username already exists");

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(userRegister.Password);

            var newUser = new User
            {
                Username = userRegister.Username,
                Email = userRegister.Email,
                Password = hashedPassword,
                PhoneNumber = userRegister.PhoneNumber,
                DeviceId = userRegister.DeviceId,
                Role = Dto.Enums.UserRolesEnum.Default
            };

            await _usersCollection.InsertOneAsync(newUser);
        }

        public bool ValidatePassword(string userName, string password)
        {
            try
            {
                var user = _usersCollection.
                    Find(u => u.Username == userName).FirstOrDefault() ?? throw new Exception($"User not found {userName}");

                bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.Password);

                if (!isPasswordValid)
                {
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }

        public User GetUser(string userName)
        {
            return _usersCollection.
                Find(u => u.Username == userName).FirstOrDefault() ?? throw new Exception($"User not found {userName}");
        }

        public async Task EditUser(UserEditDTO userEdit)
        {
            User? actualUser = _usersCollection.Find(u => u.Username == userEdit.Username).FirstOrDefault();
            if (actualUser == null)
                throw new Exception($"User to edit not found {userEdit.Username}");

            string? hashedPassword = null;
            if (!string.IsNullOrEmpty(userEdit.Password))
            {
                hashedPassword = BCrypt.Net.BCrypt.HashPassword(userEdit.Password);
            }

            var updatesDict = new Dictionary<string, object?>
            {
                { "Username", userEdit.Username },
                { "Email", userEdit.Email },
                { "Password", hashedPassword },
                { "PhoneNumber", userEdit.PhoneNumber },
                { "DeviceId", userEdit.DeviceId }
            };

            var updateDefinitionBuilder = Builders<User>.Update;
            var updates = new List<UpdateDefinition<User>>();

            foreach (var update in updatesDict)
            {
                if (update.Value != null && !string.IsNullOrEmpty(update.Value.ToString()))
                {
                    updates.Add(updateDefinitionBuilder.Set(update.Key, update.Value));
                }
            }

            if (updates.Count == 0)
                throw new Exception("No fields to update.");

            var updateDefinition = updateDefinitionBuilder.Combine(updates);

            await _usersCollection.UpdateOneAsync(
                u => u.Username == userEdit.Username,
                updateDefinition
            );

            _logger.LogInformation("Succesfully edited {username}", userEdit.Username);
        }
    }
}
