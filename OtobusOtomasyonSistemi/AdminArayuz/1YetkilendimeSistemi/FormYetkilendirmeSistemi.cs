using Guna.UI2.WinForms;
using Guna.UI2.WinForms.Suite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OtobusOtomasyonSistemi.AdminArayuz._1YetkilendimeSistemi
{
    public partial class FormYetkilendirmeSistemi : Form
    {
        static string baglanti = "Data Source=LAPTOP-R03\\SQLEXPRESS;Initial Catalog=otobusOtomasyonu;Integrated Security=True;TrustServerCertificate=True";
        SqlConnection baglan = new SqlConnection(baglanti);
        public FormYetkilendirmeSistemi()
        {
            InitializeComponent();
        }


        private void FormYetkilendirmeSistemi_Load(object sender, EventArgs e)
        {
            baglan.Open();
            sqlListelemeLoad();
        }

        private void guna2ComboBox4YetkiSec_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (guna2ComboBox4YetkiSec.SelectedItem.ToString() == "Süper Yönetici")
            {
                panelEnabled.Enabled = false;
            }
            else
            {
                panelEnabled.Enabled = true;
            }

            sqlYetkilerinKapsamlariniListeleme(guna2ComboBox4YetkiSec.SelectedItem.ToString(), guna2DataGridView1);


        }
        private void guna2Button4Ekle_Click(object sender, EventArgs e)
        {
            if (richTextBox4Kontrol.Text.Contains(guna2ComboBox4KapsamSec.SelectedItem.ToString()) == false)
            {
                richTextBox4Kontrol.Text += guna2ComboBox4KapsamSec.SelectedItem + ", ";
            }
        }
        private void guna2Button4Cikar_Click(object sender, EventArgs e)
        {
            richTextBox4Kontrol.Text = richTextBox4Kontrol.Text.Replace(guna2ComboBox4KapsamSec.SelectedItem + ", ", "");
        }

        private void guna2ButtonKapsamiGuncelle_Click(object sender, EventArgs e)
        {
            string yetkiIsmi = guna2ComboBox4YetkiSec.SelectedItem.ToString(); 

            DialogResult sonuc2 = MessageBox.Show(yetkiIsmi +" Yetkisinin Kapsamları Güncellenecek,\nEmin misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (sonuc2 == DialogResult.Yes)
            {
                sqlYetkilerinKapsamlariSilme(yetkiIsmi);
                sqlYetkilerinKapsamlariniEkleme(yetkiIsmi, richTextBox4Kontrol);
                sqlYetkilerinKapsamlariniListeleme(guna2ComboBox4YetkiSec.SelectedItem.ToString(), guna2DataGridView1);
                MessageBox.Show("Güncelleme başarılı!");
            }
        }

        private void guna2Button1KapsamEkle_Click(object sender, EventArgs e)
        {
            if (richTextBox1YetkiKontrol.Text.Contains(guna2ComboBox1KapsamEkle.SelectedItem.ToString())==false)
            {
                richTextBox1YetkiKontrol.Text += guna2ComboBox1KapsamEkle.SelectedItem + ", ";
            }
            
        }

        private void guna2Button1KapsamCikar_Click(object sender, EventArgs e)
        {
            richTextBox1YetkiKontrol.Text = richTextBox1YetkiKontrol.Text.Replace(guna2ComboBox1KapsamEkle.SelectedItem + ", ", "");
        }
    
        private void guna2Button1YetkiEkle_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(guna2TextBox1YetkiIsmi.Text))
            {
                MessageBox.Show("Yetki İsmi Giriniz.","DİKKAT");
            }
            else
            {
                if (string.IsNullOrEmpty(richTextBox1YetkiKontrol.Text))
                {
                    DialogResult sonuc1 = MessageBox.Show("Hiç Kapsam Eklemediniz,\n Devam Etmek ister Misiniz?", "DİKKAT", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (sonuc1 == DialogResult.Yes)
                    {
                        sqlYetkiEkleme();
                        sqlListelemeLoad();
                    }
                }
                else
                {
                    DialogResult sonuc2 = MessageBox.Show("Yetki Eklenecek,\nEmin misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    if (sonuc2 == DialogResult.Yes)
                    {
                        sqlYetkiEkleme();
                        sqlListelemeLoad();
                    }
                }
            }
        }

        private void guna2Button2YetkiGuncelle_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(guna2TextBox2YeniKapsamIsmi.Text))
            {
                MessageBox.Show("Yetki İsmi Giriniz.", "DİKKAT");
            }
            else 
            {
                DialogResult sonuc = MessageBox.Show("Yetki Güncellencek,\nEmin misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (sonuc == DialogResult.Yes)
                {
                    string sorgu1 = "UPDATE Yetkiler SET yetkiAdi = @YeniYetkiAdi WHERE yetkiAdi = @EskiYetkiAdi;";
                    using (SqlCommand komut = new SqlCommand(sorgu1, baglan))
                    {
                        komut.Parameters.AddWithValue("@YeniYetkiAdi", guna2TextBox2YeniKapsamIsmi.Text);
                        komut.Parameters.AddWithValue("@EskiYetkiAdi", guna2ComboBox2YetkiSec.SelectedItem.ToString());
                        int rowsAffected = komut.ExecuteNonQuery();

                        if (rowsAffected > 0)
                            MessageBox.Show("Güncelleme başarılı!");
                        else
                            MessageBox.Show("Kayıt bulunamadı veya güncellenemedi.");
                    }
                    sqlListelemeLoad();
                }
            }
        }

        private void guna2Button2YetkiSil_Click(object sender, EventArgs e)
        {
            DialogResult sonuc2 = MessageBox.Show("Yetki SİLİNECEK,\nEmin misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (sonuc2 == DialogResult.Yes)
            {
                string sorgu = "DELETE FROM Yetkiler WHERE yetkiAdi = @yetkiAdi;";

                sqlYetkilerinKapsamlariSilme(guna2ComboBox3YetkiSil.SelectedItem.ToString());

                using (SqlCommand komut = new SqlCommand(sorgu, baglan))
                {
                    komut.Parameters.AddWithValue("@yetkiAdi", guna2ComboBox3YetkiSil.SelectedItem.ToString());
                    int rowsAffected = komut.ExecuteNonQuery();
                    if (rowsAffected > 0)
                        MessageBox.Show("Silme başarılı!");
                    else
                        MessageBox.Show("Kayıt bulunamadı veya silinemedi.");
                }
                sqlListelemeLoad();
            }
            
        }

        private void sqlListelemeLoad()
        {
            string KapsamlariListeleSorgu = "select kapsamAdi from Kapsamlar;";
            string yetkileriListeleSorgu = "select yetkiAdi from Yetkiler;";

            SqlCommand command = new SqlCommand(KapsamlariListeleSorgu, baglan);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            guna2ComboBox1KapsamEkle.Items.Clear();
            guna2ComboBox4KapsamSec.Items.Clear();
            foreach (DataRow row in table.Rows)
            {
                string yetkiAdi = row["kapsamAdi"].ToString();
                guna2ComboBox1KapsamEkle.Items.Add(yetkiAdi);
                guna2ComboBox4KapsamSec.Items.Add(yetkiAdi);
            }
            guna2ComboBox1KapsamEkle.SelectedIndex = 0;
            guna2ComboBox4KapsamSec.SelectedIndex = 1;

            SqlCommand command2 = new SqlCommand(yetkileriListeleSorgu, baglan);
            SqlDataAdapter adapter2 = new SqlDataAdapter(command2);
            DataTable table2 = new DataTable();
            adapter2.Fill(table2);
            guna2ComboBox2YetkiSec.Items.Clear();
            guna2ComboBox3YetkiSil.Items.Clear();
            guna2ComboBox4YetkiSec.Items.Clear();
            foreach (DataRow row in table2.Rows)
            {
                string yetkiAdi = row["yetkiAdi"].ToString();
                guna2ComboBox2YetkiSec.Items.Add(yetkiAdi);
                guna2ComboBox3YetkiSil.Items.Add(yetkiAdi);
                guna2ComboBox4YetkiSec.Items.Add(yetkiAdi);
            }
            guna2ComboBox2YetkiSec.SelectedIndex = 0;
            guna2ComboBox3YetkiSil.SelectedIndex = 0;
            guna2ComboBox4YetkiSec.SelectedIndex = 0;

        }

        private void sqlYetkiEkleme()
        {
            
            

            string sorgu1 = "select Count(*) from Yetkiler where LOWER(REPLACE(yetkiAdi, ' ', '')) =@yetkiAdi;";
            string sorgu2 = "INSERT INTO Yetkiler(yetkiAdi) VALUES (@yetkiAdi);";
           //buraya kapsamları döngüyle yerleştirecek sorgu yazılacak

            using (SqlCommand komut1 = new SqlCommand(sorgu1, baglan))
            {
                komut1.Parameters.AddWithValue("@yetkiAdi", Regex.Replace(guna2TextBox1YetkiIsmi.Text, @"\s+", "").ToLower());
                object sonuc = komut1.ExecuteScalar();
                int yetkiSayisi = Convert.ToInt32(sonuc);
                if (yetkiSayisi > 0)
                {
                    MessageBox.Show("Bu Yetki Zaten Var");
                    return;
                }
            }

            using (SqlCommand komut2 = new SqlCommand(sorgu2, baglan))
            {
                komut2.Parameters.AddWithValue("@yetkiAdi", guna2TextBox1YetkiIsmi.Text);
                komut2.ExecuteNonQuery();
            }

            sqlYetkilerinKapsamlariniEkleme(guna2TextBox1YetkiIsmi.Text, richTextBox1YetkiKontrol);

            MessageBox.Show("Yetki başarılı bir şekilde eklendi.");

            richTextBox1YetkiKontrol.Text = "";
            guna2TextBox1YetkiIsmi.Text = "";
        }

        private void sqlYetkilerinKapsamlariSilme(string yetkiIsmi)
        {
            string sorgu = "DELETE yk FROM YetkilerinKapsamlari yk join Yetkiler y on yk.yetkiID = y.yetkiID WHERE y.yetkiAdi = @yetkiAdi; ";
            using (SqlCommand komut = new SqlCommand(sorgu, baglan))
            {
                komut.Parameters.AddWithValue("@yetkiAdi", yetkiIsmi);
                komut.ExecuteNonQuery();
            }
        }

        private void sqlYetkilerinKapsamlariniEkleme(string yetkiAdi, RichTextBox rb)
        {
            int sayac = 0, virgulIndex;
            string kapsam, kapsamAdi = "", kapsamID = "";

            string sorgu3 = "select yetkiID from Yetkiler where yetkiAdi= @yetkiAdi;";
            string sorgu4 = "select ID from Kapsamlar where kapsamAdi = @kapsamIsmi;";
            string sorgu5 = "insert into YetkilerinKapsamlari(yetkiID,kapsamID) values (@yetkiID,@kapsamID); ";

            using (SqlCommand komut3 = new SqlCommand(sorgu3, baglan))
            {
                komut3.Parameters.AddWithValue("@yetkiAdi", yetkiAdi);
                object sonuc = komut3.ExecuteScalar();
                kapsamAdi = sonuc.ToString();
            }
            foreach (char virgul in rb.Text)
            {
                if (virgul == ',')
                {
                    sayac++;
                }
            }
            for (int i = 0; i < sayac; i++)
            {
                virgulIndex = rb.Text.IndexOf(',');
                kapsam = rb.Text.Substring(0, virgulIndex);
                rb.Text = rb.Text.Replace(kapsam + ", ", "");

                using (SqlCommand komut4 = new SqlCommand(sorgu4, baglan))
                {
                    komut4.Parameters.AddWithValue("@kapsamIsmi", kapsam);
                    object sonuc = komut4.ExecuteScalar();
                    kapsamID = sonuc.ToString();
                }
                using (SqlCommand komut5 = new SqlCommand(sorgu5, baglan))
                {
                    komut5.Parameters.AddWithValue("@yetkiID", kapsamAdi);
                    komut5.Parameters.AddWithValue("@kapsamID", kapsamID);
                    komut5.ExecuteNonQuery();
                }


            }
        }

        private void sqlYetkilerinKapsamlariniListeleme(string yetkiAdi, Guna2DataGridView dgv)
        {
            string sorgu = "select k.kapsamAdi from Kapsamlar k join YetkilerinKapsamlari yk on k.ID = yk.kapsamID join Yetkiler y on yk.yetkiID = y.yetkiID where y.yetkiAdi = @yetkiAdi;";
            using (SqlCommand komut = new SqlCommand(sorgu, baglan))
            {
                komut.Parameters.AddWithValue("@yetkiAdi", yetkiAdi);
                SqlDataAdapter adapter = new SqlDataAdapter(komut);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dgv.DataSource = dt;
            }
            dgv.Columns[0].HeaderText = "Yetki Kapsamları";
            dgv.ColumnHeadersHeight = 40;
            dgv.Columns[0].Width = 200;
            dgv.Font = new Font("Segoe UI", 10);
        }
    }
}
