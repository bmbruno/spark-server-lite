using Microsoft.Data.Sqlite;
using SparkServerLite.Interfaces;
using SparkServerLite.Models.Analytics;
using SparkServerLite.Repositories;

namespace SparkServerLite.Infrastructure
{
    public class Analytics
    {
        private readonly IAppSettings _settings;
        private readonly IAnalyticsRepository<Visit> _analyticsRepo;
        private readonly Interfaces.ILogger _logger;

        public Analytics(IAppSettings settings, IAnalyticsRepository<Visit> repo, Interfaces.ILogger logger)
        {
            _settings = settings;
            _analyticsRepo = repo;
            _logger = logger;
        }

        /// <summary>
        /// Records a page visit to the database.
        /// </summary>
        /// <param name="page">Page URL of this visit.</param>
        /// <param name="userAgent">User Agent string, if available.</param>
        /// <param name="referer">URL/page referer from the request, if available.</param>
        public void RecordVisit(string page, string userAgent = "", string referer = "")
        {
            // TODO: log 'page empty' condition - this should not really happen since page is handled by the app, not the user's request
            if (String.IsNullOrEmpty(page))
            {
                return;
            }

            Visit visit = new Visit();
            visit.Active = true;
            visit.CreateDate = DateTime.Now;
            visit.Date = visit.CreateDate;
            visit.Page = !String.IsNullOrEmpty(page) ? page.Trim() : null;
            visit.Referer = !String.IsNullOrEmpty(referer) ? referer.Trim() : null;

            if (!String.IsNullOrEmpty(userAgent))
            {
                visit.UserAgent = userAgent.Trim();
                // TODO: extract useragent into variables
            }

            try
            {
                _analyticsRepo.Create(visit);
            }
            catch (Exception exc)
            {
                _logger.Exception("Exception when creating new analytics Visit object.", exc);
            }
        }
    }
}
