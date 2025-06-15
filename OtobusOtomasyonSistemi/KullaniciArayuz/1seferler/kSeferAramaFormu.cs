    using Guna.UI2.WinForms;
using OtobusOtomasyonSistemi.KullaniciArayuz._1seferler;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace OtobusOtomasyonSistemi
{
    public partial class kSeferAramaFormu : Form
    {
        FormKullaniciArayuz _formKullaniciArayuz;
        public kSeferAramaFormu(FormKullaniciArayuz formKullanici)
        {
            InitializeComponent();
            _formKullaniciArayuz = formKullanici;
        }



        FormKullaniciArayuz frmka = (FormKullaniciArayuz)Application.OpenForms["FormKullaniciArayuz"];

        string baglanti = "Data Source=LAPTOP-R03\\SQLEXPRESS;Initial Catalog=otobusOtomasyonu;Integrated Security=True;TrustServerCertificate=True";

        private void kSeferAramaFormu_Load(object sender, EventArgs e)
        {

            guna2DateTimePicker1.Text = DateTime.Now.ToString("yyyy/MM/dd");
            ComboBoxDoldur(guna2ComboBox1);
            ComboBoxDoldur(guna2ComboBox2);
            guna2ComboBox1.DropDownHeight = 200;
            guna2ComboBox1.MaxDropDownItems = 10;
            guna2ComboBox2.DropDownHeight = 200;
            guna2ComboBox2.MaxDropDownItems = 10;

            DateTime today = DateTime.Today;
            guna2DateTimePicker1.MinDate = today;
            guna2DateTimePicker2.MinDate = today;
            GlobalData.gidisDonusMu = false;

            int index = guna2ComboBox1.FindStringExact("İstanbul");

            if (index >= 0)
            {
                guna2ComboBox1.SelectedIndex = index;
            }
            index = -1;
            index = guna2ComboBox2.FindStringExact("Ankara");
            if (index >= 0)
            {
                guna2ComboBox2.SelectedIndex = index;
            }
            

        }


        private void guna2ButtonOnayla_Click(object sender, EventArgs e)
        {
            if (guna2ComboBox1.SelectedIndex >= 0 && guna2ComboBox2.SelectedIndex >= 0)
            {
                istenenSeferlerVarMi();
                if (guna2CheckBox1.Checked == true)
                {
                    GlobalData.gidisDonusMu = true;
                    GlobalData.donusTarihi = guna2DateTimePicker2.Value;
                }
                else
                {
                    GlobalData.gidisDonusMu = false;
                }
            }
        }

        
        DataTable kalkisSehirleri = new DataTable();  


        public void seferListesiDoldurma()
        {
            string sorgu = @"SELECT 
    s.seferID, 
    t1.terminalAdi AS kalkisTerminalAdi, 
    t2.terminalAdi AS varisTerminalAdi, 
    CAST(st1.kalikisTarihi AS TIME) AS kalkisSaati,
    o.koltukSayisi, 
    s.fiyat, 
    s.kacKoltukKaldi
FROM Seferler s
JOIN Otobusler o ON o.otobusID = s.otobusID
JOIN SeferTerminalleri st1 ON s.seferID = st1.seferID
JOIN SeferTerminalleri st2 ON s.seferID = st2.seferID
JOIN Terminaller t1 ON st1.terminalID = t1.terminalID
JOIN Terminaller t2 ON st2.terminalID = t2.terminalID
JOIN Sehirler seh1 ON seh1.sehirID = t1.sehirID
JOIN Sehirler seh2 ON seh2.sehirID = t2.sehirID
WHERE seh1.sehirID = @kalkisSehirID
  AND seh2.sehirID = @varisSehirID
  AND st1.siralama < st2.siralama
  AND CAST(st1.kalikisTarihi AS DATE) = @seferTarihi
  AND s.aktifMi = 1
  AND (
        @seferTarihi > CAST(GETDATE() AS DATE)
        OR CAST(st1.kalikisTarihi AS TIME) >= CAST(GETDATE() AS TIME)
    )
";


            GlobalData.seferlerListesi = new DataTable();

            using (SqlConnection baglan = new SqlConnection(baglanti))
            {
                baglan.Open();

                using (SqlCommand command = new SqlCommand(sorgu, baglan))
                {
                    command.Parameters.AddWithValue("@kalkisSehirID", guna2ComboBox1.SelectedValue);
                    command.Parameters.AddWithValue("@varisSehirID", guna2ComboBox2.SelectedValue);
                    command.Parameters.AddWithValue("@seferTarihi", guna2DateTimePicker1.Value.Date);


                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(GlobalData.seferlerListesi);
                    }
                }

                GlobalData.kalkisSehir = guna2ComboBox1.SelectedValue.ToString();
                GlobalData.varisSehir = guna2ComboBox2.SelectedValue.ToString();
                GlobalData.kalkisTarihi = guna2DateTimePicker1.Value;

                GlobalData.seferlerListesi.Columns.Add("varisSaati", typeof(TimeSpan));
                




                string sehir1 = "";
                string sehir2 = "";
                foreach (DataRow row in geciciSehirler.Rows)
                {
                    if ((int)row["sehirID"] == (int)guna2ComboBox1.SelectedValue)
                    {
                        sehir1 = row["sehirAdi"].ToString();

                    }
                    if ((int)row["sehirID"] == (int)guna2ComboBox2.SelectedValue)
                    {
                        sehir2 = row["sehirAdi"].ToString();

                    }
                }

                //varis Süresini hesaplamak için sorgu
                string sorgu3 = @"
            SELECT mesafe 
            FROM Mesafeler 
            WHERE (sehir1 = @sehir1 AND sehir2 = @sehir2) 
               OR (sehir1 = @sehir2 AND sehir2 = @sehir1);";

                using (SqlCommand command2 = new SqlCommand(sorgu3, baglan))
                {
                    command2.Parameters.AddWithValue("@sehir1", sehir1);
                    command2.Parameters.AddWithValue("@sehir2", sehir2);

                    object result = command2.ExecuteScalar();
                    double saat = (result != null) ? Convert.ToDouble(result) / 90.0 : 0;
                    TimeSpan sure = TimeSpan.FromHours(saat);
                    GlobalData.TvarisSaati = sure;

                    foreach (DataRow row in GlobalData.seferlerListesi.Rows)
                    {
                        TimeSpan kalkisSaati = (TimeSpan)row["kalkisSaati"];
                        TimeSpan varisSaati = kalkisSaati.Add(sure);
                        row["varisSaati"] = varisSaati.ToString();
                        row["fiyat"] = Math.Ceiling(Convert.ToDecimal(row["fiyat"]) * Convert.ToDecimal(result)/10)*10;
                    }
                }



            }
 

    

            frmka.SayfaDegistir(new FormSeferListeleme(_formKullaniciArayuz));
            this.Close();
            }

        private void copluk()
        {
            SqlConnection baglan = new SqlConnection(baglanti);
            string sorgu = @"
                SELECT * from Seferler";
            baglan.Open();
           SqlDataReader oku = new SqlCommand(sorgu, baglan).ExecuteReader();
            while (oku.Read())
            {
                string varisTarihi = "12-5-2025";
                MessageBox.Show(varisTarihi);
                DateTime tarih = Convert.ToDateTime(varisTarihi);
                MessageBox.Show(tarih.ToString());
            }
            baglan.Close();













        }

        DataTable istenenSeferlerVarMiTablosu = new DataTable();
        private void istenenSeferlerVarMi()
        {
            string sorgu = @"
        SELECT 
            st1.seferID,
            st1.terminalID AS terminal1,
            st2.terminalID AS terminal2,
            st1.siralama AS siralama1,
            st2.siralama AS siralama2
        FROM 
            SeferTerminalleri AS st1
        JOIN 
            SeferTerminalleri AS st2 ON st1.seferID = st2.seferID
        JOIN 
            Terminaller t1 ON t1.terminalID = st1.terminalID
        JOIN 
            Terminaller t2 ON t2.terminalID = st2.terminalID
        WHERE 
            t1.sehirID = @kalkisSehirID
            AND t2.sehirID = @varisSehirID
            AND st1.siralama < st2.siralama;";

            using (SqlConnection baglan = new SqlConnection(baglanti))
            {
                using (SqlCommand command = new SqlCommand(sorgu, baglan))
                {
                    command.Parameters.AddWithValue("@kalkisSehirID", guna2ComboBox1.SelectedValue);
                    command.Parameters.AddWithValue("@varisSehirID", guna2ComboBox2.SelectedValue);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        baglan.Open();
                        adapter.Fill(istenenSeferlerVarMiTablosu);
                        baglan.Close();
                    }
                }
            }

            seferListesiDoldurma();
        }
        DataTable geciciSehirler = new DataTable(); //sorguda gelen sehirID den sehir adını almak için ve comboboxları doldurmak için
        private void ComboBoxDoldur(Guna2ComboBox comboBox)
        {
            try
            {

                SqlConnection baglan = new SqlConnection(baglanti);

                baglan.Open();
                string sorgu = "SELECT sehirID, sehirAdi FROM sehirler ORDER BY sehirAdi";
                SqlCommand command = new SqlCommand(sorgu, baglan);

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable table = new DataTable();
                adapter.Fill(table);

                comboBox.BeginUpdate();
                comboBox.DataSource = table;
                geciciSehirler = table;
                comboBox.DisplayMember = "sehirAdi";
                comboBox.ValueMember = "sehirID";
                comboBox.EndUpdate();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private void guna2CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2CheckBox1.Checked)
            {
                guna2DateTimePicker2.Enabled = true;

                guna2DateTimePicker2.Value = guna2DateTimePicker1.Value.AddDays(1);
            }
            else
            {
                guna2DateTimePicker2.Enabled = false;
            }
        }

    }




}
