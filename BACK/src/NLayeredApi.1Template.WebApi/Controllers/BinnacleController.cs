using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NaviMente.WebApi.Dto.Device;
using NaviMente.WebApi.Infrastructure.Persistence;
using NaviMente.WebApi.Infrastructure.Services;

namespace NaviMente.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BinnacleController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ILogger<BinnacleController> _logger;
        private readonly BinnacleService _binnacleService;

        public BinnacleController(IConfiguration configuration, ILogger<BinnacleController> logger, ApplicationContext dbContext)
        {
            _config = configuration;
            _logger = logger;
            _binnacleService = new BinnacleService(dbContext, logger);
        }

        /// <summary>
        /// Método GET para recuperar la lista de logs de un dispositivo
        /// </summary>
        /// <param name="serialNumber">numero de serie del dispositivo</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{serialNumber}")]
        public async Task<IActionResult> GetDeviceLogs(string serialNumber)
        {
            try
            {
                var logs = await _binnacleService.GetDeviceLogsAsync(serialNumber);
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
