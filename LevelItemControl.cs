using RobozllueApp;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace Robozzle
{
    // HERDA DE USERCONTROL
    public partial class LevelItemControl : UserControl
    {
        public LevelEntity LevelInfo { get; private set; }

        // Eventos públicos para o HomeForm usar
        public event EventHandler<LevelEntity>? PlayRequested;
        public event EventHandler<int>? ProfileRequested;

        // Construtor obrigatório
        public LevelItemControl(LevelEntity level)
        {
            InitializeComponent();
            LevelInfo = level;
            ConfigurarDados();
        }

        private void ConfigurarDados()
        {
            bool isDark = UserSession.Theme == "dark";
            this.BackColor = isDark ? Color.FromArgb(50, 50, 50) : Color.White;
            lblTitle.ForeColor = isDark ? Color.White : Color.Black;

            lblTitle.Text = LevelInfo.Title;
            lblAuthorName.Text = "por " + LevelInfo.AuthorName;

            lblDifficulty.Text = LevelInfo.Difficulty.ToUpper();
            switch (LevelInfo.Difficulty.ToLower())
            {
                case "easy": lblDifficulty.BackColor = Color.MediumSeaGreen; break;
                case "medium": lblDifficulty.BackColor = Color.Orange; break;
                case "hard": lblDifficulty.BackColor = Color.IndianRed; break;
                case "insane": lblDifficulty.BackColor = Color.Purple; break;
                default: lblDifficulty.BackColor = Color.Gray; break;
            }

            if (LevelInfo.AuthorAvatarBytes != null && LevelInfo.AuthorAvatarBytes.Length > 0)
            {
                try
                {
                    using (var ms = new MemoryStream(LevelInfo.AuthorAvatarBytes))
                        pbAuthorAvatar.Image = Image.FromStream(ms);
                }
                catch { }
            }
            else
            {
                // Carrega avatar padrão se não tiver
                try
                {
                    string url = $"https://ui-avatars.com/api/?name={LevelInfo.AuthorName}&background=random&color=fff&size=64";
                    pbAuthorAvatar.LoadAsync(url);
                }
                catch { }
            }
        }

        public void SetPreview(Image img)
        {
            pbPreview.Image = img;
        }

        private void pbAuthorAvatar_Paint(object sender, PaintEventArgs e)
        {
            GraphicsPath gp = new GraphicsPath();
            gp.AddEllipse(0, 0, pbAuthorAvatar.Width, pbAuthorAvatar.Height);
            pbAuthorAvatar.Region = new Region(gp);
        }

        // Dispara os eventos
        private void OnPlay_Click(object sender, EventArgs e) => PlayRequested?.Invoke(this, LevelInfo);
        private void OnProfile_Click(object sender, EventArgs e) => ProfileRequested?.Invoke(this, LevelInfo.AuthorId);
    }
}