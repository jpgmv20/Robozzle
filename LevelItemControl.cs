using RobozllueApp;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace Robozzle
{
    public partial class LevelItemControl : UserControl
    {
        public LevelEntity LevelInfo { get; private set; }

        public event EventHandler<LevelEntity>? PlayRequested;
        public event EventHandler<int>? ProfileRequested;

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
            lblStats.ForeColor = isDark ? Color.LightGray : Color.DimGray;

            lblTitle.Text = LevelInfo.Title;
            lblAuthorName.Text = "por " + LevelInfo.AuthorName;

            // Stats
            lblStats.Text = $"♥ {LevelInfo.LikesCount}   ▶ {LevelInfo.PlaysCount}";

            // Cor do coração
            if (LevelInfo.IsLikedByMe)
                btnLike.ForeColor = Color.IndianRed;
            else
                btnLike.ForeColor = Color.LightGray;

            // Dificuldade
            lblDifficulty.Text = LevelInfo.Difficulty.ToUpper();
            switch (LevelInfo.Difficulty.ToLower())
            {
                case "easy": lblDifficulty.BackColor = Color.MediumSeaGreen; break;
                case "medium": lblDifficulty.BackColor = Color.Orange; break;
                case "hard": lblDifficulty.BackColor = Color.IndianRed; break;
                case "insane": lblDifficulty.BackColor = Color.Purple; break;
                default: lblDifficulty.BackColor = Color.Gray; break;
            }

            // Avatar
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
                try { pbAuthorAvatar.LoadAsync($"https://ui-avatars.com/api/?name={LevelInfo.AuthorName}&background=random&color=fff&size=64"); } catch { }
            }
        }

        private void btnLike_Click(object sender, EventArgs e)
        {
            if (UserSession.Id == 0) return;

            try
            {
                LevelRepository repo = new LevelRepository();
                bool newState = repo.ToggleLike(UserSession.Id, LevelInfo.Id, LevelInfo.IsLikedByMe);

                LevelInfo.IsLikedByMe = newState;
                LevelInfo.LikesCount += newState ? 1 : -1;

                ConfigurarDados();
            }
            catch (Exception ex) { MessageBox.Show("Erro ao curtir: " + ex.Message); }
        }

        private void btnComment_Click(object sender, EventArgs e)
        {
            // Abre o formulário de comentários que criamos
            CommentsForm frm = new CommentsForm(LevelInfo.Id);
            frm.ShowDialog();
        }

        public void SetPreview(Image img) => pbPreview.Image = img;

        private void pbAuthorAvatar_Paint(object sender, PaintEventArgs e)
        {
            GraphicsPath gp = new GraphicsPath();
            gp.AddEllipse(0, 0, pbAuthorAvatar.Width, pbAuthorAvatar.Height);
            pbAuthorAvatar.Region = new Region(gp);
        }

        private void OnPlay_Click(object sender, EventArgs e) => PlayRequested?.Invoke(this, LevelInfo);
        private void OnProfile_Click(object sender, EventArgs e) => ProfileRequested?.Invoke(this, LevelInfo.AuthorId);
    }
}