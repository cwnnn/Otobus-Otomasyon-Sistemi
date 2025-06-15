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

namespace OtobusOtomasyonSistemi.AdminArayuz._4OtobusYonetimi
{
    public partial class FormOtobusMarkaEkle : Form
    {
        static string baglanti = "Data Source=LAPTOP-R03\\SQLEXPRESS;Initial Catalog=otobusOtomasyonu;Integrated Security=True;TrustServerCertificate=True";
        SqlConnection baglan = new SqlConnection(baglanti);
        public FormOtobusMarkaEkle()
        {
            InitializeComponent();
        }

        private void FormOtobusMarkaEkle_Load(object sender, EventArgs e)
        {
            sqlListeleme();
        }

        private void sqlListeleme()
        {
            string sorgu = "SELECT *  FROM OtobusMarkalari;";
            try
            {
                guna2ComboBox1.DataSource = null;
                guna2ComboBox2.DataSource = null;


                baglan.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(sorgu, baglan);
                DataTable table = new DataTable();
                table.Clear();
                adapter.Fill(table);

                guna2ComboBox2.Items.Clear();

                guna2ComboBox2.DataSource = table;
                guna2ComboBox2.DisplayMember = "markaAdi";  
                guna2ComboBox2.ValueMember = "otobusMarkaID";
                guna2ComboBox2.SelectedIndex = -1;

                guna2ComboBox1.Items.Clear();

                guna2ComboBox1.DataSource = table;
                guna2ComboBox1.DisplayMember = "markaAdi";  
                guna2ComboBox1.ValueMember = "otobusMarkaID";
                guna2ComboBox1.SelectedIndex = -1;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Marka bilgileri yüklenirken hata oluştu: " + ex.Message,
                      "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally 
            { 
                baglan.Close();
                textBoxkalkisS.Text = "";
                guna2TextBox1.Text = "";
            }
        }

        private void guna2Button1YetkiEkle_Click(object sender, EventArgs e)
        {
            if (textBoxkalkisS.Text != "" )
            {
                DialogResult sonuc = MessageBox.Show("Otobüs Eklenecek,\nEmin misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (sonuc == DialogResult.Yes)
                {
                    try
                    {
                        baglan.Open();
                        string sorgu1 = "INSERT INTO OtobusMarkalari(markaAdi) VALUES (@markaAdi)";
                        using (SqlCommand komut = new SqlCommand(sorgu1, baglan))
                        {
                            komut.Parameters.AddWithValue("@markaAdi", textBoxkalkisS.Text);

                            int rowsAffected = komut.ExecuteNonQuery();

                            if (rowsAffected > 0)
                                MessageBox.Show("ekleme başarılı!");
                            
                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message,
                              "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        baglan.Close();
                        sqlListeleme();
                    }
                }
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (guna2TextBox1.Text != "" && guna2ComboBox2.SelectedIndex != -1)
            {
                DialogResult sonuc = MessageBox.Show("Otobüs Eklenecek,\nEmin misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (sonuc == DialogResult.Yes)
                {
                    try
                    {
                        baglan.Open();
                        string sorgu = "UPDATE OtobusMarkalari SET markaAdi=@markaAdi WHERE otobusMarkaID=@ID";
                        using (SqlCommand command = new SqlCommand(sorgu, baglan))
                        {
                            command.Parameters.AddWithValue("@markaAdi",guna2TextBox1.Text);
                            command.Parameters.AddWithValue("@ID", guna2ComboBox2.SelectedValue);
           

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
                        MessageBox.Show(ex.Message,
                              "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        baglan.Close();
                        sqlListeleme();
                    }

                }
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            if (guna2ComboBox1.SelectedIndex != -1)
            {
                DialogResult sonuc = MessageBox.Show("marka silinecek,\nEmin misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (sonuc == DialogResult.Yes)
                {
                    try
                    {
                        baglan.Open();
                        string sorgu = "DELETE FROM OtobusMarkalari WHERE  otobusMarkaID = @otobusMarkaID;";


                        using (SqlCommand komut = new SqlCommand(sorgu, baglan))
                        {
                            komut.Parameters.AddWithValue("@otobusMarkaID", guna2ComboBox1.SelectedValue);
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
                        sqlListeleme();
                    }
                }
            }
        }
    }
}
