namespace Robozzle
{
    partial class HomeForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.pnlContainer = new System.Windows.Forms.FlowLayoutPanel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnCreateLevel = new System.Windows.Forms.Button(); // Novo botão
            this.SuspendLayout();
            // 
            // pnlContainer
            // 
            this.pnlContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlContainer.AutoScroll = true;
            this.pnlContainer.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlContainer.Location = new System.Drawing.Point(0, 80); // Ajustado Y para 80 para caber o botão
            this.pnlContainer.Name = "pnlContainer";
            this.pnlContainer.Padding = new System.Windows.Forms.Padding(20);
            this.pnlContainer.Size = new System.Drawing.Size(800, 370);
            this.pnlContainer.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.DimGray;
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(267, 37);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "Selecione uma Fase";
            // 
            // btnCreateLevel (NOVO BOTÃO)
            // 
            this.btnCreateLevel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCreateLevel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215))))); // Azul
            this.btnCreateLevel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCreateLevel.FlatAppearance.BorderSize = 0;
            this.btnCreateLevel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCreateLevel.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnCreateLevel.ForeColor = System.Drawing.Color.White;
            this.btnCreateLevel.Location = new System.Drawing.Point(620, 20);
            this.btnCreateLevel.Name = "btnCreateLevel";
            this.btnCreateLevel.Size = new System.Drawing.Size(150, 40);
            this.btnCreateLevel.TabIndex = 2;
            this.btnCreateLevel.Text = "+ CRIAR FASE";
            this.btnCreateLevel.UseVisualStyleBackColor = false;
            this.btnCreateLevel.Click += new System.EventHandler(this.btnCreateLevel_Click);
            // 
            // HomeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnCreateLevel); // Adiciona ao form
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.pnlContainer);
            this.Name = "HomeForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Robozzle - Home";
            this.Load += new System.EventHandler(this.HomeForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel pnlContainer;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnCreateLevel; // Declaração
    }
}