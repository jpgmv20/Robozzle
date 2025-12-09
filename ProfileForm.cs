using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using RobozllueApp;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace Robozzle
{
    // HERANÇA ALTERADA AQUI
    public partial class ProfileForm : FormLoader
    {
        private byte[]? _avatarBytes;
        private bool _avatarChanged = false;

        public ProfileForm()
        {
            InitializeComponent();

            // Define qual painel será centralizado pelo FormLoader
            this.PainelCentral = pnlCentral;

            LoadUserData();

            // Aplica o tema atual
            ThemeManager.ApplyTheme(this);
        }

        private void LoadUserData()
        {
            // Carrega avatar da sessão
            if (UserSession.Avatar != null)
                pbAvatar.Image = new Bitmap(UserSession.Avatar);

            // Marca o radio button correto
            if (UserSession.Theme == "dark") radDark.Checked = true;
            else radLight.Checked = true;

            // Busca descrição no banco
            try
            {
                using (var conn = Database.GetConnection())
                {
                    string sql = "SELECT descricao FROM users WHERE id = @id";
                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@id", UserSession.Id);
                        var result = cmd.ExecuteScalar();
                        if (result != null) txtDesc.Text = result.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao carregar descrição: " + ex.Message);
            }
        }

        private void btnChangeAvatar_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog { Filter = "Imagens|*.jpg;*.png;*.jpeg" };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    using (Image imgOriginal = Image.FromFile(ofd.FileName))
                    {
                        // Redimensiona para evitar o erro "packet too large"
                        Image imgRedim = RedimensionarImagem(imgOriginal, 200, 200);

                        pbAvatar.Image = imgRedim;

                        using (MemoryStream ms = new MemoryStream())
                        {
                            imgRedim.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                            _avatarBytes = ms.ToArray();
                        }
                        _avatarChanged = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao processar imagem: " + ex.Message);
                }
            }
        }

        // Método auxiliar de redimensionamento (reutilizado do cadastro)
        private Image RedimensionarImagem(Image img, int width, int height)
        {
            Bitmap b = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(b))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                g.DrawImage(img, 0, 0, width, height);
            }
            return b;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string newTheme = radDark.Checked ? "dark" : "light";
            string jsonConfig = JsonConvert.SerializeObject(new { tema = newTheme });
            string desc = txtDesc.Text;

            try
            {
                using (var conn = Database.GetConnection())
                {
                    // Monta a query dinamicamente se houve troca de avatar
                    string sql = "UPDATE users SET descricao = @desc, config = @cfg" +
                                 (_avatarChanged ? ", avatar_image = @img, image_type = 'image/jpeg'" : "") +
                                 " WHERE id = @id";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@desc", desc);
                        cmd.Parameters.AddWithValue("@cfg", jsonConfig);
                        cmd.Parameters.AddWithValue("@id", UserSession.Id);

                        if (_avatarChanged)
                            cmd.Parameters.AddWithValue("@img", _avatarBytes);

                        cmd.ExecuteNonQuery();
                    }
                }

                // Atualiza a Sessão Local
                UserSession.Theme = newTheme;

                // Se mudou a imagem, atualiza na sessão também
                if (_avatarChanged && pbAvatar.Image != null)
                {
                    if (UserSession.Avatar != null) UserSession.Avatar.Dispose();
                    UserSession.Avatar = new Bitmap(pbAvatar.Image);
                }

                MessageBox.Show("Perfil atualizado com sucesso!");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro ao salvar: " + ex.Message);
            }
        }

        private void btnVoltar_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}