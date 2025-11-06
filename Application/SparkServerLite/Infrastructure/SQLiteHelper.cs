namespace SQLiteSimply
{
    /// <summary>
    /// Provides methods to translate data to and from SQLite-native data formats/types.
    /// </summary>
    public static class SQLiteHelper
    {
        /// <summary>
        /// Gets a non-null integer-based ID from a database value. ID columns are expected to have a value, so an Exception is thrown otherwise.
        /// </summary>
        /// <param name="dbValue">Generic object to be parsed</param>
        /// <returns>Integer value.</returns>
        /// <exception cref="Exception">Thrown if a null integer is returned from the database.</exception>
        public static int GetIDInt(object dbValue)
        {
            int? id = SQLiteHelper.GetInteger(dbValue);

            if (id.HasValue)
                return id.Value;
            else
                throw new Exception("Database ID column is null! This is a non-nullable integer ID column and something serious may be wrong.");
        }

        /// <summary>
        /// Gets a non-null long-based ID from a database value. ID columns are expected to have a value, so an Exception is thrown otherwise.
        /// </summary>
        /// <param name="dbValue">Generic object to be parsed</param>
        /// <returns>Long value.</returns>
        /// <exception cref="Exception">Thrown if a null long is returned from the database.</exception>
        public static long GetIDLong(object dbValue)
        {
            long? id = SQLiteHelper.GetLong(dbValue);

            if (id.HasValue)
                return id.Value;
            else
                throw new Exception("Database ID column is null! This is a non-nullable long ID column and something serious may be wrong.");
        }

        /// <summary>
        /// Gets an integer (Int32) from a database value.
        /// </summary>
        /// <param name="dbValue">Generic object to be parsed.</param>
        /// <returns>Integer value or null if no value parsed.</returns>
        public static int? GetInteger(object dbValue)
        {
            if (dbValue != null && dbValue != DBNull.Value)
                return Convert.ToInt32(dbValue);

            return null;
        }

        /// <summary>
        /// Gets a non-null integer (Int32) from a database value.
        /// </summary>
        /// <param name="dbValue">Generic object to be parsed.</param>
        /// <returns>Integer value or null if no value parsed.</returns>
        public static int GetIntegerNonNull(object dbValue)
        {
            if (dbValue != null && dbValue != DBNull.Value)
                return Convert.ToInt32(dbValue);

            throw new Exception("Value was not parsable into an integer datatype.");
        }

        /// <summary>
        /// Gets a long (Int36) from a database value.
        /// </summary>
        /// <param name="dbValue">Generic object to be parsed.</param>
        /// <returns>Long value or null if no value parsed.</returns>
        public static long? GetLong(object dbValue)
        {
            if (dbValue != null && dbValue != DBNull.Value)
                return Convert.ToInt64(dbValue);

            return null;
        }

        /// <summary>
        /// Gets a non-null long (Int64) from a database value.
        /// </summary>
        /// <param name="dbValue">Generic object to be parsed.</param>
        /// <returns>Long value or null if no value parsed.</returns>
        public static long GetLongNonNull(object dbValue)
        {
            if (dbValue != null && dbValue != DBNull.Value)
                return Convert.ToInt64(dbValue);

            throw new Exception("Value was not parsable into a long datatype.");
        }

        /// <summary>
        /// Gets a float from a database value.
        /// </summary>
        /// <param name="dbValue">Generic object to be parsed.</param>
        /// <returns>Float value or null if no value parsed.</returns>
        public static float? GetFloat(object dbValue)
        {
            if (dbValue != null && dbValue != DBNull.Value)
                return (float)dbValue;

            return null;
        }

        /// <summary>
        /// Gets a non-null float from a database value.
        /// </summary>
        /// <param name="dbValue">Generic object to be parsed.</param>
        /// <returns>Float value from parsed object.</returns>
        /// <exception cref="Exception">Null found in database value.</exception>
        public static float GetFloatNonNull(object dbValue)
        {
            if (dbValue != null && dbValue != DBNull.Value)
                return (float)dbValue;

            throw new Exception("Float value is null in database. Expecting to return a non-null type.");
        }

        /// <summary>
        /// Gets a double from a database value.
        /// </summary>
        /// <param name="dbValue">Generic object to be parsed.</param>
        /// <returns>Double value or null if no value parsed.</returns>
        public static double? GetDouble(object dbValue)
        {
            if (dbValue != null && dbValue != DBNull.Value)
                return Convert.ToDouble(dbValue);

            return null;
        }

        /// <summary>
        /// Gets a non-null double from a database value.
        /// </summary>
        /// <param name="dbValue">Generic object to be parsed.</param>
        /// <returns>Double value from parsed object.</returns>
        /// <exception cref="Exception">Null found in database value.</exception>
        public static double GetDoubleNonNull(object dbValue)
        {
            if (dbValue != null && dbValue != DBNull.Value)
                return Convert.ToDouble(dbValue);

            throw new Exception("Double value is null in database. Expecting to return a non-null type.");
        }

        /// <summary>
        /// Gets a decimal from a database value.
        /// </summary>
        /// <param name="dbValue">Generic object to be parsed.</param>
        /// <returns>Decimal value or null if no value parsed.</returns>
        public static decimal? GetDecimal(object dbValue)
        {
            if (dbValue != null && dbValue != DBNull.Value)
                return Convert.ToDecimal(dbValue);

            return null;
        }

        /// <summary>
        /// Gets a non-null decimal from a database value.
        /// </summary>
        /// <param name="dbValue">Generic object to be parsed.</param>
        /// <returns>Decimal value from parsed object.</returns>
        /// <exception cref="Exception">Null found in database value.</exception>
        public static decimal GetDecimalNonNull(object dbValue)
        {
            if (dbValue != null && dbValue != DBNull.Value)
                return Convert.ToDecimal(dbValue);

            throw new Exception("Decimal value is null in database. Expecting to return a non-null type.");
        }

        /// <summary>
        /// Gets a string from a database value.
        /// </summary>
        /// <param name="dbValue">Generic object to be parsed.</param>
        /// <returns>String value or empty string.</returns>
        public static string GetString(object dbValue)
        {
            if (dbValue != null && dbValue != DBNull.Value)
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
            if (dbValue != null && dbValue != DBNull.Value)
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

            if (dbValue != null && dbValue != DBNull.Value)
            {
                if (DateTime.TryParse(dbValue.ToString(), out output))
                {
                    return output;
                }
            }

            return null;
        }

        /// <summary>
        /// Gets a non-null DateTime from a database value.
        /// </summary>
        /// <param name="dbValue">Generic object to be parsed.</param>
        /// <returns>DateTime object</returns>
        public static DateTime GetDateTimeNonNull(object dbValue)
        {
            DateTime output;

            if (dbValue != null && dbValue != DBNull.Value)
            {
                if (DateTime.TryParse(dbValue.ToString(), out output))
                {
                    return output;
                }
            }

            throw new Exception("Value was not parseable into a DateTime object.");
        }

        /// <summary>
        /// Gets a GUID from a database value.
        /// </summary>
        /// <param name="dbValue">Generic object to be parsed.</param>
        /// <returns>Guid value or empty guid if none returned in database.</returns>
        public static Guid GetGuid(object dbValue)
        {
            Guid output;

            if (dbValue != null && dbValue != DBNull.Value)
            {
                if (Guid.TryParse(dbValue.ToString(), out output))
                {
                    return output;
                }
            }

            return Guid.Empty;
        }

        /// <summary>
        /// Gets a standardized string for the SQLite datetime format. Can optionally take a format for use in ToString method.
        /// </summary>
        /// <param name="datetime">DateTime object to be converted.</param>
        /// <param name="format">String format to use for conversion.</param>
        /// <returns>String with the format: yyyy-MM-dd HH:mm:ss; if 'format' is provided, that format is used.</returns>
        public static string FormatDateTimeForSQL(DateTime datetime, string format = "")
        {
            if (!String.IsNullOrEmpty(format))
                return datetime.ToString(format);

            return datetime.ToString("yyyy-MM-dd HH:mm:ss");
        }
    }
}