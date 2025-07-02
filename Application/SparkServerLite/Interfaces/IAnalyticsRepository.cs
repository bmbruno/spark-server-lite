using SparkServerLite.Models.Analytics;

namespace SparkServerLite.Interfaces
{
    public interface IAnalyticsRepository<T>
    {
        /// <summary>
        /// Gets an individual Visit from the database for the given ID.
        /// </summary>
        /// <param name="ID">ID of the Visit.</param>
        /// <returns>Visit object.</returns>
        public T Get(int ID);

        /// <summary>
        /// Gets a list of all Visits from the database. Should probably use this with caution, as there may be lots of records.
        /// </summary>
        /// <returns>Enumerable list of all Visits.</returns>
        public IEnumerable<T> GetAll();
        
        /// <summary>
        /// Inserts a Visit record into the database.
        /// </summary>
        /// <param name="newItem">Visit object.</param>
        void Create(T newItem);

        /// <summary>
        /// Gets data for the PageViews report.
        /// </summary>
        List<PageViewItem> ReportPageViews();
    }
}
