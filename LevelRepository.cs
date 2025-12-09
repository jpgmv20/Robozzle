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
                // Busca dados da fase + dados do autor
                string sql = @"
                    SELECT l.id, l.title, l.difficulty, l.level_json, l.author_id,
                           u.nome AS author_name, u.avatar_image AS author_avatar
                    FROM levels l
                    JOIN users u ON l.author_id = u.id
                    WHERE l.published = 1
                    ORDER BY l.created_at DESC";

                using (var cmd = new MySqlCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var level = new LevelEntity();
                        level.Id = reader.GetInt32("id");
                        level.Title = reader.GetString("title");
                        level.Difficulty = reader.GetString("difficulty");
                        level.AuthorId = reader.GetInt32("author_id");
                        level.AuthorName = reader.GetString("author_name");

                        if (!reader.IsDBNull(reader.GetOrdinal("author_avatar")))
                        {
                            level.AuthorAvatarBytes = (byte[])reader["author_avatar"];
                        }

                        string jsonRaw = reader.IsDBNull(reader.GetOrdinal("level_json")) ? "{}" : reader.GetString("level_json");
                        try { level.Data = JsonConvert.DeserializeObject<LevelData>(jsonRaw); }
                        catch { level.Data = new LevelData(); }

                        levels.Add(level);
                    }
                }
            }
            return levels;
        }
    }
}