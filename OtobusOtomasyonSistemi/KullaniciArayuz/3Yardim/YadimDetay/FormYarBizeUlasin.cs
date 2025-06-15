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

namespace OtobusOtomasyonSistemi
{
    public partial class FormYarBizeUlasin : Form
    {
        public FormYarBizeUlasin()
        {
            InitializeComponent();
        }

        private void FormYarBizeUlasin_Load(object sender, EventArgs e)
        {
            sqlBaglan();
            guna2ComboBox1.SelectedIndex = 0;
        }

        private void guna2TextBox1_Enter(object sender, EventArgs e)
        {
            textGirisAyrilis(guna2TextBox1, "TC Kimlik", true);
        }

        private void guna2TextBox1_Leave(object sender, EventArgs e)
        {
            textGirisAyrilis(guna2TextBox1, "TC Kimlik", false);
        }

        private void guna2TextBox2_Enter(object sender, EventArgs e)
        {
            textGirisAyrilis(guna2TextBox2, "E-Posta", true);
        }

        private void guna2TextBox2_Leave(object sender, EventArgs e)
        {
            textGirisAyrilis(guna2TextBox2, "E-Posta", false);
        }
        private void guna2TextBox3_Enter(object sender, EventArgs e)
        {
            textGirisAyrilis(guna2TextBox3, "Ad", true);
        }

        private void guna2TextBox3_Leave(object sender, EventArgs e)
        {
            textGirisAyrilis(guna2TextBox3, "Ad", false);
        }

        private void guna2TextBox4_Enter(object sender, EventArgs e)
        {
            textGirisAyrilis(guna2TextBox4, "Soyad", true);
        }

        private void guna2TextBox4_Leave(object sender, EventArgs e)
        {
            textGirisAyrilis(guna2TextBox4, "Soyad", false);
        }

        private void guna2TextBox5_Enter(object sender, EventArgs e)
        {
            textGirisAyrilis(guna2TextBox5, "Tel No", true);
        }

        private void guna2TextBox5_Leave(object sender, EventArgs e)
        {
            textGirisAyrilis(guna2TextBox5, "Tel No", false);
        }

        private void richTextBox1_Enter(object sender, EventArgs e)
        {
            if (richTextBox1.Text == "Mesajınız...")
            {
                richTextBox1.Text = "";
                richTextBox1.ForeColor = Color.White;

            }
        }

        private void richTextBox1_Leave(object sender, EventArgs e)
        {
            if (richTextBox1.Text == "" || richTextBox1.Text == " ")
            {
                richTextBox1.Text = "Mesajınız...";
                richTextBox1.ForeColor = Color.FromArgb(125, 137, 149);
            }
        }

        private void textGirisAyrilis(Guna2TextBox tb, string st, bool gA)
        {
            if (gA)
            {
                if (tb.Text == st)
                {
                    tb.Text = "";
                    tb.ForeColor = Color.White;
                }
            }
            else
            {
                if (tb.Text == "" || tb.Text == " ")
                {
                    tb.Text = st;
                    tb.ForeColor = Color.FromArgb(125, 137, 149);
                }
            }
        }



        private void guna2ButtonOnayla_Click(object sender, EventArgs e)
        {
            if (guna2ComboBox1.SelectedIndex != -1 && guna2TextBox1.Text != "TC Kimlik" && guna2TextBox2.Text != "E-Posta" &&
                guna2TextBox3.Text != "Ad" && guna2TextBox4.Text != "Soyad" && guna2TextBox5.Text != "Tel No" && richTextBox1.Text != "Mesajınız...")
            {
                sqleGonder();
            }
            else MessageBox.Show("lütefen tüm alanları doldurunuz");
        }
            private void sqlBaglan()
            {
                string baglanti = "Data Source=LAPTOP-R03\\SQLEXPRESS;Initial Catalog=otobusOtomasyonu;Integrated Security=True;TrustServerCertificate=True";

                using (SqlConnection baglan = new SqlConnection(baglanti))
                {
                    try
                    {
                        baglan.Open();
                        string query = "SELECT konuID, konuAdi FROM Konular";

                        SqlCommand cmd = new SqlCommand(query, baglan);
                        SqlDataReader reader = cmd.ExecuteReader();

                        var konularListesi = new List<KeyValuePair<int, string>>();

                        while (reader.Read())
                        {
                            konularListesi.Add(new KeyValuePair<int, string>(
                                reader.GetInt32(0),
                                reader.GetString(1)
                            ));
                        }

                        guna2ComboBox1.DataSource = konularListesi;
                        guna2ComboBox1.DisplayMember = "Value";
                        guna2ComboBox1.ValueMember = "Key";
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Konular yüklenirken hata oluştu:\n" + ex.Message);
                    }
                }
        
            }
            private void sqleGonder()
            {
            string baglanti = "Data Source=LAPTOP-R03\\SQLEXPRESS;Initial Catalog=otobusOtomasyonu;Integrated Security=True;TrustServerCertificate=True";

            using (SqlConnection baglan = new SqlConnection(baglanti))
            {
                try
                {
                    baglan.Open();

                    string query = "INSERT INTO YardimTalepleri (konuID, mesaj, tarih, tc, eposta, ad, soyad, tel) " +
                                   "VALUES (@konuID, @mesaj, @tarih, @tc, @eposta, @ad, @soyad, @tel)";

                    SqlCommand command = new SqlCommand(query, baglan);

                    command.Parameters.AddWithValue("@konuID", guna2ComboBox1.SelectedValue);
                    command.Parameters.AddWithValue("@mesaj", richTextBox1.Text);
                    command.Parameters.AddWithValue("@tarih", DateTime.Now);
                    command.Parameters.AddWithValue("@tc", guna2TextBox1.Text);
                    command.Parameters.AddWithValue("@eposta", guna2TextBox2.Text);
                    command.Parameters.AddWithValue("@ad", guna2TextBox3.Text);
                    command.Parameters.AddWithValue("@soyad", guna2TextBox4.Text);
                    command.Parameters.AddWithValue("@tel", guna2TextBox5.Text);

                    command.ExecuteNonQuery();
                    MessageBox.Show("Kayıt başarıyla eklendi.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Kayıt eklenirken hata oluştu:\n" + ex.Message);
                }
            }
        }



    }
}
