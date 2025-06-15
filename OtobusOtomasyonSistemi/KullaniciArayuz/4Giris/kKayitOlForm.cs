using Guna.UI2.WinForms;
using OtobusOtomasyonSistemi.KullaniciArayuz._4Giris;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OtobusOtomasyonSistemi
{
    public partial class kKayitOlForm : Form
    {

        static string baglanti = "Data Source=LAPTOP-R03\\SQLEXPRESS;Initial Catalog=otobusOtomasyonu;Integrated Security=True;TrustServerCertificate=True";
        SqlConnection baglan = new SqlConnection(baglanti);


        public kKayitOlForm()
        {
            InitializeComponent();
        }

        string tcMiPasMi;


        private void kKayitOlForm_Load(object sender, EventArgs e)
        {
            celcop();

        }

        private void label2_Click(object sender, EventArgs e)
        {

            this.Hide();
            kGirisForm girisf = new kGirisForm();
            girisf.ShowDialog();
            this.Close();
        }

        private void label2_MouseMove(object sender, MouseEventArgs e)
        {
            label2.ForeColor = Color.FromArgb(255, 152, 0);
        }

        private void label2_MouseLeave(object sender, EventArgs e)
        {
            label2.ForeColor = Color.LightGray;
        }

        private void guna2TextBoxAd_Enter(object sender, EventArgs e)
        {
            if (guna2TextBoxAd.Text == "Ad")
            {
                guna2TextBoxAd.Text = "";
                guna2TextBoxAd.ForeColor = Color.White;
            }
        }

        private void guna2TextBoxAd_Leave(object sender, EventArgs e)
        {
            if (guna2TextBoxAd.Text == "" || guna2TextBoxAd.Text == " ")
            {
                guna2TextBoxAd.Text = "Ad";
                guna2TextBoxAd.ForeColor = Color.FromArgb(44, 61, 94);
            }
        }

        private void guna2TextSoyad_Enter(object sender, EventArgs e)
        {
            if (guna2TextSoyad.Text == "Soyad")
            {
                guna2TextSoyad.Text = "";
                guna2TextSoyad.ForeColor = Color.White;
            }
        }

        private void guna2TextSoyad_Leave(object sender, EventArgs e)
        {
            if (guna2TextSoyad.Text == "" || guna2TextSoyad.Text == " ")
            {
                guna2TextSoyad.Text = "Soyad";
                guna2TextSoyad.ForeColor = Color.FromArgb(44, 61, 94);
            }
        }

        private void guna2TextEposta_Enter(object sender, EventArgs e)
        {
            if (guna2TextEposta.Text == "E-posta")
            {
                guna2TextEposta.Text = "";
                guna2TextEposta.ForeColor = Color.White;
            }
        }

        private void guna2TextEposta_Leave(object sender, EventArgs e)
        {
            if (guna2TextEposta.Text == "" || guna2TextEposta.Text == " ")
            {
                guna2TextEposta.Text = "E-posta";
                guna2TextEposta.ForeColor = Color.FromArgb(44, 61, 94);
            }
        }
        private void guna2TextEposta_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ' ')
            {
                e.Handled = true;
            }
        }

        private void guna2TextBoxTelNo_Enter(object sender, EventArgs e)
        {
            if (guna2TextBoxTelNo.Text == "Telefon No")
            {
                guna2TextBoxTelNo.Text = "";
                guna2TextBoxTelNo.ForeColor = Color.White;
            }
        }

        private void guna2TextBoxTelNo_Leave(object sender, EventArgs e)
        {
            if (guna2TextBoxTelNo.Text == "" || guna2TextBoxTelNo.Text == " ")
            {
                guna2TextBoxTelNo.Text = "Telefon No";
                guna2TextBoxTelNo.ForeColor = Color.FromArgb(44, 61, 94);
            }
        }

        private void guna2TextBoxTelNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (guna2CheckBox2.Checked == false && !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
            if (e.KeyChar == ' ')
            {
                e.Handled = true;
            }
        }

        private void guna2CheckBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2CheckBox2.Checked == true)
            {
                tcMiPasMi = "Pasaport No";

                guna2TextBoxTC.Text = "Pasaport No";
                guna2TextBoxTC.ForeColor = Color.FromArgb(44, 61, 94);

                guna2TextBoxTC.MaxLength = 9;
            }
            else
            {

                guna2TextBoxTC.Text = "TC Kimlik No";
                guna2TextBoxTC.ForeColor = Color.FromArgb(44, 61, 94);

                tcMiPasMi = "TC Kimlik No";
                guna2TextBoxTC.MaxLength = 11;
            }
        }
        private void guna2TextBoxTC_Enter(object sender, EventArgs e)
        {
            if (guna2TextBoxTC.Text == tcMiPasMi)
            {
                guna2TextBoxTC.Text = "";
                guna2TextBoxTC.ForeColor = Color.White;
            }
        }
        private void guna2TextBoxTC_Leave(object sender, EventArgs e)
        {
            if (guna2TextBoxTC.Text == "" || guna2TextBoxTC.Text == " ")
            {
                guna2TextBoxTC.Text = tcMiPasMi;
                guna2TextBoxTC.ForeColor = Color.FromArgb(44, 61, 94);
            }
        }
        private void guna2TextBoxTC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (guna2CheckBox2.Checked == false && !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
            if (e.KeyChar == ' ')
            {
                e.Handled = true;
            }
        }


        bool cins; // false = erkek, true = kadın

        private void guna2ImageButtonMale_Click(object sender, EventArgs e)
        {
            cins = false;
            panelCinsiyetM.Visible = true;
            panelCinsiyetF.Visible = false;
        }

        private void guna2ImageButtonFemale_Click(object sender, EventArgs e)
        {
            cins = true;
            panelCinsiyetF.Visible = true;
            panelCinsiyetM.Visible = false;
        }

        bool sifre1goz = true;
        private void guna2ImageButtonGoz1_Click(object sender, EventArgs e)
        {
            if (sifre1goz)
            {
                guna2TextBoxSifreg.PasswordChar = '\0';
                guna2ImageButtonGoz1.Image = Image.FromFile(@"C:\Users\ASUS\Desktop\Bitirme Projesi\tasarım\icons8-eye-50.png");
                sifre1goz = false;
            }
            else
            {
                if (guna2TextBoxSifreg.Text != "Şifre")
                {
                    guna2TextBoxSifreg.PasswordChar = '*';
                }
                guna2ImageButtonGoz1.Image = Image.FromFile(@"C:\Users\ASUS\Desktop\Bitirme Projesi\tasarım\icons8-closed-eye-50.png");
                sifre1goz = true;
            }
        }

        private void guna2TextBoxSifreg_Enter(object sender, EventArgs e)
        {
            if (guna2TextBoxSifreg.Text == "Şifre")
            {
                guna2TextBoxSifreg.Text = "";
                if (sifre1goz)
                {
                    guna2TextBoxSifreg.PasswordChar = '*';
                }
                guna2TextBoxSifreg.ForeColor = Color.White;
            }
        }

        private void guna2TextBoxSifreg_Leave(object sender, EventArgs e)
        {
            if (guna2TextBoxSifreg.Text == "" || guna2TextBoxSifreg.Text == " ")
            {
                guna2TextBoxSifreg.Text = "Şifre";
                guna2TextBoxSifreg.PasswordChar = '\0';
                guna2TextBoxSifreg.ForeColor = Color.FromArgb(44, 61, 94);
            }
        }

        bool sifre2goz = true;

        private void guna2ImageButtonGoz2_Click(object sender, EventArgs e)
        {
            if (sifre2goz)
            {
                guna2TextBoxSifreTekrar.PasswordChar = '\0';
                guna2ImageButtonGoz2.Image = Image.FromFile(@"C:\Users\ASUS\Desktop\Bitirme Projesi\tasarım\icons8-eye-50.png");
                sifre2goz = false;
            }
            else
            {
                if (guna2TextBoxSifreTekrar.Text != "Şifre Tekrar")
                {
                    guna2TextBoxSifreTekrar.PasswordChar = '*';
                }
                guna2ImageButtonGoz2.Image = Image.FromFile(@"C:\Users\ASUS\Desktop\Bitirme Projesi\tasarım\icons8-closed-eye-50.png");
                sifre2goz = true;
            }
        }
        private void guna2TextBoxSifreTekrar_Enter(object sender, EventArgs e)
        {
            if (guna2TextBoxSifreTekrar.Text == "Şifre Tekrar")
            {
                guna2TextBoxSifreTekrar.Text = "";
                if (sifre2goz)
                {
                    guna2TextBoxSifreTekrar.PasswordChar = '*';
                }
                guna2TextBoxSifreTekrar.ForeColor = Color.White;
            }
        }

        private void guna2TextBoxSifreTekrar_Leave(object sender, EventArgs e)
        {
            if (guna2TextBoxSifreTekrar.Text == "" || guna2TextBoxSifreTekrar.Text == " ")
            {
                guna2TextBoxSifreTekrar.Text = "Şifre Tekrar";
                guna2TextBoxSifreTekrar.PasswordChar = '\0';
                guna2TextBoxSifreTekrar.ForeColor = Color.FromArgb(44, 61, 94);
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

            this.Close();
        }

        private void label4_MouseMove(object sender, MouseEventArgs e)
        {
            label4.ForeColor = Color.FromArgb(215, 41, 13);
        }

        private void label4_MouseLeave(object sender, EventArgs e)
        {
            label4.ForeColor = Color.FromArgb(255, 152, 0);
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (guna2TextBoxAd.Text != "Ad" && guna2TextSoyad.Text != "Soyad" && cins != null && guna2TextEposta.Text != "E-posta" && guna2TextBoxTelNo.Text != "Telefon No"
                && (guna2TextBoxTC.Text != "TC Kimlik No" && guna2TextBoxTC.Text != "Pasaport No")
                && ((guna2CheckBox2.Checked == true && guna2TextBoxTC.Text.Length == 9)
                || (guna2CheckBox2.Checked == false && guna2TextBoxTC.Text.Length == 11))
                && guna2TextBoxSifreg.Text != "Şifre" && guna2TextBoxSifreTekrar.Text != "Şifre Tekrar")
            {
                if (guna2TextBoxSifreg.Text == guna2TextBoxSifreTekrar.Text)
                {

                    sqlKullaniciEkleme();
                    sqlgirisSOrgusu(guna2TextEposta.Text, guna2TextBoxSifreg.Text);


                }
                else
                {
                    MessageBox.Show("Şifreler Uyuşmuyor");
                }
            }
            else
            {
                MessageBox.Show("Lütfen Tüm Alanları Doldurunuz");
            }
        }



        private void sqlKullaniciEkleme()
        {
            string sorgu = "INSERT INTO Yolcular (ad, soyad, cinsiyet, ePosta, kullaniciTuruID, telefon, sifre, tc, pasaportMu, kayitOlduMu) " +
                "VALUES (@ad, @soyad, @cinsiyet, @eposta, 1, @telno, @sifre, @tc, @pasapMu, 1);";

            try
            {
                baglan.Open();

                using (SqlCommand komut = new SqlCommand(sorgu, baglan))
                {
                    komut.Parameters.AddWithValue("@ad", guna2TextBoxAd.Text);
                    komut.Parameters.AddWithValue("@soyad", guna2TextSoyad.Text);
                    komut.Parameters.AddWithValue("@cinsiyet", cins ? "K" : "E");
                    komut.Parameters.AddWithValue("@eposta", guna2TextEposta.Text);
                    komut.Parameters.AddWithValue("@telno", guna2TextBoxTelNo.Text);
                    komut.Parameters.AddWithValue("@sifre", guna2TextBoxSifreg.Text);
                    komut.Parameters.AddWithValue("@tc", guna2TextBoxTC.Text);
                    komut.Parameters.AddWithValue("@pasapMu", guna2CheckBox2.Checked ? 1 : 0);

                    int satirSayisi = komut.ExecuteNonQuery();

                    if (satirSayisi == 0)
                    {
                        MessageBox.Show("Kayıt başarısız.");
                    }
                    else
                    {
                        MessageBox.Show("Kayıt başarılı.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message);
            }
            finally
            {
                if (baglan.State == ConnectionState.Open)
                    baglan.Close();
            }

        }

        private void sqlgirisSOrgusu(string eposta, string sifre)
            {

            string sorguSifre = "SELECT y.*, yt.turAdi  FROM Yolcular y join YolcuTurleri yt on y.kullaniciTuruID = yt.turID WHERE ePosta = @eposta AND sifre = @sifre";

            try
            {
                baglan.Open();

                using (SqlCommand cmd = new SqlCommand(sorguSifre, baglan))
                {
                    cmd.Parameters.AddWithValue("@eposta", eposta);
                    cmd.Parameters.AddWithValue("@sifre", sifre);

                    GlobalData.kullaniciEposta = eposta;

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            GlobalData.kullaniciID = reader.GetInt32(0);
                            GlobalData.kullaniciAd = reader.GetString(1);
                            GlobalData.kullaniciSoyad = reader.GetString(2);
                            GlobalData.kullaniciCinsiyet = reader.GetString(3) == "K" ? "Kadın" : "Erkek";
                            GlobalData.kullaniciTelefon = reader.GetString(6);
                            GlobalData.kullaniciTC = reader.GetString(8);
                            GlobalData.kullaniciPasaportMu = reader.GetBoolean(9);
                            GlobalData.kullaniciTuru = reader.GetString(11);
                            GlobalData.kullaniciTurIndex = reader.GetInt32(5);

                            kEpostaOnayForm epostaOnayForm = new kEpostaOnayForm();
                            epostaOnayForm.Tag = "kayitform";
                            epostaOnayForm.ShowDialog();
                            this.Hide();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Bir hata oluştu: " + ex.Message);
            }
            finally
            {
                if (baglan.State == ConnectionState.Open)
                    baglan.Close();
            }

        }

        private void celcop()
        {
            tcMiPasMi = "TC Kimlik No";
            guna2TextBoxTC.MaxLength = 11;

            guna2ImageButtonMale.PerformClick();

            guna2ImageButtonMale.HoverState.ImageSize = new Size(60, 60);
            guna2ImageButtonMale.PressedState.ImageSize = new Size(40, 40);

            guna2ImageButtonFemale.HoverState.ImageSize = new Size(55, 55);
            guna2ImageButtonFemale.PressedState.ImageSize = new Size(40, 40);


            guna2ImageButtonGoz1.HoverState.ImageSize = new Size(32, 32);
            guna2ImageButtonGoz1.PressedState.ImageSize = new Size(28, 28);

            guna2ImageButtonGoz2.HoverState.ImageSize = new Size(32, 32);
            guna2ImageButtonGoz2.PressedState.ImageSize = new Size(28, 28);
        }


    }
}
