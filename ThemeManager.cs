using System.Drawing;
using System.Windows.Forms;

namespace Robozzle
{
    public static class ThemeManager
    {
        // Cores do Tema Escuro
        public static Color DarkBack = Color.FromArgb(32, 33, 36);
        public static Color DarkSurface = Color.FromArgb(48, 48, 48);
        public static Color DarkText = Color.FromArgb(232, 234, 237);
        public static Color DarkControl = Color.FromArgb(60, 64, 67);

        // Cores do Tema Claro
        public static Color LightBack = Color.WhiteSmoke;
        public static Color LightSurface = Color.White;
        public static Color LightText = Color.Black;
        public static Color LightControl = Color.White;

        public static void ApplyTheme(Form form)
        {
            bool isDark = RobozllueApp.UserSession.Theme == "dark";

            Color back = isDark ? DarkBack : LightBack;
            Color surface = isDark ? DarkSurface : LightSurface;
            Color text = isDark ? DarkText : LightText;
            Color controlParams = isDark ? DarkControl : LightControl;

            form.BackColor = back;
            form.ForeColor = text;

            foreach (Control c in form.Controls)
            {
                ApplyToControl(c, back, surface, text, controlParams);
            }
        }

        private static void ApplyToControl(Control c, Color back, Color surface, Color text, Color ctrlColor)
        {
            if (c is Panel || c is GroupBox)
            {
                c.BackColor = (c.Name.Contains("Sidebar") || c.Name.Contains("Container")) ? back : surface;
                c.ForeColor = text;
            }
            else if (c is Button btn)
            {
                // --- BOTÕES COM CORES ESPECIAIS (PINCÉIS, SETAS, CONDICIONAIS) ---

                string tag = btn.Tag?.ToString() ?? "";

                // 1. Verifica pela TAG ou pelo PAINEL
                if (tag.Contains("PAINT") ||
                    tag.Contains("COND") ||
                    tag.Contains("color_") ||
                    tag.Contains("symbol_") ||
                    tag.Contains("dir_") ||
                    (btn.Parent != null && (btn.Parent.Name == "pnlPalette" || btn.Parent.Name == "pnlFunctions" || btn.Parent.Name == "flowTools")))
                {
                    // Força LETRA PRETA para garantir contraste no fundo colorido
                    btn.ForeColor = Color.Black;
                    return; // Sai e não aplica a cor cinza do tema
                }

                // 2. Botões de Ação do Sistema (Play, Reset, etc)
                if (btn.Name.Contains("Play") ||
                    btn.Name.Contains("Reset") ||
                    btn.Name.Contains("Save") ||
                    btn.Name.Contains("Send"))
                {
                    // Mantém a cor original deles (Verde/Vermelho), mas garante texto legível
                    btn.ForeColor = Color.White;
                    return;
                }

                // --- TEMA PADRÃO (Para botões comuns) ---
                btn.BackColor = ctrlColor;
                btn.ForeColor = text;
            }
            else if (c is TextBox || c is ListBox || c is ComboBox || c is NumericUpDown)
            {
                c.BackColor = ctrlColor;
                c.ForeColor = text;
            }
            else if (c is Label)
            {
                c.ForeColor = text;
            }

            foreach (Control child in c.Controls)
            {
                ApplyToControl(child, back, surface, text, ctrlColor);
            }
        }
    }
}