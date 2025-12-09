using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace RobozllueApp
{
    public class LevelRepository
    {
        public List<LevelEntity> GetAllLevels()
        {
            List<LevelEntity> levels = new List<LevelEntity>();

            using (var conn = Database.GetConnection())
            {
                // Pegamos ID, Titulo, Dificuldade e o JSON bruto
                string sql = "SELECT id, title, difficulty, level_json FROM levels WHERE published = 1";

                using (var cmd = new MySqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var level = new LevelEntity();
                        level.Id = reader.GetInt32("id");
                        level.Title = reader.GetString("title");
                        level.Difficulty = reader.GetString("difficulty");

                        // 1. Pegamos o JSON puro (string) do banco
                        string jsonRaw = reader.IsDBNull(reader.GetOrdinal("level_json"))
                                         ? "{}"
                                         : reader.GetString("level_json");

                        // 2. Convertemos (Deserializamos) para a classe C#
                        try
                        {
                            level.Data = JsonConvert.DeserializeObject<LevelData>(jsonRaw);
                        }
                        catch (Exception ex)
                        {
                            // Se o JSON estiver corrompido, cria um level vazio para não travar
                            Console.WriteLine($"Erro ao ler JSON do level {level.Id}: {ex.Message}");
                            level.Data = new LevelData();
                        }

                        levels.Add(level);
                    }
                }
            }

            return levels;
        }
    }
}