using OtobusOtomasyonSistemi.AdminArayuz;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace OtobusOtomasyonSistemi
{
    public partial class kGirisForm : Form
    {
        bool acKapa = true;
        public kGirisForm()
        {
            InitializeComponent();
        }
        static string baglanti = "Data Source=LAPTOP-R03\\SQLEXPRESS;Initial Catalog=otobusOtomasyonu;Integrated Security=True;TrustServerCertificate=True";
        SqlConnection baglan = new SqlConnection(baglanti);

        private void kGirisForm_Load(object sender, EventArgs e)
        {
            labelUyari.Text = "";
            baglan.Open();
        }

        private void guna2ImageButton1_Click_1(object sender, EventArgs e)
        {
            if (acKapa)
            {

                guna2TextBoxSifre.PasswordChar = '\0';
                guna2ImageButton1.Image = Image.FromFile(@"C:\Users\ASUS\Desktop\Bitirme Projesi\tasarım\icons8-eye-50.png");
                acKapa = false;
            }
            else
            {
                if (guna2TextBoxSifre.Text != "Şifre")
                {
                    guna2TextBoxSifre.PasswordChar = '*';
                }

                guna2ImageButton1.Image = Image.FromFile(@"C:\Users\ASUS\Desktop\Bitirme Projesi\tasarım\icons8-closed-eye-50.png");
                acKapa = true;
            }
        }

        private void labelKayitOl_Click(object sender, EventArgs e)
        {

            this.Hide();
            kKayitOlForm kayitf = new kKayitOlForm();
            kayitf.ShowDialog();
            this.Close();
        }

        private void labelKayitOl_MouseMove(object sender, MouseEventArgs e)
        {
            labelKayitOl.ForeColor = Color.FromArgb(255, 152, 0);
        }

        private void labelKayitOl_MouseLeave(object sender, EventArgs e)
        {
            labelKayitOl.ForeColor = Color.LightGray;
        }

        private void label3_Click(object sender, EventArgs e)
        {
            this.Hide();
            FormSifreYenileme fs =new FormSifreYenileme();
            fs.ShowDialog();
            this.Close();
        }

        private void label3_MouseMove(object sender, MouseEventArgs e)
        {
            label3.ForeColor = Color.FromArgb(215, 41, 13);
        }

        private void label3_MouseLeave(object sender, EventArgs e)
        {
            label3.ForeColor = Color.White;
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

        private void guna2TextBoxKullanıcıAdi_Enter(object sender, EventArgs e)
        {
            if (guna2TextBoxEposta.Text == "E-posta")
            {
                guna2TextBoxEposta.Text = "";
                guna2TextBoxEposta.ForeColor = Color.White;
            }
        }

        private void guna2TextBoxKullanıcıAdi_Leave(object sender, EventArgs e)
        {
            if (guna2TextBoxEposta.Text == "" || guna2TextBoxEposta.Text == " ")
            {
                guna2TextBoxEposta.Text = "E-posta";
                guna2TextBoxEposta.ForeColor = Color.FromArgb(44, 61, 94);
            }

        }

        private void guna2TextBoxSifre_Enter(object sender, EventArgs e)
        {
            if (guna2TextBoxSifre.Text == "Şifre")
            {
                guna2TextBoxSifre.Text = "";
                if (acKapa)
                {
                    guna2TextBoxSifre.PasswordChar = '*';
                }
                guna2TextBoxSifre.ForeColor = Color.White;
            }
        }

        private void guna2TextBoxSifre_Leave(object sender, EventArgs e)
        {
            if (guna2TextBoxSifre.Text == "")
            {
                guna2TextBoxSifre.Text = "Şifre";
                guna2TextBoxSifre.PasswordChar = '\0';
                guna2TextBoxSifre.ForeColor = Color.FromArgb(44, 61, 94);
            }
        }

        private void sqlgirisSOrgusu(string eposta, string sifre)
        {
            string sorguEposta = "SELECT COUNT(*) FROM Yolcular WHERE eposta=@eposta";

            SqlCommand komut = new SqlCommand(sorguEposta, baglan);
            komut.Parameters.AddWithValue("@eposta", eposta);
            int emailExists = (int)komut.ExecuteScalar();

            if (emailExists == 0)
            {
                labelUyari.Text = "E-posta yanlış";
                return;
            }
            string sorguSifre = "SELECT y.*, yt.turAdi  FROM Yolcular y join YolcuTurleri yt on y.kullaniciTuruID = yt.turID WHERE ePosta = @eposta AND sifre = @sifre";
            using (SqlCommand cmd = new SqlCommand(sorguSifre, baglan))
            {
                cmd.Parameters.AddWithValue("@eposta", eposta);
                cmd.Parameters.AddWithValue("@sifre", sifre);

                GlobalData.kullaniciEposta = guna2TextBoxEposta.Text;

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
                        epostaOnayForm.Tag = "girisform";
                        epostaOnayForm.ShowDialog();
                        this.Hide();

                    }
                    else
                    {
                        labelUyari.Text = "Şifre yanlış";
                        return;
                    }
                }

            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (guna2TextBoxEposta.Text == "E-posta" || guna2TextBoxSifre.Text == "Şifre")
            {
                labelUyari.Text = "Lütfen tüm alanları doldurun";
                return;
            }
            if (guna2TextBoxEposta.Text.Replace(" ", "") == "" || guna2TextBoxSifre.Text.Replace(" ", "") == "")
            {
                labelUyari.Text = "Lütfen tüm alanları doldurun";
                return;
            }

            if (sqlAdminGiris(guna2TextBoxEposta.Text, guna2TextBoxSifre.Text))
            {
                foreach (Form form in Application.OpenForms.Cast<Form>().ToList())
                {
                    if (!(form is FormYukleme)) 
                    {
                        form.Close();
                    }
                }

                FormAdminArayuz1 formAdminArayuz1 = new FormAdminArayuz1();
                formAdminArayuz1.Show();
                this.Close();
                return;
            }
       

            sqlgirisSOrgusu(guna2TextBoxEposta.Text, guna2TextBoxSifre.Text);
        }

        private bool sqlAdminGiris(string tc, string sifre)
        {
            string sorguEposta = "select calisanID from Calisanlar where tc = @tc and sifre = @sifre;";

            SqlCommand komut = new SqlCommand(sorguEposta, baglan);
            komut.Parameters.AddWithValue("@tc", tc);
            komut.Parameters.AddWithValue("@sifre", sifre);


                using (SqlDataReader reader = komut.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        GlobalData.adminID = reader.GetInt32(0);



                    return true;
                }
                    else
                    {
                        labelUyari.Text = "Şifre yanlış";
                        return false;
                    }
                }
            }
    }
}
