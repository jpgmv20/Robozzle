namespace Robozzle
{
    partial class ChatForm
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lstConversations = new System.Windows.Forms.ListBox();
            this.txtSearchChat = new System.Windows.Forms.TextBox();
            this.lblConversationsTitle = new System.Windows.Forms.Label();
            this.pnlRightContainer = new System.Windows.Forms.Panel();
            this.flowMessages = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlHeader = new System.Windows.Forms.Panel();
            this.lblChatTitle = new System.Windows.Forms.Label();
            this.pnlInput = new System.Windows.Forms.Panel();
            this.btnSend = new System.Windows.Forms.Button();
            this.txtMessage = new System.Windows.Forms.TextBox();
            this.tmrUpdate = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.pnlRightContainer.SuspendLayout();
            this.pnlHeader.SuspendLayout();
            this.pnlInput.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lstConversations);
            this.splitContainer1.Panel1.Controls.Add(this.txtSearchChat);
            this.splitContainer1.Panel1.Controls.Add(this.lblConversationsTitle);
            this.splitContainer1.Panel1MinSize = 250;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.pnlRightContainer);
            this.splitContainer1.Size = new System.Drawing.Size(900, 600);
            this.splitContainer1.SplitterDistance = 280;
            this.splitContainer1.TabIndex = 0;
            // 
            // lblConversationsTitle
            // 
            this.lblConversationsTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblConversationsTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblConversationsTitle.Location = new System.Drawing.Point(0, 0);
            this.lblConversationsTitle.Name = "lblConversationsTitle";
            this.lblConversationsTitle.Size = new System.Drawing.Size(280, 40);
            this.lblConversationsTitle.TabIndex = 0;
            this.lblConversationsTitle.Text = "Conversas";
            this.lblConversationsTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtSearchChat
            // 
            this.txtSearchChat.Dock = System.Windows.Forms.DockStyle.Top;
            this.txtSearchChat.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtSearchChat.Location = new System.Drawing.Point(0, 40);
            this.txtSearchChat.Name = "txtSearchChat";
            this.txtSearchChat.PlaceholderText = "Pesquisar usuário...";
            this.txtSearchChat.Size = new System.Drawing.Size(280, 27);
            this.txtSearchChat.TabIndex = 1;
            this.txtSearchChat.TextChanged += new System.EventHandler(this.txtSearchChat_TextChanged);
            // 
            // lstConversations
            // 
            this.lstConversations.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lstConversations.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstConversations.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lstConversations.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.lstConversations.FormattingEnabled = true;
            this.lstConversations.IntegralHeight = false;
            this.lstConversations.ItemHeight = 50;
            this.lstConversations.Location = new System.Drawing.Point(0, 67);
            this.lstConversations.Name = "lstConversations";
            this.lstConversations.Size = new System.Drawing.Size(280, 533);
            this.lstConversations.TabIndex = 2;
            this.lstConversations.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.lstConversations_DrawItem);
            this.lstConversations.SelectedIndexChanged += new System.EventHandler(this.lstConversations_SelectedIndexChanged);
            // 
            // pnlRightContainer
            // 
            this.pnlRightContainer.BackColor = System.Drawing.Color.WhiteSmoke;
            this.pnlRightContainer.Controls.Add(this.flowMessages);
            this.pnlRightContainer.Controls.Add(this.pnlHeader);
            this.pnlRightContainer.Controls.Add(this.pnlInput);
            this.pnlRightContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlRightContainer.Location = new System.Drawing.Point(0, 0);
            this.pnlRightContainer.Name = "pnlRightContainer";
            this.pnlRightContainer.Size = new System.Drawing.Size(616, 600);
            this.pnlRightContainer.TabIndex = 0;
            // 
            // pnlHeader
            // 
            this.pnlHeader.BackColor = System.Drawing.Color.LightGray;
            this.pnlHeader.Controls.Add(this.lblChatTitle);
            this.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlHeader.Location = new System.Drawing.Point(0, 0);
            this.pnlHeader.Name = "pnlHeader";
            this.pnlHeader.Size = new System.Drawing.Size(616, 50);
            this.pnlHeader.TabIndex = 0;
            // 
            // lblChatTitle
            // 
            this.lblChatTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblChatTitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold);
            this.lblChatTitle.Location = new System.Drawing.Point(0, 0);
            this.lblChatTitle.Name = "lblChatTitle";
            this.lblChatTitle.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.lblChatTitle.Size = new System.Drawing.Size(616, 50);
            this.lblChatTitle.TabIndex = 0;
            this.lblChatTitle.Text = "Selecione uma conversa";
            this.lblChatTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // pnlInput
            // 
            this.pnlInput.BackColor = System.Drawing.Color.White;
            this.pnlInput.Controls.Add(this.txtMessage);
            this.pnlInput.Controls.Add(this.btnSend);
            this.pnlInput.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnlInput.Location = new System.Drawing.Point(0, 540);
            this.pnlInput.Name = "pnlInput";
            this.pnlInput.Padding = new System.Windows.Forms.Padding(10);
            this.pnlInput.Size = new System.Drawing.Size(616, 60);
            this.pnlInput.TabIndex = 1;
            // 
            // btnSend
            // 
            this.btnSend.BackColor = System.Drawing.Color.MediumSeaGreen;
            this.btnSend.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnSend.FlatAppearance.BorderSize = 0;
            this.btnSend.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSend.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            this.btnSend.ForeColor = System.Drawing.Color.White;
            this.btnSend.Location = new System.Drawing.Point(526, 10);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(80, 40);
            this.btnSend.TabIndex = 1;
            this.btnSend.Text = "ENVIAR";
            this.btnSend.UseVisualStyleBackColor = false;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // txtMessage
            // 
            this.txtMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMessage.Font = new System.Drawing.Font("Segoe UI", 11F);
            this.txtMessage.Location = new System.Drawing.Point(10, 10);
            this.txtMessage.Multiline = true;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(516, 40);
            this.txtMessage.TabIndex = 0;
            // 
            // flowMessages
            // 
            this.flowMessages.AutoScroll = true;
            this.flowMessages.BackColor = System.Drawing.Color.Transparent;
            this.flowMessages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowMessages.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowMessages.Location = new System.Drawing.Point(0, 50);
            this.flowMessages.Name = "flowMessages";
            this.flowMessages.Size = new System.Drawing.Size(616, 490);
            this.flowMessages.TabIndex = 2;
            this.flowMessages.WrapContents = false;
            // 
            // tmrUpdate
            // 
            this.tmrUpdate.Interval = 2000;
            this.tmrUpdate.Tick += new System.EventHandler(this.tmrUpdate_Tick);
            // 
            // ChatForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 600);
            this.Controls.Add(this.splitContainer1);
            this.Name = "ChatForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Chat Robozzle";
            this.Load += new System.EventHandler(this.ChatForm_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.pnlRightContainer.ResumeLayout(false);
            this.pnlHeader.ResumeLayout(false);
            this.pnlInput.ResumeLayout(false);
            this.pnlInput.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListBox lstConversations;
        private System.Windows.Forms.TextBox txtSearchChat;
        private System.Windows.Forms.Label lblConversationsTitle;
        private System.Windows.Forms.Panel pnlRightContainer;
        private System.Windows.Forms.Panel pnlHeader;
        private System.Windows.Forms.Label lblChatTitle;
        private System.Windows.Forms.Panel pnlInput;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.TextBox txtMessage;
        private System.Windows.Forms.FlowLayoutPanel flowMessages;
        private System.Windows.Forms.Timer tmrUpdate;
    }
}