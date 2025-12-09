using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
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
        private MessageType _type;
        private Color _bubbleColor;

        public MessageBubble()
        {
            InitializeComponent();
        }

        public MessageBubble(ChatMessage msg)
        {
            InitializeComponent();
            this.DoubleBuffered = true;
            this.ResizeRedraw = true;

            this.lblMessage.Text = msg.Content;
            this.lblTime.Text = msg.CreatedAt.ToString("HH:mm");

            // Configuração de cores e alinhamento
            if (msg.IsMine)
            {
                _type = MessageType.Out;
                _bubbleColor = Color.FromArgb(220, 248, 198); // Verde suave

                // Força texto preto e hora cinza
                lblMessage.ForeColor = Color.Black;
                lblTime.ForeColor = Color.DimGray;

                this.Padding = new Padding(50, 5, 5, 5);
                panelBackground.Dock = DockStyle.Right;
                lblTime.TextAlign = ContentAlignment.BottomRight;
            }
            else
            {
                _type = MessageType.In;
                _bubbleColor = Color.White;

                // Força texto preto e hora cinza
                lblMessage.ForeColor = Color.Black;
                lblTime.ForeColor = Color.Gray;

                this.Padding = new Padding(5, 5, 50, 5);
                panelBackground.Dock = DockStyle.Left;
                lblTime.TextAlign = ContentAlignment.BottomLeft;
            }

            // Remove estilos padrão
            panelBackground.BackColor = Color.Transparent;
            lblMessage.BackColor = Color.Transparent;
            lblTime.BackColor = Color.Transparent;
            panelBackground.BorderStyle = BorderStyle.None;

            panelBackground.Paint += PanelBackground_Paint;
        }

        private void PanelBackground_Paint(object? sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            g.SmoothingMode = SmoothingMode.AntiAlias;

            Rectangle rect = new Rectangle(0, 0, panelBackground.Width - 1, panelBackground.Height - 1);
            int radius = 15;

            using (GraphicsPath path = GraphicsUtils.CreateRoundedRectanglePath(rect, radius))
            using (SolidBrush brush = new SolidBrush(_bubbleColor))
            using (Pen pen = new Pen(Color.FromArgb(210, 210, 210), 1))
            {
                g.FillPath(brush, path);

                // Borda apenas nas mensagens recebidas
                if (_type == MessageType.In)
                {
                    g.DrawPath(pen, path);
                }
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            AdjustHeight();
        }

        private void AdjustHeight()
        {
            // Calcula altura baseada no texto
            Size size = TextRenderer.MeasureText(lblMessage.Text, lblMessage.Font, new Size(lblMessage.Width, 0), TextFormatFlags.WordBreak);

            // --- AJUSTE DE ESPAÇAMENTO AQUI ---
            // Aumentei de 30 para 45. Isso empurra o Dock=Bottom (hora) mais para baixo,
            // criando um vão maior entre o texto e a hora.
            this.Height = size.Height + lblTime.Height + 45;
        }
    }
}