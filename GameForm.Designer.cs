namespace Robozzle
{
    partial class GameForm
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
            this.components = new System.ComponentModel.Container();
            this.pnlGameContainer = new System.Windows.Forms.Panel();
            this.flpExecution = new System.Windows.Forms.FlowLayoutPanel();
            this.lblFunctionsTitle = new System.Windows.Forms.Label();
            this.lblPaletteTitle = new System.Windows.Forms.Label();
            this.lblQueue = new System.Windows.Forms.Label();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnPlay = new System.Windows.Forms.Button();
            this.pnlPalette = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlFunctions = new System.Windows.Forms.Panel();
            this.pbGrid = new System.Windows.Forms.PictureBox();
            this.gameTimer = new System.Windows.Forms.Timer(this.components);
            this.pnlGameContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlGameContainer
            // 
            this.pnlGameContainer.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlGameContainer.Controls.Add(this.flpExecution);
            this.pnlGameContainer.Controls.Add(this.lblFunctionsTitle);
            this.pnlGameContainer.Controls.Add(this.lblPaletteTitle);
            this.pnlGameContainer.Controls.Add(this.lblQueue);
            this.pnlGameContainer.Controls.Add(this.btnReset);
            this.pnlGameContainer.Controls.Add(this.btnPlay);
            this.pnlGameContainer.Controls.Add(this.pnlPalette);
            this.pnlGameContainer.Controls.Add(this.pnlFunctions);
            this.pnlGameContainer.Controls.Add(this.pbGrid);
            this.pnlGameContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlGameContainer.Location = new System.Drawing.Point(0, 0);
            this.pnlGameContainer.Name = "pnlGameContainer";
            this.pnlGameContainer.Size = new System.Drawing.Size(1000, 700);
            this.pnlGameContainer.TabIndex = 0;
            // 
            // flpExecution
            // 
            this.flpExecution.AutoScroll = true;
            this.flpExecution.BackColor = System.Drawing.Color.White;
            this.flpExecution.Location = new System.Drawing.Point(30, 350);
            this.flpExecution.Name = "flpExecution";
            this.flpExecution.Size = new System.Drawing.Size(700, 50);
            this.flpExecution.TabIndex = 9;
            this.flpExecution.WrapContents = false;
            // 
            // lblFunctionsTitle
            // 
            this.lblFunctionsTitle.AutoSize = true;
            this.lblFunctionsTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblFunctionsTitle.ForeColor = System.Drawing.Color.DimGray;
            this.lblFunctionsTitle.Location = new System.Drawing.Point(260, 430);
            this.lblFunctionsTitle.Name = "lblFunctionsTitle";
            this.lblFunctionsTitle.Size = new System.Drawing.Size(61, 15);
            this.lblFunctionsTitle.TabIndex = 8;
            this.lblFunctionsTitle.Text = "FUNÇÕES";
            // 
            // lblPaletteTitle
            // 
            this.lblPaletteTitle.AutoSize = true;
            this.lblPaletteTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblPaletteTitle.ForeColor = System.Drawing.Color.DimGray;
            this.lblPaletteTitle.Location = new System.Drawing.Point(30, 430);
            this.lblPaletteTitle.Name = "lblPaletteTitle";
            this.lblPaletteTitle.Size = new System.Drawing.Size(78, 15);
            this.lblPaletteTitle.TabIndex = 7;
            this.lblPaletteTitle.Text = "COMANDOS";
            // 
            // lblQueue
            // 
            this.lblQueue.AutoSize = true;
            this.lblQueue.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblQueue.ForeColor = System.Drawing.Color.DimGray;
            this.lblQueue.Location = new System.Drawing.Point(30, 330);
            this.lblQueue.Name = "lblQueue";
            this.lblQueue.Size = new System.Drawing.Size(95, 15);
            this.lblQueue.TabIndex = 6;
            this.lblQueue.Text = "EM EXECUÇÃO:";
            // 
            // btnReset
            // 
            this.btnReset.BackColor = System.Drawing.Color.IndianRed;
            this.btnReset.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnReset.FlatAppearance.BorderSize = 0;
            this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReset.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnReset.ForeColor = System.Drawing.Color.White;
            this.btnReset.Location = new System.Drawing.Point(860, 350);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(100, 50);
            this.btnReset.TabIndex = 4;
            this.btnReset.Text = "RESET ⟲";
            this.btnReset.UseVisualStyleBackColor = false;
            // 
            // btnPlay
            // 
            this.btnPlay.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.btnPlay.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPlay.FlatAppearance.BorderSize = 0;
            this.btnPlay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPlay.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.btnPlay.ForeColor = System.Drawing.Color.White;
            this.btnPlay.Location = new System.Drawing.Point(750, 350);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.Size = new System.Drawing.Size(100, 50);
            this.btnPlay.TabIndex = 3;
            this.btnPlay.Text = "PLAY ▶";
            this.btnPlay.UseVisualStyleBackColor = false;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // pnlPalette
            // 
            this.pnlPalette.BackColor = System.Drawing.Color.Transparent;
            this.pnlPalette.Location = new System.Drawing.Point(30, 450);
            this.pnlPalette.Name = "pnlPalette";
            this.pnlPalette.Size = new System.Drawing.Size(200, 200);
            this.pnlPalette.TabIndex = 2;
            // 
            // pnlFunctions
            // 
            this.pnlFunctions.AutoScroll = true;
            this.pnlFunctions.BackColor = System.Drawing.Color.Transparent;
            this.pnlFunctions.Location = new System.Drawing.Point(260, 450);
            this.pnlFunctions.Name = "pnlFunctions";
            this.pnlFunctions.Size = new System.Drawing.Size(700, 200);
            this.pnlFunctions.TabIndex = 1;
            // 
            // pbGrid
            // 
            this.pbGrid.BackColor = System.Drawing.Color.White;
            this.pbGrid.Location = new System.Drawing.Point(30, 20);
            this.pbGrid.Name = "pbGrid";
            this.pbGrid.Size = new System.Drawing.Size(940, 300);
            this.pbGrid.TabIndex = 0;
            this.pbGrid.TabStop = false;
            this.pbGrid.Paint += new System.Windows.Forms.PaintEventHandler(this.pbGrid_Paint);
            // 
            // gameTimer
            // 
            this.gameTimer.Interval = 500;
            this.gameTimer.Tick += new System.EventHandler(this.gameTimer_Tick);
            // 
            // GameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1000, 700);
            this.Controls.Add(this.pnlGameContainer);
            this.Name = "GameForm";
            this.Text = "Robozzle Game";
            this.Resize += new System.EventHandler(this.GameForm_Resize);
            this.pnlGameContainer.ResumeLayout(false);
            this.pnlGameContainer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbGrid)).EndInit();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel pnlGameContainer;
        private System.Windows.Forms.PictureBox pbGrid;
        private System.Windows.Forms.Panel pnlFunctions;
        private System.Windows.Forms.FlowLayoutPanel pnlPalette;
        private System.Windows.Forms.Button btnPlay;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Timer gameTimer;
        private System.Windows.Forms.FlowLayoutPanel flpExecution;
        private System.Windows.Forms.Label lblQueue;
        private System.Windows.Forms.Label lblPaletteTitle;
        private System.Windows.Forms.Label lblFunctionsTitle;
        #endregion
    }
}