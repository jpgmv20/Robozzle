namespace Robozzle
{
    partial class ChatForm
    {
        private System.ComponentModel.IContainer components = null;
        protected override void Dispose(bool disposing) { if (disposing && (components != null)) components.Dispose(); base.Dispose(disposing); }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lstConversations = new System.Windows.Forms.ListBox();
            this.lblConversationsTitle = new System.Windows.Forms.Label();
            this.pnlChatArea = new System.Windows.Forms.Panel();
            this.flowMessages = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlInput = new System.Windows.Forms.Panel();
            this.btnSend = new System.Windows.Forms.Button();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.lblChatTitle = new System.Windows.Forms.Label();
            this.tmrUpdate = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.pnlChatArea.SuspendLayout();
            this.pnlInput.SuspendLayout();
            this.SuspendLayout();

            // splitContainer1
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Size = new System.Drawing.Size(800, 500);
            this.splitContainer1.SplitterDistance = 250;

            // Panel 1 (Lista)
            this.lstConversations.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstConversations.FormattingEnabled = true;
            this.lstConversations.IntegralHeight = false;
            this.lstConversations.ItemHeight = 40;
            this.lstConversations.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lstConversations.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lstConversations_DrawItem);
            this.lstConversations.SelectedIndexChanged += new System.EventHandler(this.lstConversations_SelectedIndexChanged);

            this.lblConversationsTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblConversationsTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblConversationsTitle.Text = "Conversas";
            this.lblConversationsTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblConversationsTitle.Height = 40;

            this.splitContainer1.Panel1.Controls.Add(this.lstConversations);
            this.splitContainer1.Panel1.Controls.Add(this.lblConversationsTitle);

            // Panel 2 (Chat)
            this.pnlChatArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlChatArea.Controls.Add(this.flowMessages);
            this.pnlChatArea.Controls.Add(this.pnlInput);
            this.pnlChatArea.Controls.Add(this.lblChatTitle);

            this.lblChatTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblChatTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblChatTitle.Text = "Selecione uma conversa";
            this.lblChatTitle.Height = 40;
            this.lblChatTitle.BackColor = System.Drawing.Color.LightGray;

            this.flowMessages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowMessages.AutoScroll = true;
            this.flowMessages.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowMessages.WrapContents = false;

            this.pnlInput.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlInput.Height = 50;
            this.pnlInput.Padding = new System.Windows.Forms.Padding(5);

            this.btnSend.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSend.Text = "Enviar";
            this.btnSend.Width = 80;
            this.btnSend.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.btnSend.ForeColor = System.Drawing.Color.White;
            this.btnSend.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);

            this.txtMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMessage.Multiline = true;

            this.pnlInput.Controls.Add(this.txtMessage);
            this.pnlInput.Controls.Add(this.btnSend);

            this.splitContainer1.Panel2.Controls.Add(this.pnlChatArea);

            // Timer
            this.tmrUpdate.Interval = 2000; // 2 segundos
            this.tmrUpdate.Tick += new System.EventHandler(this.tmrUpdate_Tick);

            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 500);
            this.Controls.Add(this.splitContainer1);
            this.Name = "ChatForm";
            this.Text = "Chat Robozzle";
            this.Load += new System.EventHandler(this.ChatForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.pnlChatArea.ResumeLayout(false);
            this.pnlInput.ResumeLayout(false);
            this.pnlInput.PerformLayout();
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListBox lstConversations;
        private System.Windows.Forms.Label lblConversationsTitle;
        private System.Windows.Forms.Panel pnlChatArea;
        private System.Windows.Forms.FlowLayoutPanel flowMessages;
        private System.Windows.Forms.Panel pnlInput;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.Label lblChatTitle;
        private System.Windows.Forms.Timer tmrUpdate;
    }
}