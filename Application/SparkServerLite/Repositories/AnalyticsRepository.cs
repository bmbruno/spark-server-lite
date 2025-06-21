using System.Text;
using Microsoft.Data.Sqlite;
using SparkServerLite.Infrastructure;
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
        
        public void Create(Visit newItem)
        {
            using (var conn = new SqliteConnection(_settings.AnalyticsConnectionString))
            {
                SqliteCommand command = conn.CreateCommand();
                
                command.CommandText = @"INSERT INTO Visits (Date, UserAgent, Domain, Page, Referer, OS, Device, Active, CreateDate) VALUES ($date, $useragent, $domain, $page, $referer, $os, $device, $active, $createdate);";
                command.Parameters.AddWithValue("$date", newItem.Date);
                command.Parameters.AddWithValue("$useragent", !String.IsNullOrEmpty(newItem.UserAgent) ? newItem.UserAgent : null);
                command.Parameters.AddWithValue("$domain", !String.IsNullOrEmpty(newItem.Domain) ? newItem.Domain : null);
                command.Parameters.AddWithValue("$page", !String.IsNullOrEmpty(newItem.Page) ? newItem.Page : null);
                command.Parameters.AddWithValue("$referer", !String.IsNullOrEmpty(newItem.Referer) ? newItem.Referer : null);
                command.Parameters.AddWithValue("$os", !String.IsNullOrEmpty(newItem.OS) ? newItem.OS : null);
                command.Parameters.AddWithValue("$device", !String.IsNullOrEmpty(newItem.Device) ? newItem.Device : null);
                
                command.Parameters.AddWithValue("$active", newItem.Active);
                command.Parameters.AddWithValue("$createDate", newItem.CreateDate.ToString(FormatHelper.SQLiteDateTime));
                
                conn.Open();
                command.ExecuteNonQuery();
                conn.Close();
            }
        }
    }
}
