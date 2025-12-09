using Newtonsoft.Json;
using RobozllueApp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace Robozzle
{
    // Herda de FormLoader para manter o padrão visual (maximized)
    public partial class HomeForm : FormLoader
    {
        // Cache local das fases para permitir filtro rápido sem ir ao banco toda hora
        private List<LevelEntity> _allLevels = new List<LevelEntity>();

        public HomeForm()
        {
            InitializeComponent();
        }

        private void HomeForm_Load(object sender, EventArgs e)
        {
            // Aplica o tema (Dark/Light) conforme configuração do usuário
            ThemeManager.ApplyTheme(this);

            // Carrega a foto do usuário no canto superior
            LoadUserAvatar();

            // Busca as fases no banco de dados
            CarregarFasesDoBanco();
        }

        // --- GESTÃO DE USUÁRIO (AVATAR E MENU) ---

        private void LoadUserAvatar()
        {
            if (UserSession.Avatar != null)
            {
                // Cria uma versão redonda da imagem do avatar
                Bitmap bmp = new Bitmap(50, 50);
                using (Graphics g = Graphics.FromImage(bmp))
                {
                    g.SmoothingMode = SmoothingMode.AntiAlias;
                    g.Clear(Color.Transparent);

                    // Cria um pincel de textura com a imagem do avatar
                    using (TextureBrush tb = new TextureBrush(UserSession.Avatar))
                    {
                        // Ajusta a escala para caber no círculo de 50x50
                        tb.TranslateTransform(0, 0);
                        float scaleX = 50f / UserSession.Avatar.Width;
                        float scaleY = 50f / UserSession.Avatar.Height;
                        tb.ScaleTransform(scaleX, scaleY);

                        // Preenche uma elipse (círculo)
                        g.FillEllipse(tb, 0, 0, 50, 50);
                    }
                    // Opcional: Borda suave
                    // using (Pen p = new Pen(Color.Gray, 1)) g.DrawEllipse(p, 0, 0, 49, 49);
                }
                pbUserAvatar.Image = bmp;
            }
            else
            {
                // Se não tiver avatar, coloca uma cor padrão ou imagem genérica
                pbUserAvatar.Image = null;
                pbUserAvatar.BackColor = Color.Gray;
            }
        }

        private void pbUserAvatar_Click(object sender, EventArgs e)
        {
            // Abre o menu de contexto logo abaixo do avatar
            ctxUserMenu.Show(pbUserAvatar, new Point(0, pbUserAvatar.Height));
        }

        private void menuProfile_Click(object sender, EventArgs e)
        {
            // Abre a tela de perfil
            ProfileForm profile = new ProfileForm();
            this.Hide();
            profile.ShowDialog();

            // Ao voltar, recarrega as preferências (tema e avatar podem ter mudado)
            ThemeManager.ApplyTheme(this);
            LoadUserAvatar();
            this.Show();
        }

        private void menuLogout_Click(object sender, EventArgs e)
        {
            // Faz logout e volta para o login
            UserSession.Logout();
            LoginForm login = new LoginForm();
            this.Hide();
            login.ShowDialog();
            this.Close(); // Fecha a Home definitivamente
        }

        // --- GESTÃO DE FASES (CARREGAR E FILTRAR) ---

        private void CarregarFasesDoBanco()
        {
            try
            {
                LevelRepository repo = new LevelRepository();
                _allLevels = repo.GetAllLevels(); // Busca tudo do banco

                // Exibe a lista completa inicialmente
                RenderLevelList(_allLevels);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar fases: {ex.Message}");
            }
        }

        // Método chamado quando digita na busca ou muda o combo de dificuldade
        private void FilterLevels(object sender, EventArgs e)
        {
            string searchText = txtSearch.Text.Trim().ToLower();
            string difficultyFilter = cmbFilter.SelectedItem?.ToString() ?? "Todas";

            // Filtra a lista em memória usando LINQ
            var filteredLevels = _allLevels.Where(level =>
            {
                bool matchesText = string.IsNullOrEmpty(searchText) || level.Title.ToLower().Contains(searchText);
                bool matchesDiff = difficultyFilter == "Todas" || level.Difficulty.Equals(difficultyFilter, StringComparison.OrdinalIgnoreCase);

                return matchesText && matchesDiff;
            }).ToList();

            RenderLevelList(filteredLevels);
        }

        private void RenderLevelList(List<LevelEntity> levels)
        {
            // Suspende layout para melhorar performance visual durante a atualização
            pnlContainer.SuspendLayout();
            pnlContainer.Controls.Clear();

            if (levels.Count == 0)
            {
                Label lblEmpty = new Label();
                lblEmpty.Text = "Nenhuma fase encontrada com esses filtros.";
                lblEmpty.AutoSize = true;
                lblEmpty.Font = new Font("Segoe UI", 12);
                lblEmpty.ForeColor = Color.Gray;
                pnlContainer.Controls.Add(lblEmpty);
            }
            else
            {
                foreach (var nivel in levels)
                {
                    Button btnLevel = new Button();

                    // --- Estilo "Card" ---
                    btnLevel.Size = new Size(240, 220);
                    btnLevel.BackColor = (UserSession.Theme == "dark") ? Color.FromArgb(60, 60, 60) : Color.White;
                    btnLevel.ForeColor = (UserSession.Theme == "dark") ? Color.White : Color.FromArgb(64, 64, 64);

                    btnLevel.FlatStyle = FlatStyle.Flat;
                    btnLevel.FlatAppearance.BorderSize = 0;
                    btnLevel.Font = new Font("Segoe UI", 10, FontStyle.Regular);
                    btnLevel.Cursor = Cursors.Hand;
                    btnLevel.Margin = new Padding(15);

                    // Configura Texto
                    btnLevel.Text = $"{nivel.Title}\n[{nivel.Difficulty.ToUpper()}]";
                    btnLevel.TextAlign = ContentAlignment.BottomCenter;
                    btnLevel.TextImageRelation = TextImageRelation.ImageAboveText;
                    btnLevel.ImageAlign = ContentAlignment.MiddleCenter;
                    btnLevel.Padding = new Padding(5);

                    // Gera Preview Visual
                    btnLevel.Image = GerarPreview(nivel.Data, 220, 130);

                    // Guarda os dados no botão para usar no clique
                    btnLevel.Tag = nivel.Data;
                    btnLevel.Click += BtnLevel_Click;

                    pnlContainer.Controls.Add(btnLevel);
                }
            }

            pnlContainer.ResumeLayout();
        }

        private Bitmap GerarPreview(LevelData level, int width, int height)
        {
            Bitmap bmp = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                // Fundo transparente ou cor suave dependendo do tema
                g.Clear(Color.Transparent);
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                if (level.matrix == null || level.matrix.Count == 0) return bmp;

                int rows = level.matrix.Count;
                int cols = level.matrix[0].Count;

                float cellW = (float)width / cols;
                float cellH = (float)height / rows;

                // Arredondamento proporcional
                int cornerRadius = Math.Max(2, (int)(Math.Min(cellW, cellH) * 0.25f));

                for (int r = 0; r < rows; r++)
                {
                    for (int c = 0; c < cols; c++)
                    {
                        var cell = level.matrix[r][c];
                        RectangleF rectF = new RectangleF(c * cellW, r * cellH, cellW, cellH);
                        Rectangle tileRect = Rectangle.Round(rectF);

                        // Define cores
                        Brush brush = Brushes.Transparent;
                        string colorName = cell.color?.ToLower() ?? "none";

                        if (colorName == "blue") brush = new SolidBrush(Color.FromArgb(100, 149, 237));
                        else if (colorName == "red") brush = new SolidBrush(Color.FromArgb(205, 92, 92));
                        else if (colorName == "green") brush = new SolidBrush(Color.FromArgb(60, 179, 113));

                        // Desenha apenas se tiver cor (chão)
                        if (colorName != "none")
                        {
                            // Usa a classe utilitária para desenhar arredondado
                            using (GraphicsPath path = GraphicsUtils.CreateRoundedRectanglePath(tileRect, cornerRadius))
                            {
                                g.FillPath(brush, path);
                            }
                        }

                        if (brush is SolidBrush sb) sb.Dispose();

                        // Desenha Símbolos
                        string symbol = cell.symbol?.ToLower() ?? "none";
                        if (symbol == "star")
                        {
                            float fontSize = Math.Min(cellW, cellH) * 0.6f;
                            using (Font f = new Font("Segoe UI Symbol", fontSize, FontStyle.Bold))
                            {
                                StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                                g.DrawString("★", f, Brushes.Gold, rectF, sf);
                            }
                        }
                        else if (symbol == "play" || symbol == "player")
                        {
                            float fontSize = Math.Min(cellW, cellH) * 0.5f;
                            using (Font f = new Font("Segoe UI Symbol", fontSize, FontStyle.Bold))
                            {
                                StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                                g.DrawString("▶", f, Brushes.WhiteSmoke, rectF, sf);
                            }
                        }
                    }
                }
            }
            return bmp;
        }

        // --- AÇÕES ---

        private void BtnLevel_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            LevelData data = (LevelData)btn.Tag;

            if (data != null)
            {
                string jsonString = JsonConvert.SerializeObject(data);
                GameForm gameForm = new GameForm(jsonString);

                // Aplica tema ao jogo antes de abrir
                ThemeManager.ApplyTheme(gameForm);

                this.Hide();
                gameForm.ShowDialog();
                this.Show();
            }
        }

        private void btnCreateLevel_Click(object sender, EventArgs e)
        {
            LevelEditorForm editor = new LevelEditorForm();
            ThemeManager.ApplyTheme(editor); // Aplica tema ao editor

            this.Hide();
            editor.ShowDialog();
            this.Show();

            // Recarrega lista, pois o usuário pode ter criado uma fase nova
            CarregarFasesDoBanco();
        }
    }
}