using System.Reflection.Metadata;
using System.Text;
using Microsoft.Data.Sqlite;
using Microsoft.VisualBasic;
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
                            ViewCount = Convert.ToInt32(reader["PageViews"])

                        });
                    }
                }

                conn.Close();
            }

            return report;

        }

        public List<VisitByMonthItem> ReportVisitsByMonth()
        {
            List<VisitByMonthItem> report = new List<VisitByMonthItem>();

            using (var conn = new SqliteConnection(_settings.AnalyticsConnectionString))
            {
                SqliteCommand command = conn.CreateCommand();

                command.CommandText = @"
                    SELECT
	                    strftime('%m', [Date]) AS [Month],
	                    CASE strftime('%m', [Date])
		                    WHEN '01' THEN 'January' 
		                    WHEN '02' THEN 'February' 
		                    WHEN '03' THEN 'March' 
		                    WHEN '04' THEN 'April' 
		                    WHEN '05' THEN 'May' 
		                    WHEN '06' THEN 'June' 
		                    WHEN '07' THEN 'July' 
		                    WHEN '08' THEN 'August' 
		                    WHEN '09' THEN 'September' 
		                    WHEN '10' THEN 'October' 
		                    WHEN '11' THEN 'November' 
		                    WHEN '12' THEN 'December' 
		                    ELSE 'N/A' END AS [MonthLabel],
                        strftime('%Y', [Date]) AS [Year],
		                COUNT(*) AS [Visits]
                    FROM Visits
                    WHERE
	                    Active = 1
	                    -- AND [Page] = '/'
                    GROUP BY
	                    strftime('%m', [Date]),
                        strftime('%Y', [Date])";

                conn.Open();

                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        report.Add(new VisitByMonthItem()
                        {
                            Month = Convert.ToInt32(reader["Month"]),
                            MonthLabel = reader["MonthLabel"].ToString(),
                            Year = Convert.ToInt32(reader["Year"]),
                            Visits = Convert.ToInt32(reader["Visits"])
                        });
                    }
                }

                conn.Close();
            }

            return report;
        }
    }
}
