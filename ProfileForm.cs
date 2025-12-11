using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using RobozllueApp;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Robozzle
{
    public partial class ProfileForm : FormLoader
    {
        private int _targetUserId;
        private bool _isMyProfile;
        private bool _isFollowing;
        private byte[]? _newAvatarBytes;
        private bool _avatarChanged = false;

        public ProfileForm() : this(UserSession.Id) { }

        public ProfileForm(int userId)
        {
            InitializeComponent();
            this.PainelCentral = pnlCentral;

            _targetUserId = userId;
            _isMyProfile = (_targetUserId == UserSession.Id);

            ConfigurarInterface();
            CarregarDados();
            ThemeManager.ApplyTheme(this);
        }

        private void ConfigurarInterface()
        {
            if (_isMyProfile)
            {
                lblTitle.Text = "Meu Perfil";
                grpConfig.Visible = true;
                btnEditAvatar.Visible = true;
                btnSave.Visible = true;
                btnFollow.Visible = false;
                txtDesc.ReadOnly = false;
            }
            else
            {
                lblTitle.Text = "Perfil de Usuário";
                grpConfig.Visible = false;
                btnEditAvatar.Visible = false;
                btnSave.Visible = false;
                btnFollow.Visible = true;
                txtDesc.ReadOnly = true;
                btnVoltar.Location = new Point(50, 420);
                btnVoltar.Width = 300;
            }
        }

        private void CarregarDados()
        {
            try
            {
                using (var conn = Database.GetConnection())
                {
                    // --- QUERY CORRIGIDA ---
                    // Tabela: followers
                    // Quem é seguido: user_id
                    // Quem segue: follower_id
                    string sql = @"
                        SELECT u.nome, u.descricao, u.avatar_image, u.config,
                               (SELECT COUNT(*) FROM followers WHERE user_id = u.id) as seguidores,
                               (SELECT COUNT(*) FROM followers WHERE follower_id = u.id) as seguindo
                        FROM users u WHERE u.id = @uid";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@uid", _targetUserId);
                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                lblName.Text = reader.GetString("nome");
                                txtDesc.Text = reader.IsDBNull(1) ? "" : reader.GetString("descricao");

                                lblFollowers.Text = $"{reader.GetInt32("seguidores")} Seguidores";
                                lblFollowing.Text = $"{reader.GetInt32("seguindo")} Seguindo";

                                if (!reader.IsDBNull(2))
                                {
                                    byte[] img = (byte[])reader["avatar_image"];
                                    using (var ms = new MemoryStream(img)) pbAvatar.Image = Image.FromStream(ms);
                                }
                                else
                                {
                                    // Avatar padrão
                                    try { pbAvatar.LoadAsync($"https://ui-avatars.com/api/?name={lblName.Text}&background=random&color=fff&size=256"); } catch { }
                                }

                                if (_isMyProfile && !reader.IsDBNull(3))
                                {
                                    string json = reader.GetString("config");
                                    try
                                    {
                                        dynamic conf = JsonConvert.DeserializeObject(json);
                                        if (conf?.tema == "dark") radDark.Checked = true; else radLight.Checked = true;
                                    }
                                    catch { }
                                }
                            }
                        }
                    }

                    if (!_isMyProfile)
                    {
                        // --- QUERY CORRIGIDA ---
                        string sqlCheck = "SELECT COUNT(*) FROM followers WHERE follower_id = @me AND user_id = @target";
                        using (var cmd = new MySqlCommand(sqlCheck, conn))
                        {
                            cmd.Parameters.AddWithValue("@me", UserSession.Id);
                            cmd.Parameters.AddWithValue("@target", _targetUserId);
                            int count = Convert.ToInt32(cmd.ExecuteScalar());
                            _isFollowing = count > 0;
                            AtualizarBotaoSeguir();
                        }
                    }
                }
            }
            catch (Exception ex) { MessageBox.Show("Erro ao carregar: " + ex.Message); }
        }

        private void AtualizarBotaoSeguir()
        {
            if (_isFollowing)
            {
                btnFollow.Text = "Deixar de Seguir";
                btnFollow.BackColor = Color.Gray;
            }
            else
            {
                btnFollow.Text = "Seguir";
                btnFollow.BackColor = Color.FromArgb(0, 120, 215);
            }
        }

        private void btnFollow_Click(object sender, EventArgs e)
        {
            try
            {
                using (var conn = Database.GetConnection())
                {
                    string sql;
                    // --- QUERY CORRIGIDA ---
                    if (_isFollowing)
                        sql = "DELETE FROM followers WHERE follower_id = @me AND user_id = @target";
                    else
                        sql = "INSERT INTO followers (follower_id, user_id) VALUES (@me, @target)";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@me", UserSession.Id);
                        cmd.Parameters.AddWithValue("@target", _targetUserId);
                        cmd.ExecuteNonQuery();
                    }
                }
                _isFollowing = !_isFollowing;
                AtualizarBotaoSeguir();
                CarregarDados(); // Atualiza contadores
            }
            catch (Exception ex) { MessageBox.Show("Erro ao seguir: " + ex.Message); }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!_isMyProfile) return;

            string theme = radDark.Checked ? "dark" : "light";
            string cfg = JsonConvert.SerializeObject(new { tema = theme });

            try
            {
                using (var conn = Database.GetConnection())
                {
                    string sql = "UPDATE users SET descricao=@d, config=@c" +
                                 (_newAvatarBytes != null ? ", avatar_image=@img" : "") + " WHERE id=@id";
                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@d", txtDesc.Text);
                        cmd.Parameters.AddWithValue("@c", cfg);
                        cmd.Parameters.AddWithValue("@id", UserSession.Id);
                        if (_avatarChanged) cmd.Parameters.AddWithValue("@img", _newAvatarBytes);

                        cmd.ExecuteNonQuery();
                    }
                }
                UserSession.Theme = theme;
                if (_newAvatarBytes != null && pbAvatar.Image != null)
                    UserSession.Avatar = new Bitmap(pbAvatar.Image);

                this.Close();
            }
            catch (Exception ex) { MessageBox.Show("Erro ao salvar: " + ex.Message); }
        }

        private void btnEditAvatar_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog { Filter = "Imagens|*.jpg;*.png;*.jpeg" };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (Image imgOriginal = Image.FromFile(ofd.FileName))
                    {
                        Image imgRedim = RedimensionarImagem(imgOriginal, 200, 200);
                        pbAvatar.Image = imgRedim;
                        using (MemoryStream ms = new MemoryStream())
                        {
                            imgRedim.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                            _newAvatarBytes = ms.ToArray();
                        }
                        _avatarChanged = true;
                    }
                }
                catch { MessageBox.Show("Erro na imagem."); }
            }
        }

        private Image RedimensionarImagem(Image img, int w, int h)
        {
            Bitmap b = new Bitmap(w, h);
            using (Graphics g = Graphics.FromImage(b))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(img, 0, 0, w, h);
            }
            return b;
        }

        private void btnVoltar_Click(object sender, EventArgs e) => this.Close();

        private void lblFollowers_Click(object sender, EventArgs e)
        {
            UserListForm list = new UserListForm(_targetUserId, true);
            this.Hide(); list.ShowDialog(); this.Show();
        }

        private void lblFollowing_Click(object sender, EventArgs e)
        {
            UserListForm list = new UserListForm(_targetUserId, false);
            this.Hide(); list.ShowDialog(); this.Show();
        }

        private void btnMessage_Click(object sender, EventArgs e)
        {
            if (_isMyProfile) return;

            ChatRepository repo = new ChatRepository();
            // Cria ou pega conversa existente
            int convId = repo.StartPrivateChat(UserSession.Id, _targetUserId);

            ChatForm chat = new ChatForm(convId);
            this.Hide();
            chat.ShowDialog();
            this.Show();
        }
    }
}