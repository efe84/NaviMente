using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NaviMente.WebApi.Infrastructure.Services;
using NaviMente.WebApi.Dto.Device;
using NaviMente.WebApi.Infrastructure.Persistence;
using NaviMente.WebApi.Domain.Shared.Entities;

namespace NaviMente.WebApi.Controllers
{
    [AllowAnonymous]
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
        /// Metodo Get para recuperar la lista de dispositivos del usuario
        /// </summary>
        /// <param name="userId">Id del usuario</param>
        /// <returns>Lista de dispositivos</returns>
        [HttpGet("List")]
        public async Task<IActionResult> GetUserDevices([FromQuery] string userId)
        {
            try
            {
                long.TryParse(userId, out long userIdLong);
                var devices = await _deviceService.GetUserDevicesAsync(userIdLong);
                return Ok(devices);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en la obtencion de NaviBands para el usuario {userId}", userId);
                return BadRequest();
            }
        }

        /// <summary>
        /// Método Delete para el registro de un nuevo dispositivo
        /// </summary>
        /// <param name="userId">Id del usuario</param>
        /// <param name="serialNumber">SerialNumber del dispositivo a desvincular</param>
        /// <returns></returns>
        [HttpDelete("Unassign")]
        public async Task<IActionResult> UnassignDevice([FromQuery] long userId, [FromQuery] string serialNumber)
        {
            try
            {
                User userAct = await _deviceService.UnassignDeviceAsync(userId, serialNumber);
                return Ok(userAct);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error desenlazando el usuario {user} del NaviBand {deviceName}", userId, serialNumber);
                return BadRequest();
            }
        }

        /// <summary>
        /// Método Post para el registro de una nueva zona bloqueada
        /// </summary>
        /// <param name="zoneDto">serial del dispositivo y coordenadas de la zona</param>
        /// <returns></returns>
        [HttpPost("BlockZone")]
        public async Task<IActionResult> RegisterBlockedZone([FromBody] ZoneDTO zoneDto)
        {
            try
            {
                await _deviceService.AddRestrictedZone(zoneDto);
                return Ok(new { message = "Zona registrada satisfactoriamente" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error guardando la zona restringida");
                return BadRequest();
            }
        }

        /// <summary>
        /// Método GET para recuperar las zonas bloqueadas
        /// </summary>
        /// <param name="serialNumber">Numero de serial del dispositivo</param>
        /// <returns>Lista de zonas bloqueadas para ese dispositivo</returns>
        [HttpGet("Zones")]
        public async Task<IActionResult> GetZones([FromQuery] string serialNumber)
        {
            try
            {
                var zones = await _deviceService.GetRestrictedZones(serialNumber);
                return Ok(zones);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error recuperando las zonas restringidas");
                return BadRequest();
            }
        }

        /// <summary>
        /// Método DELETE para eliminar una zona bloqueada
        /// </summary>
        /// <param name="zoneId">Numero de identificación de la zona</param>
        /// <returns>true</returns>
        [HttpDelete("DeleteZone")]
        public async Task<IActionResult> DeleteZone([FromQuery] long zoneId)
        {
            try
            {
                await _deviceService.DeleteZone(zoneId);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error recuperando las zonas restringidas");
                return BadRequest();
            }
        }
    }
}
