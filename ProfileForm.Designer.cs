namespace Robozzle
{
    partial class ProfileForm
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
            this.pnlCentral = new System.Windows.Forms.Panel();
            this.btnVoltar = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pbAvatar = new System.Windows.Forms.PictureBox();
            this.btnChangeAvatar = new System.Windows.Forms.Button();
            this.txtDesc = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBoxConfig = new System.Windows.Forms.GroupBox();
            this.radDark = new System.Windows.Forms.RadioButton();
            this.radLight = new System.Windows.Forms.RadioButton();
            this.btnSave = new System.Windows.Forms.Button();
            this.pnlCentral.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbAvatar)).BeginInit();
            this.groupBoxConfig.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnlCentral
            // 
            this.pnlCentral.BackColor = System.Drawing.Color.White;
            this.pnlCentral.Controls.Add(this.btnVoltar);
            this.pnlCentral.Controls.Add(this.lblTitle);
            this.pnlCentral.Controls.Add(this.pbAvatar);
            this.pnlCentral.Controls.Add(this.btnChangeAvatar);
            this.pnlCentral.Controls.Add(this.txtDesc);
            this.pnlCentral.Controls.Add(this.label1);
            this.pnlCentral.Controls.Add(this.groupBoxConfig);
            this.pnlCentral.Controls.Add(this.btnSave);
            this.pnlCentral.Location = new System.Drawing.Point(200, 50);
            this.pnlCentral.Name = "pnlCentral";
            this.pnlCentral.Size = new System.Drawing.Size(400, 550);
            this.pnlCentral.TabIndex = 0;
            // 
            // btnVoltar
            // 
            this.btnVoltar.BackColor = System.Drawing.Color.IndianRed;
            this.btnVoltar.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnVoltar.ForeColor = System.Drawing.Color.White;
            this.btnVoltar.Location = new System.Drawing.Point(50, 480);
            this.btnVoltar.Name = "btnVoltar";
            this.btnVoltar.Size = new System.Drawing.Size(300, 30);
            this.btnVoltar.TabIndex = 8;
            this.btnVoltar.Text = "Voltar / Cancelar";
            this.btnVoltar.UseVisualStyleBackColor = false;
            this.btnVoltar.Click += new System.EventHandler(this.btnVoltar_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(115, 20);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(170, 30);
            this.lblTitle.TabIndex = 7;
            this.lblTitle.Text = "Perfil e Ajustes";
            // 
            // pbAvatar
            // 
            this.pbAvatar.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pbAvatar.Location = new System.Drawing.Point(150, 70);
            this.pbAvatar.Name = "pbAvatar";
            this.pbAvatar.Size = new System.Drawing.Size(100, 100);
            this.pbAvatar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbAvatar.TabIndex = 0;
            this.pbAvatar.TabStop = false;
            // 
            // btnChangeAvatar
            // 
            this.btnChangeAvatar.Location = new System.Drawing.Point(150, 176);
            this.btnChangeAvatar.Name = "btnChangeAvatar";
            this.btnChangeAvatar.Size = new System.Drawing.Size(100, 23);
            this.btnChangeAvatar.TabIndex = 1;
            this.btnChangeAvatar.Text = "Trocar Foto";
            this.btnChangeAvatar.UseVisualStyleBackColor = true;
            this.btnChangeAvatar.Click += new System.EventHandler(this.btnChangeAvatar_Click);
            // 
            // txtDesc
            // 
            this.txtDesc.Location = new System.Drawing.Point(50, 240);
            this.txtDesc.Multiline = true;
            this.txtDesc.Name = "txtDesc";
            this.txtDesc.Size = new System.Drawing.Size(300, 60);
            this.txtDesc.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(50, 222);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 15);
            this.label1.TabIndex = 3;
            this.label1.Text = "Descrição:";
            // 
            // groupBoxConfig
            // 
            this.groupBoxConfig.Controls.Add(this.radDark);
            this.groupBoxConfig.Controls.Add(this.radLight);
            this.groupBoxConfig.Location = new System.Drawing.Point(50, 320);
            this.groupBoxConfig.Name = "groupBoxConfig";
            this.groupBoxConfig.Size = new System.Drawing.Size(300, 80);
            this.groupBoxConfig.TabIndex = 4;
            this.groupBoxConfig.TabStop = false;
            this.groupBoxConfig.Text = "Configurações";
            // 
            // radDark
            // 
            this.radDark.AutoSize = true;
            this.radDark.Location = new System.Drawing.Point(160, 35);
            this.radDark.Name = "radDark";
            this.radDark.Size = new System.Drawing.Size(92, 19);
            this.radDark.TabIndex = 1;
            this.radDark.Text = "Tema Escuro";
            this.radDark.UseVisualStyleBackColor = true;
            // 
            // radLight
            // 
            this.radLight.AutoSize = true;
            this.radLight.Checked = true;
            this.radLight.Location = new System.Drawing.Point(30, 35);
            this.radLight.Name = "radLight";
            this.radLight.Size = new System.Drawing.Size(87, 19);
            this.radLight.TabIndex = 0;
            this.radLight.TabStop = true;
            this.radLight.Text = "Tema Claro";
            this.radLight.UseVisualStyleBackColor = true;
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(50, 420);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(300, 40);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "SALVAR ALTERAÇÕES";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // ProfileForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Controls.Add(this.pnlCentral);
            this.Name = "ProfileForm";
            this.Text = "Perfil do Usuário";
            this.pnlCentral.ResumeLayout(false);
            this.pnlCentral.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbAvatar)).EndInit();
            this.groupBoxConfig.ResumeLayout(false);
            this.groupBoxConfig.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnlCentral;
        private System.Windows.Forms.PictureBox pbAvatar;
        private System.Windows.Forms.Button btnChangeAvatar;
        private System.Windows.Forms.TextBox txtDesc;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBoxConfig;
        private System.Windows.Forms.RadioButton radDark;
        private System.Windows.Forms.RadioButton radLight;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Button btnVoltar;
    }
}