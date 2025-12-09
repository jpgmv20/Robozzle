using System.Collections.Generic;

namespace RobozllueApp
{
    // Representa a linha da tabela SQL 'levels'
    public class LevelEntity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Difficulty { get; set; }
        public LevelData Data { get; set; } // Aqui ficará o JSON já convertido
    }

    // --- Classes que espelham a estrutura interna do JSON ---

    // Raiz do JSON
    public class LevelData
    {
        public string title { get; set; }
        public string difficulty { get; set; }
        public int grid_cell_size { get; set; } // Note que o nome deve ser igual ao do JSON
        public List<List<CellData>> matrix { get; set; }
        public List<FunctionData> functions { get; set; }
    }

    // Itens da Matriz
    public class CellData
    {
        public string color { get; set; } = "none";
        public string symbol { get; set; } = "none";
    }

    // Itens das Funções
    public class FunctionData
    {
        public string name { get; set; }
        public int size { get; set; }
    }
}