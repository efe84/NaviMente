using NaviMente.WebApi.Infrastructure.CrossCutting.Extensions;

namespace NaviMente.WebApi.BackgroundServices
{
    public class EmailSenderBackgroundService : BackgroundService
    {
        const string SERVICE_NAME = "EmailSender";

        readonly ILogger<EmailSenderBackgroundService> _logger;
        readonly IServiceProvider _serviceProvider;
        readonly int _checkDelay;
        readonly bool _debugMode;

        public EmailSenderBackgroundService(ILogger<EmailSenderBackgroundService> logger, IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;

            _checkDelay = Convert.ToUInt16(configuration.GetSection("CheckDelay").Value);
            _debugMode = Convert.ToBoolean(configuration.GetSection("DebugMode").Value);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (_logger.BeginScope("ServiceName: {ServiceName}", SERVICE_NAME))
            {
                _logger.ServiceStarted(_debugMode);
                try
                {
                    while (!stoppingToken.IsCancellationRequested)
                    {
                        using (IServiceScope scope = _serviceProvider.CreateScope())
                        {
                            //TO DO invocar a los métodos de las capas inferiores
                        }

                        DateTime nextCheck = DateTime.Now.AddSeconds(_checkDelay);
                        _logger.NextCheck(nextCheck);
                        await Task.Delay(_checkDelay * 1000, stoppingToken);
                    }

                }
                catch (OperationCanceledException)
                {
                    _logger.OperationCancelledExceptionOccurred();
                }
                catch (Exception ex)
                {
                    _logger.ExceptionOccurred(ex);
                }

            }
        }
    }
}
