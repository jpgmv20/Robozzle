using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using RobozllueApp;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using BCrypt.Net;

namespace Robozzle
{
    public partial class LoginForm : FormLoader
    {
        public LoginForm()
        {
            InitializeComponent();
            this.PainelCentral = pnlCentral;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string loginInput = txtEmail.Text.Trim();
            string senha = txtSenha.Text;

            if (string.IsNullOrEmpty(loginInput) || string.IsNullOrEmpty(senha))
            {
                MessageBox.Show("Preencha o login e a senha.");
                return;
            }

            try
            {
                using (var conn = Database.GetConnection())
                {
                    string sql = "SELECT id, nome, email, password, avatar_image, config FROM users WHERE email = @login OR nome = @login LIMIT 1";

                    using (var cmd = new MySqlCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@login", loginInput);

                        using (var reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string hashBanco = reader.GetString("password");

                                if (BCrypt.Net.BCrypt.Verify(senha, hashBanco))
                                {
                                    UserSession.Id = reader.GetInt32("id");
                                    UserSession.Nome = reader.GetString("nome");
                                    UserSession.Email = reader.GetString("email");

                                    
                                    if (!reader.IsDBNull(reader.GetOrdinal("avatar_image")))
                                    {
                                        byte[] imgBytes = (byte[])reader["avatar_image"];
                                        if (imgBytes.Length > 0)
                                        {
                                            using (var ms = new MemoryStream(imgBytes))
                                            {
                                                // Carrega a imagem do stream
                                                using (Image tempImg = Image.FromStream(ms))
                                                {
                                                    // Cria uma CÓPIA em memória (Bitmap) que não depende do stream 'ms'
                                                    UserSession.Avatar = new Bitmap(tempImg);
                                                }
                                            }
                                            // Agora o 'ms' pode fechar sem quebrar o UserSession.Avatar
                                        }
                                    }

                                    // Carrega Configuração (Tema)
                                    if (!reader.IsDBNull(reader.GetOrdinal("config")))
                                    {
                                        string configJson = reader.GetString("config");
                                        try
                                        {
                                            dynamic config = JsonConvert.DeserializeObject(configJson);
                                            if (config != null && config.tema != null)
                                            {
                                                UserSession.Theme = config.tema;
                                            }
                                        }
                                        catch { /* Ignora erro de JSON */ }
                                    }

                                    HomeForm home = new HomeForm();
                                    this.Hide();
                                    home.ShowDialog();
                                    this.Close();
                                }
                                else
                                {
                                    MessageBox.Show("Senha incorreta.");
                                }
                            }
                            else
                            {
                                MessageBox.Show("Usuário não encontrado.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Erro de conexão: " + ex.Message);
            }
        }

        private void lblRegister_Click(object sender, EventArgs e)
        {
            RegisterForm reg = new RegisterForm();
            this.Hide();
            reg.ShowDialog();
            this.Show();
        }
    }
}