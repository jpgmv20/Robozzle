using System.Drawing;
using System.Drawing.Drawing2D;

namespace Robozzle
{
    public static class GraphicsUtils
    {
        /// <summary>
        /// Cria um caminho gráfico (GraphicsPath) representando um retângulo com cantos arredondados.
        /// </summary>
        /// <param name="rect">O retângulo base.</param>
        /// <param name="radius">O raio do arredondamento dos cantos.</param>
        /// <returns>Um objeto GraphicsPath pronto para ser desenhado ou preenchido.</returns>
        public static GraphicsPath CreateRoundedRectanglePath(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int d = radius * 2; // Diâmetro

            // Garante que o diâmetro não seja maior que o próprio retângulo
            if (d > rect.Width) d = rect.Width;
            if (d > rect.Height) d = rect.Height;

            // Define os 4 arcos dos cantos e as linhas que os conectam
            path.StartFigure();
            path.AddArc(rect.X, rect.Y, d, d, 180, 90); // Canto Superior Esquerdo
            path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90); // Canto Superior Direito
            path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90); // Canto Inferior Direito
            path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90); // Canto Inferior Esquerdo
            path.CloseFigure();

            return path;
        }
    }
}