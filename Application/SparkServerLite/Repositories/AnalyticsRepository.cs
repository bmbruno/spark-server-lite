using SparkServerLite.Interfaces;
using SparkServerLite.Models;

namespace SparkServerLite.Repositories
{
    public class AnalyticsRepository : IAnalyticsRepository<Visit>
    {
        private readonly IAppSettings _settings;

        public AnalyticsRepository(IAppSettings settings)
        {
            _settings = settings;
        }

        public Visit Get(int ID)
        {
            return new Visit();
        }

        public IEnumerable<Visit> GetAll()
        {
            return new List<Visit>();
        }

        public int Create(Visit newItem)
        {
            return 0;
        }

        public void Update(Visit updateItem)
        { 
                    
        }

        public void Delete(int ID)
        {
        
        }
    }
}
