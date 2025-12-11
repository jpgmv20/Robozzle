using MySql.Data.MySqlClient;

namespace RobozllueApp
{
    public static class Database
    {
        // Ajuste a senha se necessário (no seu config.php estava vazia)
        private static string connectionString = "Server=localhost;Database=robozzle;User=root;Password=balsaMO1";

        public static MySqlConnection GetConnection()
        {
            var conn = new MySqlConnection(connectionString);
            conn.Open();
            return conn;
        }
    }
}