using System;
using System.Collections.Generic;

namespace RobozllueApp
{
    public class LevelEntity
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Difficulty { get; set; } = string.Empty;
        public LevelData Data { get; set; } = new LevelData();
        public int AuthorId { get; set; }
        public string AuthorName { get; set; } = "Desconhecido";
        public byte[]? AuthorAvatarBytes { get; set; }

        // Contadores
        public int LikesCount { get; set; }
        public int PlaysCount { get; set; }
        public bool IsLikedByMe { get; set; }
    }

    public class LevelData
    {
        public string title { get; set; } = string.Empty;
        public string difficulty { get; set; } = "easy";
        public int grid_cell_size { get; set; } = 48;
        public List<List<CellData>> matrix { get; set; } = new List<List<CellData>>();
        public List<FunctionData> functions { get; set; } = new List<FunctionData>();
    }

    public class CellData
    {
        public string color { get; set; } = "none";
        public string symbol { get; set; } = "none";
        public int direction { get; set; } = 1;
    }

    public class FunctionData
    {
        public string name { get; set; } = string.Empty;
        public int size { get; set; }
    }

    public class CommandSlot
    {
        public string Action { get; set; } = "";
        public string ConditionColor { get; set; } = "none";
    }

    
    public class CommentEntity
    {
        public int Id { get; set; }
        public int LevelId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public byte[]? UserAvatar { get; set; }
        public string Text { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }

    public class Conversation
    {
        public int Id { get; set; }
        public int TargetUserId { get; set; }
        public string Title { get; set; } = string.Empty;
        public byte[]? Avatar { get; set; }
        public string LastMessage { get; set; } = "";
        public DateTime LastMessageDate { get; set; }
    }

    public class ChatMessage
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public string SenderName { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public bool IsMine => SenderId == UserSession.Id;
    }
}