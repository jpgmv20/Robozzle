using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace RobozllueApp
{
    public class ChatRepository
    {
        // Lista as conversas do usuário logado
        public List<Conversation> GetUserConversations(int userId)
        {
            var list = new List<Conversation>();
            using (var conn = Database.GetConnection())
            {
                // Busca conversas privadas e pega o nome/avatar do OUTRO participante
                string sql = @"
                    SELECT c.id, c.last_message_at, 
                           u.nome, u.avatar_image,
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
                                Title = r.GetString("nome"), // Nome do amigo
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

        // Pega mensagens de uma conversa específica
        public List<ChatMessage> GetMessages(int conversationId)
        {
            var list = new List<ChatMessage>();
            using (var conn = Database.GetConnection())
            {
                string sql = @"SELECT m.id, m.sender_id, m.content, m.created_at, u.nome 
                               FROM conversation_messages m
                               JOIN users u ON m.sender_id = u.id
                               WHERE m.conversation_id = @cid 
                               ORDER BY m.created_at ASC";

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
            return list;
        }

        // Envia mensagem
        public void SendMessage(int conversationId, int senderId, string content)
        {
            using (var conn = Database.GetConnection())
            {
                // 1. Insere Mensagem
                string sqlMsg = @"INSERT INTO conversation_messages (conversation_id, sender_id, content, read_by) 
                                  VALUES (@cid, @sid, @content, '[]')"; // JSON vazio para read_by
                using (var cmd = new MySqlCommand(sqlMsg, conn))
                {
                    cmd.Parameters.AddWithValue("@cid", conversationId);
                    cmd.Parameters.AddWithValue("@sid", senderId);
                    cmd.Parameters.AddWithValue("@content", content);
                    cmd.ExecuteNonQuery();
                }

                // 2. Atualiza data da conversa (para subir no topo)
                string sqlUpdate = "UPDATE conversations SET last_message_at = NOW() WHERE id = @cid";
                using (var cmd = new MySqlCommand(sqlUpdate, conn))
                {
                    cmd.Parameters.AddWithValue("@cid", conversationId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Cria ou recupera uma conversa privada (usado no botão "Mensagem" do perfil)
        public int StartPrivateChat(int myId, int targetId)
        {
            using (var conn = Database.GetConnection())
            {
                // 1. Verifica se já existe conversa entre os dois
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
                    if (result != null) return Convert.ToInt32(result); // Retorna ID existente
                }

                // 2. Se não existe, cria nova conversa
                string createConv = "INSERT INTO conversations (type, created_at, last_message_at) VALUES ('private', NOW(), NOW()); SELECT LAST_INSERT_ID();";
                int newId;
                using (var cmd = new MySqlCommand(createConv, conn))
                {
                    newId = Convert.ToInt32(cmd.ExecuteScalar());
                }

                // 3. Adiciona participantes
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