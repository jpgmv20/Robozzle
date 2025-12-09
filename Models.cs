using System.Collections.Generic;

namespace RobozllueApp
{
    // Representa a linha da tabela SQL 'levels'
    public class LevelEntity
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Difficulty { get; set; } = string.Empty;
        public LevelData Data { get; set; } = new LevelData();
    }

    // --- Classes que espelham a estrutura interna do JSON ---

    public class LevelData
    {
        public string title { get; set; } = string.Empty;
        public string difficulty { get; set; } = "easy";
        public int grid_cell_size { get; set; } = 48;
        public List<List<CellData>> matrix { get; set; } = new List<List<CellData>>();
        public List<FunctionData> functions { get; set; } = new List<FunctionData>();
    }

    public class CellData
    {
        public string color { get; set; } = "none";
        public string symbol { get; set; } = "none";
        // Direção: 0=Up, 1=Right, 2=Down, 3=Left. Padrão 1 (Direita)
        public int direction { get; set; } = 1;
    }

    public class FunctionData
    {
        public string name { get; set; } = string.Empty;
        public int size { get; set; }
    }

    // Classe para representar um slot de comando na Engine
    public class CommandSlot
    {
        public string Action { get; set; } = "";
        public string ConditionColor { get; set; } = "none";

        public override string ToString()
        {
            string cond = ConditionColor == "none" ? "" : $"[{ConditionColor}] ";
            return $"{cond}{Action}";
        }
    }
}