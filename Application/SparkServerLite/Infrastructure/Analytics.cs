using Microsoft.Data.Sqlite;
using SparkServerLite.Interfaces;
using SparkServerLite.Models;

namespace SparkServerLite.Infrastructure
{
    public class Analytics
    {
        private readonly IAppSettings _settings;

        public Analytics(AppSettings settings)
        {
            _settings = settings;
        }

        public void RecordVisit(string page, string userAgent, string referer)
        {
            // TODO: extract useragent into variables

            // TODO: store in database

            // TODO: handle concurrency issues/errors
        }
    }
}
