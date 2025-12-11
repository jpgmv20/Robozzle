namespace Robozzle
{
    partial class CommentsForm
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing) { if (disposing && (components != null)) components.Dispose(); base.Dispose(disposing); }

        private void InitializeComponent()
        {
            this.pnlList = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlInput = new System.Windows.Forms.Panel();
            this.txtComment = new System.Windows.Forms.TextBox();
            this.btnSend = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pnlInput.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblTitle
            // 
            this.lblTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblTitle.Location = new System.Drawing.Point(0, 0);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(400, 40);
            this.lblTitle.Text = "Comentários";
            this.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlList
            // 
            this.pnlList.AutoScroll = true;
            this.pnlList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlList.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.pnlList.WrapContents = false;
            this.pnlList.Location = new System.Drawing.Point(0, 40);
            this.pnlList.Name = "pnlList";
            this.pnlList.Size = new System.Drawing.Size(400, 360);
            // 
            // pnlInput
            // 
            this.pnlInput.Controls.Add(this.txtComment);
            this.pnlInput.Controls.Add(this.btnSend);
            this.pnlInput.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlInput.Height = 50;
            this.pnlInput.Location = new System.Drawing.Point(0, 400);
            this.pnlInput.Padding = new System.Windows.Forms.Padding(5);
            // 
            // btnSend
            // 
            this.btnSend.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSend.Width = 70;
            this.btnSend.Text = "Enviar";
            this.btnSend.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.btnSend.ForeColor = System.Drawing.Color.White;
            this.btnSend.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // txtComment
            // 
            this.txtComment.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtComment.Multiline = true;
            this.txtComment.Location = new System.Drawing.Point(5, 5);
            this.txtComment.Size = new System.Drawing.Size(320, 40);
            // 
            // CommentsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(400, 450);
            this.Controls.Add(this.pnlList);
            this.Controls.Add(this.pnlInput);
            this.Controls.Add(this.lblTitle);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Name = "CommentsForm";
            this.Text = "Comentários";
            this.pnlInput.ResumeLayout(false);
            this.pnlInput.PerformLayout();
            this.ResumeLayout(false);
        }
        private System.Windows.Forms.FlowLayoutPanel pnlList;
        private System.Windows.Forms.Panel pnlInput;
        private System.Windows.Forms.TextBox txtComment;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Label lblTitle;
    }
}