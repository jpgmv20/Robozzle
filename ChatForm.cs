using RobozllueApp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Robozzle
{
    public partial class ChatForm : FormLoader
    {
        private ChatRepository _repo = new ChatRepository();
        private int _currentConversationId = 0;
        private int _lastMessageCount = 0;
        private List<Conversation> _myConversations = new List<Conversation>();

        public ChatForm()
        {
            InitializeComponent();



            this.txtMessage.KeyDown += txtMessage_KeyDown;
        }

        public ChatForm(int openConversationId) : this()
        {
            _currentConversationId = openConversationId;
        }

        private void ChatForm_Load(object sender, EventArgs e)
        {
            ThemeManager.ApplyTheme(this);
            LoadMyConversations();

            // Se veio com ID, tenta abrir direto
            if (_currentConversationId > 0)
            {
                var match = _myConversations.FirstOrDefault(c => c.Id == _currentConversationId);
                if (match != null)
                {
                    lstConversations.SelectedItem = match;
                }
                else
                {
                    LoadMessages(_currentConversationId);
                }
            }

            tmrUpdate.Start();
        }

        private void LoadMyConversations()
        {
            _myConversations = _repo.GetUserConversations(UserSession.Id);
            UpdateList(_myConversations);
        }

        private void txtSearchChat_TextChanged(object sender, EventArgs e)
        {
            string term = txtSearchChat.Text.Trim();
            if (string.IsNullOrEmpty(term))
            {
                UpdateList(_myConversations);
            }
            else
            {
                var searchResults = _repo.SearchUsers(term, UserSession.Id);
                UpdateList(searchResults);
            }
        }

        private void UpdateList(List<Conversation> items)
        {
            var selected = lstConversations.SelectedItem as Conversation;
            lstConversations.Items.Clear();
            foreach (var item in items) lstConversations.Items.Add(item);

            // Tenta restaurar seleção
            if (selected != null)
            {
                foreach (Conversation c in lstConversations.Items)
                {
                    if (c.TargetUserId == selected.TargetUserId)
                    {
                        lstConversations.SelectedItem = c;
                        break;
                    }
                }
            }
        }

        private void lstConversations_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstConversations.SelectedItem is Conversation c)
            {
                // Se for resultado de busca (Id=0), cria o chat
                if (c.Id == 0)
                {
                    c.Id = _repo.StartPrivateChat(UserSession.Id, c.TargetUserId);
                    if (!_myConversations.Any(x => x.Id == c.Id)) _myConversations.Insert(0, c);
                }

                lblChatTitle.Text = c.Title;
                _currentConversationId = c.Id;
                _lastMessageCount = -1; // Força recarregar
                LoadMessages(c.Id);
            }
        }

        private void LoadMessages(int conversationId)
        {
            var msgs = _repo.GetMessages(conversationId);
            if (msgs.Count == _lastMessageCount) return;

            flowMessages.SuspendLayout();
            flowMessages.Controls.Clear();

            foreach (var msg in msgs)
            {
                var bubble = new MessageBubble(msg);

                // Container para layout
                Panel container = new Panel();
                container.AutoSize = false;
                container.Width = flowMessages.ClientSize.Width - 25;
                container.Height = bubble.Height + 10;
                container.BackColor = Color.Transparent;

                if (msg.IsMine)
                    bubble.Location = new Point(container.Width - bubble.Width, 0);
                else
                    bubble.Location = new Point(0, 0);

                container.Controls.Add(bubble);
                flowMessages.Controls.Add(container);
            }

            if (flowMessages.Controls.Count > 0)
                flowMessages.ScrollControlIntoView(flowMessages.Controls[flowMessages.Controls.Count - 1]);

            flowMessages.ResumeLayout();
            _lastMessageCount = msgs.Count;
        }

        private void txtMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (e.Shift) return; 
                e.SuppressKeyPress = true; // Remove som
                btnSend.PerformClick();
            }
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (_currentConversationId == 0) return;
            string text = txtMessage.Text.Trim();
            if (string.IsNullOrEmpty(text)) return;

            _repo.SendMessage(_currentConversationId, UserSession.Id, text);
            txtMessage.Clear();
            LoadMessages(_currentConversationId);
            LoadMyConversations(); // Atualiza a lista lateral
        }

        private void tmrUpdate_Tick(object sender, EventArgs e)
        {
            if (_currentConversationId > 0) LoadMessages(_currentConversationId);
        }

        private void lstConversations_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0 || e.Index >= lstConversations.Items.Count) return;
            Conversation item = (Conversation)lstConversations.Items[e.Index];

            e.DrawBackground();
            bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
            Brush textBrush = (UserSession.Theme == "dark" || isSelected) ? Brushes.White : Brushes.Black;
            if (UserSession.Theme == "light" && !isSelected) textBrush = Brushes.Black;

            // Avatar com try/catch para não travar
            try
            {
                if (item.Avatar != null && item.Avatar.Length > 0)
                {
                    using (var ms = new MemoryStream(item.Avatar))
                    using (var img = Image.FromStream(ms))
                        e.Graphics.DrawImage(img, e.Bounds.X + 5, e.Bounds.Y + 5, 40, 40);
                }
                else
                    e.Graphics.FillEllipse(Brushes.Gray, e.Bounds.X + 5, e.Bounds.Y + 5, 40, 40);
            }
            catch { e.Graphics.FillEllipse(Brushes.IndianRed, e.Bounds.X + 5, e.Bounds.Y + 5, 40, 40); }

            // Nome
            e.Graphics.DrawString(item.Title, new Font("Segoe UI", 11, FontStyle.Bold), textBrush, e.Bounds.X + 55, e.Bounds.Y + 5);

            // Mensagem
            if (!string.IsNullOrEmpty(item.LastMessage))
            {
                string msg = item.LastMessage.Length > 30 ? item.LastMessage.Substring(0, 30) + "..." : item.LastMessage;
                e.Graphics.DrawString(msg, new Font("Segoe UI", 9), Brushes.Gray, e.Bounds.X + 55, e.Bounds.Y + 28);
            }
            e.DrawFocusRectangle();
        }
    }
}