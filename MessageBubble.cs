using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using RobozllueApp;

namespace Robozzle
{
    public enum MessageType
    {
        In,
        Out
    }

    public partial class MessageBubble : UserControl
    {
        // Construtor Vazio (Necessário para o Designer)
        public MessageBubble()
        {
            InitializeComponent();
        }

        // Construtor para Texto simples
        public MessageBubble(string message)
        {
            InitializeComponent();
            this.MessageText = message;
            this.TimeText = DateTime.Now.ToString("HH:mm");
            this.SetBubbleType(MessageType.Out);
        }

        // --- NOVO CONSTRUTOR: Resolve o erro do ChatForm ---
        public MessageBubble(ChatMessage msg)
        {
            InitializeComponent();
            this.MessageText = msg.Content;
            this.TimeText = msg.CreatedAt.ToString("HH:mm");

            // Define o lado e cor automaticamente
            if (msg.IsMine)
                SetBubbleType(MessageType.Out);
            else
                SetBubbleType(MessageType.In);
        }

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
            if (lblMessage != null && lblTime != null)
            {
                int novaAltura = lblMessage.Height + lblTime.Height + 30;
                this.Height = novaAltura;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            AdjustHeight();
        }
    }
}