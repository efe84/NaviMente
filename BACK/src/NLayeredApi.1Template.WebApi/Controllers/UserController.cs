using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using NaviMente.WebApi.Domain.Shared.Entities;
using NaviMente.WebApi.Dto.Enums;
using NaviMente.WebApi.Dto.User;
using NaviMente.WebApi.Infrastructure.Services;

namespace NaviMente.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;

        public UserController(ILogger<UserController> logger, IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        /// <summary>
        /// Método Post para el registro de un nuevo usuario
        /// </summary>
        /// <param name="userRegister">Username, email, contraseña y numero de telefono</param>
        /// <returns></returns>
        [HttpPost("Register")]
        public IActionResult Register([FromBody] UserRegisterDTO userRegister)
        {
            try
            {
                var userId = _userService.CreateUser(userRegister);
                if (userId == 0)
                    return BadRequest("Error al crear nuevo usuario");

                return Ok(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el registro del usuario {userLogin}", userRegister.Username);
                return BadRequest();
            }
        }

        [HttpGet()]
        public IActionResult GetUser([FromQuery] string username)
        {
            try
            {
                User user = _userService.GetUser(username);
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


        [HttpPut("EditEmail")]
        public IActionResult EditEmail([FromBody] NewEmailDTO newEmail, [FromQuery] string username)
        {
            try
            {
                _logger.LogInformation("Actualizando el correo del usuario {userName}", username);
                User? user = _userService.EditEmail(username, newEmail.NewEmail);
                if (user == null)
                    return BadRequest("Error editando email del usuario");
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando el correo del usuario {userLogin}", username);
                return BadRequest();
            }
        }

        [HttpPut("EditMainPhone")]
        public async Task<IActionResult> EditMainPhone([FromBody] NewMainPhoneDTO newMainPhone, [FromQuery] string username)
        {
            try
            {
                _logger.LogInformation("Actualizando el telefono principal del usuario {userName}", username);
                User? user = _userService.EditMainPhone(username, newMainPhone.NewMainPhone);
                if (user == null)
                    return BadRequest("Error editando teléfono principal del usuario");
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error actualizando el telefono principal del usuario {userLogin}", username);
                return BadRequest();
            }
        }

        [HttpPost("AddPhone")]
        public IActionResult AddPhone([FromQuery] string username, [FromBody] NewPhoneDTO newPhone)
        {
            try
            {
                _logger.LogInformation("Añadiendo telefono al usuario {username}", username);
                User? user = _userService.AddPhone(username, newPhone.NewPhone);
                if (user == null)
                    return BadRequest("Error añadiendo teléfono al usuario");
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error añadiendo telefono al usuario {username}", username);
                return BadRequest();
            }
        }

        [HttpDelete("DeletePhone")]
        public IActionResult DeletePhone([FromQuery] string username, [FromQuery] string phoneNumber)
        {
            try
            {
                _logger.LogInformation("Eliminando telefono del usuario {username}", username);
                User? user = _userService.RemovePhone(username, phoneNumber);
                if (user == null)
                    return BadRequest("Error eliminando teléfono al usuario");
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error eliminando el telefono al usuario {username}", username);
                return BadRequest();
            }
        }

        [HttpPost("GenerateCode")]
        public IActionResult GenerateCode([FromQuery] string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return BadRequest("Invalid user ID");

            string code = _userService.GenerateLinkCode(userId);
            return Ok(new { code });
        }

        [HttpPut("UnlinkTelegram")]
        public IActionResult UnlinkTelegram([FromQuery] string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return BadRequest("Invalid user ID");

            _userService.UnlinkTelegram(userId);
            return Ok();
        }
    }

}
