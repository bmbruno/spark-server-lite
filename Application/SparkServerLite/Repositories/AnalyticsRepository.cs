using System.Text;
using Microsoft.Data.Sqlite;
using SparkServerLite.Infrastructure;
using SparkServerLite.Interfaces;
using SparkServerLite.Models.Analytics;

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
        
        public void Create(Visit newItem)
        {
            using (var conn = new SqliteConnection(_settings.AnalyticsConnectionString))
            {
                SqliteCommand command = conn.CreateCommand();
                
                command.CommandText = @"INSERT INTO Visits (Date, UserAgent, Domain, Page, Referer, OS, Device, Active, CreateDate) VALUES ($date, $useragent, $domain, $page, $referer, $os, $device, $active, $createdate);";
                command.Parameters.AddWithValue("$date", newItem.Date);
                command.Parameters.AddWithValue("$useragent", !String.IsNullOrEmpty(newItem.UserAgent) ? newItem.UserAgent : DBNull.Value);
                command.Parameters.AddWithValue("$domain", !String.IsNullOrEmpty(newItem.Domain) ? newItem.Domain : DBNull.Value);
                command.Parameters.AddWithValue("$page", !String.IsNullOrEmpty(newItem.Page) ? newItem.Page : DBNull.Value);
                command.Parameters.AddWithValue("$referer", !String.IsNullOrEmpty(newItem.Referer) ? newItem.Referer : DBNull.Value);
                command.Parameters.AddWithValue("$os", !String.IsNullOrEmpty(newItem.OS) ? newItem.OS : DBNull.Value);
                command.Parameters.AddWithValue("$device", !String.IsNullOrEmpty(newItem.Device) ? newItem.Device : DBNull.Value);
                
                command.Parameters.AddWithValue("$active", newItem.Active);
                command.Parameters.AddWithValue("$createdate", newItem.CreateDate.ToString(FormatHelper.SQLiteDateTime));
                
                // TODO: handle concurrency issues here
                
                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
            }
        }

        public List<PageViewItem> ReportPageViews()
        {
            List<PageViewItem> report = new List<PageViewItem>();

            using (var conn = new SqliteConnection(_settings.AnalyticsConnectionString))
            {
                SqliteCommand command = conn.CreateCommand();

                command.CommandText = @"
                    SELECT
	                    [Page],
	                    COUNT(*) AS [PageViews]
                    FROM Visits
                    WHERE
	                    Active = 1
	                    GROUP BY [Page]
	                    ORDER BY [PageViews] DESC, [Date] DESC";

                conn.Open();
                
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        report.Add(new PageViewItem()
                        {
                            Page = reader["Page"].ToString(),
                            PageViews = Convert.ToInt32(reader["PageViews"])

                        });
                    }
                }

                conn.Close();
            }

            return report;

        }
    }
}
