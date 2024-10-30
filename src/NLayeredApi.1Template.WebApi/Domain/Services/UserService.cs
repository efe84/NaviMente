using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using NaviMente.WebApi.Controllers;
using NaviMente.WebApi.Domain.Shared.Entities;
using NaviMente.WebApi.Dto.Login;
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
            var newUser = new User
            {
                Username = userRegister.Username,
                Email = userRegister.Email,
                Password = userRegister.Password,
                PhoneNumber = userRegister.PhoneNumber
            };

            await _usersCollection.InsertOneAsync(newUser);
        }

        public bool ValidatePassword(string userName, string password)
        {
            try
            {
                var user = _usersCollection.
                    Find(u => u.Username == userName && u.Password == password).FirstOrDefault();
                if (user == null)
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
    }
}
