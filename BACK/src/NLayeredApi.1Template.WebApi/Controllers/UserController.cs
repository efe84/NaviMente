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
using NaviMente.WebApi.Domain.Shared.Entities;
using NaviMente.WebApi.Dto.Enums;
using NaviMente.WebApi.Dto.User;

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

        [AllowAnonymous]
        [HttpGet()]
        public async Task<IActionResult> GetUser([FromQuery] string username)
        {
            try
            {
                User user = await _userService.GetUserInfo(username);
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error recuperando la informacion del usuario {userLogin}", username);
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
                    User user = _userService.GetUser(userLogin.Username);
                    await GenerateCookie(userLogin.Username, user.Role);
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
        private async Task GenerateCookie(string userName, UserRolesEnum role)
        {
            var claims = new List<Claim>
            {
                new (ClaimTypes.Role, role.ToString()),
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


        [AllowAnonymous]
        [HttpPost("EditEmail")]
        public async Task<IActionResult> EditEmail([FromBody] string newEmail, [FromQuery] string username)
        {
            try
            {
                _logger.LogInformation("Actualizando el correo del usuario {userName}", username);
                await _userService.EditEmail(username, newEmail);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando el correo del usuario {userLogin}", username);
                return BadRequest();
            }
        }

        [AllowAnonymous]
        [HttpPost("EditMainPhone")]
        public async Task<IActionResult> EditMainPhone([FromBody] string newMainPhone, [FromQuery] string username)
        {
            try
            {
                _logger.LogInformation("Actualizando el telefono principal del usuario {userName}", username);
                await _userService.EditMainPhone(username, newMainPhone);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando el telefono principal del usuario {userLogin}", username);
                return BadRequest();
            }
        }

        [AllowAnonymous]
        [HttpPost("AddPhone")]
        public async Task<IActionResult> AddPhone([FromBody] string newPhone, [FromQuery] string username)
        {
            try
            {
                _logger.LogInformation("Actualizando el telefono principal del usuario {userName}", username);
                await _userService.AddPhone(username, newPhone);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando el telefono principal del usuario {userLogin}", username);
                return BadRequest();
            }
        }

    }
}
