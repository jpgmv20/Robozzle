using System.Drawing; // Necessário para usar a classe Image

namespace RobozllueApp // Certifique-se de que o namespace é o mesmo usado no restante do projeto
{
    public static class UserSession
    {
        public static int Id { get; set; }
        public static string Nome { get; set; } = string.Empty;
        public static string Email { get; set; } = string.Empty;

        // ESTA É A PROPRIEDADE QUE ESTAVA FALTANDO
        public static Image? Avatar { get; set; }

        public static bool IsLoggedIn => Id > 0;

        public static void Logout()
        {
            Id = 0;
            Nome = string.Empty;
            Email = string.Empty;

            if (Avatar != null)
            {
                Avatar.Dispose(); // Libera a memória da imagem
                Avatar = null;
            }
        }
    }
}