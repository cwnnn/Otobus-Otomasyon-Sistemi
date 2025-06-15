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

namespace OtobusOtomasyonSistemi.AdminArayuz._5SeferYonetimi
{
    public partial class FormSeferEkleme : Form
    {
        static string baglanti = "Data Source=LAPTOP-R03\\SQLEXPRESS;Initial Catalog=otobusOtomasyonu;Integrated Security=True;TrustServerCertificate=True";
        SqlConnection baglan = new SqlConnection(baglanti);

        public FormSeferEkleme()
        {
            InitializeComponent();
        }
        private void FormSeferEkleme_Load(object sender, EventArgs e)
        {


            sqlListeleme();
            comboBoxNereden.DropDownHeight = 200;
            comboBoxNereden.MaxDropDownItems = 10;

            guna2DateTimePicker1.Format = DateTimePickerFormat.Time;
            guna2DateTimePicker1.ShowUpDown = true;

            DateTime today = DateTime.Today;
            dateTimePicker1.MinDate = today;

        }



        bool ilkSatirEklendiMi = false;
        private void SeferTerminalleriniKaydet(int seferID, RichTextBox rb)
        {
            string[] terminalAdlari = rb.Text.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            int siralama = 1;

            DateTime ilkTarih = dateTimePicker1.Value;
            DateTime selectedTime = guna2DateTimePicker1.Value;
            TimeSpan ilkSaat = new TimeSpan(selectedTime.Hour, selectedTime.Minute, selectedTime.Second);
            ilkTarih = ilkTarih.Add(ilkSaat);
            string ilkSehir = "";


            foreach (string terminalAdi in terminalAdlari)
            {
                string temizAd = terminalAdi.Trim(); 
                int terminalID = -1;

                // terminalID'yi bul
                using (SqlCommand komut = new SqlCommand("SELECT terminalID FROM Terminaller WHERE terminalAdi = @adi", baglan))
                {
                    komut.Parameters.AddWithValue("@adi", temizAd);
                    object sonuc = komut.ExecuteScalar();

                    if (sonuc != null)
                    {
                        terminalID = Convert.ToInt32(sonuc);
                    }
                    else
                    {

                        continue; 
                    }
                }

               
                using (SqlCommand komut = new SqlCommand("INSERT INTO SeferTerminalleri (seferID, terminalID, siralama, kalikisTarihi) VALUES (@seferID, @terminalID, @siralama, @tarih)", baglan))
                {
                    komut.Parameters.AddWithValue("@seferID", seferID);
                    komut.Parameters.AddWithValue("@terminalID", terminalID);
                    komut.Parameters.AddWithValue("@siralama", siralama);

                    ilkTarih = STkalkisTarihiEkleme(ilkTarih, ilkSehir, temizAd);

                    komut.Parameters.AddWithValue("@Tarih", ilkTarih);

                    
                    ilkSehir = temizAd;



                    komut.ExecuteNonQuery();
                }



                siralama++;
            }

            MessageBox.Show("Terminaller başarıyla kaydedildi.");
            ilkSatirEklendiMi = false;
        }

        private DateTime STkalkisTarihiEkleme(DateTime tarih, string s1, string s2)
        {
            SqlConnection baglan2 = new SqlConnection(baglanti);
            baglan2.Open();

            string sehir1 = "";
            string sehir2 = "";

            if (!ilkSatirEklendiMi)
            {
                
                ilkSatirEklendiMi = true;
                return tarih;

            }

            try
            {
                string sorgusehhiler = @"SELECT s1.sehirAdi, s2.sehirAdi
FROM Terminaller t1
JOIN Sehirler s1 ON t1.sehirID = s1.sehirID
JOIN Terminaller t2 ON t2.terminalAdi = @t2
JOIN Sehirler s2 ON t2.sehirID = s2.sehirID
WHERE t1.terminalAdi = @t1";

                using (SqlCommand cmd = new SqlCommand(sorgusehhiler, baglan2))
                {
                    cmd.Parameters.AddWithValue("@t1", s1);
                    cmd.Parameters.AddWithValue("@t2", s2);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            sehir1 = reader.GetString(0);
                            sehir2 = reader.GetString(1);

                        }
                    }



                    string sorgumesafe = @"
            SELECT mesafe 
            FROM Mesafeler 
            WHERE (sehir1 = @sehir1 AND sehir2 = @sehir2) 
               OR (sehir1 = @sehir2 AND sehir2 = @sehir1);";

                    using (SqlCommand command2 = new SqlCommand(sorgumesafe, baglan2))
                    {
                        command2.Parameters.AddWithValue("@sehir1", sehir1);
                        command2.Parameters.AddWithValue("@sehir2", sehir2);

                        object result = command2.ExecuteScalar();
                        double saat = (result != null) ? Convert.ToDouble(result) / 90.0 : 0;
                        TimeSpan sure = TimeSpan.FromHours(saat);
                        tarih = tarih.Add(sure);

                       
                        


                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Veritabanı bağlantısı sırasında bir hata oluştu: " + ex.Message);
            }
            finally
            {

                if (baglan2.State == ConnectionState.Open)
                {
                    baglan2.Close();
                }
            }

            
            return tarih;
        }


        private void sqlEkle()
        {
            try
            {
                int yeniSeferID = 0;
                baglan.Open();


                string otobusPlaka = (comboBoxOtobus.SelectedValue.ToString()).ToString();
                DateTime Tarih = dateTimePicker1.Value;


                DateTime saat = guna2DateTimePicker1.Value;
                string sadeceSaat = saat.ToString("HH:mm");



                decimal fiyat = Convert.ToDecimal(guna2TextBox2.Text);


                if (string.IsNullOrEmpty(otobusPlaka))
                {
                    MessageBox.Show("Lütfen tüm alanları doldurun.");
                    return;
                }



                string query = @"INSERT INTO Seferler (otobusID, kalkisSaati, tarih, fiyat, aktifMi)
                         VALUES (@OtobusID, @KalkisSaati, @Tarih, @Fiyat, 1);
                         SELECT SCOPE_IDENTITY();";

                using (SqlCommand command = new SqlCommand(query, baglan))
                {


                    command.Parameters.AddWithValue("@OtobusID", otobusPlaka);
                    command.Parameters.AddWithValue("@Tarih", Tarih.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@KalkisSaati", sadeceSaat);
                    command.Parameters.AddWithValue("@Fiyat", fiyat);


                    object sonuc = command.ExecuteScalar();
                    if (sonuc != null)
                    {
                        yeniSeferID = Convert.ToInt32(sonuc);

                        SeferTerminalleriniKaydet(yeniSeferID, richTextBox1);
                    }
                    else
                    {
                        MessageBox.Show("Sefer eklendi ama ID alınamadı.");
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Veritabanı işlemi sırasında bir hata oluştu: " + ex.Message);
            }
            finally
            {

                if (baglan.State == ConnectionState.Open)
                {
                    baglan.Close();
                }
            }
        }

        private void sqlListeleme()
        {
            try
            {

                baglan.Open();


                string querySehirler = "SELECT TerminalAdi FROM Terminaller order by TerminalAdi";
                using (SqlCommand command = new SqlCommand(querySehirler, baglan))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            comboBoxNereden.Items.Add(reader["terminalAdi"].ToString());
                        }
                    }
                }


                string queryOtobusler = "SELECT otobusID,plaka FROM Otobusler";
                using (SqlCommand command = new SqlCommand(queryOtobusler, baglan))
                {
                    comboBoxOtobus.DataSource = null;
                    SqlDataAdapter adapter = new SqlDataAdapter(queryOtobusler, baglan);
                    DataTable table = new DataTable();
                    table.Clear();
                    adapter.Fill(table);
                    comboBoxOtobus.DataSource = table;
                    comboBoxOtobus.DisplayMember = "plaka";
                    comboBoxOtobus.ValueMember = "otobusID";

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show("Veritabanı bağlantısı sırasında bir hata oluştu: " + ex.Message);
            }
            finally
            {

                if (baglan.State == ConnectionState.Open)
                {
                    baglan.Close();
                }
            }

        }
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            sqlEkle();
        }

        private void guna2Button4Ekle_Click(object sender, EventArgs e)
        {
            if (richTextBox1.Text.Contains(comboBoxNereden.SelectedItem.ToString()) == false)
            {
                richTextBox1.Text += comboBoxNereden.SelectedItem + ", ";
            }
        }

        private void guna2Button4Cikar_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = richTextBox1.Text.Replace(comboBoxNereden.SelectedItem + ", ", "");
        }



        private void dateTimePicker1_Leave(object sender, EventArgs e)
        {

        }
    }
}
