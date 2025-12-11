using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using RobozllueApp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Robozzle
{
    public partial class HomeForm : FormLoader
    {
        private List<LevelEntity> _cachedLevels = new List<LevelEntity>();

        public HomeForm()
        {
            InitializeComponent();
        }

        private void HomeForm_OnShown (object sender, EventArgs e)
        {
            if (_cachedLevels.Count == 0) CarregarFasesDoBanco();
            RenderLevelList(_cachedLevels);
        }

        private void HomeForm_Load(object sender, EventArgs e)
        {

            ThemeManager.ApplyTheme(this);
            LoadUserAvatar();

            // Define padrão inicial
            cmbSearchType.SelectedIndex = 0;
        }

        // --- ALTERNÂNCIA DE MODO ---
        private void cmbSearchType_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateFilters();
            FilterContent(sender, e);
        }

        private void UpdateFilters()
        {
            cmbFilter.SelectedIndexChanged -= FilterContent;
            cmbFilter.Items.Clear();

            if (cmbSearchType.SelectedIndex == 0) // FASES
            {
                cmbFilter.Items.AddRange(new object[] { "Todas", "Easy", "Medium", "Hard", "Insane" });
                txtSearch.PlaceholderText = "Pesquisar fase...";
            }
            else // PERFIS
            {
                cmbFilter.Items.AddRange(new object[] { "Todos", "Mais Seguidos", "Criadores Ativos" });
                txtSearch.PlaceholderText = "Pesquisar usuário...";
            }

            cmbFilter.SelectedIndex = 0;
            cmbFilter.SelectedIndexChanged += FilterContent;
        }

        private void FilterContent(object sender, EventArgs e)
        {
            if (cmbSearchType.SelectedIndex == 0)
            {
                // MODO FASES
                if (_cachedLevels.Count == 0) CarregarFasesDoBanco();
                else FiltrarFasesLocais();
            }
            else
            {
                // MODO PERFIS
                BuscarPerfisNoBanco();
            }
        }

        // --- LÓGICA DE FASES ---

        private void CarregarFasesDoBanco()
        {
            try
            {
                LevelRepository repo = new LevelRepository();
                // Passa o ID para saber se deu Like
                _cachedLevels = repo.GetAllLevels(UserSession.Id);
                FiltrarFasesLocais();
            }
            catch (Exception ex) { MessageBox.Show("Erro: " + ex.Message); }
        }

        private void FiltrarFasesLocais()
        {
            string search = txtSearch.Text.Trim().ToLower();
            string diff = cmbFilter.SelectedItem?.ToString() ?? "Todas";

            // Atalho: se digitar @ no modo fases, muda para modo perfil
            if (search.StartsWith("@"))
            {
                cmbSearchType.SelectedIndex = 1; // Muda para Perfil
                txtSearch.Text = search.Substring(1); // Remove o @
                return;
            }

            var filtered = _cachedLevels.Where(l =>
                l.Title.ToLower().Contains(search) &&
                (diff == "Todas" || l.Difficulty.Equals(diff, StringComparison.OrdinalIgnoreCase))
            ).ToList();

            RenderLevelList(filtered);
        }

        private void RenderLevelList(List<LevelEntity> levels)
        {
            pnlContainer.SuspendLayout();
            pnlContainer.Controls.Clear();

            if (levels.Count == 0) MostrarMensagemVazio("Nenhuma fase encontrada.");
            else
            {
                foreach (var nivel in levels)
                {
                    var card = new LevelItemControl(nivel);
                    card.SetPreview(GerarPreview(nivel.Data, 220, 120));

                    card.PlayRequested += (s, lvl) => {
                        string json = JsonConvert.SerializeObject(lvl.Data);

                        // Passa o ID da fase para salvar estatísticas
                        GameForm game = new GameForm(json, lvl.Id);
                        ThemeManager.ApplyTheme(game);

                        this.Hide();
                        game.ShowDialog(); // Jogo roda aqui...
                        this.Show();

                        // --- CORREÇÃO DO CONTADOR DE VISITAS ---
                        // Ao voltar do jogo, recarrega do banco para atualizar Likes e Plays
                        if (cmbSearchType.SelectedIndex == 0)
                        {
                            CarregarFasesDoBanco();
                        }
                        // ---------------------------------------
                    };

                    card.ProfileRequested += (s, authorId) => AbrirPerfil(authorId);
                    pnlContainer.Controls.Add(card);
                }
            }
            pnlContainer.ResumeLayout();
        }

        // --- LÓGICA DE PERFIS ---

        private void BuscarPerfisNoBanco()
        {
            pnlContainer.SuspendLayout();
            pnlContainer.Controls.Clear();

            string search = txtSearch.Text.Trim();
            string filter = cmbFilter.SelectedItem?.ToString() ?? "Todos";
            string orderBy = "u.nome ASC";

            if (filter == "Mais Seguidos")
                orderBy = "(SELECT COUNT(*) FROM followers f WHERE f.user_id = u.id) DESC";
            else if (filter == "Criadores Ativos")
                orderBy = "(SELECT COUNT(*) FROM levels l WHERE l.author_id = u.id AND l.published = 1) DESC";

            try
            {
                using (var conn = Database.GetConnection())
                {
                    string sql = $@"
                        SELECT u.id, u.nome, u.avatar_image, u.descricao,
                               (SELECT COUNT(*) FROM followers sub WHERE sub.user_id = u.id) as num_seguidores,
                               (SELECT COUNT(*) FROM levels l WHERE l.author_id = u.id AND l.published = 1) as num_fases
                        FROM users u 
                        WHERE u.nome LIKE @q 
                        ORDER BY {orderBy} LIMIT 50";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@q", "%" + search + "%");
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (!reader.HasRows) MostrarMensagemVazio("Nenhum usuário encontrado.");

                            while (reader.Read())
                            {
                                int id = reader.GetInt32("id");
                                string nome = reader.GetString("nome");
                                byte[]? avatar = reader.IsDBNull(reader.GetOrdinal("avatar_image")) ? null : (byte[])reader["avatar_image"];

                                string desc = reader.IsDBNull(reader.GetOrdinal("descricao")) ? "" : reader.GetString("descricao");
                                int seg = reader.GetInt32("num_seguidores");
                                int fases = reader.GetInt32("num_fases");

                                RenderRichUserCard(id, nome, avatar, desc, seg, fases);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Erro busca perfil: " + ex.Message); }
            finally { pnlContainer.ResumeLayout(); }
        }

        private void RenderRichUserCard(int id, string name, byte[]? avatarBytes, string descricao, int seguidores, int fases)
        {
            bool isDark = UserSession.Theme == "dark";
            Color textColor = isDark ? Color.White : Color.FromArgb(64, 64, 64);
            Color subTextColor = isDark ? Color.LightGray : Color.Gray;

            Panel pnlCard = new Panel();
            pnlCard.Size = new Size(360, 90);
            pnlCard.Margin = new Padding(15);
            pnlCard.Cursor = Cursors.Hand;
            pnlCard.BorderStyle = BorderStyle.FixedSingle;
            pnlCard.BackColor = isDark ? Color.FromArgb(60, 60, 60) : Color.White;

            pnlCard.Click += (s, e) => AbrirPerfil(id);

            PictureBox pb = new PictureBox();
            pb.Size = new Size(60, 60);
            pb.Location = new Point(10, 15);
            pb.SizeMode = PictureBoxSizeMode.StretchImage;
            pb.BackColor = Color.Transparent;
            pb.Enabled = false;

            if (avatarBytes != null && avatarBytes.Length > 0)
            {
                using (var ms = new MemoryStream(avatarBytes)) pb.Image = Image.FromStream(ms);
            }
            else
            {
                try { pb.LoadAsync($"https://ui-avatars.com/api/?name={name}&background=random&color=fff&size=128"); } catch { }
            }

            pb.Paint += (s, e) => {
                GraphicsPath gp = new GraphicsPath();
                gp.AddEllipse(0, 0, pb.Width, pb.Height);
                pb.Region = new Region(gp);
            };

            Label lblNome = new Label();
            lblNome.Text = name;
            lblNome.Location = new Point(80, 10);
            lblNome.AutoSize = true;
            lblNome.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblNome.ForeColor = textColor;
            lblNome.Enabled = false;

            string descCurta = descricao;
            if (descCurta.Length > 35) descCurta = descCurta.Substring(0, 35) + "...";
            if (string.IsNullOrWhiteSpace(descCurta)) descCurta = "Sem descrição.";

            Label lblDesc = new Label();
            lblDesc.Text = descCurta;
            lblDesc.Location = new Point(80, 35);
            lblDesc.Size = new Size(260, 20);
            lblDesc.Font = new Font("Segoe UI", 9F, FontStyle.Italic);
            lblDesc.ForeColor = subTextColor;
            lblDesc.Enabled = false;

            Label lblStats = new Label();
            lblStats.Text = $"👥 {seguidores} Seguidores   🎮 {fases} Fases";
            lblStats.Location = new Point(80, 58);
            lblStats.AutoSize = true;
            lblStats.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            lblStats.ForeColor = Color.FromArgb(0, 120, 215);
            lblStats.Enabled = false;

            pnlCard.Controls.Add(pb);
            pnlCard.Controls.Add(lblNome);
            pnlCard.Controls.Add(lblDesc);
            pnlCard.Controls.Add(lblStats);

            pnlContainer.Controls.Add(pnlCard);
        }

        private void AbrirPerfil(int userId)
        {
            ProfileForm p = new ProfileForm(userId);
            this.Hide(); p.ShowDialog();
            ThemeManager.ApplyTheme(this); 
            LoadUserAvatar();
            if (_cachedLevels.Count == 0) CarregarFasesDoBanco();
            RenderLevelList(_cachedLevels);
            this.Show();
        }

        private void MostrarMensagemVazio(string msg)
        {
            Label lbl = new Label { Text = msg, AutoSize = true, ForeColor = Color.Gray, Font = new Font("Segoe UI", 12), Margin = new Padding(20) };
            pnlContainer.Controls.Add(lbl);
        }

        private Bitmap GerarPreview(LevelData level, int width, int height)
        {
            Bitmap bmp = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.Transparent); g.SmoothingMode = SmoothingMode.AntiAlias; g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                if (level.matrix == null) return bmp;
                int rows = level.matrix.Count; int cols = level.matrix[0].Count;
                float cw = (float)width / cols; float ch = (float)height / rows;
                int rad = Math.Max(2, (int)(Math.Min(cw, ch) * 0.25f));

                for (int r = 0; r < rows; r++) for (int c = 0; c < cols; c++)
                    {
                        var cell = level.matrix[r][c];
                        RectangleF rf = new RectangleF(c * cw, r * ch, cw, ch);
                        Brush b = Brushes.Transparent;
                        if (cell.color == "blue") b = new SolidBrush(Color.FromArgb(100, 149, 237));
                        else if (cell.color == "red") b = new SolidBrush(Color.FromArgb(205, 92, 92));
                        else if (cell.color == "green") b = new SolidBrush(Color.FromArgb(60, 179, 113));
                        if (cell.color != "none")
                        {
                            using (GraphicsPath p = GraphicsUtils.CreateRoundedRectanglePath(Rectangle.Round(rf), rad)) g.FillPath(b, p);
                        }
                        if (b is SolidBrush sb) sb.Dispose();
                        if (cell.symbol == "star") g.DrawString("★", new Font("Segoe UI Symbol", Math.Min(cw, ch) * 0.6f, FontStyle.Bold), Brushes.Gold, rf, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                        else if (cell.symbol == "play" || cell.symbol == "player") g.DrawString("▶", new Font("Segoe UI Symbol", Math.Min(cw, ch) * 0.5f, FontStyle.Bold), Brushes.Gray, rf, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }
            }
            return bmp;
        }

        private void LoadUserAvatar()
        {
            if (UserSession.Avatar != null)
            {
                Bitmap bmp = new Bitmap(50, 50);
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.SmoothingMode = SmoothingMode.AntiAlias; g.Clear(Color.Transparent);
                    using (TextureBrush tb = new TextureBrush(UserSession.Avatar))
                    {
                        tb.TranslateTransform(0, 0);
                        tb.ScaleTransform(50f / UserSession.Avatar.Width, 50f / UserSession.Avatar.Height);
                        g.FillEllipse(tb, 0, 0, 50, 50);
                    }
                }
                pbUserAvatar.Image = bmp;
            }
            else { pbUserAvatar.Image = null; pbUserAvatar.BackColor = Color.Gray; }
        }

        private void pbUserAvatar_Click(object sender, EventArgs e) => ctxUserMenu.Show(pbUserAvatar, new Point(0, pbUserAvatar.Height));
        private void menuProfile_Click(object sender, EventArgs e) => AbrirPerfil(UserSession.Id);
        private void menuLogout_Click(object sender, EventArgs e) { UserSession.Logout(); new LoginForm().Show(); this.Close(); }
        private void btnCreateLevel_Click(object sender, EventArgs e)
        {
            LevelEditorForm editor = new LevelEditorForm();
            ThemeManager.ApplyTheme(editor); this.Hide(); editor.ShowDialog(); this.Show();
            if (cmbSearchType.SelectedIndex == 0) CarregarFasesDoBanco();
        }

        private void btnChat_Click(object sender, EventArgs e)
        {
            ChatForm chat = new ChatForm();
            this.Hide();
            chat.ShowDialog();
            this.Show();
        }
    }
}