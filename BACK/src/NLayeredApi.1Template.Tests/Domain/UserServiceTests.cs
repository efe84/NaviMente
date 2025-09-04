using Microsoft.Extensions.Logging;
using Moq;
using NaviMente.WebApi.Controllers;
using NaviMente.WebApi.Domain.Shared.Entities;
using NaviMente.WebApi.Dto.User;
using NaviMente.WebApi.Infrastructure.Persistence.Repositories;
using NaviMente.WebApi.Infrastructure.Services;
namespace NaviMente.Tests.Domain
{
    public class UserServiceTests
    {
        private readonly UserService _userService;
        private readonly Mock<IUserQueryRepository> _userQueryRepository;
        private readonly Mock<IDeviceQueryRepository> _deviceQueryRepository;
        private readonly Mock<ICodeQueryRepository> _codeQueryRepository;
        private readonly Mock<ILogger<UserController>> _logger;

        public UserServiceTests()
        {
            _userQueryRepository = new Mock<IUserQueryRepository>();
            _deviceQueryRepository = new Mock<IDeviceQueryRepository>();
            _codeQueryRepository = new Mock<ICodeQueryRepository>();
            _logger = new Mock<ILogger<UserController>>();

            _userService = new UserService(_logger.Object, _userQueryRepository.Object, _deviceQueryRepository.Object, _codeQueryRepository.Object);
        }

        [Fact]
        public void GetUser_ShouldReturnUser_WhenUserExists()
        {
            // Arrange
            var userName = "testuser";
            var expectedUser = new User
            {
                Username = userName,
                UserId = 1,
                Email = "test@example.com",
                Password = "hashedpass",
                TelegramChatId = 123123123
            };

            _userQueryRepository.Setup(r => r.GetByUsername(userName)).Returns(expectedUser);

            // Act
            var result = _userService.GetUser(userName);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedUser.Username, result.Username);
            Assert.Equal(expectedUser.UserId, result.UserId);
            Assert.Equal(expectedUser.TelegramChatId, result.TelegramChatId);
        }

        [Fact]
        public void GetUser_ShouldThrowException_WhenUserDoesNotExist()
        {
            // Arrange
            var userName = "nonexistent";
            _userQueryRepository.Setup(r => r.GetByUsername(userName)).Returns((User?)null);

            // Act & Assert
            var exception = Assert.Throws<Exception>(() => _userService.GetUser(userName));
            Assert.Equal($"User not found {userName}", exception.Message);
        }

        [Fact]
        public void CreateUser_ShouldInsertUserAndAssignDevice_WhenDataIsValid()
        {
            // Arrange
            var userRegister = new UserRegisterDTO
            {
                Username = "newuser",
                Email = "newuser@example.com",
                Password = "securePassword123",
                MainPhone = "1234567890",
                SerialNumber = "SN-001",
                DeviceName = "SmartWatch"
            };

            var insertedUser = (User)null!;

            // Simular que el usuario NO existe al inicio, pero sí después de insertarlo
            _userQueryRepository.SetupSequence(r => r.GetByUsername(userRegister.Username))
                .Returns((User)null) // Primera llamada → no existe
                .Returns(() => new User
                {
                    UserId = 42,
                    Username = userRegister.Username,
                    Email = userRegister.Email,
                    Password = insertedUser?.Password ?? ""
                }); // Segunda llamada → ya insertado

            // Simular que el dispositivo existe
            _deviceQueryRepository.Setup(d => d.GetBySerialNumber(userRegister.SerialNumber))
                                  .Returns(new Device());

            // Capturar el usuario insertado
            _userQueryRepository.Setup(r => r.InsertUser(It.IsAny<User>()))
                                .Callback<User>(u => insertedUser = u);

            // Act
            var result = _userService.CreateUser(userRegister);

            // Assert
            Assert.Equal(42, result);

            Assert.NotNull(insertedUser);
            Assert.Equal(userRegister.Username, insertedUser.Username);
            Assert.Equal(userRegister.Email, insertedUser.Email);
            Assert.Equal(userRegister.MainPhone, insertedUser.MainPhone);
            Assert.Equal("Default", insertedUser.Role.ToString());
            Assert.True(BCrypt.Net.BCrypt.Verify(userRegister.Password, insertedUser.Password));

            _deviceQueryRepository.Verify(d => d.AssignDevice(userRegister.SerialNumber, userRegister.DeviceName), Times.Once);
        }

        [Fact]
        public void CreateUser_ShouldThrowException_WhenUsernameAlreadyExists()
        {
            // Arrange
            var userRegister = new UserRegisterDTO
            {
                Username = "existinguser",
                Email = "existing@example.com",
                MainPhone = "1234567890",
                Password = "password123"
            };

            _userQueryRepository.Setup(r => r.GetByUsername(userRegister.Username))
                                .Returns(new User { Username = userRegister.Username });

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => _userService.CreateUser(userRegister));
            Assert.Equal("That username already exists", ex.Message);

            _userQueryRepository.Verify(r => r.InsertUser(It.IsAny<User>()), Times.Never);
            _deviceQueryRepository.Verify(d => d.AssignDevice(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void CreateUser_ShouldInsertUserWithoutAssigningDevice_WhenSerialNumberIsNull()
        {
            // Arrange
            var userRegister = new UserRegisterDTO
            {
                Username = "newuser",
                Email = "newuser@example.com",
                Password = "securePassword123",
                MainPhone = "1234567890"
            };

            _userQueryRepository.SetupSequence(r => r.GetByUsername(userRegister.Username))
                .Returns((User)null) // no existe
                .Returns(new User { UserId = 10, Username = userRegister.Username });

            _userQueryRepository.Setup(r => r.InsertUser(It.IsAny<User>()));

            // Act
            var result = _userService.CreateUser(userRegister);

            // Assert
            Assert.Equal(10, result);
            _deviceQueryRepository.Verify(d => d.AssignDevice(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void CreateUser_ShouldNotAssignDevice_WhenDeviceNameIsNull()
        {
            // Arrange
            var userRegister = new UserRegisterDTO
            {
                Username = "newuser",
                Email = "newuser@example.com",
                Password = "securePassword123",
                MainPhone = "1234567890",
                SerialNumber = "SN-001",
                DeviceName = null
            };

            _userQueryRepository.SetupSequence(r => r.GetByUsername(userRegister.Username))
                .Returns((User)null)
                .Returns(new User { UserId = 15, Username = userRegister.Username });

            _userQueryRepository.Setup(r => r.InsertUser(It.IsAny<User>()));

            // Act
            var result = _userService.CreateUser(userRegister);

            // Assert
            Assert.Equal(15, result);
            _deviceQueryRepository.Verify(d => d.AssignDevice(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void CreateUser_ShouldReturnZero_WhenUserNotFoundAfterInsert()
        {
            // Arrange
            var userRegister = new UserRegisterDTO
            {
                Username = "newuser",
                Email = "newuser@example.com",
                Password = "securePassword123",
                MainPhone = "1234567890"
            };

            // Simular que no existe el usuario antes de insertarlo
            _userQueryRepository.Setup(r => r.GetByUsername(userRegister.Username))
                .Returns((User?)null);

            // InsertUser no hace nada especial, solo que se llama
            _userQueryRepository.Setup(r => r.InsertUser(It.IsAny<User>()));

            // Simular que tras insertar, la búsqueda sigue sin encontrar al usuario (null)
            _userQueryRepository.SetupSequence(r => r.GetByUsername(userRegister.Username))
                .Returns((User?)null)  // primera llamada (antes insert)
                .Returns((User?)null); // segunda llamada (después insert)

            // Act
            var result = _userService.CreateUser(userRegister);

            // Assert
            Assert.Equal(0, result);

            // Verificar que InsertUser se llamó una vez
            _userQueryRepository.Verify(r => r.InsertUser(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public void ValidatePassword_ShouldReturnTrue_WhenPasswordIsCorrect()
        {
            // Arrange
            var username = "testuser";
            var plainPassword = "securePassword123";
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(plainPassword);

            var user = new User
            {
                Username = username,
                Password = hashedPassword
            };

            _userQueryRepository.Setup(r => r.GetByUsername(username))
                                .Returns(user);

            // Act
            var result = _userService.ValidatePassword(username, plainPassword);

            // Assert
            Assert.True(result);
            _userQueryRepository.Verify(r => r.GetByUsername(username), Times.Once);
        }

        [Fact]
        public void ValidatePassword_ShouldReturnFalse_WhenUserNotFound()
        {
            // Arrange
            var username = "unknownuser";
            var password = "somePassword";

            _userQueryRepository.Setup(r => r.GetByUsername(username))
                                .Returns((User)null);

            // Act
            var result = _userService.ValidatePassword(username, password);

            // Assert
            Assert.False(result);
            _userQueryRepository.Verify(r => r.GetByUsername(username), Times.Once);
        }

        [Fact]
        public void ValidatePassword_ShouldReturnFalse_WhenPasswordIsIncorrect()
        {
            // Arrange
            var username = "testuser";
            var plainPassword = "correctPassword";
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(plainPassword);

            var user = new User
            {
                Username = username,
                Password = hashedPassword
            };

            _userQueryRepository.Setup(r => r.GetByUsername(username))
                                .Returns(user);

            // Act
            var result = _userService.ValidatePassword(username, "wrongPassword");

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void EditEmail_ShouldUpdateEmail_WhenUserExists()
        {
            // Arrange
            var username = "existinguser";
            var newEmail = "new@example.com";
            var userBefore = new User { Username = username, Email = "old@example.com" };
            var userAfter = new User { Username = username, Email = newEmail };

            _userQueryRepository.SetupSequence(r => r.GetByUsername(username))
                .Returns(userBefore) // primera llamada (verifica que existe)
                .Returns(userAfter); // segunda llamada (devuelve el actualizado)

            // Act
            var result = _userService.EditEmail(username, newEmail);

            // Assert
            _userQueryRepository.Verify(r => r.UpdateEmail(username, newEmail), Times.Once);
            Assert.NotNull(result);
            Assert.Equal(newEmail, result.Email);
        }

        [Fact]
        public void EditEmail_ShouldThrowException_WhenUserDoesNotExist()
        {
            // Arrange
            var username = "nonexistent";
            var newEmail = "new@example.com";

            _userQueryRepository.Setup(r => r.GetByUsername(username))
                                .Returns((User)null);

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => _userService.EditEmail(username, newEmail));
            Assert.Equal($"User to edit not found {username}", ex.Message);

            _userQueryRepository.Verify(r => r.UpdateEmail(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void EditEmail_ShouldReturnNull_WhenUserNotFoundAfterUpdate()
        {
            // Arrange
            var username = "existinguser";
            var newEmail = "new@example.com";
            var userBefore = new User { Username = username, Email = "old@example.com" };

            _userQueryRepository.SetupSequence(r => r.GetByUsername(username))
                .Returns(userBefore)  // primera llamada: existe
                .Returns((User)null); // segunda llamada: ya no existe

            // Act
            var result = _userService.EditEmail(username, newEmail);

            // Assert
            _userQueryRepository.Verify(r => r.UpdateEmail(username, newEmail), Times.Once);
            Assert.Null(result);
        }

        [Fact]
        public void EditMainPhone_ShouldUpdatePhone_WhenUserExists()
        {
            // Arrange
            var username = "existinguser";
            var newPhone = "987654321";
            var userBefore = new User { Username = username, MainPhone = "123456789" };
            var userAfter = new User { Username = username, MainPhone = newPhone };

            _userQueryRepository.SetupSequence(r => r.GetByUsername(username))
                .Returns(userBefore) // primera llamada: existe
                .Returns(userAfter); // segunda llamada: ya actualizado

            // Act
            var result = _userService.EditMainPhone(username, newPhone);

            // Assert
            _userQueryRepository.Verify(r => r.UpdateMainPhone(username, newPhone), Times.Once);
            Assert.NotNull(result);
            Assert.Equal(newPhone, result.MainPhone);
        }

        [Fact]
        public void EditMainPhone_ShouldThrowException_WhenUserDoesNotExist()
        {
            // Arrange
            var username = "nonexistent";
            var newPhone = "987654321";

            _userQueryRepository.Setup(r => r.GetByUsername(username))
                                .Returns((User)null);

            // Act & Assert
            var ex = Assert.Throws<Exception>(() => _userService.EditMainPhone(username, newPhone));
            Assert.Equal($"User to edit not found {username}", ex.Message);

            _userQueryRepository.Verify(r => r.UpdateMainPhone(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public void EditMainPhone_ShouldReturnNull_WhenUserNotFoundAfterUpdate()
        {
            // Arrange
            var username = "existinguser";
            var newPhone = "987654321";
            var userBefore = new User { Username = username, MainPhone = "123456789" };

            _userQueryRepository.SetupSequence(r => r.GetByUsername(username))
                .Returns(userBefore)  // primera llamada: existe
                .Returns((User)null); // segunda llamada: no encontrado después de update

            // Act
            var result = _userService.EditMainPhone(username, newPhone);

            // Assert
            _userQueryRepository.Verify(r => r.UpdateMainPhone(username, newPhone), Times.Once);
            Assert.Null(result);
        }

        [Fact]
        public void GenerateLinkCode_ShouldDeleteOldCodesAndInsertNewCode()
        {
            // Arrange
            var userId = "123";
            long parsedUserId = 123;

            // Para capturar el código insertado
            TelegramLinkCode? insertedCode = null;

            _codeQueryRepository.Setup(r => r.DeleteOldcodes(parsedUserId));
            _codeQueryRepository.Setup(r => r.InsertCode(It.IsAny<TelegramLinkCode>()))
                                .Callback<TelegramLinkCode>(code => insertedCode = code);

            // Act
            var resultCode = _userService.GenerateLinkCode(userId);

            // Assert
            _codeQueryRepository.Verify(r => r.DeleteOldcodes(parsedUserId), Times.Once);
            _codeQueryRepository.Verify(r => r.InsertCode(It.IsAny<TelegramLinkCode>()), Times.Once);

            Assert.NotNull(insertedCode);
            Assert.Equal(resultCode, insertedCode.Code);
            Assert.Equal(parsedUserId, insertedCode.UserId);

            // Código tiene 6 caracteres y está en mayúsculas
            Assert.Equal(6, resultCode.Length);
            Assert.Equal(resultCode, resultCode.ToUpper());

            // ExpiresAt está en un rango válido (10 minutos +- un pequeño margen)
            var expectedExpiresAt = DateTime.UtcNow.AddMinutes(10);
            var diff = insertedCode.ExpiresAt - expectedExpiresAt;
            Assert.InRange(diff.TotalSeconds, -5, 5); // margen de 5 segundos para evitar fallos por tiempo ejecución
        }

        [Fact]
        public void UnlinkTelegram_ShouldCallRepositoryWithCorrectUserId()
        {
            // Arrange
            var userId = "123";

            _userQueryRepository.Setup(r => r.UnlinkTelegram(userId));

            // Act
            _userService.UnlinkTelegram(userId);

            // Assert
            _userQueryRepository.Verify(r => r.UnlinkTelegram(userId), Times.Once);
        }
    }
}
