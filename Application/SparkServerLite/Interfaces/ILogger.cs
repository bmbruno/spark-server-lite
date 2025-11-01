namespace SparkServerLite.Interfaces
{
    public interface ILogger
    {
        /// <summary>
        /// Logs an informational message to the log file on disk.
        /// </summary>
        /// <param name="message">Message to log.</param>
        void Info(string message);

        /// <summary>
        /// Logs an error message to the log file on disk.
        /// </summary>
        /// <param name="message">Message to log.</param>
        void Error(string message);

        /// <summary>
        /// Logs an Exception and related message to the log file on disk.
        /// </summary>
        /// <param name="message">Message to log.</param>
        /// <param name="exc">Exception object to be logged.</param>
        void Exception(string message, Exception exc);
    }
}
