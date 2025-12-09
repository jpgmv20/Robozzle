namespace Robozzle
{
    partial class LevelEditorForm
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing) { if (disposing && (components != null)) components.Dispose(); base.Dispose(disposing); }

        private void InitializeComponent()
        {
            this.pnlEditorContainer = new System.Windows.Forms.Panel();
            this.pbGrid = new System.Windows.Forms.PictureBox();
            this.panelSidebar = new System.Windows.Forms.Panel();
            this.btnSave = new System.Windows.Forms.Button();
            this.groupBoxFuncs = new System.Windows.Forms.GroupBox();
            // --- NOVOS CONTROLES ---
            this.numFuncSize = new System.Windows.Forms.NumericUpDown();
            this.lblFuncSize = new System.Windows.Forms.Label();
            // -----------------------
            this.lstFunctions = new System.Windows.Forms.ListBox();
            this.btnRemFunc = new System.Windows.Forms.Button();
            this.btnAddFunc = new System.Windows.Forms.Button();
            this.groupBoxGrid = new System.Windows.Forms.GroupBox();
            this.btnResize = new System.Windows.Forms.Button();
            this.numCols = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.numRows = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBoxTools = new System.Windows.Forms.GroupBox();
            this.flowTools = new System.Windows.Forms.FlowLayoutPanel();
            this.lblSelectedTool = new System.Windows.Forms.Label();
            this.groupBoxProps = new System.Windows.Forms.GroupBox();
            this.cmbDifficulty = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtDesc = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbGrid)).BeginInit();
            this.pnlEditorContainer.SuspendLayout();
            this.panelSidebar.SuspendLayout();
            this.groupBoxFuncs.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numFuncSize)).BeginInit();
            this.groupBoxGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCols)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRows)).BeginInit();
            this.groupBoxTools.SuspendLayout();
            this.groupBoxProps.SuspendLayout();
            this.SuspendLayout();

            // Container
            this.pnlEditorContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlEditorContainer.Controls.Add(this.pbGrid);
            this.pnlEditorContainer.Controls.Add(this.panelSidebar);

            // Sidebar
            this.panelSidebar.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelSidebar.Width = 300;
            this.panelSidebar.BackColor = System.Drawing.Color.WhiteSmoke;
            this.panelSidebar.Padding = new System.Windows.Forms.Padding(10);
            this.panelSidebar.AutoScroll = true;
            this.panelSidebar.Controls.Add(this.btnSave);
            this.panelSidebar.Controls.Add(this.groupBoxFuncs);
            this.panelSidebar.Controls.Add(this.groupBoxGrid);
            this.panelSidebar.Controls.Add(this.groupBoxTools);
            this.panelSidebar.Controls.Add(this.groupBoxProps);

            // Grid
            this.pbGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pbGrid.BackColor = System.Drawing.Color.White;
            this.pbGrid.Paint += new System.Windows.Forms.PaintEventHandler(this.pbGrid_Paint);
            this.pbGrid.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pbGrid_MouseClick);

            // Group Props
            this.groupBoxProps.Text = "Propriedades";
            this.groupBoxProps.Height = 180;
            this.groupBoxProps.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxProps.Controls.Add(this.cmbDifficulty); this.groupBoxProps.Controls.Add(this.label3);
            this.groupBoxProps.Controls.Add(this.txtDesc); this.groupBoxProps.Controls.Add(this.label2);
            this.groupBoxProps.Controls.Add(this.txtTitle); this.groupBoxProps.Controls.Add(this.label1);

            this.label1.Text = "Título:"; this.label1.Location = new System.Drawing.Point(10, 25);
            this.txtTitle.Location = new System.Drawing.Point(10, 45); this.txtTitle.Width = 260;
            this.label2.Text = "Descrição:"; this.label2.Location = new System.Drawing.Point(10, 75);
            this.txtDesc.Location = new System.Drawing.Point(10, 95); this.txtDesc.Width = 260;
            this.label3.Text = "Dificuldade:"; this.label3.Location = new System.Drawing.Point(10, 125);
            this.cmbDifficulty.Location = new System.Drawing.Point(10, 145); this.cmbDifficulty.Width = 260;

            // Group Tools
            this.groupBoxTools.Text = "Ferramentas";
            this.groupBoxTools.Height = 180;
            this.groupBoxTools.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxTools.Controls.Add(this.flowTools);
            this.groupBoxTools.Controls.Add(this.lblSelectedTool);
            this.lblSelectedTool.Text = "Selecione..."; this.lblSelectedTool.Dock = System.Windows.Forms.DockStyle.Top;
            this.flowTools.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowTools.AutoScroll = true;

            // Group Grid
            this.groupBoxGrid.Text = "Grade";
            this.groupBoxGrid.Height = 60;
            this.groupBoxGrid.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxGrid.Controls.Add(this.btnResize);
            this.groupBoxGrid.Controls.Add(this.numCols); this.groupBoxGrid.Controls.Add(this.label5);
            this.groupBoxGrid.Controls.Add(this.numRows); this.groupBoxGrid.Controls.Add(this.label4);
            this.label4.Text = "L:"; this.label4.Location = new System.Drawing.Point(10, 25); this.label4.AutoSize = true;
            this.numRows.Location = new System.Drawing.Point(30, 22); this.numRows.Width = 45; this.numRows.Minimum = 5; this.numRows.Maximum = 30;
            this.label5.Text = "C:"; this.label5.Location = new System.Drawing.Point(85, 25); this.label5.AutoSize = true;
            this.numCols.Location = new System.Drawing.Point(105, 22); this.numCols.Width = 45; this.numCols.Minimum = 5; this.numCols.Maximum = 30;
            this.btnResize.Text = "OK"; this.btnResize.Location = new System.Drawing.Point(160, 20); this.btnResize.Width = 40; this.btnResize.Click += new System.EventHandler(this.btnResize_Click);

            // Group Funcs
            this.groupBoxFuncs.Text = "Funções";
            this.groupBoxFuncs.Height = 150;
            this.groupBoxFuncs.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBoxFuncs.Controls.Add(this.numFuncSize);
            this.groupBoxFuncs.Controls.Add(this.lblFuncSize);
            this.groupBoxFuncs.Controls.Add(this.lstFunctions);
            this.groupBoxFuncs.Controls.Add(this.btnRemFunc);
            this.groupBoxFuncs.Controls.Add(this.btnAddFunc);

            // Lista
            this.lstFunctions.Location = new System.Drawing.Point(10, 20);
            this.lstFunctions.Size = new System.Drawing.Size(260, 80);
            this.lstFunctions.SelectedIndexChanged += new System.EventHandler(this.lstFunctions_SelectedIndexChanged); // Evento Novo

            // Botões + / -
            this.btnAddFunc.Text = "+"; this.btnAddFunc.Location = new System.Drawing.Point(10, 110); this.btnAddFunc.Width = 40; this.btnAddFunc.Click += new System.EventHandler(this.btnAddFunc_Click);
            this.btnRemFunc.Text = "-"; this.btnRemFunc.Location = new System.Drawing.Point(60, 110); this.btnRemFunc.Width = 40; this.btnRemFunc.Click += new System.EventHandler(this.btnRemFunc_Click);

            // Input de Tamanho
            this.lblFuncSize.Text = "Tam:";
            this.lblFuncSize.AutoSize = true;
            this.lblFuncSize.Location = new System.Drawing.Point(120, 115);

            this.numFuncSize.Location = new System.Drawing.Point(155, 112);
            this.numFuncSize.Width = 50;
            this.numFuncSize.Minimum = 1;
            this.numFuncSize.Maximum = 50;
            this.numFuncSize.Value = 5;
            this.numFuncSize.ValueChanged += new System.EventHandler(this.numFuncSize_ValueChanged); // Evento Novo

            // Botão Salvar
            this.btnSave.Text = "SALVAR E PUBLICAR";
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnSave.Height = 50;
            this.btnSave.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);

            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1000, 700);
            this.Controls.Add(this.pnlEditorContainer);
            this.Name = "LevelEditorForm";
            this.Text = "Editor de Fases";
            ((System.ComponentModel.ISupportInitialize)(this.pbGrid)).EndInit();
            this.pnlEditorContainer.ResumeLayout(false);
            this.panelSidebar.ResumeLayout(false);
            this.groupBoxFuncs.ResumeLayout(false);
            this.groupBoxFuncs.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numFuncSize)).EndInit();
            this.groupBoxGrid.ResumeLayout(false);
            this.groupBoxGrid.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCols)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRows)).EndInit();
            this.groupBoxTools.ResumeLayout(false);
            this.groupBoxTools.PerformLayout();
            this.groupBoxProps.ResumeLayout(false);
            this.groupBoxProps.PerformLayout();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.Panel pnlEditorContainer;
        private System.Windows.Forms.PictureBox pbGrid;
        private System.Windows.Forms.Panel panelSidebar;
        private System.Windows.Forms.GroupBox groupBoxProps;
        private System.Windows.Forms.TextBox txtTitle, txtDesc;
        private System.Windows.Forms.ComboBox cmbDifficulty;
        private System.Windows.Forms.Label label1, label2, label3;
        private System.Windows.Forms.GroupBox groupBoxTools;
        private System.Windows.Forms.FlowLayoutPanel flowTools;
        private System.Windows.Forms.Label lblSelectedTool;
        private System.Windows.Forms.GroupBox groupBoxGrid;
        private System.Windows.Forms.NumericUpDown numRows, numCols;
        private System.Windows.Forms.Button btnResize;
        private System.Windows.Forms.Label label4, label5;
        private System.Windows.Forms.GroupBox groupBoxFuncs;
        private System.Windows.Forms.ListBox lstFunctions;
        private System.Windows.Forms.Button btnAddFunc, btnRemFunc;
        private System.Windows.Forms.Button btnSave;
        // Novos
        private System.Windows.Forms.NumericUpDown numFuncSize;
        private System.Windows.Forms.Label lblFuncSize;
    }
}