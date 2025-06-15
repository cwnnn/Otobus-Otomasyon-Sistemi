using Guna.UI2.WinForms;
using OtobusOtomasyonSistemi.KullaniciArayuz._4Profilim;
using OtobusOtomasyonSistemi.KullaniciArayuz._4Profilim.ProfilimDetay;
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

namespace OtobusOtomasyonSistemi.KullaniciArayuz._1seferler
{
    public partial class FormSatinAlinacakBiletleriListele : Form
    {
        DataTable dt = new DataTable();

        private FormSatinAlmaIslemi _FormSatinAlmaIslemi;
        public FormSatinAlinacakBiletleriListele(FormSatinAlmaIslemi patent)
        {
            InitializeComponent();
            _FormSatinAlmaIslemi = patent;
        }

        private void FormSatinAlinacakBiletleriListele_Load(object sender, EventArgs e)
        {
            sqlYolcuTuruListeleme();
            biletPaneliUret(GlobalData.biletListesi);

        }


        private void biletPaneliUret(BindingList<Biletler> biletler)
        {
            int yKonum = panel1.Location.Y;
            int yPanel = this.Size.Height;
            GlobalData.geciciFiyatToplam = 0;
            foreach (Control control in panelAna.Controls.OfType<Panel>().ToList())
            {
                if (control != panel1)
                {
                    panelAna.Controls.Remove(control);
                    control.Dispose();
                }
            }
            int index = 0;
            foreach (Biletler b in biletler)
            {
                Panel yeniPanel = new Panel();
                yeniPanel.Size = panel1.Size;
                yeniPanel.Location = new Point(12, yKonum);
                yeniPanel.BorderStyle = panel1.BorderStyle;
                yeniPanel.BackColor = panel1.BackColor;
                yeniPanel.Tag = index.ToString();
                index++;

                CheckBox cB = new CheckBox();
                cB.Location = checkBox1.Location;
                cB.Dock = checkBox1.Dock;
                cB.Size = checkBox1.Size;
                cB.Text = checkBox1.Text;
                cB.Checked = b.biletAktifMi;
                cB.AutoSize = checkBox1.AutoSize;
                cB.CheckAlign = checkBox1.CheckAlign;
                cB.BackColor = checkBox1.BackColor;
                
                cB.CheckedChanged += checkBox_CheckedChanged;
                

                Panel p = new Panel();
                p.Location = panel2.Location;
                p.Size = panel2.Size;
                p.BackColor = panel2.BackColor;
                p.BorderStyle = panel2.BorderStyle;

                Label lblyolcu = new Label();
                lblyolcu.Text = label4.Text;
                lblyolcu.Location = label4.Location;
                lblyolcu.Size = label4.Size;
                lblyolcu.BackColor = label4.BackColor;
                lblyolcu.AutoSize = label4.AutoSize;
                lblyolcu.ForeColor = label4.ForeColor;
                lblyolcu.Font = label4.Font;

                Label lblAd = new Label();
                lblAd.Text = b.Ad + " " + b.Soyad;
                lblAd.Location = labelAdSoyad.Location;
                lblAd.Size = labelAdSoyad.Size;
                lblAd.BackColor = labelAdSoyad.BackColor;
                lblAd.AutoSize = labelAdSoyad.AutoSize;
                lblAd.ForeColor = labelAdSoyad.ForeColor;
                lblAd.Font = labelAdSoyad.Font;

                Label lbltc = new Label();
                lbltc.Text = b.tc;
                lbltc.Location = label5.Location;
                lbltc.Size = label5.Size;
                lbltc.BackColor = label5.BackColor;
                lbltc.AutoSize = label5.AutoSize;
                lbltc.ForeColor = label5.ForeColor;
                lbltc.Font = label5.Font;

                Label lbltur = new Label();
                lbltur.Text = b.tur;
                lbltur.Location = label6.Location;
                lbltur.Size = label6.Size;
                lbltur.BackColor = label6.BackColor;
                lbltur.AutoSize = label6.AutoSize;
                lbltur.ForeColor = label6.ForeColor;
                lbltur.Font = label6.Font;


                var indirimOrani = indirimOranlari.Select($"turAdi = '{b.tur}'");

                if (indirimOrani.Length > 0)
                {
                    b.IndirimOrani = Convert.ToDecimal(indirimOrani[0]["indirimOrani"]);
                }
                else
                {
                    b.IndirimOrani = 0;
                }

       
                PictureBox pb = new PictureBox();
                pb.Location = pictureBox1.Location;
                pb.Size = pictureBox1.Size;
                pb.BackColor = pictureBox1.BackColor;
                pb.SizeMode = pictureBox1.SizeMode;
                if (b.Cinsiyet == "Erkek")
                {
                    pb.Image = Properties.Resources.icons8_male_64;

                }
                else
                {
                    pb.Image = Properties.Resources.icons8_female_64;

                }

                Label lblotobus = new Label();
                lblotobus.Text = label1.Text;
                lblotobus.Location = label1.Location;
                lblotobus.Size = label1.Size;
                lblotobus.BackColor = label1.BackColor;
                lblotobus.AutoSize = label1.AutoSize;
                lblotobus.ForeColor = label1.ForeColor;
                lblotobus.Font = label1.Font;


                Label lblseferk = new Label();
                lblseferk.Text = b.seferAdiK + "-";
                lblseferk.Location = labelSeferAdi.Location;
                lblseferk.Size = labelSeferAdi.Size;
                lblseferk.BackColor = labelSeferAdi.BackColor;
                lblseferk.AutoSize = labelSeferAdi.AutoSize;
                lblseferk.ForeColor = labelSeferAdi.ForeColor;
                lblseferk.Font = labelSeferAdi.Font;

                Label lblseferV = new Label();
                lblseferV.Text = b.seferAdiV;
                lblseferV.Location = label7.Location;
                lblseferV.Size = label7.Size;
                lblseferV.BackColor = label7.BackColor;
                lblseferV.AutoSize = label7.AutoSize;
                lblseferV.ForeColor = label7.ForeColor;
                lblseferV.Font = label7.Font;


                Label lblKoltuk = new Label();
                lblKoltuk.Text = label3.Text;
                lblKoltuk.Location = label3.Location;
                lblKoltuk.Size = label3.Size;
                lblKoltuk.BackColor = label3.BackColor;
                lblKoltuk.AutoSize = label3.AutoSize;
                lblKoltuk.ForeColor = label3.ForeColor;
                lblKoltuk.Font = label3.Font;

                Label lblkoltukNo = new Label();
                lblkoltukNo.Text = b.koltukNo.ToString();
                lblkoltukNo.Location = labelKoltukNo.Location;
                lblkoltukNo.Size = labelKoltukNo.Size;
                lblkoltukNo.BackColor = labelKoltukNo.BackColor;
                lblkoltukNo.AutoSize = labelKoltukNo.AutoSize;
                lblkoltukNo.ForeColor = labelKoltukNo.ForeColor;
                lblkoltukNo.Font = labelKoltukNo.Font;

                
                Panel p2 = new Panel();
                p2.Location = panel3.Location;
                p2.Size = panel3.Size;
                p2.BackColor = panel3.BackColor;
                p2.BorderStyle = panel3.BorderStyle;

                Label lblindrim = new Label();
                lblindrim.Location = label8.Location;
                lblindrim.Size = labelFiyat.Size;
                lblindrim.BackColor = labelFiyat.BackColor;
                lblindrim.AutoSize = labelFiyat.AutoSize;
                lblindrim.ForeColor = labelFiyat.ForeColor;
                lblindrim.Font = label8.Font;


                Label lblfiyat = new Label();
                lblfiyat.Font = labelFiyat.Font;

                
               
                lblfiyat.Location = labelFiyat.Location;
                lblfiyat.Size = labelFiyat.Size;
                lblfiyat.BackColor = labelFiyat.BackColor;
                lblfiyat.AutoSize = labelFiyat.AutoSize;
                lblfiyat.ForeColor = labelFiyat.ForeColor;
               

                if (b.IndirimOrani > 0)
                {
                    lblfiyat.Text = b.Fiyat.ToString();
                    lblfiyat.Font = new Font(lblfiyat.Font.FontFamily, lblfiyat.Font.Size - 6, FontStyle.Strikeout); 
                    lblfiyat.ForeColor = Color.Gray;

                    lblindrim.Text = (b.Fiyat * (1 - (b.IndirimOrani / 100))).ToString() + " TL";

                    lblindrim.ForeColor = Color.Green;
                    lblindrim.Font = new Font(lblindrim.Font, FontStyle.Bold);
                }
                else
                {
                    lblindrim.Visible = false;
                    lblfiyat.Text = b.Fiyat.ToString() + " TL";
                }


                Guna2Button btn = new Guna2Button();
                btn.Location = guna2ButtonOnayla.Location;
                btn.Size = guna2ButtonOnayla.Size;
                btn.BackColor = guna2ButtonOnayla.BackColor;
                btn.BorderColor = guna2ButtonOnayla.BorderColor;
                btn.BorderRadius = guna2ButtonOnayla.BorderRadius;
                btn.BorderThickness = guna2ButtonOnayla.BorderThickness;
                btn.FillColor = guna2ButtonOnayla.FillColor;
                btn.ForeColor = guna2ButtonOnayla.ForeColor;
                btn.Text = guna2ButtonOnayla.Text;
                btn.Tag = b.seferID.ToString() + "|" + b.koltukNo.ToString();
                btn.Click += button_Click;

                yeniPanel.Controls.Add(cB);
                yeniPanel.Controls.Add(p);
                yeniPanel.Controls.Add(lblyolcu);
                yeniPanel.Controls.Add(lblAd);
                yeniPanel.Controls.Add(lbltc);
                yeniPanel.Controls.Add(lbltur);
                yeniPanel.Controls.Add(pb);
                yeniPanel.Controls.Add(lblotobus);
                yeniPanel.Controls.Add(lblseferk);
                yeniPanel.Controls.Add(lblseferV);
                yeniPanel.Controls.Add(lblKoltuk);
                yeniPanel.Controls.Add(lblkoltukNo);
                yeniPanel.Controls.Add(p2);
                yeniPanel.Controls.Add(lblfiyat);
                yeniPanel.Controls.Add(lblindrim);
                yeniPanel.Controls.Add(btn);

                panelAna.Size = new Size(panelAna.Width, yPanel);

                yPanel += 160; 
                yKonum += 160;
                panelAna.Dock = DockStyle.Fill;
                panelAna.AutoScroll = true;
                panelAna.Controls.Add(yeniPanel);

                cB.Tag = (b.Fiyat * (1 - (b.IndirimOrani / 100))).ToString();
                GlobalData.geciciFiyatToplam += (b.Fiyat * (1 - (b.IndirimOrani / 100)));
            }


        }
        private void button_Click(object sender, EventArgs e)
        {
            Guna2Button button = (Guna2Button)sender;

            string[] parts = button.Tag.ToString().Split('|');

            int seferId = Convert.ToInt32(parts[0]);
            int koltukNo = Convert.ToInt32(parts[1]);

            Biletler silinecekBilet = GlobalData.biletListesi.FirstOrDefault(b => b.seferID == seferId && b.koltukNo == koltukNo);

            if (silinecekBilet != null)
            {
                GlobalData.biletListesi.Remove(silinecekBilet);
            }

            biletPaneliUret(GlobalData.biletListesi);
            _FormSatinAlmaIslemi.labelFiyat.Text = "Fiyat: " + GlobalData.geciciFiyatToplam + " TL";

        }

        private void checkBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkBox = (CheckBox)sender;
            Panel parentPanel = checkBox.Parent as Panel;
            int index = Convert.ToInt32(parentPanel.Tag);

            if (checkBox.Checked)
            {
                GlobalData.geciciFiyatToplam += Convert.ToDecimal(checkBox.Tag);
                GlobalData.biletListesi[index].biletAktifMi = true;
            }
            else
            {
                GlobalData.geciciFiyatToplam -= Convert.ToDecimal(checkBox.Tag);
                GlobalData.biletListesi[index].biletAktifMi = false;
            }

            _FormSatinAlmaIslemi.labelFiyat.Text = "Fiyat: " + GlobalData.geciciFiyatToplam + " TL";

        }
        DataTable indirimOranlari = new DataTable();
        private void sqlYolcuTuruListeleme()
        {
            string baglanti = "Data Source=LAPTOP-R03\\SQLEXPRESS;Initial Catalog=otobusOtomasyonu;Integrated Security=True;TrustServerCertificate=True";



            using (SqlConnection conn = new SqlConnection(baglanti))
            {
                string sorgu = "SELECT *  FROM YolcuTurleri";

                SqlDataAdapter da = new SqlDataAdapter(sorgu, conn);

                da.Fill(indirimOranlari);
            }
        }
    }
}
