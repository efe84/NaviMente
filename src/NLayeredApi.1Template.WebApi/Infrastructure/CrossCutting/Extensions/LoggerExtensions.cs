namespace NaviMente.WebApi.Infrastructure.CrossCutting.Extensions
{
    public static class LoggerExtensions
    {
        public static class EventIds
        {
            public static readonly EventId ExceptionCaught = new EventId(1000, "ExceptionCaught");
            public static readonly EventId OperationCancelledExceptionCaught = new EventId(1001, "OperationCancelledExceptionCaught");
        }

        public static void ExceptionOccurred(this ILogger logger, Exception ex) => logger.Log(LogLevel.Error, EventIds.ExceptionCaught, ex, "An exception occurred and was caught.");

        public static void OperationCancelledExceptionOccurred(this ILogger logger) => logger.Log(LogLevel.Information, EventIds.OperationCancelledExceptionCaught, "A task/operation cancelled exception was caught.");

        public static void ServiceStarted(this ILogger logger, bool debugMode)
        {
            logger.LogInformation("*******  SERVICIO INICIADO   ********");
            if (debugMode)
                logger.LogInformation($"Modo debug activado.");
        }

        public static void NextCheck(this ILogger logger, DateTime nextCheck) => logger.LogInformation($"Próxima comprobación a las {nextCheck.ToString("HH:mm:ss")}");

    }
}
