using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace RobozllueApp
{
    public enum Direction { Up = 0, Right = 1, Down = 2, Left = 3 }

    public class Robot
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public Direction Dir { get; set; }
    }

    public class GameEngine
    {
        public LevelData Level { get; private set; }
        public Robot Player { get; private set; }
        public int StarsCollected { get; private set; }
        public int TotalStars { get; private set; }

        // Estado de Execução
        private Stack<string> _commandStack; // Pilha de comandos a executar
        private int _stepsTaken;
        private const int MAX_STEPS = 1000;

        // Definição das funções montadas pelo usuário (F0: [Fwd, Right, F1...])
        public Dictionary<string, string[]> UserProgram { get; set; }

        public event Action OnVictory;
        public event Action OnDefeat;
        public event Action OnStep; // Para atualizar a UI

        public GameEngine(LevelData levelData)
        {
            Level = levelData;
            UserProgram = new Dictionary<string, string[]>();
            Reset();
        }

        public void Reset()
        {
            _commandStack = new Stack<string>();
            _stepsTaken = 0;
            StarsCollected = 0;
            TotalStars = 0;

            // Encontrar posição inicial e contar estrelas
            for (int r = 0; r < Level.matrix.Count; r++)
            {
                for (int c = 0; c < Level.matrix[r].Count; c++)
                {
                    var cell = Level.matrix[r][c];
                    if (cell.symbol == "play" || cell.symbol == "player")
                    {
                        Player = new Robot { Row = r, Col = c, Dir = Direction.Right }; // Default Right
                    }
                    if (cell.symbol == "star") TotalStars++;
                }
            }

            // Inicia chamando a Main (F0) se existir
            if (UserProgram.ContainsKey("F0"))
            {
                LoadFunctionToStack("F0");
            }
        }

        // Adiciona os comandos de uma função à pilha (na ordem inversa para execução correta)
        private void LoadFunctionToStack(string funcName)
        {
            if (!UserProgram.ContainsKey(funcName)) return;

            string[] commands = UserProgram[funcName];

            // Empilhamos de trás para frente para que o índice 0 seja o primeiro a sair (Pop)
            for (int i = commands.Length - 1; i >= 0; i--)
            {
                if (!string.IsNullOrEmpty(commands[i]))
                    _commandStack.Push(commands[i]);
            }
        }

        public void Tick()
        {
            if (_commandStack.Count == 0) return; // Nada a fazer
            if (_stepsTaken >= MAX_STEPS) { OnDefeat?.Invoke(); return; }

            string cmd = _commandStack.Pop();
            _stepsTaken++;

            ExecuteCommand(cmd);
            OnStep?.Invoke();

            // Verifica vitória
            if (StarsCollected == TotalStars)
            {
                OnVictory?.Invoke();
                _commandStack.Clear(); // Para execução
            }
        }

        private void ExecuteCommand(string cmd)
        {
            switch (cmd)
            {
                case "FORWARD": MoveForward(); break;
                case "TURN_LEFT":
                    Player.Dir = (Direction)(((int)Player.Dir + 3) % 4);
                    break;
                case "TURN_RIGHT":
                    Player.Dir = (Direction)(((int)Player.Dir + 1) % 4);
                    break;
                case "PAINT_BLUE": PaintCurrent("blue"); break;
                case "PAINT_GREEN": PaintCurrent("green"); break;
                case "PAINT_RED": PaintCurrent("red"); break;
                default:
                    // Verifica se é chamada de função (ex: CALL_F0, F0)
                    if (UserProgram.ContainsKey(cmd))
                    {
                        LoadFunctionToStack(cmd);
                    }
                    else if (cmd.StartsWith("F")) // Caso venha apenas F1
                    {
                        LoadFunctionToStack(cmd);
                    }
                    break;
            }
        }

        private void MoveForward()
        {
            int dr = 0, dc = 0;
            switch (Player.Dir)
            {
                case Direction.Up: dr = -1; break;
                case Direction.Right: dc = 1; break;
                case Direction.Down: dr = 1; break;
                case Direction.Left: dc = -1; break;
            }

            int nextR = Player.Row + dr;
            int nextC = Player.Col + dc;

            // Valida limites
            if (nextR >= 0 && nextR < Level.matrix.Count &&
                nextC >= 0 && nextC < Level.matrix[0].Count)
            {
                var cell = Level.matrix[nextR][nextC];
                // Lógica simples: se for cor 'none' e não tiver estrela, cai no buraco?
                // No Robozzle original, se sair da cor ou cair no vazio, morre.
                // Aqui vamos simplificar: só anda se não for 'none' ou se tiver estrela

                if (cell.color != "none" || cell.symbol == "star" || cell.symbol == "play")
                {
                    Player.Row = nextR;
                    Player.Col = nextC;
                    CheckCellInteraction();
                }
                else
                {
                    OnDefeat?.Invoke(); // Caiu no vazio
                }
            }
            else
            {
                OnDefeat?.Invoke(); // Saiu do mapa
            }
        }

        private void PaintCurrent(string color)
        {
            // Pinta a célula atual
            Level.matrix[Player.Row][Player.Col].color = color;
        }

        private void CheckCellInteraction()
        {
            var cell = Level.matrix[Player.Row][Player.Col];
            if (cell.symbol == "star")
            {
                StarsCollected++;
                cell.symbol = "none"; // Remove estrela coletada
            }
        }
    }
}