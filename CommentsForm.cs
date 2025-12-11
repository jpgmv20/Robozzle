using RobozllueApp;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace Robozzle
{
    public partial class CommentsForm : FormLoader
    {
        private int _levelId;
        private LevelRepository _repo = new LevelRepository();

        public CommentsForm(int levelId)
        {
            InitializeComponent();
            this.PainelCentral = pnlList;
            _levelId = levelId;

            ThemeManager.ApplyTheme(this);
            LoadComments();
        }

        private void LoadComments()
        {
            pnlList.Controls.Clear();
            var comments = _repo.GetComments(_levelId);

            if (comments.Count == 0)
            {
                Label lbl = new Label { Text = "Seja o primeiro a comentar!", AutoSize = true, ForeColor = Color.Gray, Margin = new Padding(20) };
                pnlList.Controls.Add(lbl);
                return;
            }

            foreach (var c in comments)
            {
                Panel card = new Panel { Width = 360, Height = 70, Margin = new Padding(10), BackColor = Color.WhiteSmoke };

                // Avatar
                PictureBox pb = new PictureBox { Size = new Size(40, 40), Location = new Point(10, 10), SizeMode = PictureBoxSizeMode.StretchImage };
                if (c.UserAvatar != null) using (var ms = new MemoryStream(c.UserAvatar)) pb.Image = Image.FromStream(ms);
                else pb.BackColor = Color.Gray;

                // Arredonda avatar
                GraphicsPath gp = new GraphicsPath(); gp.AddEllipse(0, 0, 40, 40); pb.Region = new Region(gp);

                Label lblName = new Label { Text = c.UserName, Location = new Point(60, 10), AutoSize = true, Font = new Font("Segoe UI", 9, FontStyle.Bold) };
                Label lblText = new Label { Text = c.Text, Location = new Point(60, 30), Size = new Size(280, 35), Font = new Font("Segoe UI", 9) };

                card.Controls.Add(pb);
                card.Controls.Add(lblName);
                card.Controls.Add(lblText);
                pnlList.Controls.Add(card);
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            string txt = txtComment.Text.Trim();
            if (string.IsNullOrEmpty(txt)) return;

            _repo.AddComment(_levelId, UserSession.Id, txt);
            txtComment.Clear();
            LoadComments();
        }
    }
}