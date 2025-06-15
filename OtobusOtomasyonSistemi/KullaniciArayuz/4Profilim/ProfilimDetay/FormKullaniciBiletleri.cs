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
using TheArtOfDevHtmlRenderer.Adapters;

namespace OtobusOtomasyonSistemi.KullaniciArayuz._4Profilim.ProfilimDetay
{
    public partial class FormKullaniciBiletleri : Form
    {
        static string baglanti = "Data Source=LAPTOP-R03\\SQLEXPRESS;Initial Catalog=otobusOtomasyonu;Integrated Security=True;TrustServerCertificate=True";
        SqlConnection baglan = new SqlConnection(baglanti);
        public FormKullaniciBiletleri(FormProfilim parentForm)
        {
            InitializeComponent();
            _parentForm = parentForm;

        }

        DataTable dt = new DataTable();

        private FormProfilim _parentForm;

        private void FormKullaniciBiletleri_Load(object sender, EventArgs e)
        {
            this.ActiveControl = label1;
            baglan.Open();
            sqlListeleme();
            panelUret(dt);
        }

        private void sqlListeleme()
        {
            string sorgu = @"SELECT 
    y.ad + ' ' + y.soyad AS YolcuAdiSoyadi,
    y.cinsiyet,
    b.koltukNo,
    yt.turAdi AS YolcuTuru,
    b.ucret AS BiletUcreti,
    b.biletID,
    s.kalkisSaati,
	b.binisTerminalID AS KalkisTerminali,
	b.varisTerminalID AS VarisTerminali,
    binis.terminalAdi AS KalkisTerminaliAd,
    varis.terminalAdi AS VarisTerminaliAD,
    s.tarih AS SeferTarihi,
    o.plaka AS OtobusPlakasi,
    o.koltukSayisi AS KoltukSayisi,
    b.iptalEdildiMi AS iptalMi
FROM 
    Biletler b
JOIN 
    Yolcular y ON b.yolcuID = y.yolcuID
JOIN 
    YolcuTurleri yt ON y.kullaniciTuruID = yt.turID
JOIN 
    Seferler s ON b.seferID = s.seferID
JOIN 
    Otobusler o ON s.otobusID = o.otobusID
-- Kalkış terminali
JOIN 
    Terminaller binis ON b.binisTerminalID = binis.terminalID
-- Varış terminali
JOIN 
    Terminaller varis ON b.varisTerminalID = varis.terminalID
WHERE 
    y.yolcuID = @kullaniciID
ORDER BY 
    b.iptalEdildiMi ASC;";
            using (SqlCommand komut = new SqlCommand(sorgu, baglan))
            {
                komut.Parameters.AddWithValue("@kullaniciID", GlobalData.kullaniciID);

                SqlDataAdapter da = new SqlDataAdapter(komut);
                da.Fill(dt);

            }
            if (dt.Rows.Count <= 0)
            {
                label5.Visible = true;
            }
        }
        private void panelUret(DataTable dt)
        {
            int yKonum = panel1.Location.Y;
            int yForm = 160;
            TimeSpan tahmini, kalkis;

            foreach (DataRow row in dt.Rows)
            {
                Panel yeniPanel = new Panel();
                yeniPanel.Size = panel1.Size;
                yeniPanel.Location = new Point(26, yKonum);
                yeniPanel.BorderStyle = panel1.BorderStyle;

                DateTime detayTarih = Convert.ToDateTime(row["SeferTarihi"]) + TimeSpan.Parse(row["kalkisSaati"].ToString());
                if (DateTime.Now > detayTarih)
                {
                    yeniPanel.BackColor = Color.Gray;
                }
                else
                {
                    yeniPanel.BackColor = panel1.BackColor;
                }
               


                Label lblKalkisS = new Label();
                kalkis = TimeSpan.Parse(row["kalkisSaati"].ToString());
                lblKalkisS.Text = kalkis.ToString(@"hh\:mm");
                lblKalkisS.Location = labelKalkisSaati.Location;
                lblKalkisS.AutoSize = true;
                lblKalkisS.ForeColor = labelKalkisSaati.ForeColor;
                lblKalkisS.Font = labelKalkisSaati.Font;

                Label lblTahminiVaris = new Label();
                tahmini = tahminivarisSuresi(row["KalkisTerminali"].ToString(), row["VarisTerminali"].ToString());
                lblTahminiVaris.Text = tahmini.ToString(@"hh\:mm");
                lblTahminiVaris.Location = labelYolculukSuresi.Location;
                lblTahminiVaris.AutoSize = true;
                lblTahminiVaris.ForeColor = labelYolculukSuresi.ForeColor;
                lblTahminiVaris.Font = labelYolculukSuresi.Font;

                Label lblVarisS = new Label();
              
                lblVarisS.Text = (kalkis+tahmini).ToString(@"hh\:mm");
                lblVarisS.Location = labelVarisaati.Location;
                lblVarisS.AutoSize = true;
                lblVarisS.ForeColor = labelVarisaati.ForeColor;
                lblVarisS.Font = labelVarisaati.Font;

                

                Label lblCizgi = new Label();
                lblCizgi.Text = label2.Text;
                lblCizgi.Location = label2.Location;
                lblCizgi.AutoSize = true;
                lblCizgi.ForeColor = label2.ForeColor;
                lblCizgi.Font = label2.Font;

                Label lbltxtBilet = new Label();
                lbltxtBilet.Text = label1.Text;
                lbltxtBilet.Location = label1.Location;
                lbltxtBilet.AutoSize = true;
                lbltxtBilet.ForeColor = label1.ForeColor;
                lbltxtBilet.Font = label1.Font;

                Label lblBiletNo = new Label();
                lblBiletNo.Text = row["biletID"].ToString();
                lblBiletNo.Location = labelBiletNo.Location;
                lblBiletNo.AutoSize = true;
                lblBiletNo.ForeColor = labelBiletNo.ForeColor;
                lblBiletNo.Font = labelBiletNo.Font;


                Label lblFiyat = new Label();
                lblFiyat.Text = row["BiletUcreti"].ToString() + " TL";
                lblFiyat.Location = labelFiyat.Location;
                lblFiyat.AutoSize = true;
                lblFiyat.ForeColor = labelFiyat.ForeColor;
                lblFiyat.Font = labelFiyat.Font;

                Label lblKalkisT = new Label();
                lblKalkisT.Text = row["KalkisTerminaliAd"].ToString();
                lblKalkisT.Location = labelKalkisYeri.Location;
                lblKalkisT.AutoSize = true;
                lblKalkisT.ForeColor = labelKalkisYeri.ForeColor;
                lblKalkisT.Font = labelKalkisYeri.Font;

                Label lblVarisT = new Label();
                lblVarisT.Text = row["VarisTerminaliAd"].ToString();
                lblVarisT.Location = labelVarisYeri.Location;
                lblVarisT.AutoSize = true;
                lblVarisT.ForeColor = labelVarisYeri.ForeColor;
                lblVarisT.Font = labelVarisYeri.Font;

               
                Label lblUyari = new Label();
                lblUyari.Text = "";
                lblUyari.Location = labelUyari.Location;
                lblUyari.AutoSize = true;
                lblUyari.Site = labelUyari.Site;
                lblUyari.TextAlign = labelUyari.TextAlign;
                lblUyari.ForeColor = labelUyari.ForeColor;
                lblUyari.Font = labelUyari.Font;
        
                if (row["iptalMi"] != DBNull.Value && Convert.ToBoolean(row["iptalMi"]))
                {
                    lblUyari.Text = "İptal Edilmiştir.";
                    yeniPanel.BackColor = Color.Gray;
                }

                Label lblTarih = new Label();
                lblTarih.Text = Convert.ToDateTime(row["SeferTarihi"]).ToString("dd.MM.yyyy");
                lblTarih.Location = labelTarih.Location;
                lblTarih.AutoSize = true;
                lblTarih.ForeColor = labelTarih.ForeColor;
                lblTarih.Font = labelTarih.Font;


                Guna2TextBox otobusBilgisi = new Guna2TextBox();
                otobusBilgisi.TextAlign = HorizontalAlignment.Center;
                otobusBilgisi.Margin = guna2TextBox1.Margin;
                otobusBilgisi.Size = new Size(150, 18);
                otobusBilgisi.Location = new Point(15, 63);
                otobusBilgisi.BorderRadius = guna2TextBox1.BorderRadius;
                otobusBilgisi.BorderThickness = guna2TextBox1.BorderThickness;
                otobusBilgisi.ReadOnly = true;
                otobusBilgisi.BackColor = guna2TextBox1.BackColor;
                otobusBilgisi.ForeColor = guna2TextBox1.ForeColor;
                otobusBilgisi.Font = guna2TextBox1.Font;
                otobusBilgisi.FillColor = guna2TextBox1.FillColor;
                if (row["KoltukSayisi"].ToString() == "40")
                {
                    otobusBilgisi.Text = "Otobüs | 2 + 1";
                }
                else
                {
                    otobusBilgisi.Text = "Otobüs | 2 + 2";
                }

                Label lbltxtplaka = new Label();
                lbltxtplaka.Text = label4.Text;
                lbltxtplaka.Location = label4.Location;
                lbltxtplaka.AutoSize = true;
                lbltxtplaka.ForeColor = label4.ForeColor;
                lbltxtplaka.Font = label4.Font;

                Label lblPlaka = new Label();
                lblPlaka.Text = row["OtobusPlakasi"].ToString();
                lblPlaka.Location = labelPlaka.Location;
                lblPlaka.AutoSize = true;
                lblPlaka.ForeColor = labelPlaka.ForeColor;
                lblPlaka.Font = labelPlaka.Font;

                Label lbltxtKoltukNo = new Label();
                lbltxtKoltukNo.Text = label3.Text;
                lbltxtKoltukNo.Location = label3.Location;
                lbltxtKoltukNo.AutoSize = true;
                lbltxtKoltukNo.ForeColor = label3.ForeColor;
                lbltxtKoltukNo.Font = label3.Font;

                Label lblKoltukNo = new Label();
                lblKoltukNo.Text = row["koltukNo"].ToString();
                lblKoltukNo.Location = labelKoltukNo.Location;
                lblKoltukNo.AutoSize = true;
                lblKoltukNo.ForeColor = labelKoltukNo.ForeColor;
                lblKoltukNo.Font = labelKoltukNo.Font;


                Guna2Button devamEt = new Guna2Button();
                devamEt.Text = "Detayları Gör";
                devamEt.Location = guna2ButtonOnayla.Location;
                devamEt.Size = guna2ButtonOnayla.Size;
                devamEt.BorderRadius = guna2ButtonOnayla.BorderRadius;
                devamEt.Font = guna2ButtonOnayla.Font;
                devamEt.ForeColor = guna2ButtonOnayla.ForeColor;
                devamEt.FillColor = guna2ButtonOnayla.FillColor;
                devamEt.Tag = row["biletID"];
                devamEt.Click += button_Click;


                yeniPanel.Controls.Add(lblKalkisS);
                yeniPanel.Controls.Add(lblVarisS);
                yeniPanel.Controls.Add(lblTahminiVaris);
                yeniPanel.Controls.Add(lblCizgi);
                yeniPanel.Controls.Add(lblBiletNo);
                yeniPanel.Controls.Add(lblFiyat);
                yeniPanel.Controls.Add(lblKalkisT);
                yeniPanel.Controls.Add(lblVarisT);
                yeniPanel.Controls.Add(lblUyari);
                yeniPanel.Controls.Add(otobusBilgisi);
                yeniPanel.Controls.Add(lblPlaka);
                yeniPanel.Controls.Add(lblKoltukNo);
                yeniPanel.Controls.Add(devamEt);
                yeniPanel.Controls.Add(lbltxtBilet);
                yeniPanel.Controls.Add(lbltxtKoltukNo);
                yeniPanel.Controls.Add(lbltxtplaka);
                yeniPanel.Controls.Add(lblTarih);

                panelAna.Size = new Size(panelAna.Size.Width, yForm);
                yForm += 150; // Panel yüksekliği + boşluk
                yKonum += 150;
                panelAna.Dock = DockStyle.Fill;
                panelAna.AutoScroll = true;
                panelAna.Controls.Add(yeniPanel);
            }

        }

        private TimeSpan tahminivarisSuresi(string sehir1, string sehir2)
        {
            string sehirAdi1 = "";
            string sehirAdi2 = "";
            double mesafe = 0;

            using (SqlConnection baglanti = new SqlConnection(baglan.ConnectionString))
            {
                baglanti.Open();

                string sorgu = @"SELECT
            (SELECT sehirAdi FROM Sehirler WHERE sehirID = @sehirID1) AS sehir1,
            (SELECT sehirAdi FROM Sehirler WHERE sehirID = @sehirID2) AS sehir2;";

                using (SqlCommand command = new SqlCommand(sorgu, baglanti))
                {
                    command.Parameters.AddWithValue("@sehirID1", sehir1);
                    command.Parameters.AddWithValue("@sehirID2", sehir2);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            sehirAdi1 = reader["sehir1"].ToString();
                            sehirAdi2 = reader["sehir2"].ToString();
                        }
                    }
                }

                string sorgu2 = @"
            SELECT mesafe 
            FROM Mesafeler 
            WHERE (sehir1 = @sehir1 AND sehir2 = @sehir2) 
               OR (sehir1 = @sehir2 AND sehir2 = @sehir1);";

                using (SqlCommand command2 = new SqlCommand(sorgu2, baglanti))
                {
                    command2.Parameters.AddWithValue("@sehir1", sehirAdi1);
                    command2.Parameters.AddWithValue("@sehir2", sehirAdi2);

                    object result = command2.ExecuteScalar();
                    mesafe = (result != null) ? Convert.ToDouble(result) : 0;
                }

                baglanti.Close();
            }

            double saat = mesafe / 90.0;
            TimeSpan sure = TimeSpan.FromHours(saat);
            return sure;
        }


        private void button_Click(object sender, EventArgs e)
        {
            Guna2Button button = (Guna2Button)sender;
            GlobalData.biletID = button.Tag.ToString();

            _parentForm.SayfaDegistir(new FormBiletDetay(_parentForm));

        }
    }
}
