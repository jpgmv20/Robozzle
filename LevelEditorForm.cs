using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using RobozllueApp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace Robozzle
{
    public partial class LevelEditorForm : FormLoader
    {
        private LevelData _level;
        private int _cellSize = 40;
        private string _selectedTool = "none";
        private bool _isUpdatingUI = false; // Evita loop infinito no evento ValueChanged

        private readonly Color _colBlue = Color.FromArgb(0, 120, 215);
        private readonly Color _colGreen = Color.FromArgb(50, 180, 80);
        private readonly Color _colRed = Color.FromArgb(220, 50, 50);
        private readonly Color _colNone = Color.WhiteSmoke;

        public LevelEditorForm()
        {
            InitializeComponent();
            this.PainelCentral = pnlEditorContainer;

            _level = new LevelData
            {
                title = "Nova Fase",
                difficulty = "Easy",
                grid_cell_size = 48,
                matrix = new List<List<CellData>>(),
                functions = new List<FunctionData>
                {
                    new FunctionData { name = "F0", size = 5 } // Inicia só com F0 tam 5
                }
            };
            ResizeGrid(10, 10);

            cmbDifficulty.Items.AddRange(new string[] { "Easy", "Medium", "Hard", "Insane" });
            cmbDifficulty.SelectedIndex = 0;

            // Cria botões da paleta
            CreateToolButton("Azul", "color_blue", _colBlue);
            CreateToolButton("Verde", "color_green", _colGreen);
            CreateToolButton("Vermelho", "color_red", _colRed);
            CreateToolButton("Borracha", "color_none", Color.White);
            CreateToolButton("Estrela", "symbol_star", Color.Gold);
            CreateToolButton("Player", "symbol_player", Color.LightGray);
            CreateToolButton("Apagar Item", "symbol_eraser", Color.White);
            CreateToolButton("⬆", "dir_up", Color.Silver);
            CreateToolButton("➡", "dir_right", Color.Silver);
            CreateToolButton("⬇", "dir_down", Color.Silver);
            CreateToolButton("⬅", "dir_left", Color.Silver);

            UpdateFunctionListUI();
            pbGrid.Invalidate();
        }

        // --- FUNÇÕES E TAMANHO ---

        private void btnAddFunc_Click(object sender, EventArgs e)
        {
            if (_level.functions.Count >= 5)
            {
                MessageBox.Show("Máximo de 5 funções.");
                return;
            }

            // Usa o tamanho definido na caixa numérica
            int size = (int)numFuncSize.Value;

            _level.functions.Add(new FunctionData { name = "F" + _level.functions.Count, size = size });
            UpdateFunctionListUI();

            // Seleciona a nova função
            lstFunctions.SelectedIndex = _level.functions.Count - 1;
        }

        private void btnRemFunc_Click(object sender, EventArgs e)
        {
            if (_level.functions.Count > 0)
            {
                _level.functions.RemoveAt(_level.functions.Count - 1);
                UpdateFunctionListUI();
            }
        }

        private void UpdateFunctionListUI()
        {
            _isUpdatingUI = true; // Bloqueia eventos

            int savedIndex = lstFunctions.SelectedIndex;
            lstFunctions.Items.Clear();

            foreach (var f in _level.functions)
            {
                lstFunctions.Items.Add($"{f.name} (Tam: {f.size})");
            }

            // Tenta restaurar seleção
            if (savedIndex >= 0 && savedIndex < lstFunctions.Items.Count)
                lstFunctions.SelectedIndex = savedIndex;
            else if (lstFunctions.Items.Count > 0)
                lstFunctions.SelectedIndex = 0; // Seleciona o primeiro se perdeu

            _isUpdatingUI = false;
        }

        private void lstFunctions_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstFunctions.SelectedIndex >= 0 && lstFunctions.SelectedIndex < _level.functions.Count)
            {
                // Atualiza a caixa numérica com o tamanho da função selecionada
                _isUpdatingUI = true;
                numFuncSize.Value = _level.functions[lstFunctions.SelectedIndex].size;
                _isUpdatingUI = false;
            }
        }

        private void numFuncSize_ValueChanged(object sender, EventArgs e)
        {
            if (_isUpdatingUI) return;

            // Se tem algo selecionado, atualiza o tamanho dele em tempo real
            if (lstFunctions.SelectedIndex >= 0 && lstFunctions.SelectedIndex < _level.functions.Count)
            {
                _level.functions[lstFunctions.SelectedIndex].size = (int)numFuncSize.Value;

                // Atualiza o texto na lista (sem perder a seleção)
                // Truque: apenas atualizamos o item atual
                _isUpdatingUI = true;
                int idx = lstFunctions.SelectedIndex;
                var f = _level.functions[idx];
                lstFunctions.Items[idx] = $"{f.name} (Tam: {f.size})";
                _isUpdatingUI = false;
            }
        }

        private void CreateToolButton(string text, string tag, Color bg)
        {
            Button btn = new Button();
            btn.Text = text;
            btn.Tag = tag;
            btn.BackColor = bg;
            btn.Size = new Size(50, 40);
            btn.FlatStyle = FlatStyle.Flat;
            btn.Click += btnTool_Click;
            flowTools.Controls.Add(btn);
        }

        private void btnTool_Click(object sender, EventArgs e)
        {
            foreach (Control c in flowTools.Controls)
                if (c is Button b) b.FlatAppearance.BorderColor = Color.White;

            Button btn = (Button)sender;
            _selectedTool = btn.Tag.ToString();
            btn.FlatAppearance.BorderColor = Color.Black;
            lblSelectedTool.Text = "Ferramenta: " + btn.Text;
        }

        private void btnResize_Click(object sender, EventArgs e)
        {
            ResizeGrid((int)numRows.Value, (int)numCols.Value);
        }

        private void ResizeGrid(int rows, int cols)
        {
            _level.matrix.Clear();
            for (int r = 0; r < rows; r++)
            {
                var row = new List<CellData>();
                for (int c = 0; c < cols; c++) row.Add(new CellData());
                _level.matrix.Add(row);
            }
            numRows.Value = rows;
            numCols.Value = cols;
            pbGrid.Invalidate();
        }

        private void pbGrid_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            int rows = _level.matrix.Count;
            int cols = _level.matrix[0].Count;

            int maxH = (pbGrid.Height - 10) / rows;
            int maxW = (pbGrid.Width - 10) / cols;
            _cellSize = Math.Min(maxW, maxH);
            _cellSize = Math.Max(20, Math.Min(60, _cellSize));

            int gridW = cols * _cellSize;
            int gridH = rows * _cellSize;
            int startX = (pbGrid.Width - gridW) / 2;
            int startY = (pbGrid.Height - gridH) / 2;

            g.TranslateTransform(startX, startY);

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    var cell = _level.matrix[r][c];
                    Rectangle rect = new Rectangle(c * _cellSize, r * _cellSize, _cellSize, _cellSize);

                    Brush bg = new SolidBrush(_colNone);
                    if (cell.color == "blue") bg = new SolidBrush(_colBlue);
                    else if (cell.color == "green") bg = new SolidBrush(_colGreen);
                    else if (cell.color == "red") bg = new SolidBrush(_colRed);

                    using (GraphicsPath path = GraphicsUtils.CreateRoundedRectanglePath(rect, 8))
                    {
                        g.FillPath(bg, path);
                        using (Pen border = new Pen(Color.LightGray, 1)) g.DrawPath(border, path);
                    }
                    bg.Dispose();

                    StringFormat sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };

                    if (cell.symbol == "star")
                    {
                        g.DrawString("★", new Font("Segoe UI Symbol", _cellSize / 2.2f), Brushes.Gold, rect, sf);
                    }
                    else if (cell.symbol == "play" || cell.symbol == "player")
                    {
                        var state = g.Save();
                        float cx = rect.X + _cellSize / 2f;
                        float cy = rect.Y + _cellSize / 2f;
                        g.TranslateTransform(cx, cy);

                        float angle = 0;
                        if (cell.direction == 1) angle = 90;
                        else if (cell.direction == 2) angle = 180;
                        else if (cell.direction == 3) angle = 270;

                        g.RotateTransform(angle);
                        g.DrawString("⬆", new Font("Arial", _cellSize / 2f, FontStyle.Bold), Brushes.White, 0, -2, sf);
                        g.Restore(state);
                    }
                }
            }
        }

        private void pbGrid_MouseClick(object sender, MouseEventArgs e)
        {
            int rows = _level.matrix.Count;
            int cols = _level.matrix[0].Count;
            int gridW = cols * _cellSize;
            int gridH = rows * _cellSize;
            int startX = (pbGrid.Width - gridW) / 2;
            int startY = (pbGrid.Height - gridH) / 2;

            int c = (e.X - startX) / _cellSize;
            int r = (e.Y - startY) / _cellSize;

            if (r >= 0 && r < rows && c >= 0 && c < cols)
            {
                var cell = _level.matrix[r][c];

                switch (_selectedTool)
                {
                    case "color_blue": cell.color = "blue"; break;
                    case "color_green": cell.color = "green"; break;
                    case "color_red": cell.color = "red"; break;
                    case "color_none": cell.color = "none"; break;
                    case "symbol_star": cell.symbol = (cell.symbol == "star") ? "none" : "star"; break;
                    case "symbol_player":
                        ClearOtherPlayers(r, c);
                        cell.symbol = "player";
                        break;
                    case "symbol_eraser": cell.symbol = "none"; break;
                    case "dir_up": if (cell.symbol == "player") cell.direction = 0; break;
                    case "dir_right": if (cell.symbol == "player") cell.direction = 1; break;
                    case "dir_down": if (cell.symbol == "player") cell.direction = 2; break;
                    case "dir_left": if (cell.symbol == "player") cell.direction = 3; break;
                }
                pbGrid.Invalidate();
            }
        }

        private void ClearOtherPlayers(int currentR, int currentC)
        {
            for (int r = 0; r < _level.matrix.Count; r++)
                for (int c = 0; c < _level.matrix[0].Count; c++)
                    if ((r != currentR || c != currentC) && (_level.matrix[r][c].symbol == "player" || _level.matrix[r][c].symbol == "play"))
                        _level.matrix[r][c].symbol = "none";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text)) { MessageBox.Show("Digite um título."); return; }
            if (UserSession.Id == 0) { MessageBox.Show("Você precisa estar logado."); return; }

            _level.title = txtTitle.Text;
            _level.difficulty = cmbDifficulty.SelectedItem?.ToString() ?? "Easy";

            string json = JsonConvert.SerializeObject(_level);

            try
            {
                using (var conn = Database.GetConnection())
                {
                    using (var cmd = new MySqlCommand("create_level", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("p_author_id", UserSession.Id);
                        cmd.Parameters.AddWithValue("p_title", txtTitle.Text);
                        cmd.Parameters.AddWithValue("p_descricao", txtDesc.Text);
                        cmd.Parameters.AddWithValue("p_difficulty", _level.difficulty);
                        cmd.Parameters.AddWithValue("p_level_json", json);
                        cmd.Parameters.AddWithValue("p_published", true);
                        cmd.ExecuteNonQuery();


                        this.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao salvar: " + ex.Message);
            }
        }
    }
}