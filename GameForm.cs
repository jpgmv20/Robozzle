using Newtonsoft.Json;
using RobozllueApp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Robozzle
{
    public partial class GameForm : Form
    {
        private GameEngine _engine;
        private LevelData _originalData; // Para resetar
        private int _cellSize = 48;

        // UI State
        private Button _selectedSlot = null; // Slot que está sendo editado

        public GameForm(string levelJson)
        {
            InitializeComponent();

            // Desserializa o JSON vindo do banco
            _originalData = JsonConvert.DeserializeObject<LevelData>(levelJson);

            // Configura Engine
            // Precisamos clonar o objeto para que o reset funcione sem recarregar do banco
            var clonedData = JsonConvert.DeserializeObject<LevelData>(levelJson);
            _engine = new GameEngine(clonedData);

            _engine.OnStep += () => pbGrid.Invalidate();
            _engine.OnVictory += () => { gameTimer.Stop(); MessageBox.Show("Vitória!"); };
            _engine.OnDefeat += () => { gameTimer.Stop(); MessageBox.Show("Falha! Tente novamente."); };

            InitializeUI();
        }

        private void InitializeUI()
        {
            // Cria a Paleta de Comandos
            string[] commands = { "FORWARD", "TURN_LEFT", "TURN_RIGHT", "PAINT_BLUE", "PAINT_RED", "PAINT_GREEN" };
            foreach (var cmd in commands) AddPaletteButton(cmd, cmd);

            // Adiciona botões para chamar funções (F0, F1...) baseado no nível
            foreach (var fn in _originalData.functions)
            {
                AddPaletteButton(fn.name, fn.name);
            }

            // Cria slots de programação
            CreateFunctionSlots();
        }

        private void AddPaletteButton(string text, string tag)
        {
            Button btn = new Button();
            btn.Text = GetSymbol(text);
            btn.Tag = tag;
            btn.Width = 50; btn.Height = 50;
            btn.Click += Palette_Click;
            pnlPalette.Controls.Add(btn);
        }

        private string GetSymbol(string cmd)
        {
            if (cmd == "FORWARD") return "⬆";
            if (cmd == "TURN_LEFT") return "↰";
            if (cmd == "TURN_RIGHT") return "↱";
            if (cmd.Contains("PAINT")) return "🎨";
            return cmd;
        }

        private void CreateFunctionSlots()
        {
            pnlFunctions.Controls.Clear();
            int topOffset = 10;

            foreach (var func in _originalData.functions)
            {
                Label lbl = new Label { Text = func.name, Top = topOffset, Left = 10, AutoSize = true };
                pnlFunctions.Controls.Add(lbl);

                for (int i = 0; i < func.size; i++)
                {
                    Button slot = new Button();
                    slot.Size = new Size(40, 40);
                    slot.Location = new Point(50 + (i * 45), topOffset - 10);
                    slot.Tag = new Tuple<string, int>(func.name, i); // Guarda qual função e índice
                    slot.BackColor = Color.White;
                    slot.Click += Slot_Click;
                    pnlFunctions.Controls.Add(slot);
                }
                topOffset += 50;
            }
        }

        private void Slot_Click(object sender, EventArgs e)
        {
            // Seleciona o slot para receber comando
            if (_selectedSlot != null) _selectedSlot.BackColor = Color.White;
            _selectedSlot = (Button)sender;
            _selectedSlot.BackColor = Color.LightYellow;
        }

        private void Palette_Click(object sender, EventArgs e)
        {
            if (_selectedSlot == null) return;

            Button cmdBtn = (Button)sender;
            string command = cmdBtn.Tag.ToString();

            // Atualiza visual
            _selectedSlot.Text = cmdBtn.Text;

            // Salva na engine
            var info = (Tuple<string, int>)_selectedSlot.Tag;
            string funcName = info.Item1;
            int index = info.Item2;

            if (!_engine.UserProgram.ContainsKey(funcName))
            {
                // Inicializa array se não existir
                int size = _originalData.functions.Find(f => f.name == funcName).size;
                _engine.UserProgram[funcName] = new string[size];
            }

            _engine.UserProgram[funcName][index] = command;
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            // Reset state before playing
            var json = JsonConvert.SerializeObject(_originalData);
            _engine = new GameEngine(JsonConvert.DeserializeObject<LevelData>(json));
            // Restaura o programa do usuário (a engine nova está vazia)
            // (Numa implementação real, você separaria o programa do usuário do estado do level)
            RebuildEngineProgram();

            gameTimer.Start();
        }

        private void RebuildEngineProgram()
        {
            // Varre os botões da UI para reconstruir a lógica na engine nova
            foreach (Control c in pnlFunctions.Controls)
            {
                if (c is Button btn && btn.Tag is Tuple<string, int> info)
                {
                    // Lógica simplificada: recupera o que foi setado na engine anterior ou na UI
                    // Aqui você precisaria persistir o "UserProgram" fora da Engine que é resetada.
                }
            }
            // OBS: Para simplificar, assuma que a Engine não perde o UserProgram no Reset()
            // ou passe o UserProgram antigo para a nova Engine.
            _engine.UserProgram = CollectProgramFromUI();
        }

        private Dictionary<string, string[]> CollectProgramFromUI()
        {
            var prog = new Dictionary<string, string[]>();
            foreach (var func in _originalData.functions)
            {
                prog[func.name] = new string[func.size];
            }

            foreach (Control c in pnlFunctions.Controls)
            {
                if (c is Button btn && btn.Tag is Tuple<string, int> info)
                {
                    // Mapear de volta Símbolo -> Comando (ex: ⬆ -> FORWARD)
                    string text = btn.Text;
                    string cmd = "";
                    if (text == "⬆") cmd = "FORWARD";
                    else if (text == "↰") cmd = "TURN_LEFT";
                    else if (text == "↱") cmd = "TURN_RIGHT";
                    else if (text == "🎨") cmd = "PAINT_BLUE"; // Simplificação
                    else cmd = text; // F0, F1...

                    if (!string.IsNullOrEmpty(cmd))
                        prog[info.Item1][info.Item2] = cmd;
                }
            }
            return prog;
        }

        private void gameTimer_Tick(object sender, EventArgs e)
        {
            _engine.Tick();
        }

        // Desenho GDI+
        private void pbGrid_Paint(object sender, PaintEventArgs e)
        {
            if (_engine == null) return;
            Graphics g = e.Graphics;
            var matrix = _engine.Level.matrix;

            for (int r = 0; r < matrix.Count; r++)
            {
                for (int c = 0; c < matrix[r].Count; c++)
                {
                    var cell = matrix[r][c];
                    Rectangle rect = new Rectangle(c * _cellSize, r * _cellSize, _cellSize, _cellSize);

                    // Fundo
                    Brush bgBrush = Brushes.White;
                    if (cell.color == "blue") bgBrush = Brushes.LightBlue;
                    if (cell.color == "red") bgBrush = Brushes.LightPink;
                    if (cell.color == "green") bgBrush = Brushes.LightGreen;

                    g.FillRectangle(bgBrush, rect);
                    g.DrawRectangle(Pens.Gray, rect);

                    // Símbolos
                    if (cell.symbol == "star")
                    {
                        g.DrawString("★", new Font("Arial", 20), Brushes.Gold, rect, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
                    }
                }
            }

            // Desenhar Robô
            var p = _engine.Player;
            if (p != null)
            {
                Rectangle playerRect = new Rectangle(p.Col * _cellSize, p.Row * _cellSize, _cellSize, _cellSize);
                string arrow = "⬆";
                if (p.Dir == Direction.Right) arrow = "➡";
                if (p.Dir == Direction.Down) arrow = "⬇";
                if (p.Dir == Direction.Left) arrow = "⬅";

                g.DrawString(arrow, new Font("Arial", 24, FontStyle.Bold), Brushes.Black, playerRect, new StringFormat { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center });
            }
        }
    }
}
