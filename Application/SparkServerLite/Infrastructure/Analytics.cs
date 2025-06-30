using Microsoft.Data.Sqlite;
using SparkServerLite.Interfaces;
using SparkServerLite.Models;
using SparkServerLite.Repositories;

namespace SparkServerLite.Infrastructure
{
    public class Analytics
    {
        private readonly IAppSettings _settings;
        private readonly IAnalyticsRepository<Visit> _analyticsRepo;

        public Analytics(IAppSettings settings, IAnalyticsRepository<Visit> repo)
        {
            _settings = settings;
            _analyticsRepo = repo;
        }

        /// <summary>
        /// Records a single visit to the database.
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
                // Store in database
                _analyticsRepo.Create(visit);
            }
            catch (Exception exc)
            {
                // TODO: handle concurrency issues/errors - move this into the Create method (concurrency issues are a responsibility of the repo, not the service class)
            }
        }
    }
}
