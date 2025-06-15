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

namespace OtobusOtomasyonSistemi.KullaniciArayuz._2OtobusumNerede
{
    public partial class FormBiletSorgu : Form
    {
        FormKullaniciArayuz _FormKullaniciArayuz;
        public FormBiletSorgu(FormKullaniciArayuz formKullaniciArayuz)
        {
            InitializeComponent();
            _FormKullaniciArayuz = formKullaniciArayuz;
        }
        private void FormBiletSorgu_Load(object sender, EventArgs e)
        {
            label1.Text = "";

            if (GlobalData.kullaniciID != null)
            {
                guna2ComboBox1.Visible = true;
                BiletleriGetir(GlobalData.kullaniciID);
                guna2ComboBox1.SelectedIndex = 0;
            }

        }

        static string baglanti = "Data Source=LAPTOP-R03\\SQLEXPRESS;Initial Catalog=otobusOtomasyonu;Integrated Security=True;TrustServerCertificate=True";
        private void guna2Button1YetkiEkle_Click(object sender, EventArgs e)
        {
            string  bno= textBoxkalkisS.Text;

            if (bno !="" && bno.Length >=15)
            {
                otobusBilegieriniAl(bno);
            }
            else if (guna2ComboBox1.SelectedIndex !=0)
            {
                otobusBilegieriniAl(guna2ComboBox1.SelectedItem.ToString());
            }
            else
            {
                label1.Text = "Lütfen geçerli bir bilet numarası giriniz.";
            }

        }
        public void otobusBilegieriniAl(string biletNo)
        {
            

            string sorgu = @"
        SELECT 
   b.seferID, o.plaka
FROM Biletler b
join Seferler s on b.seferID = s.seferID
join Otobusler o on s.otobusID = o.otobusID
WHERE b.biletNo = @biletNo";
            SqlConnection connection = new SqlConnection(baglanti);
            SqlCommand command = new SqlCommand(sorgu, connection);

            command.Parameters.AddWithValue("@biletNo", biletNo);

            try
            {
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    GlobalData.koltukSecmeSeferID = Convert.ToInt32(reader["seferID"]);
                    GlobalData.otobusPlaka = reader["plaka"].ToString();


                    _FormKullaniciArayuz.SayfaDegistir(new haritaForm());
                }
                else
                {
                    label1.Text = "Bilet bulunamadı.";
                }

                reader.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        public void BiletleriGetir(int? yolcuID)
        {
            guna2ComboBox1.Items.Clear();
            guna2ComboBox1.Items.Add("Biletlerimden Seçiniz...");
            DataTable dt = new DataTable();

            using (SqlConnection baglan = new SqlConnection(baglanti))
            {
                try
                {
                    baglan.Open();

                    string query = @"
                SELECT b.biletNo, s.tarih 
                FROM Biletler b
                JOIN Seferler s ON b.seferID = s.seferID
                WHERE b.yolcuID = @yolcuID";

                    using (SqlCommand komut = new SqlCommand(query, baglan))
                    {
                        komut.Parameters.AddWithValue("@yolcuID", yolcuID);
                        using (SqlDataReader reader = komut.ExecuteReader())
                        {
                           
                            while (reader.Read())
                            {
                                DateTime yarin = DateTime.Now.Date.AddDays(1);
                                //if (Convert.ToDateTime(reader["tarih"]) >= yarin)
                                //{
                                    guna2ComboBox1.Items.Add(reader["biletNo"].ToString());    
                                //}
                                
                            }
                        }
                    }
                }
                finally
                {
                    baglan.Close();
                }
            }
            
        }

        
    }
}
