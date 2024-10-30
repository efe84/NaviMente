using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NLayeredApi._1Template.WebApi.Model.Login;
using System.Security.Claims;
using NaviMente.WebApi.Dto.Login;
using NaviMente.WebApi.Infrastructure.Persistence;
using NaviMente.WebApi.Domain.Services;
using Microsoft.EntityFrameworkCore;

namespace NaviMente.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ILogger<UserController> _logger;
        private readonly UserService _userService;

        public UserController(IConfiguration configuration, ILogger<UserController> logger, ApplicationContext dbContext)
        {
            _config = configuration;
            _logger = logger;
            _userService = new UserService(dbContext, logger);
        }

        /// <summary>
        /// Método Post para el registro de un nuevo usuario
        /// </summary>
        /// <param name="userRegister">Username, email, contraseña y numero de telefono</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegisterDTO userRegister)
        {
            try
            {
                await _userService.CreateUserAsync(userRegister);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el registro del usuario {userLogin}", userRegister.Username);
                return BadRequest();
            }
        }

        /// <summary>
        /// Método Post para el logeo de un usuario ya existente
        /// </summary>
        /// <param name="userLogin">Username y contraseña</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO userLogin)
        {
            try
            {
                _logger.LogInformation("Login del usuario {userLogin}", userLogin.Username);
                if (_userService.ValidatePassword(userLogin.Username, userLogin.Password))
                {
                    await GenerateCookie(userLogin.Username);
                    return Ok();
                }
                return Unauthorized(new { message = "invalid credentials" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el login del usuario {userLogin}", userLogin.Username);
                return BadRequest();
            }
        }

        /// <summary>
        /// Funcion auxiliar para generar la cookie del usuario
        /// </summary>
        /// <param name="userName">Nombre de usuario</param>
        /// <returns></returns>
        private async Task GenerateCookie(string userName)
        {
            var claims = new List<Claim>
            {
                new (ClaimTypes.Role, "Administrator"),
                new (ClaimTypes.NameIdentifier, userName),
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(60),
                IsPersistent = true
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(claimsIdentity),
                authProperties);
        }
        
    }
}
