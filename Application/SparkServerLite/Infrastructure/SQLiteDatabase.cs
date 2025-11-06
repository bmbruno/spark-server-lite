using Microsoft.Data.Sqlite;
using System.Text;

namespace SQLiteSimply
{
    /// <summary>
    /// Provides functionality for executing SQL queries, inserts, and statements against a SQLite database file.
    /// </summary>
    public class SQLiteDatabase
    {
        /// <summary>
        /// Connection string to the SQLite database.
        /// </summary>
        private readonly string _connectionString;

        /// <summary>
        /// 
        /// </summary>
        private StringBuilder _sql;

        /// <summary>
        /// 
        /// </summary>
        private List<SqliteParameter> _parameters;

        /// <summary>
        /// Gets the internal list of SQliteParameter objects.
        /// </summary>
        public List<SqliteParameter> Parameters
        {
            get { return _parameters; }
        }

        /// <summary>
        /// Constructor. Required to set up a connection to a SQLite database.
        /// </summary>
        /// <param name="connectionString"></param>
        public SQLiteDatabase(string connectionString)
        {
            _connectionString = connectionString;

            _sql = new StringBuilder();
            _parameters = new List<SqliteParameter>();
        }

        /// <summary>
        /// Creates a connection to SQLite and returns it.
        /// </summary>
        /// <returns>Connection to SQlite database.</returns>
        public SqliteConnection CreateConnection()
        {
            return new SqliteConnection(_connectionString);
        }

        /// <summary>
        /// Executes a query against the connected SQLite database. Internal SQL statement and parameters will be used.
        /// </summary>
        /// <returns>Datareader for iteration.</returns>
        /// <exception cref="Exception">Thrown if SQLite connection isn't opened first.</exception>
        public SqliteDataReader Query(SqliteConnection connection)
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = _sql.ToString();

            if (_parameters != null && _parameters.Count > 0)
            {
                foreach (var parameter in _parameters)
                    command.Parameters.Add(parameter);
            }

            if (connection.State == System.Data.ConnectionState.Closed)
                connection.Open();

            return command.ExecuteReader();
        }

        /// <summary>
        /// Executes an INSERT statement to the database and returns the new ID of the row. New row ID will be a long (signed 64-bit). Internal SQL statement and parameters will be used.
        /// </summary>
        /// <param name="connection">The active SQLite database connection.</param>
        /// <returns>New row ID as a long.</returns>
        public long Insert(SqliteConnection connection)
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = _sql.ToString();

            if (_parameters != null && _parameters.Count > 0)
            {
                foreach (var parameter in _parameters)
                    command.Parameters.Add(parameter);
            }

            if (connection.State == System.Data.ConnectionState.Closed)
                connection.Open();

            command.ExecuteNonQuery();
            command.Parameters.Clear();

            // Note: last_insert_rowid() has some pitfalls; for example, another INSERT could interfere and return a value not related to this INSERT
            // TODO: explore ways to ensure we get the correct value back for this
            command.CommandText = "SELECT last_insert_rowid()";
            long newID = Convert.ToInt64(command.ExecuteScalar());

            return newID;
        }

        /// <summary>
        /// Executes a SQL statement. Internal SQL statement and parameters will be used.
        /// </summary>
        /// <param name="sql">SQL statement to run.</param>
        public void Execute(SqliteConnection connection)
        {
            SqliteCommand command = connection.CreateCommand();
            command.CommandText = _sql.ToString();

            if (_parameters != null && _parameters.Count > 0)
            {
                foreach (var parameter in _parameters)
                    command.Parameters.Add(parameter);
            }

            if (connection.State == System.Data.ConnectionState.Closed)
                connection.Open();

            command.ExecuteNonQuery();
        }

        /// <summary>
        /// Sets the SQL statement for this query/execution. This is usually a SELECT, INSERT, or UPDATE, but other parameter-driven statements can be used.
        /// </summary>
        /// <param name="selectStatement">SQL statement.</param>
        public void CreateStatement(string selectStatement)
        {
            this.ClearAll();
            _sql.Append(selectStatement);
        }

        /// <summary>
        /// Appends the given SQL code onto the existing SELECT statement. Whitespace considerations will be handled by this method.
        /// </summary>
        /// <param name="statement">SQL code to append.</param>
        public void AppendStatement(string statement)
        {
            _sql.Append($" {statement} ");
        }

        /// <summary>
        /// Adds a parameter to this SQL statement. Parameter should be present in SQL statements used in CreateStatement method.
        /// </summary>
        /// <param name="parameterName"></param>
        /// <param name="parameterValue"></param>
        public void AddParameter(string parameterName, object? parameterValue)
        {
            _parameters.Add(new SqliteParameter(name: parameterName, value: parameterValue));
        }

        /// <summary>
        /// Clears the current SQL statement and parameter list.
        /// </summary>
        public void ClearAll()
        {
            _sql.Clear();
            _parameters.Clear();
        }
    }
}