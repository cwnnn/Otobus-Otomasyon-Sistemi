using GMap.NET.Internals;
using Guna.UI2.WinForms;
using OtobusOtomasyonSistemi.KullaniciArayuz._1seferler;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OtobusOtomasyonSistemi
{

    public partial class FormKoltukSecmeSayfasi : Form
    {
        public bool koltukOnaylandiMi { get; private set; }  // hangi koltuk işlem aşamasında olduğunu belirlemek için
        public string tc { get; private set; } 

        public bool pasaportMu { get; private set; }
        public string ad { get; private set; }
        public string soyad { get; private set; }
        public string cinsiyet { get; private set; }
        public string tur { get; private set; }

        static string baglanti = "Data Source=LAPTOP-R03\\SQLEXPRESS;Initial Catalog=otobusOtomasyonu;Integrated Security=True;TrustServerCertificate=True";

        string tcMiPasMi;
        public FormKoltukSecmeSayfasi()
        {
            InitializeComponent();
        }

        private void FormKoltukSecmeSayfasi_Load(object sender, EventArgs e)
        {
            bool sonuc = GlobalData.biletListesi.Any(b => b.seferID == GlobalData.koltukSecmeSeferID && b.seferAdiK ==GlobalData.koltukSecmeseferAdiK && b.kendineBiletAldiMi == true);
            if (GlobalData.kullaniciID == 0 || GlobalData.kullaniciID == null || sonuc)
            {
                guna2RadioButton2.Checked = true;
                guna2RadioButton1.Enabled = false;
            }
            else
            {
                bool cins = GlobalData.kullaniciCinsiyet == "Erkek" ? true : false;
                hangiKoltukSecili(cins);
            }


            if (guna2RadioButton1.Checked == true)
            {

                panelYolcuBilgileri.Visible = false;
                this.Size = new Size(545, 332 - 83);
                guna2ButtonIptal.Location = new Point(112, 201);
                guna2ButtonOnayla.Location = new Point(279, 201);
            }

            tcMiPasMi = "TC Kimlik No";
            guna2TextBoxTC.MaxLength = 11;


            comboboxdoldur();
        }
        private void comboboxdoldur()
        {
            guna2ComboBoxTur.Items.Clear();

            using (SqlConnection conn = new SqlConnection(baglanti))
            {
                string query = "SELECT turID, turAdi FROM YolcuTurleri";
                SqlDataAdapter da = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);

                guna2ComboBoxTur.DisplayMember = "turAdi"; 
                guna2ComboBoxTur.ValueMember = "turID";   
                guna2ComboBoxTur.DataSource = dt;
            }
        }

        public void hangiKoltukSecili(bool sayi)
        {
            if (sayi == true)
            {
                guna2ImageButtonErkek.ImageSize = new Size(80, 80);
                guna2ImageButtonErkek.HoverState.ImageSize = new Size(80, 80);
                guna2ImageButtonErkek.PressedState.ImageSize = new Size(70, 70);

                guna2ImageButtonKadin.ImageSize = new Size(50, 50);
                guna2ImageButtonKadin.HoverState.ImageSize = new Size(60, 60);
                guna2ImageButtonKadin.PressedState.ImageSize = new Size(40, 40);

                cinsiyet = "Erkek";

            }
            else
            {
                guna2ImageButtonKadin.ImageSize = new Size(80, 80);
                guna2ImageButtonKadin.HoverState.ImageSize = new Size(80, 80);
                guna2ImageButtonKadin.PressedState.ImageSize = new Size(70, 70);

                guna2ImageButtonErkek.ImageSize = new Size(50, 50);
                guna2ImageButtonErkek.HoverState.ImageSize = new Size(60, 60);
                guna2ImageButtonErkek.PressedState.ImageSize = new Size(40, 40);

                cinsiyet = "Kadın";
            }
        }
        private void guna2ImageButtonErkek_Click(object sender, EventArgs e)
        {
            hangiKoltukSecili(true);
        }

        private void guna2ImageButtonKadin_Click(object sender, EventArgs e)
        {
            hangiKoltukSecili(false);
        }
        private void guna2RadioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2RadioButton1.Checked == true)
            {
                panelYolcuBilgileri.Visible = false;
                this.Size = new Size(545, 332 - 83);

                guna2ButtonIptal.Location = new Point(112, 201);
                guna2ButtonOnayla.Location = new Point(279, 201);
            }
        }

        private void guna2RadioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2RadioButton2.Checked == true)
            {

                panelYolcuBilgileri.Visible = true;
                this.Size = new Size(545, 332);
                guna2ButtonIptal.Location = new Point(112, 271);
                guna2ButtonOnayla.Location = new Point(279, 271);
            }
        }

        private void guna2TextBoxTC_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (guna2CheckBox2.Checked == false && !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
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

        private void guna2TextBox1_Enter(object sender, EventArgs e)
        {
            if (guna2TextBoxTC.Text == tcMiPasMi)
            {
                guna2TextBoxTC.Text = "";
                guna2TextBoxTC.ForeColor = Color.White;
            }
        }

        private void guna2TextBox1_Leave(object sender, EventArgs e)
        {
            if (guna2TextBoxTC.Text == "" || guna2TextBoxTC.Text == " ")
            {
                guna2TextBoxTC.Text = tcMiPasMi;
                guna2TextBoxTC.ForeColor = Color.FromArgb(44, 61, 94);
            }
        }

        private void guna2ComboBoxTur_SelectedIndexChanged(object sender, EventArgs e)
        {


        }
        private void guna2TextBoxAd_Enter(object sender, EventArgs e)
        {
            if (guna2TextBoxAd.Text == "Yolcu Adı")
            {
                guna2TextBoxAd.Text = "";
                guna2TextBoxAd.ForeColor = Color.White;
            }
        }

        private void guna2TextBoxAd_Leave(object sender, EventArgs e)
        {
            if (guna2TextBoxAd.Text == "" || guna2TextBoxAd.Text == " ")
            {
                guna2TextBoxAd.Text = "Yolcu Adı";
                guna2TextBoxAd.ForeColor = Color.FromArgb(44, 61, 94);
            }
        }
        private void guna2TextBoxSoyad_Enter(object sender, EventArgs e)
        {
            if (guna2TextBoxSoyad.Text == "Yolcu Soyadı")
            {
                guna2TextBoxSoyad.Text = "";
                guna2TextBoxSoyad.ForeColor = Color.White;
            }
        }

        private void guna2TextBoxSoyad_Leave(object sender, EventArgs e)
        {
            if (guna2TextBoxSoyad.Text == "" || guna2TextBoxSoyad.Text == " ")
            {
                guna2TextBoxSoyad.Text = "Yolcu Soyadı";
                guna2TextBoxSoyad.ForeColor = Color.FromArgb(44, 61, 94);
            }
        }

        private void guna2ButtonIptal_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void guna2ButtonOnayla_Click(object sender, EventArgs e)
        {
            if (cinsiyet!=null &&(guna2RadioButton1.Checked==true || (guna2TextBoxTC.Text != "TC Kimlik No" && guna2TextBoxTC.Text != "Pasaport No")
                &&((guna2CheckBox2.Checked == true && guna2TextBoxTC.Text.Length == 9 )
                || (guna2CheckBox2.Checked == false && guna2TextBoxTC.Text.Length == 11))
                && guna2TextBoxAd.Text != "Yolcu Adı" && guna2TextBoxSoyad.Text != "Yolcu Soyadı") )
            {
                koltukOnaylandiMi = true;
                tc = guna2TextBoxTC.Text;
                pasaportMu = guna2CheckBox2.Checked;
                ad = guna2TextBoxAd.Text;
                soyad = guna2TextBoxSoyad.Text;
                tur = guna2ComboBoxTur.Text;


                int satirIndex = GlobalData.seferlerListesi.AsEnumerable()
                  .ToList()
                  .FindIndex(row => row["seferID"].ToString() == GlobalData.koltukSecmeSeferID.ToString());

                

                if (guna2RadioButton2.Checked == true )
                {
                    
                    GlobalData.biletListesi.Add(new Biletler
                    {
                        kendineBiletAldiMi = false,
                        biletAktifMi = true,
                        seferID = GlobalData.koltukSecmeSeferID,
                        seferAdiK = GlobalData.koltukSecmeseferAdiK,
                        seferAdiV = GlobalData.koltukSecmeseferAdiV,
                        Ad = ad,
                        Soyad = soyad,
                        Cinsiyet = cinsiyet,
                        tur = tur,
                        turIndex = Convert.ToInt32(guna2ComboBoxTur.SelectedValue),
                        pasaportMu = guna2CheckBox2.Checked,
                        tc = tc,
                        koltukNo = GlobalData.gecicikoltukNo,
                        Fiyat = Convert.ToDecimal(GlobalData.seferlerListesi.Rows[satirIndex]["fiyat"].ToString()),
                        kalkisSehri = GlobalData.kalkisSehir,
                        varisSehri = GlobalData.varisSehir,
                        kalkisSaati = GlobalData.seferKalkisSaati,
                        tarihi = GlobalData.kalkisTarihi.ToString("dd/MM/yyyy")

                    });
                  
                }

                else
                {
                    GlobalData.biletListesi.Add(new Biletler
                    {
                        kendineBiletAldiMi = true,
                        biletAktifMi = true,
                        seferID = GlobalData.koltukSecmeSeferID,
                        seferAdiK = GlobalData.koltukSecmeseferAdiK,
                        seferAdiV = GlobalData.koltukSecmeseferAdiV,
                        Ad = GlobalData.kullaniciAd,
                        Soyad = GlobalData.kullaniciSoyad,
                        Cinsiyet = GlobalData.kullaniciCinsiyet,
                        tur = GlobalData.kullaniciTuru,
                        turIndex = GlobalData.kullaniciTurIndex,
                        pasaportMu = GlobalData.kullaniciPasaportMu,
                        tc = GlobalData.kullaniciTC,
                        koltukNo = GlobalData.gecicikoltukNo,
                        Fiyat = Convert.ToDecimal(GlobalData.seferlerListesi.Rows[satirIndex]["fiyat"].ToString()),
                        kalkisSehri = GlobalData.kalkisSehir,
                        varisSehri = GlobalData.varisSehir,
                        kalkisSaati = GlobalData.seferKalkisSaati,
                         tarihi = GlobalData.kalkisTarihi.ToString("dd/MM/yyyy")
                    });
                    
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Lütfen tüm alanları doldurunuz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }
    }
}
