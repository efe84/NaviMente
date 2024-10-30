using Microsoft.EntityFrameworkCore;
using Hellang.Middleware.ProblemDetails;
using Microsoft.Data.SqlClient;
using NaviMente.WebApi.Infrastructure.Persistence;
using NaviMente.WebApi.Domain.Services;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace NaviMente.WebApi
{
    public static class ServicesRegistration
    {
        public static IServiceCollection AddAuthSchemas(this IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
             .AddCookie(options =>
             {
                 options.Cookie.SameSite = SameSiteMode.None;
                 options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                 options.Events.OnRedirectToAccessDenied =
                 options.Events.OnRedirectToLogin = c =>
                 {
                     //Evitar redireccion a una pagina, enviar 401 y dejar que lo controle la SPA
                     c.Response.StatusCode = StatusCodes.Status401Unauthorized;
                     return Task.CompletedTask;
                 };
             });

            return services;
        }

        public static IServiceCollection AddErrorHandling(this IServiceCollection services)
        {
            services.AddProblemDetails(opts =>
            {
                opts.IncludeExceptionDetails = (ctx, ex) => false;

                opts.OnBeforeWriteDetails = (ctx, dtls) =>
                {
                    if (dtls.Status == StatusCodes.Status500InternalServerError)
                    {
                        dtls.Detail = "An error occurred in our API. Use the trace id when contacting us.";
                    }
                };
                opts.MapToStatusCode<Exception>(StatusCodes.Status500InternalServerError);
                opts.Rethrow<SqlException>();
            });

            return services;
        }

        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            string? connectionString = configuration.GetConnectionString("Default") ?? throw new ArgumentException("Cadena de conexión no encontrada en la configuración", nameof(configuration));

            services.AddDbContext<ApplicationContext>();
            services.AddScoped<UserService>();

            return services;
        }
    }
}
