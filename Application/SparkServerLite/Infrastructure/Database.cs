namespace SparkServerLite.Infrastructure
{
    public class Database
    {
        public static string SQLiteConnectionString = "Data Source=SparkServer.db";

        private static string _TypeInt64 = "Int64";
        private static string _TypeString = "String";

        public static int GetInteger(object dbValue)
        {
            if (dbValue != null && dbValue.GetType().Name == _TypeInt64)
                return Convert.ToInt32(dbValue);

            return 0;
        }

        public static string GetString(object dbValue)
        {
            if (dbValue != null && dbValue.GetType().Name == _TypeString)
                return dbValue.ToString() ?? string.Empty;

            return string.Empty;
        }

        public static bool GetBoolean(object dbValue)
        {
            if (dbValue != null && dbValue.GetType().Name == _TypeInt64)
                return Convert.ToBoolean(dbValue);

            return false;
        }

        public static DateTime? GetDateTime(object dbValue)
        {
            DateTime output;

            if (dbValue != null && dbValue.GetType().Name == _TypeString)
            {
                if (DateTime.TryParse(dbValue.ToString(), out output))
                {
                    return output;
                }
            }

            return null;
        }

        public static Guid GetGuid(object dbValue)
        {
            Guid output;

            if (dbValue != null && dbValue.GetType().Name == _TypeString)
            {
                if (Guid.TryParse(dbValue.ToString(), out output))
                {
                    return output;
                }
            }

            return Guid.Empty;
        }
    }
}
