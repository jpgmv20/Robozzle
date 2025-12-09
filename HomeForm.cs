using Newtonsoft.Json;
using RobozllueApp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Robozzle
{
    // Lembre-se de herdar de FormLoader se estiver usando a herança visual
    public partial class HomeForm : FormLoader
    {
        public HomeForm()
        {
            InitializeComponent();
        }

        private void HomeForm_Load(object sender, EventArgs e)
        {
            CarregarFases();
        }

        private void btnCreateLevel_Click(object sender, EventArgs e)
        {
            // Abre o editor de níveis
            LevelEditorForm editor = new LevelEditorForm();
            this.Hide(); // Esconde a Home
            editor.ShowDialog(); // Espera o editor fechar
            this.Show(); // Mostra a Home de novo

            // Recarrega a lista para mostrar a nova fase criada
            CarregarFases();
        }

        // ... MANTENHA O RESTANTE DO CÓDIGO (CarregarFases, GerarPreview, BtnLevel_Click) IGUAL ...
        // ... Apenas certifique-se que o método btnCreateLevel_Click está lá dentro.

        private void CarregarFases()
        {
            try
            {
                LevelRepository repo = new LevelRepository();
                List<LevelEntity> niveis = repo.GetAllLevels();

                pnlContainer.Controls.Clear();

                if (niveis.Count == 0)
                {
                    Label lblEmpty = new Label();
                    lblEmpty.Text = "Nenhuma fase encontrada. Crie uma nova!";
                    lblEmpty.AutoSize = true;
                    lblEmpty.Font = new Font("Segoe UI", 12);
                    lblEmpty.ForeColor = Color.Gray;
                    pnlContainer.Controls.Add(lblEmpty);
                    return;
                }

                foreach (var nivel in niveis)
                {
                    Button btnLevel = new Button();
                    // --- Estilo "Flat Card" ---
                    btnLevel.Size = new Size(240, 200);
                    btnLevel.BackColor = Color.White;
                    btnLevel.ForeColor = Color.FromArgb(64, 64, 64);
                    btnLevel.FlatStyle = FlatStyle.Flat;
                    btnLevel.FlatAppearance.BorderSize = 0;
                    btnLevel.Font = new Font("Segoe UI", 10, FontStyle.Regular);
                    btnLevel.Cursor = Cursors.Hand;
                    btnLevel.Margin = new Padding(15);

                    btnLevel.Text = $"{nivel.Title}\n{nivel.Difficulty.ToUpper()}";
                    btnLevel.TextAlign = ContentAlignment.BottomCenter;
                    btnLevel.TextImageRelation = TextImageRelation.ImageAboveText;
                    btnLevel.ImageAlign = ContentAlignment.MiddleCenter;
                    btnLevel.Padding = new Padding(0, 10, 0, 10);

                    // Gera Preview (com quadrados arredondados e encostados)
                    btnLevel.Image = GerarPreview(nivel.Data, 220, 120);

                    btnLevel.Tag = nivel.Data;
                    btnLevel.Click += BtnLevel_Click;

                    pnlContainer.Controls.Add(btnLevel);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar fases: {ex.Message}");
            }
        }

        // Mantenha o método GerarPreview atualizado que fizemos anteriormente
        private Bitmap GerarPreview(LevelData level, int width, int height)
        {
            Bitmap bmp = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.Clear(Color.FromArgb(248, 249, 250));
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;

                if (level.matrix == null || level.matrix.Count == 0) return bmp;

                int rows = level.matrix.Count;
                int cols = level.matrix[0].Count;

                float cellW = (float)width / cols;
                float cellH = (float)height / rows;
                int cornerRadius = Math.Max(2, (int)(Math.Min(cellW, cellH) * 0.25f));

                for (int r = 0; r < rows; r++)
                {
                    for (int c = 0; c < cols; c++)
                    {
                        var cell = level.matrix[r][c];
                        float x = c * cellW;
                        float y = r * cellH;
                        RectangleF rectF = new RectangleF(x, y, cellW, cellH);
                        Rectangle tileRect = Rectangle.Round(rectF);

                        Brush brush = Brushes.Transparent;
                        string colorName = cell.color?.ToLower() ?? "none";

                        if (colorName == "blue") brush = new SolidBrush(Color.FromArgb(100, 149, 237));
                        else if (colorName == "red") brush = new SolidBrush(Color.FromArgb(205, 92, 92));
                        else if (colorName == "green") brush = new SolidBrush(Color.FromArgb(60, 179, 113));

                        if (colorName != "none")
                        {
                            using (GraphicsPath path = GraphicsUtils.CreateRoundedRectanglePath(tileRect, cornerRadius))
                            {
                                g.FillPath(brush, path);
                            }
                        }

                        if (brush is SolidBrush sb) sb.Dispose();

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
                                g.DrawString("▶", f, Brushes.DimGray, rectF, sf);
                            }
                        }
                    }
                }
            }
            return bmp;
        }

        private void BtnLevel_Click(object sender, EventArgs e)
        {
            Button btn = (Button)sender;
            LevelData data = (LevelData)btn.Tag;

            if (data != null)
            {
                string jsonString = JsonConvert.SerializeObject(data);
                GameForm gameForm = new GameForm(jsonString);
                this.Hide();
                gameForm.ShowDialog();
                this.Show();
            }
        }
    }
}