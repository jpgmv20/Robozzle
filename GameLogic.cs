using System;
using System.Collections.Generic;
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

        public Dictionary<string, CommandSlot[]> UserProgram { get; set; }

        public event Action? OnVictory;
        public event Action? OnDefeat;
        public event Action? OnStep;

        private Stack<CommandSlot> _commandStack;

        // --- CORREÇÃO AQUI: Propriedade Pública ---
        public int StepsTaken { get; private set; }
        // ------------------------------------------

        private const int MAX_STEPS = 2000;

        public GameEngine(LevelData levelData)
        {
            Level = levelData;
            UserProgram = new Dictionary<string, CommandSlot[]>();
            _commandStack = new Stack<CommandSlot>();

            if (Level.functions != null)
            {
                foreach (var fn in Level.functions)
                {
                    UserProgram[fn.name] = new CommandSlot[fn.size];
                    for (int i = 0; i < fn.size; i++) UserProgram[fn.name][i] = new CommandSlot();
                }
            }

            Reset();
        }

        public void Reset()
        {
            _commandStack = new Stack<CommandSlot>();
            StepsTaken = 0; // Alterado para a propriedade
            StarsCollected = 0;
            TotalStars = 0;

            if (Level.matrix == null) return;

            for (int r = 0; r < Level.matrix.Count; r++)
            {
                for (int c = 0; c < Level.matrix[r].Count; c++)
                {
                    var cell = Level.matrix[r][c];

                    if (cell.symbol == "star") TotalStars++;

                    if (cell.symbol == "play" || cell.symbol == "player")
                    {
                        int safeDir = Math.Max(0, Math.Min(3, cell.direction));
                        Player = new Robot { Row = r, Col = c, Dir = (Direction)safeDir };
                    }
                }
            }

            if (Player == null) Player = new Robot { Row = 0, Col = 0, Dir = Direction.Right };

            if (UserProgram.ContainsKey("F0"))
            {
                PushFunctionToStack("F0");
            }
        }

        private void PushFunctionToStack(string funcName)
        {
            if (!UserProgram.ContainsKey(funcName)) return;

            CommandSlot[] cmds = UserProgram[funcName];

            for (int i = cmds.Length - 1; i >= 0; i--)
            {
                var slot = cmds[i];
                if (!string.IsNullOrEmpty(slot.Action))
                {
                    _commandStack.Push(new CommandSlot { Action = slot.Action, ConditionColor = slot.ConditionColor });
                }
            }
        }

        public void Tick()
        {
            if (_commandStack.Count == 0) return;
            if (StepsTaken >= MAX_STEPS) { OnDefeat?.Invoke(); return; } // Alterado

            CommandSlot cmd = _commandStack.Pop();

            if (cmd.ConditionColor != "none")
            {
                var currentFloorColor = "none";
                if (Player.Row < Level.matrix.Count && Player.Col < Level.matrix[0].Count)
                {
                    currentFloorColor = Level.matrix[Player.Row][Player.Col].color;
                }

                if (!string.Equals(currentFloorColor, cmd.ConditionColor, StringComparison.OrdinalIgnoreCase))
                {
                    OnStep?.Invoke();
                    return;
                }
            }

            StepsTaken++; // Alterado
            ExecuteCommand(cmd);
            OnStep?.Invoke();

            if (StarsCollected == TotalStars && TotalStars > 0)
            {
                OnVictory?.Invoke();
                _commandStack.Clear();
            }
        }

        private void ExecuteCommand(CommandSlot slot)
        {
            string action = slot.Action;

            switch (action)
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
                    if (UserProgram.ContainsKey(action))
                        PushFunctionToStack(action);
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

            if (nextR >= 0 && nextR < Level.matrix.Count &&
                nextC >= 0 && nextC < Level.matrix[0].Count)
            {
                var cell = Level.matrix[nextR][nextC];

                if (cell.color == "none" && cell.symbol != "star" && cell.symbol != "play" && cell.symbol != "player")
                {
                    OnDefeat?.Invoke();
                }
                else
                {
                    Player.Row = nextR;
                    Player.Col = nextC;

                    if (cell.symbol == "star")
                    {
                        StarsCollected++;
                        cell.symbol = "none";
                    }
                }
            }
            else
            {
                OnDefeat?.Invoke();
            }
        }

        private void PaintCurrent(string color)
        {
            if (Player.Row < Level.matrix.Count && Player.Col < Level.matrix[0].Count)
                Level.matrix[Player.Row][Player.Col].color = color;
        }

        public List<CommandSlot> GetNextCommandsPreview(int count)
        {
            if (_commandStack == null) return new List<CommandSlot>();
            return _commandStack.Take(count).Select(c => new CommandSlot { Action = c.Action, ConditionColor = c.ConditionColor }).ToList();
        }
    }
}