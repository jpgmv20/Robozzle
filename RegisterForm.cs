using MySql.Data.MySqlClient;
using RobozllueApp;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using BCrypt.Net;

namespace Robozzle
{
    public partial class RegisterForm : FormLoader
    {
        private byte[] avatarBytes = null;
        private string avatarMime = "image/jpeg";

        public RegisterForm()
        {
            InitializeComponent();

            this.PainelCentral = pnlCentral;
        }

        
        private void btnFoto_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Imagens (*.jpg;*.png)|*.jpg;*.jpeg;*.png";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Carrega a imagem original
                    using (Image imgOriginal = Image.FromFile(ofd.FileName))
                    {
                        // Redimensiona para evitar o erro "packet too large" do MySQL
                        // Tamanho 200x200 é suficiente para avatar
                        Image imgRedimensionada = RedimensionarImagem(imgOriginal, 200, 200);

                        // Mostra no PictureBox
                        pbAvatar.Image = imgRedimensionada;

                        // Converte para Bytes (Sempre salvando como JPEG para economizar espaço)
                        using (MemoryStream ms = new MemoryStream())
                        {
                            imgRedimensionada.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                            avatarBytes = ms.ToArray();
                        }

                        avatarMime = "image/jpeg";
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro ao processar imagem: " + ex.Message);
                }
            }
        }

        // Método auxiliar para redimensionar a imagem
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

        
        private void btnCadastrar_Click(object sender, EventArgs e)
        {
            string nome = txtNome.Text.Trim();
            string email = txtEmail.Text.Trim();
            string senha = txtSenha.Text;

            if (string.IsNullOrEmpty(nome) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(senha))
            {
                MessageBox.Show("Preencha todos os campos.");
                return;
            }

            // Gera Hash da senha
            string hashSenha = BCrypt.Net.BCrypt.HashPassword(senha);

            try
            {
                using (var conn = Database.GetConnection())
                {
                    using (var cmd = new MySqlCommand("create_user", conn))
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("p_nome", nome);
                        cmd.Parameters.AddWithValue("p_descricao", "Usuário Desktop");
                        cmd.Parameters.AddWithValue("p_email", email);
                        cmd.Parameters.AddWithValue("p_password", hashSenha);
                        cmd.Parameters.AddWithValue("p_image_type", avatarMime);

                        // Passa o array de bytes 
                        cmd.Parameters.AddWithValue("p_avatar_image", avatarBytes);

                        cmd.Parameters.AddWithValue("p_config", "{\"tema\": \"theme-dark\"}");

                        cmd.ExecuteNonQuery();


                        this.Close();
                    }
                }
            }
            catch (MySqlException ex)
            {
                if (ex.Number == 1062)
                    MessageBox.Show("Este email já está cadastrado ou nome já utilizado.");
                else if (ex.Message.Contains("max_allowed_packet"))
                    MessageBox.Show("A imagem ainda é muito grande. Tente uma menor.");
                else
                    MessageBox.Show("Erro ao cadastrar: " + ex.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro genérico: " + ex.Message);
            }
        }
    }
}