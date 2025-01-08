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
        private readonly ILogger<UserController> _logger;

        public UserService(ApplicationContext dbContext, ILogger<UserController> logger)
        {
            _usersCollection = dbContext.Users;
            _logger = logger;
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
                MainPhone = userRegister.MainPhone,
                Role = Dto.Enums.UserRolesEnum.Default
            };

            await _usersCollection.InsertOneAsync(newUser);
        }

        public async Task<User> GetUserInfo(string username)
        {
            User? user = await _usersCollection.Find(u => u.Username == username).FirstOrDefaultAsync();

            if (user == null)
                throw new Exception($"User with username {username} not found.");

            return user;
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

        public async Task EditEmail(string username, string newEmail)
        {
            User? actualUser = _usersCollection.Find(u => u.Username == username).FirstOrDefault();
            if (actualUser == null)
                throw new Exception($"User to edit not found {username}");

            var updateDefinition = Builders<User>.Update.Set(u => u.Email, newEmail);

            var result = await _usersCollection.UpdateOneAsync(
                u => u.Username == username,
                updateDefinition
            );

            if (result.MatchedCount == 0)
                throw new Exception($"Failed to update email for user: {username}");

            _logger.LogInformation("Succesfully edited {username}", username);
        }

        public async Task EditMainPhone(string username, string newMainPhone)
        {
            User? actualUser = _usersCollection.Find(u => u.Username == username).FirstOrDefault();
            if (actualUser == null)
                throw new Exception($"User to edit not found {username}");

            var updateDefinition = Builders<User>.Update.Set(u => u.MainPhone, newMainPhone);

            var result = await _usersCollection.UpdateOneAsync(
                u => u.Username == username,
                updateDefinition
            );

            if (result.MatchedCount == 0)
                throw new Exception($"Failed to update email for user: {username}");

            _logger.LogInformation("Succesfully edited {username}", username);
        }

        public async Task AddPhone(string username, string newPhoneNumber)
        {
            User? actualUser = _usersCollection.Find(u => u.Username == username).FirstOrDefault();
            if (actualUser == null)
                throw new Exception($"User to edit not found: {username}");

            var updateDefinition = Builders<User>.Update.Push(u => u.OtherPhones, newPhoneNumber);

            var result = await _usersCollection.UpdateOneAsync(
                u => u.Username == username,
                updateDefinition
            );

            if (result.MatchedCount == 0)
                throw new Exception($"Failed to add phone number for user: {username}");

            _logger.LogInformation("Successfully added phone number for {username}", username);
        }
    }
}
