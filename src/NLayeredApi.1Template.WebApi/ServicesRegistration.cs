using Microsoft.EntityFrameworkCore;
using Hellang.Middleware.ProblemDetails;
using Microsoft.Data.SqlClient;
using NaviMente.WebApi.Infrastructure.Persistence;

namespace NaviMente.WebApi
{
    public static class ServicesRegistration
    {
        public static IServiceCollection AddAuthSchemas(this IServiceCollection services)
        {
            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = "https://sts.windows.net/9c1dfdd0-7032-44d6-adfe-27ae4e0d2326/";
                    options.Audience = "api://26e5bf9c-9ca7-47c9-bbbc-13f11b4b41ba";
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

            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(connectionString)/*.AddInterceptors(new ChangesInterceptor(services.BuildServiceProvider()))*/);
            services.AddScoped<DatabaseService>();
            //services.AddScoped<CandidatoQueryRepository>();

            //services.Configure<EmailSettings>(options => configuration.GetSection("MailSettings").Bind(options));
            //services.AddHttpClient<EmailService>(client =>
            //{
            //    client.BaseAddress = new Uri(configuration.GetValue<string>("EmailSettings:BaseUrl") ?? "");
            //    client.DefaultRequestHeaders.Add("client_id", configuration.GetValue<string>("EmailSettings:ClientId"));
            //    client.DefaultRequestHeaders.Add("secret", configuration.GetValue<string>("EmailSettings:Secret"));

            //}).ConfigurePrimaryHttpMessageHandler(handler => new HttpClientHandler()
            //{
            //    ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => {                    
            //        return errors == SslPolicyErrors.None;
            //    }
            //});

            return services;
        }
    }
}
