using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using static Guna.UI2.WinForms.Suite.Descriptions;

namespace OtobusOtomasyonSistemi
{
    public partial class FormSifreYenileme : Form
    {
        static string baglanti = "Data Source=LAPTOP-R03\\SQLEXPRESS;Initial Catalog=otobusOtomasyonu;Integrated Security=True;TrustServerCertificate=True";
        SqlConnection baglan = new SqlConnection(baglanti);

        public FormSifreYenileme()
        {
            InitializeComponent();
        }

        int asamaSayisi = 1; //3 aşamadan oluşucak : 1 = eposta kontrol etme, 2 = kod doğrulama, 3 = sifre yenileme

        int dogrulamaKodu, yanlisGirmeSayisi = 0;

        private int kod = 0;
        private void FormSifreYenileme_Load(object sender, EventArgs e)
        {
            baglan.Open();
            this.ActiveControl = label1;
            this.Size = new Size(338, 389);
        }
        private void labelGiris_Click(object sender, EventArgs e)
        {
  
        }

        private void labelGiris_MouseMove(object sender, MouseEventArgs e)
        {
            labelGiris.ForeColor = Color.FromArgb(255, 152, 0);

        }

        private void labelGiris_MouseLeave(object sender, EventArgs e)
        {
            labelGiris.ForeColor = Color.LightGray;
        }

        private void labelKayitOl_Click(object sender, EventArgs e)
        {
        
        }

        private void labelKayitOl_MouseMove(object sender, MouseEventArgs e)
        {
            labelKayitOl.ForeColor = Color.FromArgb(255, 152, 0);
        }

        private void labelKayitOl_MouseLeave(object sender, EventArgs e)
        {
            labelKayitOl.ForeColor = Color.LightGray;
        }

        private void textBoxEposta_Enter(object sender, EventArgs e)
        {
            if (textBoxEposta.Text == "E-posta")
            {
                textBoxEposta.Text = "";
                textBoxEposta.ForeColor = Color.White;
            }
        }

        private void textBoxEposta_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxEposta.Text))
            {
                textBoxEposta.Text = "E-posta";
                textBoxEposta.ForeColor = Color.FromArgb(44, 61, 94);
            }
        }

        private void textBoxKod_Enter(object sender, EventArgs e)
        {
            if (textBoxKod.Text == "Kod")
            {
                textBoxKod.Text = "";
                textBoxKod.ForeColor = Color.White;
            }
        }
        private void textBoxKod_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxKod.Text))
            {
                textBoxKod.Text = "Kod";
                textBoxKod.ForeColor = Color.FromArgb(44, 61, 94);
            }
        }
        bool sifre1goz = true;
        private void pictureBoxGoz1_Click(object sender, EventArgs e)
        {
            if (sifre1goz)
            {
                textBoxSifre.PasswordChar = '\0';
                pictureBoxGoz1.Image = Properties.Resources.icons8_eye_50;
                sifre1goz = false;
            }
            else
            {
                if (textBoxSifre.Text != "Şifre")
                {
                    textBoxSifre.PasswordChar = '*';
                }
                pictureBoxGoz1.Image = Properties.Resources.icons8_closed_eye_50;
                sifre1goz = true;
            }
        }

        private void textBoxSifre_Enter(object sender, EventArgs e)
        {
            if (textBoxSifre.Text == "Şifre")
            {
                textBoxSifre.Text = "";
                if (sifre1goz)
                {
                    textBoxSifre.PasswordChar = '*';
                }
                textBoxSifre.ForeColor = Color.White;
            }
        }

        private void textBoxSifre_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxSifre.Text))
            {
                textBoxSifre.Text = "Şifre";
                textBoxSifre.PasswordChar = '\0';
                textBoxSifre.ForeColor = Color.FromArgb(44, 61, 94);
            }
        }

        bool sifre2goz = true;

        private void pictureBoxGoz2_Click(object sender, EventArgs e)
        {
            if (sifre2goz)
            {
                textBoxSifreTekrar.PasswordChar = '\0';
                pictureBoxGoz2.Image = Properties.Resources.icons8_eye_50;
                sifre2goz = false;
            }
            else
            {
                if (textBoxSifreTekrar.Text != "Şifre Tekrar")
                {
                    textBoxSifreTekrar.PasswordChar = '*';
                }
                pictureBoxGoz2.Image = Properties.Resources.icons8_closed_eye_50;
                sifre2goz = true;
            }
        }

        private void textBoxSifreTekrar_Enter(object sender, EventArgs e)
        {
            if (textBoxSifreTekrar.Text == "Şifre Tekrar")
            {
                textBoxSifreTekrar.Text = "";
                if (sifre2goz)
                {
                    textBoxSifreTekrar.PasswordChar = '*';
                }
                textBoxSifreTekrar.ForeColor = Color.White;
            }
        }

        private void textBoxSifreTekrar_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxSifreTekrar.Text))
            {
                textBoxSifreTekrar.Text = "Şifre Tekrar";
                textBoxSifreTekrar.PasswordChar = '\0';
                textBoxSifreTekrar.ForeColor = Color.FromArgb(44, 61, 94);
            }
        }

        private void pictureBoxKodTekrar_Click(object sender, EventArgs e)
        {
            Random rastgele = new Random();
            dogrulamaKodu = rastgele.Next(1000, 9999);
            MessageBox.Show("Yeni Kodunuz: " + dogrulamaKodu.ToString());
            button1.Text = "Doğrula";
            yanlisGirmeSayisi = 0;
        }



        private void button1_Click(object sender, EventArgs e)
        {
            if (asamaSayisi == 1)
            {
 
                

                sqlepostaSorgu();
                
            }
            else if (asamaSayisi == 2)
            {
                if (textBoxKod.Text == kod.ToString())
                {
                    panelKod.Enabled = false;
                    panelSifreler.Visible = true;
                    button1.Text = "Şifre Değiştir";
                    asamaSayisi = 3;
                    this.Size = new Size(338, 557);

                }
                else if (yanlisGirmeSayisi > 1)
                {
                    epostayaKodGonder();
                    MessageBox.Show("3 Kez yalnış girdiniz \nYeni Kodunuz: " + dogrulamaKodu.ToString());
                    yanlisGirmeSayisi = 0;
                }
                else
                {
                    MessageBox.Show("Kod yanlış.");
                    yanlisGirmeSayisi++;
                }
            }
            else if (asamaSayisi == 3)
            {
                if (textBoxSifre.Text == textBoxSifreTekrar.Text)
                {
                    sifreDegistir();
                    MessageBox.Show("Şifreniz başarıyla değiştirildi.");
                    this.Hide();

                }
                else
                {
                    MessageBox.Show("Şifreler eşleşmiyor.");
                }

            }


        }

        private void sqlepostaSorgu()
        {

            string girilenEposta = textBoxEposta.Text;

            using (SqlCommand komut = new SqlCommand("SELECT count(*) FROM Yolcular WHERE eposta = @eposta", baglan))
            {
                komut.Parameters.AddWithValue("@eposta", girilenEposta);
                int emailExists = (int)komut.ExecuteScalar();

                if (emailExists > 0)
                {
                    GlobalData.kullaniciEposta = girilenEposta;
                    // Kod gönderme işlemi
                    epostayaKodGonder();

                        // Arayüz güncellemeleri
                        asamaSayisi = 2;
                        panelEposta.Enabled = false;
                        panelKod.Visible = true;
                        button1.Text = "Doğrula";
                        this.Size = new Size(338, 435);
                    }
                    else
                    {
                        MessageBox.Show("E-posta adresi sistemde kayıtlı değil.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        

        private void kodUret()
        {
            Random random = new Random();
            kod = random.Next(100000, 999999);

        }

        private async void epostayaKodGonder()
        {
            kodUret();

            try
            {

                await Task.Run(() =>
                {
                    SmtpClient smtpServer = new SmtpClient("smtp.gmail.com")
                    {
                        Port = 587,
                        EnableSsl = true,
                        Credentials = new NetworkCredential("wootturizm@gmail.com", "vcor thxn vsnw pgch")
                    };

                    MailMessage mail = new MailMessage()
                    {
                        From = new MailAddress("wootturizm@gmail.com"),
                        Subject = "WOOT TURİZM - Şifre Sıfırlama Doğrulama Kodu",
                        Body = "Merhaba " + GlobalData.kullaniciEposta + "," + Environment.NewLine + Environment.NewLine +
                        "Woot Turizm hesabınız için bir şifre sıfırlama isteği aldık." + Environment.NewLine +
                        "Şifrenizi sıfırlamak için kullanmanız gereken tek kullanımlık doğrulama kodunuz:" + Environment.NewLine +
                        "**" + kod.ToString() + "**" + Environment.NewLine + Environment.NewLine +
                        "Bu isteği siz yapmadıysanız, lütfen bu e-postayı dikkate almayın. " +
                        "Başka biri yanlışlıkla sizin e-posta adresinizi girmiş olabilir." + Environment.NewLine + Environment.NewLine +
                        "İyi günler dileriz," + Environment.NewLine +
                        "Woot Turizm Destek Ekibi"
                    };

                    mail.To.Add(GlobalData.kullaniciEposta);

                    smtpServer.Send(mail);
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }

        }

        private void sifreDegistir()
        {

            SqlCommand komut = new SqlCommand("Update Yolcular set sifre=@sifre where eposta=@eposta", baglan);
            komut.Parameters.AddWithValue("@sifre", textBoxSifre.Text);
            komut.Parameters.AddWithValue("@eposta", textBoxEposta.Text);
            komut.ExecuteNonQuery();
            baglan.Close();
        }

        private void labelCikis_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
