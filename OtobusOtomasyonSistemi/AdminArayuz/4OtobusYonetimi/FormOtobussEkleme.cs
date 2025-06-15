using Guna.UI2.WinForms;
using System;
using System.Collections;
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
    public partial class FormOtobussEkleme : Form
    {
        static string baglanti = "Data Source=LAPTOP-R03\\SQLEXPRESS;Initial Catalog=otobusOtomasyonu;Integrated Security=True;TrustServerCertificate=True";
        SqlConnection baglan = new SqlConnection(baglanti);

        public FormOtobussEkleme()
        {
            InitializeComponent();
        }

        private void FormOtobussEkleme_Load(object sender, EventArgs e)
        {
            sqllisteleme();


        }

        private void guna2Button1YetkiEkle_Click(object sender, EventArgs e)
        {
            if (textBoxkalkisS.Text != "" && comboBoxOtobus.SelectedIndex != -1 && guna2ComboBox1.SelectedIndex != -1 && guna2ComboBox5.SelectedIndex != -1)
            {
                DialogResult sonuc = MessageBox.Show("Otobüs Eklenecek,\nEmin misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (sonuc == DialogResult.Yes)
                {
                    try
                    {
                        baglan.Open();
                        string sorgu1 = "INSERT INTO Otobusler(plaka,koltukSayisi,markaID,aktifMi) VALUES (@plaka,@koltukSayisi,@markaID,@aktifMi)";
                        using (SqlCommand komut = new SqlCommand(sorgu1, baglan))
                        {
                            komut.Parameters.AddWithValue("@plaka", textBoxkalkisS.Text);
                            komut.Parameters.AddWithValue("@koltukSayisi", comboBoxOtobus.SelectedItem);
                            komut.Parameters.AddWithValue("@markaID", guna2ComboBox1.SelectedValue);
                            komut.Parameters.AddWithValue("@aktifMi", guna2ComboBox5.SelectedIndex);
                            int rowsAffected = komut.ExecuteNonQuery();

                            if (rowsAffected > 0)
                                MessageBox.Show("ekleme başarılı!");
                            else
                                MessageBox.Show("Kayıt bulunamadı veya güncellenemedi.");
                        }
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show("Hata oluştu: " + ex.Message);
                    }
                    finally
                    {
                        baglan.Close();
                        guna2ComboBox7.SelectedIndexChanged -= guna2ComboBox7_SelectedIndexChanged;
                        sqllisteleme();
                    }
                    
                }
            }
        }
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (guna2TextBox1.Text != "" && guna2ComboBox3.SelectedIndex != -1 && guna2ComboBox2.SelectedIndex != -1 && guna2ComboBox4.SelectedIndex != -1)
            {
                DialogResult sonuc = MessageBox.Show("Otobüs Güncellencek,\nEmin misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (sonuc == DialogResult.Yes)
                {
                    try
                    {
                        baglan.Open();
                        string sorgu = "UPDATE Otobusler SET plaka=@plaka, koltukSayisi=@koltukSayisi, markaID=@markaID, aktifMi=@aktifMi WHERE otobusID=@ID";
                        using (SqlCommand command = new SqlCommand(sorgu, baglan))
                        {
                            command.Parameters.AddWithValue("@ID", guna2ComboBox7.SelectedValue);
                            command.Parameters.AddWithValue("@plaka", guna2TextBox1.Text); 
                            command.Parameters.AddWithValue("@koltukSayisi", guna2ComboBox3.SelectedItem);
                            command.Parameters.AddWithValue("@markaID", guna2ComboBox2.SelectedValue);
                            command.Parameters.AddWithValue("@aktifMi", guna2ComboBox4.SelectedIndex);

                            int result = command.ExecuteNonQuery();

                            if (result > 0)
                            {
                                MessageBox.Show("Kayıt başarıyla güncellendi!");
                            }
                            else
                            {
                                MessageBox.Show("Güncelleme başarısız. Kayıt bulunamadı.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show("Hata oluştu: " + ex.Message);
                    }
                    finally
                    {
                        baglan.Close();
                        guna2ComboBox7.SelectedIndexChanged -= guna2ComboBox7_SelectedIndexChanged;
                        sqllisteleme();
                    }
                    
                }
            }
        }
        private void guna2Button2_Click(object sender, EventArgs e)
        {
            if (guna2ComboBox6.SelectedIndex != -1)
            {
                DialogResult sonuc = MessageBox.Show("Otobüs silinecek,\nEmin misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (sonuc == DialogResult.Yes)
                {
                    try
                    {
                        baglan.Open();  
                        string sorgu = "DELETE FROM Otobusler WHERE  otobusID= @otobusID;";


                        using (SqlCommand komut = new SqlCommand(sorgu, baglan))
                        {
                            komut.Parameters.AddWithValue("@otobusID", guna2ComboBox6.SelectedValue);
                            int rowsAffected = komut.ExecuteNonQuery();
                            if (rowsAffected > 0)
                                MessageBox.Show("Silme başarılı!");
                            else
                                MessageBox.Show("Kayıt bulunamadı veya silinemedi.");
                        }
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show("Hata oluştu: " + ex.Message);
                    }
                    finally 
                    {
                        baglan.Close();
                        guna2ComboBox7.SelectedIndexChanged -= guna2ComboBox7_SelectedIndexChanged;
                        sqllisteleme();
                    }
                }
            }
        }

        private void sqllisteleme()
        {
            string sorgu1 = "SELECT otobusMarkaID,markaAdi  FROM OtobusMarkalari";
            string sorgu2 = "SELECT otobusID, plaka  FROM Otobusler";
            try
            {
                guna2ComboBox1.DataSource = null;
                guna2ComboBox2.DataSource = null;
                guna2ComboBox6.DataSource = null;
                guna2ComboBox7.DataSource = null;
                baglan.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(sorgu1, baglan);
                DataTable table = new DataTable();
                table.Clear();
                adapter.Fill(table);

                guna2ComboBox1.Items.Clear();
                guna2ComboBox2.Items.Clear();

                guna2ComboBox1.DataSource = table;
                guna2ComboBox1.DisplayMember = "markaAdi"; 
                guna2ComboBox1.ValueMember = "otobusMarkaID"; 

                guna2ComboBox2.DataSource = table;
                guna2ComboBox2.DisplayMember = "markaAdi";
                guna2ComboBox2.ValueMember = "otobusMarkaID";
                guna2ComboBox2.SelectedIndex = -1;

                SqlDataAdapter adapter2 = new SqlDataAdapter(sorgu2, baglan);
                DataTable table2 = new DataTable();
                table2.Clear();
                adapter2.Fill(table2);

                guna2ComboBox6.Items.Clear();
                guna2ComboBox6.DataSource = table2;
                guna2ComboBox6.DisplayMember = "plaka";
                guna2ComboBox6.ValueMember = "otobusID";
                guna2ComboBox6.SelectedIndex = -1;


                guna2ComboBox7.Items.Clear();
                guna2ComboBox7.DataSource = table2;
                guna2ComboBox7.DisplayMember = "plaka";
                guna2ComboBox7.ValueMember = "otobusID";
                guna2ComboBox7.SelectedIndex = -1;
                guna2ComboBox7.SelectedIndexChanged += guna2ComboBox7_SelectedIndexChanged;



            }
            catch (Exception ex)
            {
                MessageBox.Show("Marka bilgileri yüklenirken hata oluştu: " + ex.Message,
                      "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { baglan.Close(); }

        }


        private void sqlGuncellemeIcinListeleme(string otobusID)
        {
            string sorgu = "SELECT plaka, koltukSayisi, markaID, aktifMi FROM Otobusler WHERE otobusID = @otobusID";

            try
            {

                using (SqlCommand komut = new SqlCommand(sorgu, baglan))
                {
                    komut.Parameters.AddWithValue("@otobusID", otobusID);
                    baglan.Open();

                    using (SqlDataReader reader = komut.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            guna2TextBox1.Text = reader["plaka"].ToString();

                            string koltukSayisi = reader["koltukSayisi"].ToString();
                            if (guna2ComboBox3.Items.Contains(koltukSayisi))
                            {
                                guna2ComboBox3.SelectedItem = koltukSayisi;
                            }

                            object markaID = reader["markaID"];
                            if (markaID != DBNull.Value)
                            {
                                guna2ComboBox2.SelectedValue = Convert.ToInt32(markaID);
                            }

                            object aktifMi = reader["aktifMi"];
                            if (aktifMi != DBNull.Value)
                            {
                                int aktifIndex = Convert.ToBoolean(aktifMi) ? 1 : 0;
                                if (guna2ComboBox4.Items.Count > aktifIndex)
                                {
                                    guna2ComboBox4.SelectedIndex = aktifIndex;
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("Otobüs bulunamadı!", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show($"Veritabanı hatası oluştu: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                baglan.Close();
            }

        }

        private void guna2ComboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (guna2ComboBox7.SelectedIndex != -1)
            {
                sqlGuncellemeIcinListeleme(guna2ComboBox7.SelectedValue.ToString());
            }
        }

        private void guna2ComboBox7_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }
    }
}

