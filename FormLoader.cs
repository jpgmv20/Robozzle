using System;
using System.Drawing;
using System.Windows.Forms;

namespace Robozzle
{
    public partial class FormLoader : Form
    {
        // Propriedade que os filhos (Login, Game, Register) vão usar
        // para dizer: "Pai, por favor, mantenha ESTE painel no centro".
        protected Control? PainelCentral { get; set; }

        public FormLoader()
        {
            InitializeComponent();

            // Configuração padrão para TODOS os forms
            this.WindowState = FormWindowState.Maximized;
            this.StartPosition = FormStartPosition.CenterScreen;

            // O pai escuta seu próprio redimensionamento
            this.Resize += FormLoader_Resize;
        }

        private void FormLoader_Resize(object? sender, EventArgs e)
        {
            // Se o filho definiu um painel para centralizar, nós centralizamos
            if (PainelCentral != null)
            {
                PainelCentral.Location = new Point(
                    (this.ClientSize.Width - PainelCentral.Width) / 2,
                    (this.ClientSize.Height - PainelCentral.Height) / 2
                );
            }
        }

        private void FormLoader_Load(object sender, EventArgs e)
        {
            // Força a centralização logo ao abrir
            FormLoader_Resize(this, EventArgs.Empty);
        }
    }
}