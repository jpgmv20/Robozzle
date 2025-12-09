using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel; // <--- OBRIGATÓRIO para corrigir o erro de propriedade

namespace Robozzle
{
    public enum MessageType
    {
        In,
        Out
    }

    public partial class MessageBubble : UserControl
    {
        // 1. MANTENHA este construtor vazio (O Visual Studio precisa dele para o Designer funcionar)
        public MessageBubble()
        {
            InitializeComponent();
        }

        // 2. ADICIONE este novo construtor que aceita 1 argumento (o texto)
        public MessageBubble(string message)
        {
            InitializeComponent(); // Obrigatório chamar isso sempre primeiro
            this.MessageText = message;
            this.TimeText = DateTime.Now.ToString("HH:mm"); // Pega a hora atual automático
            this.SetBubbleType(MessageType.Out); // Define um padrão (ex: mensagem enviada)
        }

        // 3. (OPCIONAL mas Recomendado) Adicione este para passar Texto e Tipo de uma vez
        public MessageBubble(string message, MessageType type)
        {
            InitializeComponent();
            this.MessageText = message;
            this.TimeText = DateTime.Now.ToString("HH:mm");
            this.SetBubbleType(type);
        }

        // --- CORREÇÃO: Atributos para o Visual Studio não dar erro ao salvar ---
        [Category("Custom Properties")]
        [Description("Texto da mensagem")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string MessageText
        {
            get { return lblMessage.Text; }
            set
            {
                lblMessage.Text = value;
                AdjustHeight();
            }
        }

        // --- CORREÇÃO: Atributos para o Visual Studio não dar erro ao salvar ---
        [Category("Custom Properties")]
        [Description("Hora da mensagem")]
        [Browsable(true)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string TimeText
        {
            get { return lblTime.Text; }
            set { lblTime.Text = value; }
        }

        public void SetBubbleType(MessageType type)
        {
            if (type == MessageType.Out)
            {
                this.Padding = new Padding(50, 5, 5, 5);
                panelBackground.BackColor = Color.LightGreen;
                panelBackground.Dock = DockStyle.Right;
                lblTime.TextAlign = ContentAlignment.MiddleRight;
            }
            else
            {
                this.Padding = new Padding(5, 5, 50, 5);
                panelBackground.BackColor = Color.White;
                panelBackground.Dock = DockStyle.Left;
                lblTime.TextAlign = ContentAlignment.MiddleLeft;
            }
        }

        private void AdjustHeight()
        {
            // Proteção: Só ajusta se os componentes já tiverem sido criados
            if (lblMessage != null && lblTime != null)
            {
                int novaAltura = lblMessage.Height + lblTime.Height + 30;
                this.Height = novaAltura;
            }
        }

        // Garante que o tamanho se ajusta assim que o componente é carregado
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            AdjustHeight();
        }
    }
}