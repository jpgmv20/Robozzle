using Newtonsoft.Json;
using RobozllueApp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;

namespace Robozzle
{
    public partial class GameForm : FormLoader
    {
        private GameEngine? _engine;
        private LevelData? _originalData;
        private int _cellSize = 48;
        private Button? _selectedSlot = null;
        private int _levelId = 0; // Armazena o ID da fase

        public class SlotInfo
        {
            public string FuncName { get; set; } = "";
            public int Index { get; set; }
            public string Condition { get; set; } = "none";
            public string Action { get; set; } = "";
        }

        public GameForm(string levelJson, int levelId = 0)
        {
            InitializeComponent();
            _levelId = levelId;
            this.PainelCentral = pnlGameContainer;

            if (string.IsNullOrEmpty(levelJson)) { this.Close(); return; }

            _originalData = JsonConvert.DeserializeObject<LevelData>(levelJson);
            if (_originalData != null) this.Text = $"Robozzle - {_originalData.title}";

            if (Application.OpenForms.Count > 0 && Application.OpenForms[0].Icon != null)
                this.Icon = Application.OpenForms[0].Icon;

            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);

            LoadLevelInitialState();
            ApplyResponsiveLayout();
        }

        private void GameForm_Resize(object sender, EventArgs e) => ApplyResponsiveLayout();

        private void ApplyResponsiveLayout()
        {
            if (this.ClientSize.Width == 0 || this.ClientSize.Height == 0) return;

            int w = pnlGameContainer.Width;
            int h = pnlGameContainer.Height;
            int margin = 20;

            // 1. GRADE
            int gridH = (int)(h * 0.55);

            if (_engine != null && _engine.Level != null)
            {
                int rows = Math.Max(1, _engine.Level.matrix.Count);
                int cols = Math.Max(1, _engine.Level.matrix[0].Count);
                int maxCellH = (gridH - margin) / rows;
                int maxCellW = (w - (margin * 2)) / cols;
                _cellSize = Math.Min(maxCellW, maxCellH);
                _cellSize = Math.Max(20, Math.Min(100, _cellSize));

                int finalGridW = cols * _cellSize;
                int finalGridH = rows * _cellSize;

                pbGrid.Size = new Size(finalGridW, finalGridH);
                pbGrid.Location = new Point((w - finalGridW) / 2, margin);
            }

            // 2. BARRA DE EXECUÇÃO
            int execY = gridH + margin + 10;
            int execH = 50;
            int btnW = 100;

            lblQueue.Location = new Point(margin, execY - 20);
            int buttonsX = w - margin - (btnW * 2) - 10;
            btnPlay.Location = new Point(buttonsX, execY);
            btnPlay.Size = new Size(btnW, execH);
            btnReset.Location = new Point(buttonsX + btnW + 10, execY);
            btnReset.Size = new Size(btnW, execH);
            flpExecution.Location = new Point(margin, execY);
            flpExecution.Size = new Size(buttonsX - margin - 20, execH);

            // 3. ÁREA INFERIOR
            int bottomY = execY + execH + 30;
            int bottomH = h - bottomY - margin;
            int paletteW = 180;
            lblPaletteTitle.Location = new Point(margin, bottomY - 20);
            pnlPalette.Location = new Point(margin, bottomY);
            pnlPalette.Size = new Size(paletteW, bottomH);
            int funcsX = margin + paletteW + margin;
            int funcsW = w - funcsX - margin;
            lblFunctionsTitle.Location = new Point(funcsX, bottomY - 20);
            pnlFunctions.Location = new Point(funcsX, bottomY);
            pnlFunctions.Size = new Size(funcsW, bottomH);

            pbGrid.Invalidate();
        }

        private void LoadLevelInitialState()
        {
            if (_originalData == null) return;
            var json = JsonConvert.SerializeObject(_originalData);
            var clonedData = JsonConvert.DeserializeObject<LevelData>(json);
            if (clonedData == null) return;
            _engine = new GameEngine(clonedData);
            _engine.OnStep += UpdateGameView;
            _engine.OnVictory += HandleVictory;
            _engine.OnDefeat += HandleDefeat;
            InitializeUI();
            pbGrid.Invalidate();
            UpdateQueueView();
        }

        private void SetEditMode(bool enable)
        {
            pnlPalette.Enabled = enable; pnlFunctions.Enabled = enable; btnPlay.Enabled = enable; btnReset.Enabled = true;
            if (!enable) { btnPlay.Text = "RODANDO..."; btnPlay.BackColor = Color.Gray; }
            else { btnPlay.Text = "PLAY ▶"; btnPlay.BackColor = Color.MediumSeaGreen; }
        }

        private void HandleVictory()
        {
            gameTimer.Stop();
            MessageBox.Show("Vitória!");

            // --- SALVAR NO BANCO ---
            if (_levelId > 0 && UserSession.IsLoggedIn && _engine != null)
            {
                try
                {
                    var repo = new LevelRepository();
                    repo.RegisterRun(_levelId, UserSession.Id, "success", _engine.StepsTaken);
                    repo.SaveProgram(_levelId, UserSession.Id, _engine.UserProgram);
                }
                catch { }
            }
            // -----------------------

            SetEditMode(true);
            this.Close();
        }

        private void HandleDefeat()
        {
            gameTimer.Stop();
            MessageBox.Show("Falha!");

            // --- SALVAR NO BANCO ---
            if (_levelId > 0 && UserSession.IsLoggedIn && _engine != null)
            {
                try
                {
                    var repo = new LevelRepository();
                    repo.RegisterRun(_levelId, UserSession.Id, "failure", _engine.StepsTaken);
                }
                catch { }
            }
            // -----------------------

            _engine?.Reset();
            pbGrid.Invalidate();
            UpdateQueueView();
            SetEditMode(true);
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            if (_engine == null) return;
            UpdateEngineFromUI();
            _engine.Reset();
            SetEditMode(false);
            gameTimer.Start();
        }

        private void ResetGame()
        {
            if (_originalData == null) return;
            var json = JsonConvert.SerializeObject(_originalData);
            var clonedData = JsonConvert.DeserializeObject<LevelData>(json);
            if (clonedData == null) return;
            _engine = new GameEngine(clonedData);
            _engine.OnStep += UpdateGameView;
            _engine.OnVictory += HandleVictory;
            _engine.OnDefeat += HandleDefeat;
            UpdateEngineFromUI();
            _engine.Reset();
            pbGrid.Invalidate();
            UpdateQueueView();
        }

        private void btnReset_Click(object? sender, EventArgs e)
        {
            gameTimer.Stop();
            ResetGame();
            pbGrid.Invalidate();
            UpdateQueueView();
            SetEditMode(true);
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            if (_engine != null) _engine.Tick();
        }

        private void UpdateGameView() { pbGrid.Invalidate(); UpdateQueueView(); }

        private void UpdateQueueView()
        {
            flpExecution.SuspendLayout();
            flpExecution.Controls.Clear();

            if (_engine != null)
            {
                var slots = _engine.GetNextCommandsPreview(15);
                foreach (var slot in slots)
                {
                    Label lbl = new Label();
                    lbl.Size = new Size(40, 40);
                    lbl.Margin = new Padding(2);
                    lbl.TextAlign = ContentAlignment.MiddleCenter;
                    lbl.Font = new Font("Segoe UI Symbol", 14F, FontStyle.Bold);
                    lbl.BorderStyle = BorderStyle.FixedSingle;

                    if (slot.ConditionColor == "blue") lbl.BackColor = Color.LightBlue;
                    else if (slot.ConditionColor == "green") lbl.BackColor = Color.LightGreen;
                    else if (slot.ConditionColor == "red") lbl.BackColor = Color.LightPink;
                    else lbl.BackColor = Color.White;

                    lbl.ForeColor = Color.Black;
                    lbl.Text = GetSymbol(slot.Action);
                    flpExecution.Controls.Add(lbl);
                }
            }
            flpExecution.ResumeLayout();
        }

        private void InitializeUI()
        {
            pnlPalette.Controls.Clear(); pnlFunctions.Controls.Clear();
            string[] movements = { "FORWARD", "TURN_LEFT", "TURN_RIGHT" };
            foreach (var cmd in movements) AddPaletteButton(cmd, cmd, false);
            AddPaletteButton("PAINT_BLUE", "PAINT_BLUE", false);
            AddPaletteButton("PAINT_GREEN", "PAINT_GREEN", false);
            AddPaletteButton("PAINT_RED", "PAINT_RED", false);
            AddPaletteButton("COND_BLUE", "COND_BLUE", true);
            AddPaletteButton("COND_GREEN", "COND_GREEN", true);
            AddPaletteButton("COND_RED", "COND_RED", true);
            AddPaletteButton("COND_NONE", "COND_NONE", true);

            if (_originalData != null)
            {
                foreach (var fn in _originalData.functions) AddPaletteButton(fn.name, fn.name, false);
                CreateFunctionSlots();
            }
        }

        private void AddPaletteButton(string text, string tag, bool isCondition)
        {
            Button btn = new Button();
            btn.Tag = tag;
            btn.Width = 40; btn.Height = 40;
            btn.FlatStyle = FlatStyle.Flat;
            btn.Margin = new Padding(3);
            btn.Click += Palette_Click;

            if (isCondition)
            {
                if (tag == "COND_BLUE") btn.BackColor = Color.LightBlue;
                else if (tag == "COND_GREEN") btn.BackColor = Color.LightGreen;
                else if (tag == "COND_RED") btn.BackColor = Color.LightPink;
                else btn.BackColor = Color.White;
                if (tag == "COND_NONE") btn.Text = "X";
            }
            else
            {
                btn.Text = GetSymbol(text);
                btn.BackColor = Color.WhiteSmoke;
                btn.Font = new Font("Segoe UI Symbol", 10F, FontStyle.Bold);
            }
            pnlPalette.Controls.Add(btn);
        }

        private string GetSymbol(string cmd)
        {
            if (cmd == "FORWARD") return "⬆";
            if (cmd == "TURN_LEFT") return "↰";
            if (cmd == "TURN_RIGHT") return "↱";
            if (cmd == "PAINT_BLUE") return "🖌🔵";
            if (cmd == "PAINT_GREEN") return "🖌🟢";
            if (cmd == "PAINT_RED") return "🖌🔴";
            if (cmd.StartsWith("F")) return cmd;
            return cmd;
        }

        private void CreateFunctionSlots()
        {
            if (_originalData == null) return;
            pnlFunctions.Controls.Clear();
            int topOffset = 10;
            foreach (var func in _originalData.functions)
            {
                Label lbl = new Label { Text = func.name, Top = topOffset, Left = 10, AutoSize = true, Font = new Font("Segoe UI", 10, FontStyle.Bold) };
                pnlFunctions.Controls.Add(lbl);
                for (int i = 0; i < func.size; i++)
                {
                    Button slot = new Button();
                    slot.Size = new Size(40, 40);
                    slot.Location = new Point(50 + (i * 45), topOffset - 10);
                    slot.Tag = new SlotInfo { FuncName = func.name, Index = i, Condition = "none", Action = "" };
                    slot.BackColor = Color.White;
                    slot.FlatStyle = FlatStyle.Flat;
                    slot.MouseDown += Slot_MouseDown;
                    pnlFunctions.Controls.Add(slot);
                }
                topOffset += 50;
            }
        }

        private void Slot_MouseDown(object? sender, MouseEventArgs e)
        {
            if (sender is not Button slot) return; if (slot.Tag is not SlotInfo info) return; if (gameTimer.Enabled) return;
            if (e.Button == MouseButtons.Left)
            {
                if (_selectedSlot != null && _selectedSlot != slot) ResetSlotVisual(_selectedSlot);
                _selectedSlot = slot;
                _selectedSlot.FlatAppearance.BorderColor = Color.Blue;
                _selectedSlot.FlatAppearance.BorderSize = 2;
            }
            else if (e.Button == MouseButtons.Right)
            {
                switch (info.Condition)
                {
                    case "none": info.Condition = "blue"; break;
                    case "blue": info.Condition = "green"; break;
                    case "green": info.Condition = "red"; break;
                    case "red": info.Condition = "none"; break;
                }
                SetSlotColor(slot, info.Condition);
            }
        }

        private void ResetSlotVisual(Button btn)
        {
            if (btn.Tag is not SlotInfo info) return;
            btn.FlatAppearance.BorderSize = 1;
            btn.FlatAppearance.BorderColor = Color.Gray;
            SetSlotColor(btn, info.Condition);
        }

        private void SetSlotColor(Button btn, string condition)
        {
            if (condition == "none") btn.BackColor = Color.White;
            else if (condition == "blue") btn.BackColor = Color.LightBlue;
            else if (condition == "green") btn.BackColor = Color.LightGreen;
            else if (condition == "red") btn.BackColor = Color.LightPink;
        }

        private void Palette_Click(object? sender, EventArgs e)
        {
            if (_selectedSlot == null) { MessageBox.Show("Selecione um quadrado na função primeiro!"); return; }
            if (sender is not Button cmdBtn) return;
            if (_selectedSlot.Tag is not SlotInfo info) return;

            string tag = cmdBtn.Tag?.ToString() ?? "";
            if (tag.StartsWith("COND_"))
            {
                string c = tag.Replace("COND_", "").ToLower();
                info.Condition = c;
                SetSlotColor(_selectedSlot, c);
            }
            else
            {
                info.Action = tag;
                _selectedSlot.Text = GetSymbol(tag);
            }
        }

        private void UpdateEngineFromUI()
        {
            if (_engine == null) return;
            foreach (Control c in pnlFunctions.Controls)
            {
                if (c is Button btn && btn.Tag is SlotInfo info)
                {
                    if (_engine.UserProgram.ContainsKey(info.FuncName))
                    {
                        var cmdSlot = _engine.UserProgram[info.FuncName][info.Index];
                        cmdSlot.Action = info.Action;
                        cmdSlot.ConditionColor = info.Condition;
                    }
                }
            }
        }

        private void pbGrid_Paint(object sender, PaintEventArgs e)
        {
            if (_engine == null || _engine.Level == null) return;
            Graphics g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.PixelOffsetMode = PixelOffsetMode.HighQuality;

            var matrix = _engine.Level.matrix;
            int cornerRadius = 8;

            for (int r = 0; r < matrix.Count; r++)
            {
                for (int c = 0; c < matrix[r].Count; c++)
                {
                    var cell = matrix[r][c];
                    Rectangle tileRect = new Rectangle(c * _cellSize, r * _cellSize, _cellSize, _cellSize);
                    Brush bgBrush = new SolidBrush(Color.FromArgb(245, 245, 245));
                    string color = cell.color?.ToLower() ?? "none";
                    if (color == "blue") bgBrush = new SolidBrush(Color.FromArgb(0, 120, 215));
                    else if (color == "red") bgBrush = new SolidBrush(Color.FromArgb(220, 50, 50));
                    else if (color == "green") bgBrush = new SolidBrush(Color.FromArgb(50, 180, 80));
                    else if (color == "none") bgBrush = new SolidBrush(Color.FromArgb(235, 235, 235));

                    using (GraphicsPath path = GraphicsUtils.CreateRoundedRectanglePath(tileRect, cornerRadius))
                    {
                        g.FillPath(bgBrush, path);
                        using (Pen borderPen = new Pen(this.BackColor, 2f)) g.DrawPath(borderPen, path);
                    }
                    bgBrush.Dispose();

                    if (cell.symbol == "star")
                    {
                        float fontSize = _cellSize / 2.2f;
                        var sf = new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
                        g.DrawString("★", new Font("Segoe UI Symbol", fontSize), Brushes.White, tileRect, sf);
                    }
                }
            }

            var p = _engine.Player;
            if (p != null)
            {
                var state = g.Save();
                g.TranslateTransform(p.Col * _cellSize + _cellSize / 2f, p.Row * _cellSize + _cellSize / 2f);
                float angle = 0; switch (p.Dir) { case Direction.Right: angle = 90; break; case Direction.Down: angle = 180; break; case Direction.Left: angle = 270; break; case Direction.Up: angle = 0; break; }
                g.RotateTransform(angle);
                g.DrawString("⬆", new Font("Arial", _cellSize / 1.8f, FontStyle.Bold), Brushes.White, 0, -2, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                g.Restore(state);
            }
        }
    }
}