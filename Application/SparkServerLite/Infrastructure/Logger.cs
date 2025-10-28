using SparkServerLite.Interfaces;

namespace SparkServerLite.Infrastructure
{
    public class Logger : SparkServerLite.Interfaces.ILogger
    {
        /// <summary>
        /// Path to the log file on disk.
        /// </summary>
        private string _logPath = string.Empty;

        private IAppSettings _settings;
        
        public Logger(IAppSettings settings)
        {
            _settings = settings;
            _logPath = $"{_settings.LogFolder}/{DateTime.Today.ToString("yyyyMMdd")}.txt";
        }

        public void Info(string message)
        {
            File.AppendAllText(_logPath, $"[{DateTime.Now.ToString("HH:mm:ss:FFF")}] [INFO] {message}\n");
        }

        public void Error(string message)
        {
            File.AppendAllText(_logPath, $"[{DateTime.Now.ToString("HH:mm:ss:FFF")}] [ERROR] {message}\n");
        }

        public void Exception(string message, Exception exc)
        {
            File.AppendAllText(_logPath, $"[{DateTime.Now.ToString("HH:mm:ss:FFF")}] [EXCEPTION] {message}\nEXCEPTION: {exc.ToString()}\n");
        }
    }
}
