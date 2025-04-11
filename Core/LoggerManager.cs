using NLog;

namespace Core
{
    public class LoggerManager
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public static void LogInfo(string message) => logger.Info(message);
        public static void LogError(string message) => logger.Error(message);
    }
}
