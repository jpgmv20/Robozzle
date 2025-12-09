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
            // Verifica a preferência na sessão
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
            // Aplica cores baseadas no tipo de controle
            if (c is Panel || c is GroupBox)
            {
                c.BackColor = (c.Name.Contains("Sidebar") || c.Name.Contains("Container")) ? back : surface;
                c.ForeColor = text;
            }
            else if (c is Button btn)
            {
                // Botões específicos mantêm suas cores (Play/Reset), outros seguem o tema
                if (!btn.Name.Contains("Play") && !btn.Name.Contains("Reset") && !btn.Name.Contains("Save"))
                {
                    btn.BackColor = ctrlColor;
                    btn.ForeColor = text;
                }
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

            // Recursão para controles filhos
            foreach (Control child in c.Controls)
            {
                ApplyToControl(child, back, surface, text, ctrlColor);
            }
        }
    }
}