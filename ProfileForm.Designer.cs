namespace Robozzle
{
    partial class ProfileForm
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing) { if (disposing && (components != null)) components.Dispose(); base.Dispose(disposing); }

        private void InitializeComponent()
        {
            this.pnlCentral = new System.Windows.Forms.Panel();
            this.btnVoltar = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.grpConfig = new System.Windows.Forms.GroupBox();
            this.radDark = new System.Windows.Forms.RadioButton();
            this.radLight = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDesc = new System.Windows.Forms.TextBox();
            this.btnEditAvatar = new System.Windows.Forms.Button();
            this.pbAvatar = new System.Windows.Forms.PictureBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.lblFollowers = new System.Windows.Forms.Label();
            this.lblFollowing = new System.Windows.Forms.Label();
            this.btnFollow = new System.Windows.Forms.Button();
            this.btnMessage = new System.Windows.Forms.Button(); // <--- NOVO
            this.pnlCentral.SuspendLayout();
            this.grpConfig.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbAvatar)).BeginInit();
            this.SuspendLayout();
            // pnlCentral
            this.pnlCentral.BackColor = System.Drawing.Color.White;
            this.pnlCentral.Controls.Add(this.btnMessage); // <--- ADICIONAR
            this.pnlCentral.Controls.Add(this.btnFollow);
            this.pnlCentral.Controls.Add(this.lblFollowing);
            this.pnlCentral.Controls.Add(this.lblFollowers);
            this.pnlCentral.Controls.Add(this.lblName);
            this.pnlCentral.Controls.Add(this.btnVoltar);
            this.pnlCentral.Controls.Add(this.btnSave);
            this.pnlCentral.Controls.Add(this.grpConfig);
            this.pnlCentral.Controls.Add(this.label1);
            this.pnlCentral.Controls.Add(this.txtDesc);
            this.pnlCentral.Controls.Add(this.btnEditAvatar);
            this.pnlCentral.Controls.Add(this.pbAvatar);
            this.pnlCentral.Controls.Add(this.lblTitle);
            this.pnlCentral.Location = new System.Drawing.Point(200, 20);
            this.pnlCentral.Name = "pnlCentral";
            this.pnlCentral.Size = new System.Drawing.Size(400, 600);
            this.pnlCentral.TabIndex = 0;
            // lblTitle
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.Gray;
            this.lblTitle.Location = new System.Drawing.Point(0, 10);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(400, 30);
            this.lblTitle.Text = "Perfil";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // pbAvatar
            this.pbAvatar.BackColor = System.Drawing.Color.LightGray;
            this.pbAvatar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbAvatar.Location = new System.Drawing.Point(140, 50);
            this.pbAvatar.Name = "pbAvatar";
            this.pbAvatar.Size = new System.Drawing.Size(120, 120);
            this.pbAvatar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            // btnEditAvatar
            this.btnEditAvatar.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnEditAvatar.Location = new System.Drawing.Point(270, 145);
            this.btnEditAvatar.Name = "btnEditAvatar";
            this.btnEditAvatar.Size = new System.Drawing.Size(75, 25);
            this.btnEditAvatar.Text = "Alterar";
            this.btnEditAvatar.Click += new System.EventHandler(this.btnEditAvatar_Click);
            // lblName
            this.lblName.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblName.Location = new System.Drawing.Point(0, 180);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(400, 35);
            this.lblName.Text = "Nome";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // lblFollowers
            this.lblFollowers.AutoSize = true;
            this.lblFollowers.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblFollowers.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblFollowers.ForeColor = System.Drawing.Color.DimGray;
            this.lblFollowers.Location = new System.Drawing.Point(80, 220);
            this.lblFollowers.Name = "lblFollowers";
            this.lblFollowers.Size = new System.Drawing.Size(95, 19);
            this.lblFollowers.Text = "0 Seguidores";
            this.lblFollowers.Click += new System.EventHandler(this.lblFollowers_Click);
            // lblFollowing
            this.lblFollowing.AutoSize = true;
            this.lblFollowing.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblFollowing.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblFollowing.ForeColor = System.Drawing.Color.DimGray;
            this.lblFollowing.Location = new System.Drawing.Point(220, 220);
            this.lblFollowing.Name = "lblFollowing";
            this.lblFollowing.Size = new System.Drawing.Size(86, 19);
            this.lblFollowing.Text = "0 Seguindo";
            this.lblFollowing.Click += new System.EventHandler(this.lblFollowing_Click);
            // btnFollow
            this.btnFollow.BackColor = System.Drawing.Color.FromArgb(0, 120, 215);
            this.btnFollow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFollow.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnFollow.ForeColor = System.Drawing.Color.White;
            this.btnFollow.Location = new System.Drawing.Point(50, 250);
            this.btnFollow.Name = "btnFollow";
            this.btnFollow.Size = new System.Drawing.Size(145, 35);
            this.btnFollow.Text = "Seguir";
            this.btnFollow.UseVisualStyleBackColor = false;
            this.btnFollow.Click += new System.EventHandler(this.btnFollow_Click);
            // 
            // btnMessage (NOVO)
            // 
            this.btnMessage.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.btnMessage.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMessage.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnMessage.ForeColor = System.Drawing.Color.White;
            this.btnMessage.Location = new System.Drawing.Point(205, 250);
            this.btnMessage.Name = "btnMessage";
            this.btnMessage.Size = new System.Drawing.Size(145, 35);
            this.btnMessage.Text = "Mensagem";
            this.btnMessage.UseVisualStyleBackColor = false;
            this.btnMessage.Click += new System.EventHandler(this.btnMessage_Click);

            // label1
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(50, 290);
            this.label1.Text = "Sobre:";
            // txtDesc
            this.txtDesc.Location = new System.Drawing.Point(50, 310);
            this.txtDesc.Multiline = true;
            this.txtDesc.Name = "txtDesc";
            this.txtDesc.Size = new System.Drawing.Size(300, 60);
            // grpConfig
            this.grpConfig.Controls.Add(this.radDark);
            this.grpConfig.Controls.Add(this.radLight);
            this.grpConfig.Location = new System.Drawing.Point(50, 390);
            this.grpConfig.Name = "grpConfig";
            this.grpConfig.Size = new System.Drawing.Size(300, 70);
            this.grpConfig.Text = "Configurações";
            // radLight
            this.radLight.AutoSize = true;
            this.radLight.Checked = true;
            this.radLight.Location = new System.Drawing.Point(30, 30);
            this.radLight.Text = "Claro";
            // radDark
            this.radDark.AutoSize = true;
            this.radDark.Location = new System.Drawing.Point(150, 30);
            this.radDark.Text = "Escuro";
            // btnSave
            this.btnSave.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(50, 480);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(300, 40);
            this.btnSave.Text = "SALVAR";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // btnVoltar
            this.btnVoltar.BackColor = System.Drawing.Color.IndianRed;
            this.btnVoltar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVoltar.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnVoltar.ForeColor = System.Drawing.Color.White;
            this.btnVoltar.Location = new System.Drawing.Point(50, 530);
            this.btnVoltar.Name = "btnVoltar";
            this.btnVoltar.Size = new System.Drawing.Size(300, 35);
            this.btnVoltar.Text = "Voltar";
            this.btnVoltar.UseVisualStyleBackColor = false;
            this.btnVoltar.Click += new System.EventHandler(this.btnVoltar_Click);
            // Form
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 700);
            this.Controls.Add(this.pnlCentral);
            this.Name = "ProfileForm";
            this.Text = "Perfil";
            this.pnlCentral.ResumeLayout(false);
            this.pnlCentral.PerformLayout();
            this.grpConfig.ResumeLayout(false);
            this.grpConfig.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbAvatar)).EndInit();
            this.ResumeLayout(false);
        }
        private System.Windows.Forms.Panel pnlCentral;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.PictureBox pbAvatar;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Button btnEditAvatar;
        private System.Windows.Forms.Label lblFollowers;
        private System.Windows.Forms.Label lblFollowing;
        private System.Windows.Forms.Button btnFollow;
        private System.Windows.Forms.Button btnMessage; // <--- NOVO
        private System.Windows.Forms.TextBox txtDesc;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox grpConfig;
        private System.Windows.Forms.RadioButton radDark;
        private System.Windows.Forms.RadioButton radLight;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnVoltar;
    }
}