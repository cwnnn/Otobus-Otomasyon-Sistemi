using Guna.UI2.WinForms;
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

namespace OtobusOtomasyonSistemi.KullaniciArayuz._4Profilim.ProfilimDetay
{
    public partial class FormKullaniciBilgierli : Form
    {
        private FormProfilim _parentForm;
        public FormKullaniciBilgierli(FormProfilim formProfilim1)
        {
            InitializeComponent();
            _parentForm = formProfilim1;
        }
        static string baglanti = "Data Source=LAPTOP-R03\\SQLEXPRESS;Initial Catalog=otobusOtomasyonu;Integrated Security=True;TrustServerCertificate=True";
        SqlConnection baglan = new SqlConnection(baglanti);


        bool pasaportMu;
        string tcMiPasMi;


        private void FormKullaniciBilgierli_Load(object sender, EventArgs e)
        {
            baglan.Open();

            celCop();
            tcMiPasMi = "TC Kimlik No";
            guna2TextBoxTC.MaxLength = 11;
            KullaniciBilgileriYukleme();
        }

        private void KullaniciBilgileriYukleme()
        {
            guna2TextBoxAd.Text = GlobalData.kullaniciAd;
            guna2TextSoyad.Text = GlobalData.kullaniciSoyad;
            guna2TextEposta.Text = GlobalData.kullaniciEposta;

            if (GlobalData.kullaniciPasaportMu)
            {
                guna2CheckBox2.Checked = true;
                guna2TextBoxTC.ForeColor = Color.White;
            }
            else
            {
                guna2CheckBox2.Checked = false;
                guna2TextBoxTC.ForeColor = Color.White;
            }

            guna2TextBoxTC.Text = GlobalData.kullaniciTC;
            guna2TextBoxTelNo.Text = GlobalData.kullaniciTelefon;
            //şifre
            guna2TextBox2.Text = GlobalData.kullaniciTuru;

            if (GlobalData.kullaniciCinsiyet == "Kadın")
            {
                guna2ImageButtonFemale.PerformClick();
            }
            else
            {
                guna2ImageButtonMale.PerformClick();
            }


        }
        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            label2.Visible = true;
        }
        private void pictureBox1_MouseLeave(object sender, EventArgs e)
        {
            label2.Visible = false;
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
        }
        private void guna2ButtonOnayla_Click(object sender, EventArgs e)
        {
            if (guna2TextBoxAd.Text.Trim() == GlobalData.kullaniciAd && guna2TextSoyad.Text.Trim()== GlobalData.kullaniciSoyad &&
                guna2TextEposta.Text.Trim() == GlobalData.kullaniciEposta && guna2TextBoxTelNo.Text.Trim() == GlobalData.kullaniciTelefon &&
                guna2TextBoxTC.Text.Trim() == GlobalData.kullaniciTC && (cins ? "Kadın" : "Erkek") == GlobalData.kullaniciCinsiyet)
            { 
                MessageBox.Show("Bir Değişiklik yapmadınız", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            sqlBilgiGuncelleme();
            KullaniciBilgileriYukleme();


        }
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (guna2TextBoxSifreE.Text.Trim() == guna2TextBoxSifreE.Tag.ToString())
            {
                MessageBox.Show("Lütfen tüm alanları doldurunuz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (guna2TextBoxSifreY.Text.Trim() == guna2TextBoxSifreY.Tag.ToString())
            {
                MessageBox.Show("Lütfen tüm alanları doldurunuz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (guna2TextBoxSifreYT.Text.Trim() == guna2TextBoxSifreYT.Tag.ToString())
            {
                MessageBox.Show("Lütfen tüm alanları doldurunuz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (guna2TextBoxSifreY.Text != guna2TextBoxSifreYT.Text)
            {
                MessageBox.Show("Yeni şifreler birbirleriyle uyuşmuyor.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (guna2TextBoxSifreE.Text == guna2TextBoxSifreY.Text)
            {
                MessageBox.Show("Eski şifre ile yeni şifre aynı olamaz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (guna2TextBoxSifreE.Text != "Eski Şifre" && guna2TextBoxSifreY.Text != "Yeni Şifre" && guna2TextBoxSifreYT.Text != "Yeni Şifre (Tekrar)")
            {
                sqlSifreYenileme();


            }
            else
            {
                MessageBox.Show("Lütfen tüm alanları doldurunuz.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }


        private void guna2TextBoxSifreg_Enter(object sender, EventArgs e)
        {
            Guna2TextBox basilan = sender as Guna2TextBox;
            if (basilan.Text == basilan.Tag.ToString())
            {
                basilan.Text = "";
                if (sifre1goz || sifre2goz || sifre3goz)
                {
                    basilan.PasswordChar = '*';
                }
                basilan.ForeColor = Color.White;
            }
        }
        private void guna2TextBoxSifreg_Leave(object sender, EventArgs e)
        {
            Guna2TextBox basilan = sender as Guna2TextBox;
            if (basilan.Text == "" || basilan.Text == " ")
            {
                basilan.Text = basilan.Tag.ToString();
                basilan.PasswordChar = '\0';
                basilan.ForeColor = Color.FromArgb(44, 61, 94);
            }
        }
        bool sifre1goz = true;
        private void guna2ImageButtonGoz1_Click(object sender, EventArgs e)
        {
            if (sifre1goz)
            {
                guna2TextBoxSifreE.PasswordChar = '\0';
                guna2ImageButtonGoz1.Image = Image.FromFile(@"C:\Users\ASUS\Desktop\Bitirme Projesi\tasarım\icons8-eye-50.png");
                sifre1goz = false;
            }
            else
            {
                if (guna2TextBoxSifreE.Text != "Eski Şifre")
                {
                    guna2TextBoxSifreE.PasswordChar = '*';
                }
                guna2ImageButtonGoz1.Image = Image.FromFile(@"C:\Users\ASUS\Desktop\Bitirme Projesi\tasarım\icons8-closed-eye-50.png");
                sifre1goz = true;
            }
        }
        bool sifre2goz = true;
        private void guna2ImageButtonGoz2_Click(object sender, EventArgs e)
        {
            if (sifre2goz)
            {
                guna2TextBoxSifreY.PasswordChar = '\0';
                guna2ImageButtonGoz2.Image = Image.FromFile(@"C:\Users\ASUS\Desktop\Bitirme Projesi\tasarım\icons8-eye-50.png");
                sifre2goz = false;
            }
            else
            {
                if (guna2TextBoxSifreY.Text != "Yeni Şifre")
                {
                    guna2TextBoxSifreY.PasswordChar = '*';
                }
                guna2ImageButtonGoz2.Image = Image.FromFile(@"C:\Users\ASUS\Desktop\Bitirme Projesi\tasarım\icons8-closed-eye-50.png");
                sifre2goz = true;
            }
        }
        bool sifre3goz = true;
        private void guna2ImageButtonGoz3_Click(object sender, EventArgs e)
        {
            if (sifre3goz)
            {
                guna2TextBoxSifreYT.PasswordChar = '\0';
                guna2ImageButtonGoz3.Image = Image.FromFile(@"C:\Users\ASUS\Desktop\Bitirme Projesi\tasarım\icons8-eye-50.png");
                sifre3goz = false;
            }
            else
            {
                if (guna2TextBoxSifreYT.Text != "Yeni Şifre")
                {
                    guna2TextBoxSifreYT.PasswordChar = '*';
                }
                guna2ImageButtonGoz3.Image = Image.FromFile(@"C:\Users\ASUS\Desktop\Bitirme Projesi\tasarım\icons8-closed-eye-50.png");
                sifre3goz = true;
            }
        }
        private void sqlSifreYenileme()
        {
            string sorgu = "update Yolcular set sifre = @yeniSifre where yolcuID = @yolcuID and sifre =@eskiSifre; ";
            using (SqlCommand cmd = new SqlCommand(sorgu, baglan))
            {
                cmd.Parameters.AddWithValue("@yolcuID", GlobalData.kullaniciID.ToString());
                cmd.Parameters.AddWithValue("@eskiSifre", guna2TextBoxSifreE.Text);
                cmd.Parameters.AddWithValue("@yeniSifre", guna2TextBoxSifreY.Text);

                int sonuc = cmd.ExecuteNonQuery();

                if (sonuc > 0)
                    MessageBox.Show("Güncelleme başarılı!");
                else
                    MessageBox.Show("Şifreniz Yanlış.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }
        private void sqlBilgiGuncelleme()
        {
            string sorgu = "update Yolcular set ad = @ad, soyad = @soyad, cinsiyet = @cins, ePosta = @eposta, telefon =@tel, tc = @tc, pasaportMu = @pasaportMu where yolcuID = @id;";
            using (SqlCommand cmd = new SqlCommand(sorgu, baglan))
            {
                cmd.Parameters.AddWithValue("@id", GlobalData.kullaniciID.ToString());
                cmd.Parameters.AddWithValue("@ad", guna2TextBoxAd.Text);
                cmd.Parameters.AddWithValue("@soyad", guna2TextSoyad.Text);
                cmd.Parameters.AddWithValue("@cins", cins ? "K" : "E");
                cmd.Parameters.AddWithValue("@eposta", guna2TextEposta.Text);
                cmd.Parameters.AddWithValue("@tel", guna2TextBoxTelNo.Text);
                cmd.Parameters.AddWithValue("@tc", guna2TextBoxTC.Text);
                cmd.Parameters.AddWithValue("@pasaportMu", guna2CheckBox2.Checked);

                int sonuc = cmd.ExecuteNonQuery();

                if (sonuc > 0)
                    MessageBox.Show("Güncelleme başarılı!");
                else
                {
                    MessageBox.Show("Güncelleme başarısız.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                GlobalData.kullaniciAd = guna2TextBoxAd.Text;
                GlobalData.kullaniciSoyad = guna2TextSoyad.Text;
                GlobalData.kullaniciCinsiyet = cins ? "Kadın" : "Erkek";
                GlobalData.kullaniciEposta = guna2TextEposta.Text;
                GlobalData.kullaniciTelefon = guna2TextBoxTelNo.Text;
                GlobalData.kullaniciTC = guna2TextBoxTC.Text;
                GlobalData.kullaniciPasaportMu = guna2CheckBox2.Checked;
            }
        }

        private void celCop()
        {
            guna2ImageButtonMale.HoverState.ImageSize = new Size(60, 60);
            guna2ImageButtonMale.PressedState.ImageSize = new Size(40, 40);

            guna2ImageButtonFemale.HoverState.ImageSize = new Size(55, 55);
            guna2ImageButtonFemale.PressedState.ImageSize = new Size(40, 40);


            guna2ImageButtonGoz1.HoverState.ImageSize = new Size(32, 32);
            guna2ImageButtonGoz1.PressedState.ImageSize = new Size(28, 28);
            guna2ImageButtonGoz2.HoverState.ImageSize = new Size(32, 32);
            guna2ImageButtonGoz2.PressedState.ImageSize = new Size(28, 28);
            guna2ImageButtonGoz3.HoverState.ImageSize = new Size(32, 32);
            guna2ImageButtonGoz3.PressedState.ImageSize = new Size(28, 28);

        }

       
    }
}
