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
            components = new System.ComponentModel.Container();
            pnlGameContainer = new Panel();
            flpExecution = new FlowLayoutPanel();
            lblFunctionsTitle = new Label();
            lblPaletteTitle = new Label();
            lblQueue = new Label();
            btnReset = new Button();
            btnPlay = new Button();
            pnlPalette = new FlowLayoutPanel();
            pnlFunctions = new Panel();
            pbGrid = new PictureBox();
            gameTimer = new System.Windows.Forms.Timer(components);
            pnlGameContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pbGrid).BeginInit();
            SuspendLayout();
            // 
            // pnlGameContainer
            // 
            pnlGameContainer.BackColor = Color.WhiteSmoke;
            pnlGameContainer.Controls.Add(flpExecution);
            pnlGameContainer.Controls.Add(lblFunctionsTitle);
            pnlGameContainer.Controls.Add(lblPaletteTitle);
            pnlGameContainer.Controls.Add(lblQueue);
            pnlGameContainer.Controls.Add(btnReset);
            pnlGameContainer.Controls.Add(btnPlay);
            pnlGameContainer.Controls.Add(pnlPalette);
            pnlGameContainer.Controls.Add(pnlFunctions);
            pnlGameContainer.Controls.Add(pbGrid);
            pnlGameContainer.Dock = DockStyle.Fill;
            pnlGameContainer.Location = new Point(0, 0);
            pnlGameContainer.Name = "pnlGameContainer";
            pnlGameContainer.Size = new Size(1000, 700);
            pnlGameContainer.TabIndex = 0;
            // 
            // flpExecution
            // 
            flpExecution.AutoScroll = true;
            flpExecution.BackColor = Color.White;
            flpExecution.Location = new Point(30, 350);
            flpExecution.Name = "flpExecution";
            flpExecution.Size = new Size(700, 50);
            flpExecution.TabIndex = 9;
            flpExecution.WrapContents = false;
            // 
            // lblFunctionsTitle
            // 
            lblFunctionsTitle.AutoSize = true;
            lblFunctionsTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblFunctionsTitle.ForeColor = Color.DimGray;
            lblFunctionsTitle.Location = new Point(260, 430);
            lblFunctionsTitle.Name = "lblFunctionsTitle";
            lblFunctionsTitle.Size = new Size(60, 15);
            lblFunctionsTitle.TabIndex = 8;
            lblFunctionsTitle.Text = "FUNÇÕES";
            // 
            // lblPaletteTitle
            // 
            lblPaletteTitle.AutoSize = true;
            lblPaletteTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblPaletteTitle.ForeColor = Color.DimGray;
            lblPaletteTitle.Location = new Point(30, 430);
            lblPaletteTitle.Name = "lblPaletteTitle";
            lblPaletteTitle.Size = new Size(76, 15);
            lblPaletteTitle.TabIndex = 7;
            lblPaletteTitle.Text = "COMANDOS";
            // 
            // lblQueue
            // 
            lblQueue.AutoSize = true;
            lblQueue.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            lblQueue.ForeColor = Color.DimGray;
            lblQueue.Location = new Point(30, 330);
            lblQueue.Name = "lblQueue";
            lblQueue.Size = new Size(90, 15);
            lblQueue.TabIndex = 6;
            lblQueue.Text = "EM EXECUÇÃO:";
            // 
            // btnReset
            // 
            btnReset.BackColor = Color.IndianRed;
            btnReset.Cursor = Cursors.Hand;
            btnReset.FlatAppearance.BorderSize = 0;
            btnReset.FlatStyle = FlatStyle.Flat;
            btnReset.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            btnReset.ForeColor = Color.White;
            btnReset.Location = new Point(860, 350);
            btnReset.Name = "btnReset";
            btnReset.Size = new Size(100, 50);
            btnReset.TabIndex = 4;
            btnReset.Text = "RESET ⟲";
            btnReset.UseVisualStyleBackColor = false;
            // 
            // btnPlay
            // 
            btnPlay.BackColor = Color.MediumSeaGreen;
            btnPlay.Cursor = Cursors.Hand;
            btnPlay.FlatAppearance.BorderSize = 0;
            btnPlay.FlatStyle = FlatStyle.Flat;
            btnPlay.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            btnPlay.ForeColor = Color.White;
            btnPlay.Location = new Point(750, 350);
            btnPlay.Name = "btnPlay";
            btnPlay.Size = new Size(100, 50);
            btnPlay.TabIndex = 3;
            btnPlay.Text = "PLAY ▶";
            btnPlay.UseVisualStyleBackColor = false;
            btnPlay.Click += btnPlay_Click;
            // 
            // pnlPalette
            // 
            pnlPalette.BackColor = Color.Transparent;
            pnlPalette.Location = new Point(30, 450);
            pnlPalette.Name = "pnlPalette";
            pnlPalette.Size = new Size(200, 200);
            pnlPalette.TabIndex = 2;
            // 
            // pnlFunctions
            // 
            pnlFunctions.AutoScroll = true;
            pnlFunctions.BackColor = Color.Transparent;
            pnlFunctions.Location = new Point(260, 450);
            pnlFunctions.Name = "pnlFunctions";
            pnlFunctions.Size = new Size(700, 200);
            pnlFunctions.TabIndex = 1;
            // 
            // pbGrid
            // 
            pbGrid.BackColor = Color.White;
            pbGrid.Location = new Point(30, 20);
            pbGrid.Name = "pbGrid";
            pbGrid.Size = new Size(940, 300);
            pbGrid.TabIndex = 0;
            pbGrid.TabStop = false;
            pbGrid.Paint += pbGrid_Paint;
            // 
            // gameTimer
            // 
            gameTimer.Interval = 200;
            gameTimer.Tick += gameTimer_Tick;
            // 
            // GameForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1000, 700);
            Controls.Add(pnlGameContainer);
            Name = "GameForm";
            Text = "Robozzle Game";
            Resize += GameForm_Resize;
            pnlGameContainer.ResumeLayout(false);
            pnlGameContainer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pbGrid).EndInit();
            ResumeLayout(false);
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