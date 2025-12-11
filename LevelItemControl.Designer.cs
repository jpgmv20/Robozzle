namespace Robozzle
{
    partial class LevelItemControl
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pbPreview = new System.Windows.Forms.PictureBox();
            this.lblDifficulty = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pbAuthorAvatar = new System.Windows.Forms.PictureBox();
            this.lblAuthorName = new System.Windows.Forms.Label();
            this.lblStats = new System.Windows.Forms.Label();
            this.btnLike = new System.Windows.Forms.Label();
            this.btnComment = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pbPreview)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbAuthorAvatar)).BeginInit();
            this.SuspendLayout();
            // 
            // pbPreview
            // 
            this.pbPreview.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.pbPreview.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbPreview.Location = new System.Drawing.Point(10, 10);
            this.pbPreview.Name = "pbPreview";
            this.pbPreview.Size = new System.Drawing.Size(220, 120);
            this.pbPreview.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbPreview.TabIndex = 0;
            this.pbPreview.TabStop = false;
            this.pbPreview.Click += new System.EventHandler(this.OnPlay_Click);
            // 
            // lblDifficulty
            // 
            this.lblDifficulty.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblDifficulty.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.lblDifficulty.ForeColor = System.Drawing.Color.White;
            this.lblDifficulty.Location = new System.Drawing.Point(0, 230);
            this.lblDifficulty.Name = "lblDifficulty";
            this.lblDifficulty.Size = new System.Drawing.Size(240, 30);
            this.lblDifficulty.TabIndex = 1;
            this.lblDifficulty.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblDifficulty.Click += new System.EventHandler(this.OnPlay_Click);
            // 
            // pbAuthorAvatar
            // 
            this.pbAuthorAvatar.BackColor = System.Drawing.Color.LightGray;
            this.pbAuthorAvatar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbAuthorAvatar.Location = new System.Drawing.Point(10, 140);
            this.pbAuthorAvatar.Name = "pbAuthorAvatar";
            this.pbAuthorAvatar.Size = new System.Drawing.Size(40, 40);
            this.pbAuthorAvatar.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbAuthorAvatar.TabIndex = 2;
            this.pbAuthorAvatar.TabStop = false;
            this.pbAuthorAvatar.Click += new System.EventHandler(this.OnProfile_Click);
            this.pbAuthorAvatar.Paint += new System.Windows.Forms.PaintEventHandler(this.pbAuthorAvatar_Paint);
            // 
            // lblTitle
            // 
            this.lblTitle.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(55, 140);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(140, 23);
            this.lblTitle.TabIndex = 3;
            this.lblTitle.Text = "Nome";
            this.lblTitle.Click += new System.EventHandler(this.OnPlay_Click);
            // 
            // lblAuthorName
            // 
            this.lblAuthorName.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblAuthorName.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.lblAuthorName.ForeColor = System.Drawing.Color.Gray;
            this.lblAuthorName.Location = new System.Drawing.Point(55, 163);
            this.lblAuthorName.Name = "lblAuthorName";
            this.lblAuthorName.Size = new System.Drawing.Size(175, 20);
            this.lblAuthorName.TabIndex = 4;
            this.lblAuthorName.Text = "Autor";
            this.lblAuthorName.Click += new System.EventHandler(this.OnProfile_Click);
            // 
            // lblStats
            // 
            this.lblStats.AutoSize = true;
            this.lblStats.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.lblStats.ForeColor = System.Drawing.Color.DimGray;
            this.lblStats.Location = new System.Drawing.Point(55, 185);
            this.lblStats.Name = "lblStats";
            this.lblStats.Size = new System.Drawing.Size(70, 13);
            this.lblStats.TabIndex = 5;
            this.lblStats.Text = "♥ 0   ▶ 0";
            // 
            // btnLike
            // 
            this.btnLike.AutoSize = true;
            this.btnLike.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLike.Font = new System.Drawing.Font("Segoe UI Symbol", 16F);
            this.btnLike.ForeColor = System.Drawing.Color.Gray;
            this.btnLike.Location = new System.Drawing.Point(195, 135);
            this.btnLike.Name = "btnLike";
            this.btnLike.Size = new System.Drawing.Size(30, 30);
            this.btnLike.TabIndex = 6;
            this.btnLike.Text = "♥";
            this.btnLike.Click += new System.EventHandler(this.btnLike_Click);
            // 
            // btnComment
            // 
            this.btnComment.AutoSize = true;
            this.btnComment.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnComment.Font = new System.Drawing.Font("Segoe UI Symbol", 14F);
            this.btnComment.ForeColor = System.Drawing.Color.Gray;
            this.btnComment.Location = new System.Drawing.Point(195, 175);
            this.btnComment.Name = "btnComment";
            this.btnComment.Size = new System.Drawing.Size(30, 25);
            this.btnComment.TabIndex = 7;
            this.btnComment.Text = "💬";
            this.btnComment.Click += new System.EventHandler(this.btnComment_Click);
            // 
            // LevelItemControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.btnComment);
            this.Controls.Add(this.btnLike);
            this.Controls.Add(this.lblStats);
            this.Controls.Add(this.lblAuthorName);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.pbAuthorAvatar);
            this.Controls.Add(this.lblDifficulty);
            this.Controls.Add(this.pbPreview);
            this.Name = "LevelItemControl";
            this.Size = new System.Drawing.Size(240, 260);
            ((System.ComponentModel.ISupportInitialize)(this.pbPreview)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbAuthorAvatar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.PictureBox pbPreview;
        private System.Windows.Forms.Label lblDifficulty;
        private System.Windows.Forms.PictureBox pbAuthorAvatar;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label lblAuthorName;
        private System.Windows.Forms.Label lblStats;
        private System.Windows.Forms.Label btnLike;
        private System.Windows.Forms.Label btnComment;
    }
}