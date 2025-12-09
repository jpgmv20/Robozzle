using RobozllueApp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Robozzle
{
    public partial class ChatForm : FormLoader
    {
        private ChatRepository _repo = new ChatRepository();
        private int _currentConversationId = 0;
        private int _lastMessageCount = 0; // Para saber se precisa rolar a tela

        public ChatForm()
        {
            InitializeComponent();
            this.PainelCentral = splitContainer1; // Tela cheia
        }

        // Construtor para abrir já numa conversa
        public ChatForm(int openConversationId) : this()
        {
            _currentConversationId = openConversationId;
        }

        private void ChatForm_Load(object sender, EventArgs e)
        {
            ThemeManager.ApplyTheme(this);
            LoadConversations();

            // Se abriu com ID específico, carrega ele
            if (_currentConversationId > 0)
            {
                LoadMessages(_currentConversationId);
                // Tenta selecionar na lista
                foreach (Conversation c in lstConversations.Items)
                    if (c.Id == _currentConversationId) lstConversations.SelectedItem = c;
            }

            tmrUpdate.Start();
        }

        private void LoadConversations()
        {
            int selectedId = _currentConversationId;
            var list = _repo.GetUserConversations(UserSession.Id);

            lstConversations.Items.Clear();
            foreach (var c in list)
            {
                lstConversations.Items.Add(c);
                if (c.Id == selectedId) lstConversations.SelectedItem = c;
            }
        }

        private void LoadMessages(int conversationId)
        {
            _currentConversationId = conversationId;
            var msgs = _repo.GetMessages(conversationId);

            // Otimização simples: só recria se mudou a quantidade
            // Num app real, verificaria ID da última mensagem
            if (msgs.Count == _lastMessageCount) return;

            flowMessages.SuspendLayout();
            flowMessages.Controls.Clear();

            foreach (var msg in msgs)
            {
                var bubble = new MessageBubble(msg);

                // Hack para alinhar no FlowLayoutPanel
                Panel container = new Panel();
                container.AutoSize = true;
                container.Width = flowMessages.Width - 25;
                container.BackColor = Color.Transparent;

                bubble.Location = msg.IsMine
                    ? new Point(container.Width - bubble.Width, 0)
                    : new Point(0, 0);

                container.Controls.Add(bubble);
                container.Height = bubble.Height + 5;

                flowMessages.Controls.Add(container);
            }

            // Scroll para o fim
            if (flowMessages.Controls.Count > 0)
                flowMessages.ScrollControlIntoView(flowMessages.Controls[flowMessages.Controls.Count - 1]);

            flowMessages.ResumeLayout();
            _lastMessageCount = msgs.Count;
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if (_currentConversationId == 0) return;
            string text = txtMessage.Text.Trim();
            if (string.IsNullOrEmpty(text)) return;

            _repo.SendMessage(_currentConversationId, UserSession.Id, text);
            txtMessage.Clear();
            LoadMessages(_currentConversationId); // Atualiza imediato
        }

        private void tmrUpdate_Tick(object sender, EventArgs e)
        {
            // Atualiza mensagens da conversa atual
            if (_currentConversationId > 0)
                LoadMessages(_currentConversationId);
        }

        private void lstConversations_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstConversations.SelectedItem is Conversation c)
            {
                lblChatTitle.Text = c.Title;
                _lastMessageCount = -1; // Força recarregar
                LoadMessages(c.Id);
            }
        }

        // Desenha a lista de conversas customizada (Avatar + Nome)
        private void lstConversations_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;
            Conversation item = (Conversation)lstConversations.Items[e.Index];

            e.DrawBackground();
            bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;
            Brush textBrush = isSelected ? Brushes.White : Brushes.Black;
            if (UserSession.Theme == "dark" && !isSelected) textBrush = Brushes.White;

            // Avatar
            if (item.Avatar != null && item.Avatar.Length > 0)
            {
                using (var ms = new MemoryStream(item.Avatar))
                using (var img = Image.FromStream(ms))
                    e.Graphics.DrawImage(img, e.Bounds.X + 5, e.Bounds.Y + 5, 30, 30);
            }
            else
            {
                e.Graphics.FillEllipse(Brushes.Gray, e.Bounds.X + 5, e.Bounds.Y + 5, 30, 30);
            }

            // Nome
            e.Graphics.DrawString(item.Title, new Font("Segoe UI", 10, FontStyle.Bold), textBrush, e.Bounds.X + 45, e.Bounds.Y + 5);

            // Última Mensagem (Cinza)
            if (!string.IsNullOrEmpty(item.LastMessage))
                e.Graphics.DrawString(item.LastMessage, new Font("Segoe UI", 8), Brushes.Gray, e.Bounds.X + 45, e.Bounds.Y + 22);

            e.DrawFocusRectangle();
        }
    }
}