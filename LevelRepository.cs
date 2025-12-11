using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace RobozllueApp
{
    public class LevelRepository
    {
        // 1. Busca Fases + Likes + Plays
        public List<LevelEntity> GetAllLevels(int currentUserId)
        {
            List<LevelEntity> levels = new List<LevelEntity>();

            using (var conn = Database.GetConnection())
            {
                string sql = @"
                    SELECT l.id, l.title, l.difficulty, l.level_json, l.author_id,
                           l.likes_count, l.plays_count,
                           u.nome AS author_name, u.avatar_image AS author_avatar,
                           (SELECT COUNT(*) FROM likes WHERE user_id = @uid AND level_id = l.id) as liked_by_me
                    FROM levels l
                    JOIN users u ON l.author_id = u.id
                    WHERE l.published = 1
                    ORDER BY l.created_at DESC";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@uid", currentUserId);
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
                            level.LikesCount = reader.GetInt32("likes_count");
                            level.PlaysCount = reader.GetInt32("plays_count");
                            level.IsLikedByMe = reader.GetInt32("liked_by_me") > 0;

                            if (!reader.IsDBNull(reader.GetOrdinal("author_avatar")))
                                level.AuthorAvatarBytes = (byte[])reader["author_avatar"];

                            string jsonRaw = reader.IsDBNull(reader.GetOrdinal("level_json")) ? "{}" : reader.GetString("level_json");
                            try { level.Data = JsonConvert.DeserializeObject<LevelData>(jsonRaw); }
                            catch { level.Data = new LevelData(); }

                            levels.Add(level);
                        }
                    }
                }
            }
            return levels;
        }

        // 2. Comentários (Listar e Adicionar)
        public List<CommentEntity> GetComments(int levelId)
        {
            var list = new List<CommentEntity>();
            using (var conn = Database.GetConnection())
            {
                string sql = @"
                    SELECT c.id, c.text, c.created_at, c.user_id,
                           u.nome, u.avatar_image
                    FROM comments c
                    JOIN users u ON c.user_id = u.id
                    WHERE c.level_id = @lid
                    ORDER BY c.created_at DESC";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@lid", levelId);
                    using (var r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            list.Add(new CommentEntity
                            {
                                Id = r.GetInt32("id"),
                                LevelId = levelId,
                                UserId = r.GetInt32("user_id"),
                                Text = r.GetString("text"),
                                CreatedAt = r.GetDateTime("created_at"),
                                UserName = r.GetString("nome"),
                                UserAvatar = r.IsDBNull(r.GetOrdinal("avatar_image")) ? null : (byte[])r["avatar_image"]
                            });
                        }
                    }
                }
            }
            return list;
        }

        public void AddComment(int levelId, int userId, string text)
        {
            using (var conn = Database.GetConnection())
            {
                using (var cmd = new MySqlCommand("create_comment", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_level_id", levelId);
                    cmd.Parameters.AddWithValue("p_user_id", userId);
                    cmd.Parameters.AddWithValue("p_text", text);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // 3. Runs e Programs (Salvar progresso)
        public void RegisterRun(int levelId, int userId, string result, int steps)
        {
            using (var conn = Database.GetConnection())
            {
                using (var cmd = new MySqlCommand("create_run", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_level_id", levelId);
                    cmd.Parameters.AddWithValue("p_program_id", null);
                    cmd.Parameters.AddWithValue("p_user_id", userId);
                    cmd.Parameters.AddWithValue("p_result", result); // success, failure, etc.
                    cmd.Parameters.AddWithValue("p_steps_used", steps);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void SaveProgram(int levelId, int userId, Dictionary<string, CommandSlot[]> program)
        {
            // Serializa apenas as ações para JSON
            var sequence = new List<string>();
            foreach (var kvp in program)
            {
                foreach (var slot in kvp.Value)
                    if (!string.IsNullOrEmpty(slot.Action)) sequence.Add(slot.Action);
            }
            string jsonSeq = JsonConvert.SerializeObject(sequence);

            using (var conn = Database.GetConnection())
            {
                using (var cmd = new MySqlCommand("create_program", conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_level_id", levelId);
                    cmd.Parameters.AddWithValue("p_owner_id", userId);
                    cmd.Parameters.AddWithValue("p_sequence", jsonSeq);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // 4. Like
        public bool ToggleLike(int userId, int levelId, bool currentStatus)
        {
            using (var conn = Database.GetConnection())
            {
                string proc = currentStatus ? "delete_like" : "create_like";
                using (var cmd = new MySqlCommand(proc, conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("p_user_id", userId);
                    cmd.Parameters.AddWithValue("p_level_id", levelId);
                    cmd.ExecuteNonQuery();
                }
            }
            return !currentStatus;
        }
    }
}