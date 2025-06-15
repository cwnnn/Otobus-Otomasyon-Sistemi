using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OtobusOtomasyonSistemi.KullaniciArayuz._4Profilim.ProfilimDetay
{
    public partial class FormBiletDetay : Form
    {
        static string baglanti = "Data Source=LAPTOP-R03\\SQLEXPRESS;Initial Catalog=otobusOtomasyonu;Integrated Security=True;TrustServerCertificate=True";
        SqlConnection baglan = new SqlConnection(baglanti);

        FormProfilim _formProfilim;
        public FormBiletDetay(FormProfilim formProfilim)
        {
            InitializeComponent();
            _formProfilim = formProfilim;

        }

        private void FormBiletDetay_Load(object sender, EventArgs e)
        {
            
            baglan.Open();

            sqlListeleme();
        }
        private void sqlListeleme()
        {
            this.ActiveControl = label1; //sinirimi bozdu


            string sorgu = @"SELECT 
    y.ad + ' ' + y.soyad AS YolcuAdiSoyadi,
    y.cinsiyet,
    b.koltukNo,
    yt.turAdi AS YolcuTuru,
    b.ucret AS BiletUcreti,
    b.biletID,
    s.kalkisSaati,
	bt.terminalAdi AS KalkisTerminali,
    vt.terminalAdi AS VarisTerminali,
    s.tarih AS SeferTarihi,
    o.plaka AS OtobusPlakasi,
    o.koltukSayisi AS KoltukSayisi,
    b.iptalEdildiMi AS iptalMi
FROM Biletler b
JOIN Yolcular y ON b.yolcuID = y.yolcuID
JOIN YolcuTurleri yt ON y.kullaniciTuruID = yt.turID
JOIN Seferler s ON b.seferID = s.seferID
JOIN SeferTerminalleri st1 ON s.seferID = st1.seferID
JOIN SeferTerminalleri st2 ON s.seferID =st2.seferID
JOIN Terminaller vt ON st1.terminalID = vt.terminalID
JOIN Terminaller bt ON st2.terminalID = bt.terminalID	
JOIN Otobusler o ON s.otobusID = o.otobusID
WHERE vt.terminalID = b.varisTerminalID and bt.terminalID =b.binisTerminalID and 
y.yolcuID = @kullaniciID and b.biletId =@biletid";

            TimeSpan kalkis, varis;
            using (SqlCommand komut = new SqlCommand(sorgu, baglan))
            {
                komut.Parameters.AddWithValue("@kullaniciID", GlobalData.kullaniciID);
                komut.Parameters.AddWithValue("@biletid", GlobalData.biletID);
                SqlDataAdapter da = new SqlDataAdapter(komut);

                SqlDataReader reader = komut.ExecuteReader();
                if (reader.Read())
                {
                    label4.Text = reader["YolcuAdiSoyadi"].ToString();
                    if (reader["cinsiyet"].ToString() == "E")
                    {
                        label6.Text = "Erkek";
                    }
                    else
                    {
                        label6.Text = "Kadın";
                    }
                    label8.Text = reader["YolcuTuru"].ToString();

                    labelBiletNo.Text = reader["biletID"].ToString();
                    label12.Text = Convert.ToDateTime(reader["SeferTarihi"]).ToString("dd.MM.yyyy");
                    labelKoltukNo.Text = reader["koltukNo"].ToString();
                    labelPlaka.Text = reader["OtobusPlakasi"].ToString();

                    kalkis = TimeSpan.Parse(reader["kalkisSaati"].ToString());
                    labelKalkisSaati.Text = kalkis.ToString(@"hh\:mm");

                    labelKalkisYeri.Text = reader["KalkisTerminali"].ToString();
                    labelVarisYeri.Text = reader["VarisTerminali"].ToString();

                    if (reader["KoltukSayisi"].ToString() == "40")
                    {
                        guna2TextBox1.Text = "Otobüs | 2 + 1";
                    }
                    else
                    {
                        guna2TextBox1.Text = "Otobüs | 2 + 2";
                    }
                    labelFiyat.Text = reader["BiletUcreti"].ToString() + " TL";

                    DateTime detayTarih = Convert.ToDateTime(reader["SeferTarihi"]) + TimeSpan.Parse(reader["kalkisSaati"].ToString());

                    if (DateTime.Now.AddHours(1) > detayTarih)
                    {
                        guna2ButtonOnayla.Enabled = false;
                    }
                    else
                    {
                        guna2ButtonOnayla.Enabled = true;
                    }
                    if (reader["iptalMi"] != DBNull.Value && Convert.ToBoolean(reader["iptalMi"]))
                    {
                        guna2ButtonOnayla.Enabled = false;
                        labelUyari.Text = "Bu bilet iptal edilmiştir.";
                    }
                    reader.Close();

                    string sorgu3 = @"
            SELECT mesafe 
            FROM Mesafeler 
            WHERE (sehir1 = @sehir1 AND sehir2 = @sehir2) 
               OR (sehir1 = @sehir2 AND sehir2 = @sehir1);";

                    using (SqlCommand command2 = new SqlCommand(sorgu3, baglan))
                    {
                        command2.Parameters.AddWithValue("@sehir1", labelKalkisYeri.Text);
                        command2.Parameters.AddWithValue("@sehir2", labelVarisYeri.Text);
                       
                        object result = command2.ExecuteScalar();
                        double saat = (result != null) ? Convert.ToDouble(result) / 90.0 : 0;
                        TimeSpan sure = TimeSpan.FromHours(saat);
                        

                        labelVarisaati.Text = (kalkis+sure).ToString(@"hh\:mm");
                        labelYolculukSuresi.Text = sure.ToString(@"hh\:mm");
                    }
                }
            }
        }

        private void guna2ButtonOnayla_Click(object sender, EventArgs e)
        {

            DialogResult sonuc = MessageBox.Show("Biletinizi iptal etmek istediğinize emin misiniz?", "Bilet İptali", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (sonuc == DialogResult.Yes)
            {
                string sorgu = "UPDATE Biletler SET iptalEdildiMi = 1 WHERE biletID = @biletID";
                using (SqlCommand komut = new SqlCommand(sorgu, baglan))
                {
                    komut.Parameters.AddWithValue("@biletID", GlobalData.biletID);
                    int rowsEtkilenen = komut.ExecuteNonQuery();
                    if (rowsEtkilenen >0)
                    {
                        MessageBox.Show("Bilet iptali başarılı.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        sqlListeleme();
                    }
                }
                
                
            }
        }
    }
}
