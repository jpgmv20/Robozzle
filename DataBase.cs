using MySql.Data.MySqlClient;

namespace RobozllueApp
{
    public static class Database
    {
        
        private static string connectionString = "Server=localhost;Database=robozzle;User=root;Password=";

        public static MySqlConnection GetConnection()
        {
            var conn = new MySqlConnection(connectionString);
            conn.Open();
            return conn;
        }
    }
}