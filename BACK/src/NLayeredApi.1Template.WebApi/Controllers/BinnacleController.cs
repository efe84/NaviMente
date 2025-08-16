using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NaviMente.WebApi.Infrastructure.Services;

namespace NaviMente.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BinnacleController : ControllerBase
    {
        private readonly ILogger<BinnacleController> _logger;
        private readonly IBinnacleService _binnacleService;

        public BinnacleController(ILogger<BinnacleController> logger, IBinnacleService binnacleService)
        {
            _logger = logger;
            _binnacleService = binnacleService;
        }

        /// <summary>
        /// Método GET para recuperar la lista de logs de un dispositivo
        /// </summary>
        /// <param name="serialNumber">numero de serie del dispositivo</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{serialNumber}")]
        public IActionResult GetDeviceLogs(string serialNumber, [FromQuery] int? severity)
        {
            try
            {
                var logs = _binnacleService.GetDeviceLogs(serialNumber, severity);
                return Ok(logs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en la busqueda de logs para el dispositivo {serialnumber}", serialNumber);
                return BadRequest();
            }
        }
    }
}
