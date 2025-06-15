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
    public partial class FormYolcuDuzenleme : Form
    {
        static string baglanti = "Data Source=LAPTOP-R03\\SQLEXPRESS;Initial Catalog=otobusOtomasyonu;Integrated Security=True;TrustServerCertificate=True";
        SqlConnection baglan = new SqlConnection(baglanti);


        public FormYolcuDuzenleme()
        {
            InitializeComponent();
        }

        private void FormYolcuDuzenleme_Load(object sender, EventArgs e)
        {
            guna2ComboBox1.SelectedIndexChanged -= guna2ComboBox1_SelectedIndexChanged;
            sqllisteleme();
            guna2ComboBox1.SelectedIndex = -1;
            guna2ComboBox1.SelectedIndexChanged += guna2ComboBox1_SelectedIndexChanged;
        }

        private void sqllisteleme()
        {
            string sorgu1 = "select yolcuID, ad+' '+soyad as yolcuAdi from  yolcular";
            string sorgu2 = "SELECT  turID, turAdi  FROM YolcuTurleri";
            try
            {
                guna2ComboBox1.DataSource = null;
                guna2ComboBox2.DataSource = null;
                guna2ComboBox3.DataSource = null;

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
                guna2ComboBox1.DisplayMember = "yolcuAdi";
                guna2ComboBox1.ValueMember = "yolcuID";



                SqlDataAdapter adapter2 = new SqlDataAdapter(sorgu2, baglan);
                DataTable table2 = new DataTable();
                table2.Clear();
                adapter2.Fill(table2);

                guna2ComboBox2.Items.Clear();
                guna2ComboBox2.DataSource = table2;
                guna2ComboBox2.DisplayMember = "turAdi";
                guna2ComboBox2.ValueMember = "turID";
                guna2ComboBox2.SelectedIndex = -1;

                guna2ComboBox3.Items.Clear();
                guna2ComboBox3.DataSource = table2;
                guna2ComboBox3.DisplayMember = "turAdi";
                guna2ComboBox3.ValueMember = "turID";
                guna2ComboBox3.SelectedIndex = -1;


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
            if (guna2ComboBox1.SelectedIndex !=-1)
            {
                
                sqlGuncellemeIcinListeleme(guna2ComboBox1.SelectedValue.ToString());
            }
        }
        private void sqlGuncellemeIcinListeleme(string ID)
        {
            string sorgu = "SELECT * FROM Yolcular WHERE yolcuID = @ID";

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
                            guna2TextBox1YetkiIsmi.Text = reader["ad"].ToString();
                            guna2TextBox1.Text = reader["soyad"].ToString();
                            guna2TextBox2.Text = reader["cinsiyet"].ToString();
                            guna2TextBox3.Text = reader["ePosta"].ToString();

                            string turID = reader["kullaniciTuruID"].ToString();
                          
                            
                                guna2ComboBox2.SelectedValue = turID;
                            

                            guna2TextBox4.Text = reader["telefon"].ToString();
                            guna2TextBox5.Text = reader["sifre"].ToString();
                            guna2TextBox6.Text = reader["tc"].ToString();

               
                            if (reader["pasaportMu"].ToString()=="True")
                            {
                                guna2CheckBox1.Checked = true;
                            }
                            else
                            {
                                guna2CheckBox1.Checked = false;
                            }

                            if (reader["kayitOlduMu"].ToString() == "True")
                            {
                                guna2CustomCheckBox1.Checked = true;
                            }
                            else
                            {
                                guna2CustomCheckBox1.Checked= false;
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

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            string sorgu = "UPDATE Yolcular SET ad = @ad, soyad = @soyad, cinsiyet = @cinsiyet, ePosta = @ePosta, kullaniciTuruID = @turID, telefon = @telefon, sifre = @sifre, tc = @tc, pasaportMu = @pasaport, kayitOlduMu = @kayit WHERE yolcuID = @id";

            try
            {
                using (SqlCommand komut = new SqlCommand(sorgu, baglan))
                {
                    komut.Parameters.AddWithValue("@ad", guna2TextBox1YetkiIsmi.Text);
                    komut.Parameters.AddWithValue("@soyad", guna2TextBox1.Text);
                    komut.Parameters.AddWithValue("@cinsiyet", guna2TextBox2.Text);
                    komut.Parameters.AddWithValue("@ePosta", guna2TextBox3.Text);
                    komut.Parameters.AddWithValue("@turID", guna2ComboBox2.SelectedValue ?? DBNull.Value);
                    komut.Parameters.AddWithValue("@telefon", guna2TextBox4.Text);
                    komut.Parameters.AddWithValue("@sifre", guna2TextBox5.Text);
                    komut.Parameters.AddWithValue("@tc", guna2TextBox6.Text);
                    komut.Parameters.AddWithValue("@pasaport", guna2CheckBox1.Checked);
                    komut.Parameters.AddWithValue("@kayit", guna2CustomCheckBox1.Checked);
                    komut.Parameters.AddWithValue("@id", guna2ComboBox1.SelectedValue);

                    baglan.Open();
                    int sonuc = komut.ExecuteNonQuery();
                    if (sonuc > 0)
                    {
                        MessageBox.Show("Yolcu bilgileri başarıyla güncellendi!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                    }
                    else
                    {
                        MessageBox.Show("Güncelleme başarısız!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Güncelleme hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            if ( guna2TextBox7.Text.Replace(" ", "") != "" && guna2TextBox8.Text.Replace(" ", "") != "" && guna2TextBox9.Text.Replace(" ", "") != "" &&
                guna2TextBox10.Text.Replace(" ", "") != "" && guna2TextBox11.Text.Replace(" ", "") != "" && guna2TextBox12.Text.Replace(" ", "") != "" &&
                guna2TextBox12.Text.Replace(" ", "") != "" && guna2ComboBox3.SelectedIndex !=-1)
            {
                yolcuEkle();
            }
        }

        private void yolcuEkle()
        {
            string sorgu = "INSERT INTO Yolcular (ad, soyad, cinsiyet, ePosta, telefon, sifre, kullaniciTuruID,tc,pasaportMu,kayitOlduMu) " +
                       "VALUES (@ad, @soyad, @cinsiyet, @ePosta, @telefon, @sifre, @turID, @tc, @pasmi, @kayitomi)";

            try
            {
                using (SqlCommand komut = new SqlCommand(sorgu, baglan))
                {
                    komut.Parameters.AddWithValue("@ad", guna2TextBox7.Text.Trim());
                    komut.Parameters.AddWithValue("@soyad", guna2TextBox8.Text.Trim());
                    komut.Parameters.AddWithValue("@cinsiyet", guna2TextBox9.Text.Trim());
                    komut.Parameters.AddWithValue("@ePosta", guna2TextBox10.Text.Trim());
                    komut.Parameters.AddWithValue("@telefon", guna2TextBox11.Text.Trim());
                    komut.Parameters.AddWithValue("@sifre", guna2TextBox12.Text.Trim());
                    komut.Parameters.AddWithValue("@turID", guna2ComboBox3.SelectedValue);
                    komut.Parameters.AddWithValue("@tc", guna2TextBox13.Text.Trim());

                    string pasmi, kayitmi;
                    if (guna2CheckBox2.Checked)
                    {
                        pasmi = "1";
                    }
                    else { pasmi = "0"; }
                    if (guna2CustomCheckBox2.Checked)
                    {
                        kayitmi = "1";
                    }
                    else { kayitmi = "0"; }

                    komut.Parameters.AddWithValue("@pasmi", pasmi);
                    komut.Parameters.AddWithValue("@kayitomi", kayitmi);


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

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            YolcuSil(guna2ComboBox1.SelectedValue.ToString());
        }
        private void YolcuSil(string yolcuID)
        {
            DialogResult onay = MessageBox.Show("Bu yolcuyu silmek istediğinize emin misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (onay == DialogResult.Yes)
            {
                string sorgu = "DELETE FROM Yolcular WHERE yolcuID = @id";

                try
                {
                    using (SqlCommand komut = new SqlCommand(sorgu, baglan))
                    {
                        komut.Parameters.AddWithValue("@id", yolcuID);
                        baglan.Open();
                        int sonuc = komut.ExecuteNonQuery();

                        if (sonuc > 0)
                        {
                            MessageBox.Show("Yolcu başarıyla silindi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Yolcu bulunamadı veya silinemedi.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
    }
    }

