namespace Robozzle
{
    partial class MessageBubble
    {
        /// <summary> 
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Designer de Componentes

        /// <summary> 
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            panelBackground = new Panel();
            lblMessage = new Label();
            lblTime = new Label();
            panelBackground.SuspendLayout();
            SuspendLayout();
            // 
            // panelBackground
            // 
            panelBackground.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            panelBackground.BackColor = Color.WhiteSmoke;
            panelBackground.Controls.Add(lblMessage);
            panelBackground.Controls.Add(lblTime);
            panelBackground.Location = new Point(3, 3);
            panelBackground.Name = "panelBackground";
            panelBackground.Padding = new Padding(10);
            panelBackground.Size = new Size(268, 65);
            panelBackground.TabIndex = 0;
            // 
            // lblMessage
            // 
            lblMessage.AutoSize = true;
            lblMessage.Dock = DockStyle.Top;
            lblMessage.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblMessage.Location = new Point(10, 10);
            lblMessage.MaximumSize = new Size(250, 0);
            lblMessage.Name = "lblMessage";
            lblMessage.Size = new Size(76, 19);
            lblMessage.TabIndex = 0;
            lblMessage.Text = "Mensagem";
            // 
            // lblTime
            // 
            lblTime.AutoSize = true;
            lblTime.Dock = DockStyle.Bottom;
            lblTime.Font = new Font("Segoe UI", 7F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lblTime.ForeColor = Color.Gray;
            lblTime.Location = new Point(10, 43);
            lblTime.Name = "lblTime";
            lblTime.Size = new Size(29, 12);
            lblTime.TabIndex = 1;
            lblTime.Text = "00:00";
            lblTime.TextAlign = ContentAlignment.BottomRight;
            // 
            // MessageBubble
            // 
            AutoScaleDimensions = new SizeF(6F, 13F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.Transparent;
            Controls.Add(panelBackground);
            Name = "MessageBubble";
            Size = new Size(274, 71);
            panelBackground.ResumeLayout(false);
            panelBackground.PerformLayout();
            ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelBackground;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.Label lblTime;
    }
}