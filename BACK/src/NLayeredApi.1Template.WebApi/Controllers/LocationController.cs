﻿using Microsoft.AspNetCore.Authorization;
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
            _locationService = new LocationService(dbContext, logger);
        }

        [AllowAnonymous]
        [HttpPost()]
        public IActionResult GetLocation([FromBody] LocationDTO location)
        {
            try
            {
                LocationPointDTO foundLocation = _locationService.GetLocation(location.SerialNumber, location.TimeStamp);
                return Ok(foundLocation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en la busqueda de la localización del dispositivo {location}", location.SerialNumber);
                return BadRequest();
            }
        }

        [AllowAnonymous]
        [HttpPost("Last")]
        public IActionResult GetLastLocation([FromBody] string serialNumber)
        {
            try
            {
                LocationPointDTO foundLocation = _locationService.GetLastLocation(serialNumber);
                return Ok(foundLocation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en la busqueda de la localización del NaviBand {location}", serialNumber);
                return BadRequest();
            }
        }

        [AllowAnonymous]
        [HttpPost("Route")]
        public IActionResult GetRoute([FromBody] RouteDTO route)
        {
            try
            {
                LocationLineDTO foundLocation = _locationService.GetRoute(route.SerialNumber, route.StartDate, route.EndDate);
                return Ok(foundLocation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en la busqueda de la ruta para el dispositivo {location} entre las fechas {fecha1} y {fecha2}",
                      route.SerialNumber, route.StartDate, route.EndDate);
                return BadRequest();
            }
        }
    }
}
