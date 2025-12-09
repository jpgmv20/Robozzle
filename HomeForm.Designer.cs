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
            this.components = new System.ComponentModel.Container();
            this.pnlContainer = new System.Windows.Forms.FlowLayoutPanel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.btnCreateLevel = new System.Windows.Forms.Button();
            this.btnChat = new System.Windows.Forms.Button(); // <--- NOVO
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.cmbFilter = new System.Windows.Forms.ComboBox();
            this.cmbSearchType = new System.Windows.Forms.ComboBox();
            this.pbUserAvatar = new System.Windows.Forms.PictureBox();
            this.ctxUserMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuProfile = new System.Windows.Forms.ToolStripMenuItem();
            this.menuLogout = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.pbUserAvatar)).BeginInit();
            this.ctxUserMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlContainer
            // 
            this.pnlContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlContainer.AutoScroll = true;
            this.pnlContainer.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlContainer.Location = new System.Drawing.Point(0, 80);
            this.pnlContainer.Name = "pnlContainer";
            this.pnlContainer.Padding = new System.Windows.Forms.Padding(20);
            this.pnlContainer.Size = new System.Drawing.Size(900, 400);
            this.pnlContainer.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 20F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.DimGray;
            this.lblTitle.Location = new System.Drawing.Point(20, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(137, 37);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "Robozzle";
            // 
            // cmbSearchType
            // 
            this.cmbSearchType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbSearchType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSearchType.FormattingEnabled = true;
            this.cmbSearchType.Items.AddRange(new object[] { "Fases", "Perfis" });
            this.cmbSearchType.Location = new System.Drawing.Point(200, 30);
            this.cmbSearchType.Name = "cmbSearchType";
            this.cmbSearchType.Size = new System.Drawing.Size(80, 23);
            this.cmbSearchType.TabIndex = 6;
            this.cmbSearchType.SelectedIndexChanged += new System.EventHandler(this.cmbSearchType_SelectedIndexChanged);
            // 
            // txtSearch
            // 
            this.txtSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSearch.Location = new System.Drawing.Point(290, 30);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.PlaceholderText = "Pesquisar...";
            this.txtSearch.Size = new System.Drawing.Size(190, 23); // Reduzi um pouco para caber o botão chat
            this.txtSearch.TabIndex = 3;
            this.txtSearch.TextChanged += new System.EventHandler(this.FilterContent);
            // 
            // cmbFilter
            // 
            this.cmbFilter.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFilter.FormattingEnabled = true;
            this.cmbFilter.Location = new System.Drawing.Point(490, 30);
            this.cmbFilter.Name = "cmbFilter";
            this.cmbFilter.Size = new System.Drawing.Size(110, 23);
            this.cmbFilter.TabIndex = 4;
            this.cmbFilter.SelectedIndexChanged += new System.EventHandler(this.FilterContent);
            // 
            // btnChat (NOVO)
            // 
            this.btnChat.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChat.BackColor = System.Drawing.Color.MediumPurple;
            this.btnChat.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnChat.FlatAppearance.BorderSize = 0;
            this.btnChat.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChat.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnChat.ForeColor = System.Drawing.Color.White;
            this.btnChat.Location = new System.Drawing.Point(610, 25);
            this.btnChat.Name = "btnChat";
            this.btnChat.Size = new System.Drawing.Size(60, 32);
            this.btnChat.TabIndex = 7;
            this.btnChat.Text = "CHAT";
            this.btnChat.UseVisualStyleBackColor = false;
            this.btnChat.Click += new System.EventHandler(this.btnChat_Click);
            // 
            // btnCreateLevel
            // 
            this.btnCreateLevel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCreateLevel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(120)))), ((int)(((byte)(215)))));
            this.btnCreateLevel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCreateLevel.FlatAppearance.BorderSize = 0;
            this.btnCreateLevel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCreateLevel.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnCreateLevel.ForeColor = System.Drawing.Color.White;
            this.btnCreateLevel.Location = new System.Drawing.Point(680, 25); // Empurrei um pouco pra direita
            this.btnCreateLevel.Name = "btnCreateLevel";
            this.btnCreateLevel.Size = new System.Drawing.Size(90, 32);
            this.btnCreateLevel.TabIndex = 2;
            this.btnCreateLevel.Text = "+ FASE";
            this.btnCreateLevel.UseVisualStyleBackColor = false;
            this.btnCreateLevel.Click += new System.EventHandler(this.btnCreateLevel_Click);
            // 
            // pbUserAvatar
            // 
            this.pbUserAvatar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbUserAvatar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbUserAvatar.Location = new System.Drawing.Point(780, 15);
            this.pbUserAvatar.Name = "pbUserAvatar";
            this.pbUserAvatar.Size = new System.Drawing.Size(50, 50);
            this.pbUserAvatar.TabIndex = 5;
            this.pbUserAvatar.TabStop = false;
            this.pbUserAvatar.Click += new System.EventHandler(this.pbUserAvatar_Click);
            // 
            // ctxUserMenu
            // 
            this.ctxUserMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuProfile,
            this.menuLogout});
            this.ctxUserMenu.Name = "ctxUserMenu";
            this.ctxUserMenu.Size = new System.Drawing.Size(129, 48);
            // 
            // menuProfile
            // 
            this.menuProfile.Name = "menuProfile";
            this.menuProfile.Size = new System.Drawing.Size(128, 22);
            this.menuProfile.Text = "Meu Perfil";
            this.menuProfile.Click += new System.EventHandler(this.menuProfile_Click);
            // 
            // menuLogout
            // 
            this.menuLogout.Name = "menuLogout";
            this.menuLogout.Size = new System.Drawing.Size(128, 22);
            this.menuLogout.Text = "Sair";
            this.menuLogout.Click += new System.EventHandler(this.menuLogout_Click);
            // 
            // HomeForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(900, 500);
            this.Controls.Add(this.btnChat); // <--- ADICIONAR AQUI
            this.Controls.Add(this.cmbSearchType);
            this.Controls.Add(this.pbUserAvatar);
            this.Controls.Add(this.btnCreateLevel);
            this.Controls.Add(this.cmbFilter);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.pnlContainer);
            this.Name = "HomeForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Robozzle - Home";
            this.Load += new System.EventHandler(this.HomeForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pbUserAvatar)).EndInit();
            this.ctxUserMenu.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel pnlContainer;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnCreateLevel;
        private System.Windows.Forms.Button btnChat; // <--- NOVO
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.ComboBox cmbFilter;
        private System.Windows.Forms.ComboBox cmbSearchType;
        private System.Windows.Forms.PictureBox pbUserAvatar;
        private System.Windows.Forms.ContextMenuStrip ctxUserMenu;
        private System.Windows.Forms.ToolStripMenuItem menuProfile;
        private System.Windows.Forms.ToolStripMenuItem menuLogout;
    }
}