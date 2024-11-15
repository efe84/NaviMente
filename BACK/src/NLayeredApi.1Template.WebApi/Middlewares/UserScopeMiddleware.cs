using System.Security.Claims;

namespace NaviMente.WebApi.Middlewares
{
    public class UserScopeMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<UserScopeMiddleware> _logger;

        public UserScopeMiddleware(RequestDelegate next, ILogger<UserScopeMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.User.Identity is { IsAuthenticated: true })
            {
                ClaimsIdentity? identity = context.User.Identity as ClaimsIdentity;

                var username = identity?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value ?? "";
                var subjectId = identity?.Claims?.FirstOrDefault(c => c.Type == "sub")?.Value ?? "";

                using (_logger.BeginScope("User:{user}, SubjectId:{subject}", username, subjectId))
                {
                    await _next(context);
                }
            }
            else
            {
                await _next(context);
            }
        }
    }
}
