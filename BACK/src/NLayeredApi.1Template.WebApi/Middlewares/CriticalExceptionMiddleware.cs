using Microsoft.Data.SqlClient;

namespace NaviMente.WebApi.Middlewares
{
    public class CriticalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<CriticalExceptionMiddleware> _logger;

        public CriticalExceptionMiddleware(RequestDelegate next, ILogger<CriticalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (SqlException sqlex)
            {
                if (sqlex.Number == 258) //Connection timeout
                {
                    _logger.LogCritical(sqlex, "Fatal error occurred in database!!");
                }
            }
        }
    }
}
