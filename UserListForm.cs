using MySql.Data.MySqlClient;
using RobozllueApp;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Windows.Forms;

namespace Robozzle
{
    public partial class UserListForm : FormLoader
    {
        private int _targetUserId;
        private bool _showFollowers;

        public UserListForm(int userId, bool showFollowers)
        {
            InitializeComponent();
            this.PainelCentral = pnlList;

            _targetUserId = userId;
            _showFollowers = showFollowers;

            ConfigurarTitulo();
            ThemeManager.ApplyTheme(this);
            pnlList.BackColor = (UserSession.Theme == "dark") ? Color.FromArgb(40, 40, 40) : Color.WhiteSmoke;

            CarregarUsuarios();
        }

        private void ConfigurarTitulo()
        {
            lblTitle.Text = _showFollowers ? "Seguidores" : "Seguindo";
            this.Text = lblTitle.Text;
        }

        private void CarregarUsuarios()
        {
            pnlList.Controls.Clear();
            try
            {
                using (var conn = Database.GetConnection())
                {
                    string sql;

                    / --- MUDANÇA NA QUERY: Adicionamos subqueries para contar Fases e Seguidores ---
                    string commonSelect = @"
                        SELECT u.id, u.nome, u.avatar_image, u.descricao,
                               (SELECT COUNT(*) FROM followers sub WHERE sub.user_id = u.id) as num_seguidores,
                               (SELECT COUNT(*) FROM levels l WHERE l.author_id = u.id AND l.published = 1) as num_fases ";

                    if (_showFollowers)
                    {
                        // Quem segue o alvo
                        sql = commonSelect + @"
                                FROM users u 
                                JOIN followers f ON u.id = f.follower_id 
                                WHERE f.user_id = @uid";
                    }
                    else
                    {
                        // Quem o alvo segue
                        sql = commonSelect + @"
                                FROM users u 
                                JOIN followers f ON u.id = f.user_id 
                                WHERE f.follower_id = @uid";
                    }

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@uid", _targetUserId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (!reader.HasRows)
                            {
                                Label lblEmpty = new Label
                                {
                                    Text = "Nenhum usuário encontrado.",
                                    AutoSize = true,
                                    ForeColor = Color.Gray,
                                    Font = new Font("Segoe UI", 12),
                                    Margin = new Padding(20)
                                };
                                pnlList.Controls.Add(lblEmpty);
                            }

                            while (reader.Read())
                            {
                                int id = reader.GetInt32("id");
                                string nome = reader.GetString("nome");
                                byte[]? avatarBytes = reader.IsDBNull(reader.GetOrdinal("avatar_image")) ? null : (byte[])reader["avatar_image"];

                                // Lê os novos dados
                                string desc = reader.IsDBNull(reader.GetOrdinal("descricao")) ? "" : reader.GetString("descricao");
                                int seguidores = reader.GetInt32("num_seguidores");
                                int fases = reader.GetInt32("num_fases");

                                AdicionarCardUsuario(id, nome, avatarBytes, desc, seguidores, fases);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar lista: " + ex.Message);
            }
        }

        // Método atualizado para receber e mostrar os dados extras
        private void AdicionarCardUsuario(int id, string nome, byte[]? avatarBytes, string descricao, int seguidores, int fases)
        {
            bool isDark = UserSession.Theme == "dark";
            Color textColor = isDark ? Color.White : Color.FromArgb(64, 64, 64);
            Color subTextColor = isDark ? Color.LightGray : Color.Gray;

            // 1. CARD (Painel) - Altura aumentada para caber descrição e stats
            Panel pnlCard = new Panel();
            pnlCard.Size = new Size(360, 90);
            pnlCard.Margin = new Padding(10, 5, 10, 10);
            pnlCard.Cursor = Cursors.Hand;
            pnlCard.BorderStyle = BorderStyle.FixedSingle;
            pnlCard.BackColor = isDark ? Color.FromArgb(60, 60, 60) : Color.White;

            pnlCard.Click += (s, e) => AbrirPerfil(id);

            // 2. AVATAR
            PictureBox pb = new PictureBox();
            pb.Size = new Size(60, 60); // Avatar um pouco maior
            pb.Location = new Point(10, 15);
            pb.SizeMode = PictureBoxSizeMode.StretchImage;
            pb.BackColor = Color.Transparent;
            pb.Enabled = false;

            if (avatarBytes != null && avatarBytes.Length > 0)
            {
                using (var ms = new MemoryStream(avatarBytes)) pb.Image = Image.FromStream(ms);
            }
            else
            {
                try { pb.LoadAsync($"https://ui-avatars.com/api/?name={nome}&background=random&color=fff&size=128"); } catch { }
            }

            pb.Paint += (s, e) => {
                GraphicsPath gp = new GraphicsPath();
                gp.AddEllipse(0, 0, pb.Width, pb.Height);
                pb.Region = new Region(gp);
            };

            // 3. NOME
            Label lblNome = new Label();
            lblNome.Text = nome;
            lblNome.Location = new Point(80, 10);
            lblNome.AutoSize = true;
            lblNome.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            lblNome.ForeColor = textColor;
            lblNome.Enabled = false;

            // 4. DESCRIÇÃO (Truncada para caber em uma linha)
            string descCurta = descricao;
            if (descCurta.Length > 35) descCurta = descCurta.Substring(0, 35) + "...";
            if (string.IsNullOrWhiteSpace(descCurta)) descCurta = "Sem descrição.";

            Label lblDesc = new Label();
            lblDesc.Text = descCurta;
            lblDesc.Location = new Point(80, 35);
            lblDesc.Size = new Size(260, 20);
            lblDesc.Font = new Font("Segoe UI", 9F, FontStyle.Italic); // Itálico para diferenciar
            lblDesc.ForeColor = subTextColor;
            lblDesc.Enabled = false;

            // 5. ESTATÍSTICAS (Seguidores | Fases)
            Label lblStats = new Label();
            lblStats.Text = $"👥 {seguidores} Seguidores   🎮 {fases} Fases";
            lblStats.Location = new Point(80, 58);
            lblStats.AutoSize = true;
            lblStats.Font = new Font("Segoe UI", 8F, FontStyle.Bold);
            lblStats.ForeColor = Color.FromArgb(0, 120, 215); // Azul destaque
            lblStats.Enabled = false;

            // Adiciona tudo ao card
            pnlCard.Controls.Add(pb);
            pnlCard.Controls.Add(lblNome);
            pnlCard.Controls.Add(lblDesc);
            pnlCard.Controls.Add(lblStats);

            pnlList.Controls.Add(pnlCard);
        }

        private void AbrirPerfil(int userId)
        {
            ProfileForm profile = new ProfileForm(userId);
            this.Hide();
            profile.ShowDialog();
            this.Show();
        }

        private void btnClose_Click(object sender, EventArgs e) => this.Close();
    }
}