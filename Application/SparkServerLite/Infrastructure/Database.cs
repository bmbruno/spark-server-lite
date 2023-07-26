using System.Runtime.CompilerServices;

namespace SparkServerLite.Infrastructure
{
    public class Database
    {
        public static string SQLiteConnectionString = "Data Source=SparkServer.db";

        private static string _TypeInt64 = "Int64";
        private static string _TypeString = "String";

        /// <summary>
        /// Gets a non-null integer-based ID from a database value. ID columns are expected to have a value, so an Exception is thrown otherwise.
        /// </summary>
        /// <param name="dbValue">Generic object to be parsed</param>
        /// <returns>Integer value.</returns>
        /// <exception cref="Exception">Thrown if a null integer is returned from the database.</exception>
        public static int GetID(object dbValue)
        {
            int? id = Database.GetInteger(dbValue);

            if (id.HasValue)
                return id.Value;
            else
                throw new Exception("Database ID column is null! This should not happen.");
        }

        /// <summary>
        /// Gets an integer (Int32) from a database value.
        /// </summary>
        /// <param name="dbValue">Generic object to be parsed.</param>
        /// <returns>Integer value or null if no value parsed.</returns>
        public static int? GetInteger(object dbValue)
        {
            if (dbValue != null && dbValue.GetType().Name == _TypeInt64)
                return Convert.ToInt32(dbValue);

            return null;
        }

        /// <summary>
        /// Gets a string from a database value.
        /// </summary>
        /// <param name="dbValue">Generic object to be parsed.</param>
        /// <returns>String value or empty string.</returns>
        public static string GetString(object dbValue)
        {
            if (dbValue != null && dbValue.GetType().Name == _TypeString)
                return dbValue.ToString() ?? string.Empty;

            return string.Empty;
        }

        /// <summary>
        /// Gets a bool from a database value.
        /// </summary>
        /// <param name="dbValue">Generic object to be parsed.</param>
        /// <returns>Bool value (true/false).</returns>
        /// <exception cref="Exception">Thrown if a null boolean is returned from the database.</exception>
        public static bool GetBoolean(object dbValue)
        {
            if (dbValue != null && dbValue.GetType().Name == _TypeInt64)
                return Convert.ToBoolean(dbValue);

            throw new Exception("Database boolean value might be null. This should not happen.");
        }

        /// <summary>
        /// Gets a DateTime from a database value.
        /// </summary>
        /// <param name="dbValue">Generic object to be parsed.</param>
        /// <returns>DateTime object or null if no value in database.</returns>
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

        /// <summary>
        /// Gets a GUID from a database value.
        /// </summary>
        /// <param name="dbValue">Generic object to be parsed.</param>
        /// <returns>Guid value or empty guid if none returned in database.</returns>
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
