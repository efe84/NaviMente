using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Amqp.Transaction;
using Microsoft.Extensions.Logging;
using Moq;
using NaviMente.WebApi.Controllers;
using NaviMente.WebApi.Domain.Shared.Entities;
using NaviMente.WebApi.Dto.Enums;
using NaviMente.WebApi.Dto.User;
using NaviMente.WebApi.Infrastructure.Services;
using System.Security.Claims;

namespace NaviMente.Tests.Domain
{
    public class UserControllerTests
    {
        private readonly Mock<IUserService> _userServiceMock;
        private readonly Mock<ILogger<UserController>> _loggerMock;
        private readonly UserController _userController;

        public UserControllerTests()
        {
            _userServiceMock = new Mock<IUserService>();
            _loggerMock = new Mock<ILogger<UserController>>();
            _userController = new UserController(_loggerMock.Object, _userServiceMock.Object);
        }

        [Fact]
        public void Register_ReturnsOk_WhenUserCreated()
        {
            // Arrange
            var userRegisterDto = new UserRegisterDTO
            {
                Username = "testuser",
                Email = "test@example.com",
                Password = "password123",
                MainPhone = "1234567890"
            };

            _userServiceMock.Setup(s => s.CreateUser(It.IsAny<UserRegisterDTO>())).Returns(1);

            // Act
            var result = _userController.Register(userRegisterDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal((long)1, okResult.Value);
        }

        [Fact]
        public void Register_ReturnsBadRequest_WhenUserIdIsZero()
        {
            // Arrange
            var userRegisterDto = new UserRegisterDTO
            {
                Username = "testuser",
                Email = "test@example.com",
                Password = "Password123!",
                MainPhone = "1234567890"
            };

            _userServiceMock.Setup(s => s.CreateUser(userRegisterDto)).Returns(0);

            // Act
            var result = _userController.Register(userRegisterDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Error al crear nuevo usuario", badRequestResult.Value);
        }

        [Fact]
        public void Register_ReturnsBadRequest_WhenExceptionThrown()
        {
            // Arrange
            var userRegisterDto = new UserRegisterDTO
            {
                Username = "testuser",
                Email = "test@example.com",
                Password = "Password123!",
                MainPhone = "1234567890"
            };

            _userServiceMock.Setup(s => s.CreateUser(userRegisterDto))
                            .Throws(new Exception("Some error"));

            // Act
            var result = _userController.Register(userRegisterDto);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void GetUser_ReturnsOk_WithUser()
        {
            // Arrange
            string username = "testuser";
            var user = new User { Username = username };

            _userServiceMock.Setup(s => s.GetUser(username)).Returns(user);

            // Act
            var result = _userController.GetUser(username);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(user, okResult.Value);
        }

        [Fact]
        public void GetUser_ReturnsBadRequest_WhenExceptionThrown()
        {
            // Arrange
            string username = "testuser";

            _userServiceMock.Setup(s => s.GetUser(username))
                            .Throws(new Exception("User not found"));

            // Act
            var result = _userController.GetUser(username);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task Login_ReturnsOk_WhenPasswordValid()
        {
            // Arrange
            var userLogin = new UserLoginDTO { Username = "testuser", Password = "pass" };
            var user = new User { Username = userLogin.Username, Role = UserRolesEnum.Default };

            _userServiceMock.Setup(s => s.ValidatePassword(userLogin.Username, userLogin.Password)).Returns(true);
            _userServiceMock.Setup(s => s.GetUser(userLogin.Username)).Returns(user);

            // Mock AuthenticationService
            var authServiceMock = new Mock<IAuthenticationService>();
            authServiceMock
                .Setup(a => a.SignInAsync(
                    It.IsAny<HttpContext>(),
                    It.IsAny<string>(),
                    It.IsAny<ClaimsPrincipal>(),
                    It.IsAny<AuthenticationProperties>()))
                .Returns(Task.CompletedTask);

            // Crear HttpContext con servicio mockeado
            var httpContext = new DefaultHttpContext();
            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock
                .Setup(sp => sp.GetService(typeof(IAuthenticationService)))
                .Returns(authServiceMock.Object);
            httpContext.RequestServices = serviceProviderMock.Object;

            _userController.ControllerContext = new ControllerContext { HttpContext = httpContext };

            // Act
            var result = await _userController.Login(userLogin);

            // Assert
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Login_ReturnsUnauthorized_WhenPasswordInvalid()
        {
            // Arrange
            var userLogin = new UserLoginDTO { Username = "testuser", Password = "wrongpass" };

            _userServiceMock.Setup(s => s.ValidatePassword(userLogin.Username, userLogin.Password)).Returns(false);

            // Act
            var result = await _userController.Login(userLogin);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);

            Assert.Equal("{ message = invalid credentials }", unauthorizedResult.Value.ToString());
        }

        [Fact]
        public async Task Login_ReturnsBadRequest_WhenExceptionThrown()
        {
            // Arrange
            var userLogin = new UserLoginDTO { Username = "testuser", Password = "pass" };

            _userServiceMock.Setup(s => s.ValidatePassword(userLogin.Username, userLogin.Password)).Throws(new Exception("error"));

            // Act
            var result = await _userController.Login(userLogin);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void EditEmail_ReturnsOk_WhenSuccess()
        {
            string username = "user1";
            var newEmailDto = new NewEmailDTO { NewEmail = "new@mail.com" };
            var updatedUser = new User { Username = username, Email = newEmailDto.NewEmail };

            _userServiceMock.Setup(s => s.EditEmail(username, newEmailDto.NewEmail)).Returns(updatedUser);

            var result = _userController.EditEmail(newEmailDto, username);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(updatedUser, okResult.Value);
        }

        [Fact]
        public void EditEmail_ReturnsBadRequest_WhenUserNull()
        {
            string username = "user1";
            var newEmailDto = new NewEmailDTO { NewEmail = "new@mail.com" };

            _userServiceMock.Setup(s => s.EditEmail(username, newEmailDto.NewEmail)).Returns((User?)null);

            var result = _userController.EditEmail(newEmailDto, username);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Error editando email del usuario", badRequestResult.Value);
        }

        [Fact]
        public void EditEmail_ReturnsBadRequest_WhenExceptionThrown()
        {
            string username = "user1";
            var newEmailDto = new NewEmailDTO { NewEmail = "new@mail.com" };

            _userServiceMock.Setup(s => s.EditEmail(username, newEmailDto.NewEmail))
                            .Throws(new Exception("error"));

            var result = _userController.EditEmail(newEmailDto, username);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task EditMainPhone_ReturnsOk_WhenSuccess()
        {
            string username = "user1";
            var newPhoneDto = new NewMainPhoneDTO { NewMainPhone = "123456789" };
            var updatedUser = new User { Username = username, MainPhone = newPhoneDto.NewMainPhone };

            _userServiceMock.Setup(s => s.EditMainPhone(username, newPhoneDto.NewMainPhone)).Returns(updatedUser);

            var result = await _userController.EditMainPhone(newPhoneDto, username);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(updatedUser, okResult.Value);
        }

        [Fact]
        public async Task EditMainPhone_ReturnsBadRequest_WhenUserNull()
        {
            string username = "user1";
            var newPhoneDto = new NewMainPhoneDTO { NewMainPhone = "123456789" };

            _userServiceMock.Setup(s => s.EditMainPhone(username, newPhoneDto.NewMainPhone)).Returns((User?)null);

            var result = await _userController.EditMainPhone(newPhoneDto, username);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Error editando teléfono principal del usuario", badRequestResult.Value);
        }

        [Fact]
        public async Task EditMainPhone_ReturnsBadRequest_WhenExceptionThrown()
        {
            string username = "user1";
            var newPhoneDto = new NewMainPhoneDTO { NewMainPhone = "123456789" };

            _userServiceMock.Setup(s => s.EditMainPhone(username, newPhoneDto.NewMainPhone))
                            .Throws(new Exception("error"));

            var result = await _userController.EditMainPhone(newPhoneDto, username);

            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public void GenerateCode_ReturnsOk_WithCode()
        {
            string userId = "42";
            string expectedCode = "ABC123";
            string resultValue = "{ code = ABC123 }";

            _userServiceMock.Setup(s => s.GenerateLinkCode(userId)).Returns(expectedCode);

            var result = _userController.GenerateCode(userId);

            var okResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(resultValue, okResult.Value.ToString());
        }

        [Fact]
        public void GenerateCode_ReturnsBadRequest_WhenUserIdIsNullOrEmpty()
        {
            var result = _userController.GenerateCode(null);

            Assert.IsType<BadRequestObjectResult>(result);

            var result2 = _userController.GenerateCode("");

            Assert.IsType<BadRequestObjectResult>(result2);
        }

        [Fact]
        public void UnlinkTelegram_ReturnsOk_WhenUserIdValid()
        {
            string userId = "42";

            var result = _userController.UnlinkTelegram(userId);

            Assert.IsType<OkResult>(result);
            _userServiceMock.Verify(s => s.UnlinkTelegram(userId), Times.Once);
        }

        [Fact]
        public void UnlinkTelegram_ReturnsBadRequest_WhenUserIdNullOrEmpty()
        {
            var result = _userController.UnlinkTelegram(null);
            Assert.IsType<BadRequestObjectResult>(result);

            var result2 = _userController.UnlinkTelegram("");
            Assert.IsType<BadRequestObjectResult>(result2);
        }

    }
}
