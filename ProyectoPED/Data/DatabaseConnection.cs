using MySqlConnector;
using DotNetEnv;

namespace ProyectoPED.Data
{
    public class DatabaseConnection
    {
        private static string connectionString = null!;

        static DatabaseConnection()
        {
            Env.Load();
            string server = Env.GetString("DB_SERVER", "localhost");
            string database = Env.GetString("DB_NAME", "taskuni_db");
            string user = Env.GetString("DB_USER", "root");
            string password = Env.GetString("DB_PASSWORD", "");
            
            connectionString = $"Server={server};Database={database};Uid={user};Pwd={password};";
        }

        public static MySqlConnection GetConnection()
        {
            return new MySqlConnection(connectionString);
        }

        public static string GetConnectionString()
        {
            return connectionString;
        }
    }
}