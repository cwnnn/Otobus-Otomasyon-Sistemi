using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace OtobusOtomasyonSistemi.KullaniciArayuz._4Giris
{
    public partial class kEpostaOnayForm : Form
    {
        public kEpostaOnayForm()
        {
            InitializeComponent();
        }
       
        private void kEpostaOnayForm_Load(object sender, EventArgs e)
        {

            epostayaKodGonder();
            if (this.Tag == "girisform")
            {
                labelUyari.Text = "E posta adersinize gelen güvenlik kodunu giriniz.";
            }
            else if (this.Tag == "kayitform")
            {
                labelUyari.Text = "E posta adersinize gelen doğrulama kodunu giriniz.";
            }
            else
            {
                labelUyari.Text = "E-posta adresi bulunamadı.";
            }

            this.ActiveControl = label1;
            timer1.Start();
        }
        int sayac = 180;
        private int kod = 0;

        private void guna2TextBoxSifre_Enter(object sender, EventArgs e)
        {
            if (guna2TextBoxSifre.Text == "Onay Kodu")
            {
                guna2TextBoxSifre.Text = "";
                guna2TextBoxSifre.ForeColor = Color.White;
            }

        }

        private void guna2TextBoxSifre_Leave(object sender, EventArgs e)
        {
            if (guna2TextBoxSifre.Text == "" || guna2TextBoxSifre.Text == " ")
            {
                guna2TextBoxSifre.Text = "Onay Kodu";
                guna2TextBoxSifre.ForeColor = Color.FromArgb(44, 61, 94);
            }
        }

        private void labelGeriSayim()
        {
            TimeSpan time = TimeSpan.FromSeconds(sayac);
            labelSayac.Text = time.ToString(@"mm\:ss");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (sayac > 0)
            {
                sayac--;
                labelGeriSayim();
            }
            else
            {
                timer1.Stop();
                labelUyari.Text = "Onay Kodu Süresi Doldu";
                labelUyari.ForeColor = Color.Red;
                guna2TextBoxSifre.Enabled = false;
                guna2Button1.Enabled = false;
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
                        Host = "smtp.gmail.com",
                        Port = 587,
                        EnableSsl = true,
                        Credentials = new NetworkCredential("wootturizm@gmail.com", "vcor thxn vsnw pgch")
                    };

                    MailMessage mail = new MailMessage()
                    {
                        From = new MailAddress("wootturizm@gmail.com"),
                        Subject = "WOOT TURİZM"
                    };

                    string icerik = "";

                    if (this.Tag?.ToString() == "girisform")
                    {
                        mail.Subject += " Giriş";
                        icerik = "Merhaba " + GlobalData.kullaniciEposta + "," + Environment.NewLine +
                                 "Woot Turizm hesabınızla oturum açma isteği aldık." + Environment.NewLine +
                                 "Tek kullanımlık giriş kodunuz: " + kod.ToString() + Environment.NewLine + Environment.NewLine +
                                 "Eğer bu isteği siz yapmadıysanız bu e-postayı görmezden gelebilirsiniz." + Environment.NewLine +
                                 "Başka biri yanlışlıkla sizin e-posta adresinizi girmiş olabilir." + Environment.NewLine + Environment.NewLine +
                                 "Teşekkürler," + Environment.NewLine +
                                 "Woot Turizm Destek Ekibi";
                    }
                    else if (this.Tag?.ToString() == "kayitform")
                    {
                        mail.Subject += " Kayıt";
                        icerik = "Merhaba " + GlobalData.kullaniciEposta + "," + Environment.NewLine +
                                 "Woot Turizm'e kayıt olmak üzere bir istek aldık." + Environment.NewLine +
                                 "Hesabınızı oluşturmak için gerekli doğrulama kodunuz: " + kod.ToString() + Environment.NewLine + Environment.NewLine +
                                 "Eğer bu kayıt isteği size ait değilse, bu e-postayı yok sayabilirsiniz." + Environment.NewLine +
                                 "Hesabınız oluşturulmamış olacaktır." + Environment.NewLine + Environment.NewLine +
                                 "Woot Turizm'e gösterdiğiniz ilgi için teşekkür ederiz." + Environment.NewLine +
                                 "Woot Turizm Kayıt Ekibi";
                    }
                    else
                    {
                        icerik = "Kod: " + kod.ToString(); 
                    }


                    mail.Body = icerik;
                    mail.To.Add(GlobalData.kullaniciEposta);

                    smtpServer.Send(mail);
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }

        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (guna2TextBoxSifre.Text == "Onay Kodu")
            {
                MessageBox.Show("Lütfen Onay Kodunu Giriniz");
                return;
            }
            if (guna2TextBoxSifre.Text == "")
            {
                MessageBox.Show("Lütfen Onay Kodunu Giriniz");
                return;
            }
            if (Convert.ToInt32(guna2TextBoxSifre.Text) ==kod)
            {
                foreach (Form form in Application.OpenForms.Cast<Form>().ToList())
                {
                    if (!(form is FormYukleme))
                    {
                        form.Close();
                    }
                }

                FormYukleme formYukleme = Application.OpenForms["FormYukleme"] as FormYukleme;
                formYukleme?.Show();
                formYukleme.timer1.Start();
                this.Close();
            }
        }

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {
            timer1.Stop();
            epostayaKodGonder();
            sayac = 180;
            timer1.Start();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                guna2TextBoxSifre.ForeColor = Color.White;
                guna2TextBoxSifre.Text = kod.ToString();

            }
        }

        private void label4_Click(object sender, EventArgs e)
        {
            GlobalData.kullaniciID = null;
            GlobalData.kullaniciAd = "";
            GlobalData.kullaniciSoyad = "";
            GlobalData.kullaniciEposta = "";
            GlobalData.kullaniciTelefon = "";
            GlobalData.kullaniciCinsiyet = "";
            GlobalData.kullaniciTuru = "";
            GlobalData.kullaniciTurIndex = -1;
            GlobalData.kullaniciTC = "";
            GlobalData.kullaniciPasaportMu = false;

            this.Close();


        }
    }
}
