using System.Drawing;

namespace RobozllueApp
{
    public static class UserSession
    {
        public static int Id { get; set; }
        public static string Nome { get; set; } = string.Empty;
        public static string Email { get; set; } = string.Empty;
        public static Image? Avatar { get; set; }

        // NOVO CAMPO
        public static string Theme { get; set; } = "light";

        public static bool IsLoggedIn => Id > 0;

        public static void Logout()
        {
            Id = 0;
            Nome = string.Empty;
            Email = string.Empty;
            Theme = "light"; // Reseta tema ao sair
            if (Avatar != null) { Avatar.Dispose(); Avatar = null; }
        }
    }
}