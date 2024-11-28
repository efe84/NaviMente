using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NaviMente.WebApi.Domain.Services;
using NaviMente.WebApi.Domain.Shared.Entities;
using NaviMente.WebApi.Dto.Location;
using NaviMente.WebApi.Dto.Login;
using NaviMente.WebApi.Infrastructure.Persistence;
using NaviMente.WebApi.Infrastructure.Services;

namespace NaviMente.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LocationController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly ILogger<LocationController> _logger;
        private readonly LocationService _locationService;

        public LocationController(IConfiguration configuration, ILogger<LocationController> logger, ApplicationContext dbContext)
        {
            _config = configuration;
            _logger = logger;
            _locationService = new LocationService(dbContext);
        }

        [AllowAnonymous]
        [HttpPost()]
        public IActionResult GetLocation([FromBody] LocationDTO location)
        {
            try
            {
                LocationPointDTO foundLocation = _locationService.GetLocation(location.DeviceId, location.TimeStamp);
                return Ok(foundLocation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en la busqueda de la localización del dispositivo {location}", location.DeviceId);
                return BadRequest();
            }
        }

        [AllowAnonymous]
        [HttpPost("Last")]
        public IActionResult GetLastLocation([FromBody] long deviceId)
        {
            try
            {
                LocationPointDTO foundLocation = _locationService.GetLastLocation(deviceId);
                return Ok(foundLocation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en la busqueda de la localización del dispositivo {location}", deviceId);
                return BadRequest();
            }
        }
    }
}
