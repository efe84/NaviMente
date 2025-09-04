using NaviMente.WebApi.Controllers;
using NaviMente.WebApi.Domain.Shared.Entities;
using NaviMente.WebApi.Dto.User;
using NaviMente.WebApi.Infrastructure.Persistence.Repositories;

namespace NaviMente.WebApi.Infrastructure.Services
{
    public class UserService: IUserService
    {
        private readonly IUserQueryRepository _userQueryRepository;
        private readonly IDeviceQueryRepository _deviceQueryRepository;
        private readonly ICodeQueryRepository _codeQueryRepository;
        private readonly ICounterQueryRepository _counterQueryRepository;
        private readonly ILogger<UserController> _logger;

        public UserService(ILogger<UserController> logger, IUserQueryRepository userQueryRepository, IDeviceQueryRepository deviceQueryRepository, ICodeQueryRepository codeQueryRepository, ICounterQueryRepository counterQueryRepository)
        {
            _userQueryRepository = userQueryRepository;
            _deviceQueryRepository = deviceQueryRepository;
            _codeQueryRepository = codeQueryRepository;
            _logger = logger;
            _counterQueryRepository = counterQueryRepository;
        }

        public long CreateUser(UserRegisterDTO userRegister)
        {
            if (_userQueryRepository.GetByUsername(userRegister.Username) != null)
                throw new Exception("That username already exists");

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(userRegister.Password);
            long userId = _counterQueryRepository.GetNextSequenceValue("userId");

            var newUser = new User
            {
                UserId = userId,
                Username = userRegister.Username,
                Email = userRegister.Email,
                Password = hashedPassword,
                MainPhone = userRegister.MainPhone,
                Role = Dto.Enums.UserRolesEnum.Default
            };

            _userQueryRepository.InsertUser(newUser);

            if (userRegister.SerialNumber != null)
            {
                if (userRegister.DeviceName != null && _deviceQueryRepository.GetBySerialNumber(userRegister.SerialNumber) != null)
                    _deviceQueryRepository.AssignDevice(userRegister.SerialNumber, userRegister.DeviceName);
            }

            return _userQueryRepository.GetByUsername(userRegister.Username)?.UserId ?? 0;
        }

        public bool ValidatePassword(string userName, string password)
        {
            try
            {
                User user = _userQueryRepository.GetByUsername(userName) ?? throw new Exception($"User not found {userName}");
                
                bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.Password);

                if (!isPasswordValid)
                    return false;

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
            return _userQueryRepository.GetByUsername(userName) ?? throw new Exception($"User not found {userName}");
        }

        public User? EditEmail(string username, string newEmail)
        {
            _ = _userQueryRepository.GetByUsername(username) ?? throw new Exception($"User to edit not found {username}");
            _userQueryRepository.UpdateEmail(username, newEmail);

            _logger.LogInformation("Succesfully edited {username}", username);
            return _userQueryRepository.GetByUsername(username);
        }

        public User? EditMainPhone(string username, string newMainPhone)
        {
            _ = _userQueryRepository.GetByUsername(username) ?? throw new Exception($"User to edit not found {username}");
            _userQueryRepository.UpdateMainPhone(username, newMainPhone);

            _logger.LogInformation("Succesfully edited {username}", username);
            return _userQueryRepository.GetByUsername(username);
        }

        public string GenerateLinkCode(string userId)
        {
            string code = Guid.NewGuid().ToString("N")[..6].ToUpper();

            DateTime expiresAt = DateTime.UtcNow.AddMinutes(10);

            TelegramLinkCode codeEntry = new()
            {
                Code = code,
                UserId = long.Parse(userId),
                ExpiresAt = expiresAt
            };

            _codeQueryRepository.DeleteOldcodes(long.Parse(userId));
            _codeQueryRepository.InsertCode(codeEntry);

            return code;
        }

        public void UnlinkTelegram(string userId)
        {
            _userQueryRepository.UnlinkTelegram(userId);
        }
    }
}
