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

        /// <summary>
        /// Records a single visit to the database.
        /// </summary>
        /// <param name="page">Page URL of this visit.</param>
        /// <param name="userAgent">User Agent string, if available.</param>
        /// <param name="referer"></param>
        public void RecordVisit(string page, string userAgent = "", string referer = "")
        {
            if (String.IsNullOrEmpty(page))
            {
                // TODO: log 'page empty' condition
                return;
            }

            Visit visit = new Visit();
            visit.Date = DateTime.Now;
            visit.Page = page.Trim();

            // TODO: extract useragent into variables
            if (!String.IsNullOrEmpty(userAgent))
            {
                visit.UserAgent.Trim();
            }

            // TODO: store in database

            // TODO: handle concurrency issues/errors
        }
    }
}
