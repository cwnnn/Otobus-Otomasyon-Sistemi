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

namespace OtobusOtomasyonSistemi.AdminArayuz._3KullaniciYönetimi
{


    public partial class FormCalisanDuzenleme : Form
    {
        static string baglanti = "Data Source=LAPTOP-R03\\SQLEXPRESS;Initial Catalog=otobusOtomasyonu;Integrated Security=True;TrustServerCertificate=True";
        SqlConnection baglan = new SqlConnection(baglanti);

        public FormCalisanDuzenleme()
        {
            InitializeComponent();
        }

        private void FormCalisanDuzenleme_Load(object sender, EventArgs e)
        {
            guna2ComboBox1.SelectedIndexChanged -= guna2ComboBox1_SelectedIndexChanged;
            sqllisteleme();
            guna2ComboBox1.SelectedIndex = -1;
            guna2ComboBox1.SelectedIndexChanged += guna2ComboBox1_SelectedIndexChanged;
        }
        private void sqllisteleme()
        {
            string sorgu1 = @"SELECT calisanID
      ,[ad] +' '+[soyad] as ad
  FROM [otobusOtomasyonu].[dbo].[Calisanlar]
";

            try
            {
                guna2ComboBox1.DataSource = null;

                baglan.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(sorgu1, baglan);
                DataTable table = new DataTable();
                table.Clear();
                adapter.Fill(table);
                guna2DataGridView1.DataSource = table;
                guna2DataGridView1.ColumnHeadersHeight = 40;
                guna2DataGridView1.Columns[0].Width = 50;

                guna2ComboBox1.Items.Clear();


                guna2ComboBox1.DataSource = table;
                guna2ComboBox1.DisplayMember = "ad";
                guna2ComboBox1.ValueMember = "calisanID";


            }
            catch (Exception ex)
            {
                MessageBox.Show("Yolcu bilgileri yüklenirken hata oluştu: " + ex.Message,
                      "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally { baglan.Close(); }

        }
        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (guna2ComboBox1.SelectedIndex != -1)
            {

                sqlGuncellemeIcinListeleme(guna2ComboBox1.SelectedValue.ToString());
            }
        }
        private void sqlGuncellemeIcinListeleme(string ID)
        {
            string sorgu = "SELECT * FROM Calisanlar WHERE calisanID = @ID";

            try
            {

                using (SqlCommand komut = new SqlCommand(sorgu, baglan))
                {
                    komut.Parameters.AddWithValue("@ID", ID);
                    baglan.Open();

                    using (SqlDataReader reader = komut.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            guna2TextBox1.Text = reader["ad"].ToString();
                            guna2TextBox2.Text = reader["soyad"].ToString();
                            guna2TextBox3.Text = reader["telefon"].ToString();
                            guna2TextBox4.Text = reader["tc"].ToString();
                            guna2TextBox5.Text = reader["sifre"].ToString();


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

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            CalisanGuncelle();
        }

        private void CalisanGuncelle()
        {
            if (guna2ComboBox1.SelectedIndex != -1 &&
                !string.IsNullOrWhiteSpace(guna2TextBox1.Text) &&
                !string.IsNullOrWhiteSpace(guna2TextBox2.Text) &&
                !string.IsNullOrWhiteSpace(guna2TextBox3.Text) &&
                !string.IsNullOrWhiteSpace(guna2TextBox4.Text) &&
                !string.IsNullOrWhiteSpace(guna2TextBox5.Text))
            {
                string sorgu = @"UPDATE Calisanlar 
                         SET ad = @ad, soyad = @soyad, telefon = @telefon, tc = @tc, sifre = @sifre 
                         WHERE calisanID = @id";

                try
                {
                    using (SqlCommand komut = new SqlCommand(sorgu, baglan))
                    {
                        komut.Parameters.AddWithValue("@ad", guna2TextBox1.Text.Trim());
                        komut.Parameters.AddWithValue("@soyad", guna2TextBox2.Text.Trim());
                        komut.Parameters.AddWithValue("@telefon", guna2TextBox3.Text.Trim());
                        komut.Parameters.AddWithValue("@tc", guna2TextBox4.Text.Trim());
                        komut.Parameters.AddWithValue("@sifre", guna2TextBox5.Text.Trim());
                        komut.Parameters.AddWithValue("@id", guna2ComboBox1.SelectedValue);

                        baglan.Open();
                        int sonuc = komut.ExecuteNonQuery();

                        if (sonuc > 0)
                        {
                            MessageBox.Show("Çalışan bilgileri başarıyla güncellendi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                           
                        }
                        else
                        {
                            MessageBox.Show("Güncelleme başarısız. Lütfen tekrar deneyin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Güncelleme sırasında hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    baglan.Close();
                    guna2ComboBox1.SelectedIndexChanged += guna2ComboBox1_SelectedIndexChanged;
                    sqllisteleme();
                    guna2ComboBox1.SelectedIndexChanged += guna2ComboBox1_SelectedIndexChanged;
                }
            }
            else
            {
                MessageBox.Show("Lütfen tüm alanları doldurun ve bir çalışan seçin.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void calisanEkle()
        {
            string sorgu = "INSERT INTO Calisanlar (ad, soyad, telefon, sifre, tc) " +
                       "VALUES (@ad, @soyad, @telefon, @sifre, @tc)";

            try
            {
                using (SqlCommand komut = new SqlCommand(sorgu, baglan))
                {
                    komut.Parameters.AddWithValue("@ad", guna2TextBox7.Text.Trim());
                    komut.Parameters.AddWithValue("@soyad", guna2TextBox8.Text.Trim());
                    komut.Parameters.AddWithValue("@telefon", guna2TextBox11.Text.Trim());
                    komut.Parameters.AddWithValue("@tc", guna2TextBox13.Text.Trim());
                    komut.Parameters.AddWithValue("@sifre", guna2TextBox12.Text.Trim());
              

                    baglan.Open();
                    int sonuc = komut.ExecuteNonQuery();

                    if (sonuc > 0)
                    {
                        MessageBox.Show("Yolcu başarıyla eklendi.", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Kayıt eklenemedi.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                baglan.Close();
                guna2ComboBox1.SelectedIndexChanged += guna2ComboBox1_SelectedIndexChanged;
                sqllisteleme();
                guna2ComboBox1.SelectedIndexChanged += guna2ComboBox1_SelectedIndexChanged;
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            calisanEkle();
        }
        private void CalisanSil(string calisanID)
        {
            DialogResult onay = MessageBox.Show("Bu çalışanı silmek istediğinize emin misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (onay == DialogResult.Yes)
            {
                string sorgu = "DELETE FROM Calisanlar WHERE calisanID = @id";

                try
                {
                    using (SqlCommand komut = new SqlCommand(sorgu, baglan))
                    {
                        komut.Parameters.AddWithValue("@id", calisanID);
                        baglan.Open();
                        int sonuc = komut.ExecuteNonQuery();

                        if (sonuc > 0)
                        {
                            MessageBox.Show("Çalışan başarıyla silindi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                       
                            guna2ComboBox1.SelectedIndex = -1;
                        }
                        else
                        {
                            MessageBox.Show("Çalışan silinemedi veya bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Silme sırasında hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    baglan.Close();
                    guna2ComboBox1.SelectedIndexChanged += guna2ComboBox1_SelectedIndexChanged;
                    sqllisteleme();
                    guna2ComboBox1.SelectedIndexChanged += guna2ComboBox1_SelectedIndexChanged;
                }
            }
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            CalisanSil(guna2ComboBox1.SelectedValue.ToString());
        }
    }
}
