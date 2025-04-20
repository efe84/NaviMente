using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NaviMente.WebApi.Infrastructure.Services;
using NaviMente.WebApi.Dto.Device;
using NaviMente.WebApi.Infrastructure.Persistence;
using MongoDB.Driver.GeoJsonObjectModel;
using MongoDB.Driver;

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
        /// <param name="userName">Nombre del usuario</param>
        /// <returns>Lista de dispositivos</returns>
        [HttpPost("List")]
        public async Task<IActionResult> GetUserDevices([FromBody] string userName)
        {
            try
            {
                var devices = await _deviceService.GetUserDevicesAsync(userName);
                return Ok(devices);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en la obtencion de NaviBands para el usuario {userName}", userName);
                return BadRequest();
            }
        }

        /// <summary>
        /// Método Delete para el registro de un nuevo dispositivo
        /// </summary>
        /// <param name="deviceRegister">Username, email, contraseña y numero de telefono</param>
        /// <returns></returns>
        [HttpDelete("Unassign")]
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
        [HttpPost("Zones")]
        public async Task<IActionResult> GetZones([FromBody] string serialNumber)
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
    }
}
