using HealthChecks.UI.Client;
using Serilog;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.HttpLogging;
using Serilog.Exceptions;
using Serilog.Enrichers.Span;
using Prometheus;
using NaviMente.WebApi;
using NaviMente.WebApi.Infrastructure.Persistence;
using NaviMente.WebApi.Middlewares;
using NaviMente.WebApi.BackgroundServices;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration)
    .WriteTo.Console()
    .CreateLogger();

logger.Information("Starting web application");

try
{
    var builder = WebApplication.CreateBuilder(args);
    builder.Logging.ClearProviders();
    builder.Host.UseSerilog((context, loggerConfig) => {
        loggerConfig
        .ReadFrom.Configuration(context.Configuration)
        .WriteTo.Console()
        .Enrich.WithExceptionDetails()
        .Enrich.With<ActivityEnricher>()
        .Enrich.FromLogContext();
    });

    builder.Services.AddErrorHandling();

    builder.Services.AddControllers();

    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, SwaggerOptions>();
    builder.Services.AddSwaggerGen();

    builder.Services.AddAuthSchemas();
    builder.Services.AddInfrastructureServices(builder.Configuration);
    builder.Services.AddHostedService<EmailSenderBackgroundService>();

    builder.Services
        .AddHealthChecks()
        .AddDbContextCheck<ApplicationContext>()
        .AddCheck<CustomHealthCheck>(nameof(CustomHealthCheck));

    builder.Services.AddHttpLogging(logging =>
    {
        logging.LoggingFields = HttpLoggingFields.All;
        logging.RequestBodyLogLimit = 4096;
        logging.ResponseBodyLogLimit = 4096;
    });

    var app = builder.Build();

    app.UsePathBase("/navimente");
    app.UseRouting();

    app.UseHttpLogging();

    app.UseProblemDetails();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.OAuthClientId("f0c9edd6-3287-4720-9f68-1ee6b9d3f4af");
            options.OAuthAppName("NaviMenteApiClient");
            options.OAuthUsePkce();
        });
    }

    app.MapFallback(() => Results.Redirect("/swagger"));
    app.UseHttpsRedirection();

    //Ruta de la plantilla
    app.UseCors(
        options => options.WithOrigins("http://localhost:5173").AllowAnyMethod().AllowAnyHeader().AllowCredentials()
    );

    app.UseAuthentication();
    app.UseMiddleware<UserScopeMiddleware>();
    app.UseAuthorization();

    app.UseMetricServer();
    app.UseHttpMetrics();

    app.MapControllers().RequireAuthorization();

    app.MapHealthChecks("/health", new()
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    }).AllowAnonymous();

    app.Run();
}
catch (Exception ex)
{
    logger.Fatal(ex, "Application terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}