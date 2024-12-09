using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NaviMente.WebApi.Domain.Services;
using NaviMente.WebApi.Dto.Device;
using NaviMente.WebApi.Infrastructure.Persistence;
using NaviMente.WebApi.Infrastructure.Services;

namespace NaviMente.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController : ControllerBase
    {

        private readonly IConfiguration _config;
        private readonly ILogger<DeviceController> _logger;
        private readonly DeviceService _deviceService;

        public DeviceController(IConfiguration configuration, ILogger<DeviceController> logger, ApplicationContext dbContext)
        {
            _config = configuration;
            _logger = logger;
            _deviceService = new DeviceService(dbContext, logger);
        }

        /// <summary>
        /// Método Post para el registro de un nuevo dispositivo
        /// </summary>
        /// <param name="deviceRegister">Username, email, contraseña y numero de telefono</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] DeviceRegisterDTO deviceRegister)
        {
            try
            {
                await _deviceService.RegisterDeviceAsync(deviceRegister);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en el registro del usuario {deviceName}", deviceRegister.DeviceName);
                return BadRequest();
            }
        }

        /// <summary>
        /// Método Post para el registro de un nuevo dispositivo
        /// </summary>
        /// <param name="deviceRegister">Username, email, contraseña y numero de telefono</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("List")]
        public async Task<IActionResult> GetUserDevices([FromBody] long userId)
        {
            try
            {
                var devices = await _deviceService.GetUserDevicesAsync(userId);
                return Ok(devices);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en la obtencion de NaviBands para el usuario {userId}", userId);
                return BadRequest();
            }
        }

        /// <summary>
        /// Método Post para el registro de un nuevo dispositivo
        /// </summary>
        /// <param name="deviceRegister">Username, email, contraseña y numero de telefono</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("Unassign")]
        public async Task<IActionResult> UnassignDevice([FromBody] DeviceUnassignDTO deviceUnassign)
        {
            try
            {
                await _deviceService.UnassignDeviceAsync(deviceUnassign.UserId, deviceUnassign.SerialNumber);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error desenlazando el usuario {user} del NaviBand {deviceName}", deviceUnassign.UserId, deviceUnassign.SerialNumber);
                return BadRequest();
            }
        }
    }
}
