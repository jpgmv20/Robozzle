using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace RobozllueApp
{
    public class ChatRepository
    {
        // Busca conversas existentes
        public List<Conversation> GetUserConversations(int userId)
        {
            var list = new List<Conversation>();
            using (var conn = Database.GetConnection())
            {
                // Agora pegamos também o 'other.user_id'
                string sql = @"
                    SELECT c.id, c.last_message_at, 
                           u.id as target_uid, u.nome, u.avatar_image,
                           (SELECT content FROM conversation_messages m WHERE m.conversation_id = c.id ORDER BY m.created_at DESC LIMIT 1) as last_msg
                    FROM conversations c
                    JOIN conversation_participants me ON c.id = me.conversation_id
                    JOIN conversation_participants other ON c.id = other.conversation_id
                    JOIN users u ON other.user_id = u.id
                    WHERE me.user_id = @uid AND other.user_id != @uid
                    ORDER BY c.last_message_at DESC";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@uid", userId);
                    using (var r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            list.Add(new Conversation
                            {
                                Id = r.GetInt32("id"),
                                TargetUserId = r.GetInt32("target_uid"), // <--- Preenchendo
                                Title = r.GetString("nome"),
                                Avatar = r.IsDBNull(r.GetOrdinal("avatar_image")) ? null : (byte[])r["avatar_image"],
                                LastMessage = r.IsDBNull(r.GetOrdinal("last_msg")) ? "" : r.GetString("last_msg"),
                                LastMessageDate = r.IsDBNull(r.GetOrdinal("last_message_at")) ? DateTime.MinValue : r.GetDateTime("last_message_at")
                            });
                        }
                    }
                }
            }
            return list;
        }

        // NOVO: Pesquisa usuários globais para iniciar conversa
        public List<Conversation> SearchUsers(string term, int myId)
        {
            var list = new List<Conversation>();
            using (var conn = Database.GetConnection())
            {
                string sql = @"SELECT id, nome, avatar_image FROM users 
                               WHERE nome LIKE @term AND id != @me 
                               LIMIT 20";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@term", "%" + term + "%");
                    cmd.Parameters.AddWithValue("@me", myId);

                    using (var r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            // Retorna como uma 'Conversation' fake (Id=0)
                            list.Add(new Conversation
                            {
                                Id = 0, // 0 indica que ainda não abrimos o chat
                                TargetUserId = r.GetInt32("id"),
                                Title = r.GetString("nome"),
                                Avatar = r.IsDBNull(r.GetOrdinal("avatar_image")) ? null : (byte[])r["avatar_image"],
                                LastMessage = "Novo Chat", // Indicador visual
                                LastMessageDate = DateTime.Now
                            });
                        }
                    }
                }
            }
            return list;
        }

        public List<ChatMessage> GetMessages(int conversationId)
        {
            var list = new List<ChatMessage>();
            using (var conn = Database.GetConnection())
            {
                string sql = @"SELECT m.id, m.sender_id, m.content, m.created_at, u.nome 
                               FROM conversation_messages m
                               JOIN users u ON m.sender_id = u.id
                               WHERE m.conversation_id = @cid 
                               ORDER BY m.created_at DESC 
                               LIMIT 50";

                using (var cmd = new MySqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@cid", conversationId);
                    using (var r = cmd.ExecuteReader())
                    {
                        while (r.Read())
                        {
                            list.Add(new ChatMessage
                            {
                                Id = r.GetInt32("id"),
                                SenderId = r.GetInt32("sender_id"),
                                SenderName = r.GetString("nome"),
                                Content = r.GetString("content"),
                                CreatedAt = r.GetDateTime("created_at")
                            });
                        }
                    }
                }
            }
            list.Reverse();
            return list;
        }

        public void SendMessage(int conversationId, int senderId, string content)
        {
            using (var conn = Database.GetConnection())
            {
                string sqlMsg = @"INSERT INTO conversation_messages (conversation_id, sender_id, content, read_by) 
                                  VALUES (@cid, @sid, @content, '[]')";
                using (var cmd = new MySqlCommand(sqlMsg, conn))
                {
                    cmd.Parameters.AddWithValue("@cid", conversationId);
                    cmd.Parameters.AddWithValue("@sid", senderId);
                    cmd.Parameters.AddWithValue("@content", content);
                    cmd.ExecuteNonQuery();
                }

                string sqlUpdate = "UPDATE conversations SET last_message_at = NOW() WHERE id = @cid";
                using (var cmd = new MySqlCommand(sqlUpdate, conn))
                {
                    cmd.Parameters.AddWithValue("@cid", conversationId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public int StartPrivateChat(int myId, int targetId)
        {
            using (var conn = Database.GetConnection())
            {
                string checkSql = @"
                    SELECT c.id FROM conversations c
                    JOIN conversation_participants p1 ON c.id = p1.conversation_id
                    JOIN conversation_participants p2 ON c.id = p2.conversation_id
                    WHERE p1.user_id = @u1 AND p2.user_id = @u2 AND c.type = 'private' LIMIT 1";

                using (var cmd = new MySqlCommand(checkSql, conn))
                {
                    cmd.Parameters.AddWithValue("@u1", myId);
                    cmd.Parameters.AddWithValue("@u2", targetId);
                    var result = cmd.ExecuteScalar();
                    if (result != null) return Convert.ToInt32(result);
                }

                string createConv = "INSERT INTO conversations (type, created_at, last_message_at) VALUES ('private', NOW(), NOW()); SELECT LAST_INSERT_ID();";
                int newId;
                using (var cmd = new MySqlCommand(createConv, conn))
                {
                    newId = Convert.ToInt32(cmd.ExecuteScalar());
                }

                string addPart = "INSERT INTO conversation_participants (conversation_id, user_id) VALUES (@cid, @u1), (@cid, @u2)";
                using (var cmd = new MySqlCommand(addPart, conn))
                {
                    cmd.Parameters.AddWithValue("@cid", newId);
                    cmd.Parameters.AddWithValue("@u1", myId);
                    cmd.Parameters.AddWithValue("@u2", targetId);
                    cmd.ExecuteNonQuery();
                }

                return newId;
            }
        }
    }
}