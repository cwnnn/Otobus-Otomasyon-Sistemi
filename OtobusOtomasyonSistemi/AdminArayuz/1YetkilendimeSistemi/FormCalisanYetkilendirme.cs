using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using TheArtOfDevHtmlRenderer.Adapters;
using static System.ComponentModel.Design.ObjectSelectorEditor;

namespace OtobusOtomasyonSistemi.AdminArayuz._1YetkilendimeSistemi
{
    public partial class FormCalisanYetkilendirme : Form
    {
        public FormCalisanYetkilendirme()
        {
            InitializeComponent();
        }
        static string baglanti = "Data Source=LAPTOP-R03\\SQLEXPRESS;Initial Catalog=otobusOtomasyonu;Integrated Security=True;TrustServerCertificate=True";
        SqlConnection baglan = new SqlConnection(baglanti);

        string aratilanIsim = "";

        private void FormCalisanYetkilendirme_Load(object sender, EventArgs e)
        {
            baglan.Open();
            sqlCalisanListeleme("",guna2ComboBox1);
            sqlCalisanListeleme("",guna2ComboBox3);
            sqlYetkiListeleme();

            sqlCalisanlarinYetkileriniListeleme("", guna2DataGridView1);


        }
        private void guna2ComboBox1_KeyDown(object sender, KeyEventArgs e)
        {
            Guna2ComboBox cb = sender as Guna2ComboBox;
            if (e.KeyCode == Keys.Back)
            {
                if (aratilanIsim.Length > 0)
                    aratilanIsim = aratilanIsim.Substring(0, aratilanIsim.Length - 1);
                sqlCalisanListeleme(aratilanIsim, cb);
                sqlCalisanlarinYetkileriniListeleme(aratilanIsim, guna2DataGridView1);
                return;
            }

            if (e.Control || e.Shift || e.Alt || e.KeyCode == Keys.CapsLock || e.KeyCode == Keys.LWin || e.KeyCode == Keys.RWin || e.KeyCode == Keys.Tab)
            {
                return;
            }

            if (e.KeyCode == Keys.Space)
            {
                aratilanIsim += "";
                return;
            }

            if (e.KeyCode >= Keys.A && e.KeyCode <= Keys.Z)
            {
                char c = (char)e.KeyCode;
                aratilanIsim += (e.Shift ? c : char.ToLower(c));
            }
            sqlCalisanListeleme(aratilanIsim, cb);
            sqlCalisanlarinYetkileriniListeleme(aratilanIsim, guna2DataGridView1);
        }
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            DialogResult sonuc = MessageBox.Show("Yetki Güncellencek,\nEmin misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (sonuc == DialogResult.Yes)
            {
                string sorgu1 = "UPDATE Calisanlar SET yetkiID = @YetkiID WHERE calisanID = @calisanID;";
                using (SqlCommand komut = new SqlCommand(sorgu1, baglan))
                {
                    komut.Parameters.AddWithValue("@YetkiID", guna2ComboBox2.SelectedValue.ToString());
                    komut.Parameters.AddWithValue("@calisanID", guna2ComboBox1.SelectedValue.ToString());
                    int rowsAffected = komut.ExecuteNonQuery();

                    if (rowsAffected > 0)
                        MessageBox.Show("Güncelleme başarılı!");
                    else
                        MessageBox.Show("Kayıt bulunamadı veya güncellenemedi.");
                    sqlCalisanlarinYetkileriniListeleme("", guna2DataGridView1);
                }
            }


        }
        private void guna2Button2_Click(object sender, EventArgs e)
        {
            DialogResult sonuc = MessageBox.Show("Yetki Güncellencek,\nEmin misiniz?", "Onay", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (sonuc == DialogResult.Yes)
            {
                string sorgu1 = "UPDATE Calisanlar SET yetkiID = NULL WHERE calisanID = @calisanID;";
                using (SqlCommand komut = new SqlCommand(sorgu1, baglan))
                {
                    komut.Parameters.AddWithValue("@calisanID", guna2ComboBox3.SelectedValue.ToString());
                    int rowsAffected = komut.ExecuteNonQuery();

                    if (rowsAffected > 0)
                        MessageBox.Show("Güncelleme başarılı!");
                    else
                        MessageBox.Show("Kayıt bulunamadı veya güncellenemedi.");
                    sqlCalisanlarinYetkileriniListeleme("", guna2DataGridView1);
                }
            }
        }

        private void sqlCalisanListeleme(string deger, Guna2ComboBox cb)
        {
            string sorgu1 = "SELECT calisanID, ad, soyad FROM Calisanlar where replace(ad + soyad, ' ', '') like @deger;";


            using (SqlCommand komut = new SqlCommand(sorgu1, baglan))
            {
                komut.Parameters.AddWithValue("@deger", "%" + deger + "%");
                SqlDataAdapter adapter = new SqlDataAdapter(komut);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dt.Columns.Add("adsoyad", typeof(string), "ad + ' ' + soyad");

                cb.DataSource = dt;
                cb.DisplayMember = "adsoyad";
                cb.ValueMember = "calisanID";

            }

        }
        private void sqlYetkiListeleme()
        {
            string yetkileriListeleSorgu = "select yetkiID, yetkiAdi from Yetkiler;";
            SqlCommand command2 = new SqlCommand(yetkileriListeleSorgu, baglan);
            SqlDataAdapter adapter2 = new SqlDataAdapter(command2);
            DataTable table2 = new DataTable();
            adapter2.Fill(table2);
            guna2ComboBox2.DataSource = table2;
            guna2ComboBox2.DisplayMember = "yetkiAdi";
            guna2ComboBox2.ValueMember = "yetkiID";


        }

        private void sqlCalisanlarinYetkileriniListeleme(string deger, Guna2DataGridView dgv)
        {
            string sorgu = "select c.ad+' '+c.soyad as AdSoyad, isnull(y.yetkiAdi,'(Yok)') from Calisanlar c left join Yetkiler y on c.yetkiID = y.yetkiID where  replace(c.ad + c.soyad, ' ', '') like @deger;";
            using (SqlCommand komut = new SqlCommand(sorgu, baglan))
            {
                komut.Parameters.AddWithValue("@deger", "%" + deger + "%");
                SqlDataAdapter adapter = new SqlDataAdapter(komut);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dgv.DataSource = dt;
            }
            dgv.Columns[0].HeaderText = "Çalışan Adı";
            dgv.Columns[1].HeaderText = "Çalışanın Yetkisi";
           
            dgv.ColumnHeadersHeight = 40;
            dgv.Font = new Font("Segoe UI", 10);
            dgv.BorderStyle = BorderStyle.Fixed3D;
            
        }

        private void guna2ComboBox1_Leave(object sender, EventArgs e)
        {
            aratilanIsim = "";
        }
        
        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GlobalData.kullaniciYetki == "Yönetici")
            {
                KontrolEt();
            }
        }

        private void guna2ComboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GlobalData.kullaniciYetki == "Yönetici")
            {
                KontrolEt();
            }
        }

        private void KontrolEt()
        {
            string bulunanYetki = "";
            string aranan = guna2ComboBox1.Text;

            foreach (DataGridViewRow row in guna2DataGridView1.Rows)
            {
                if (row.Cells[0].Value != null && row.Cells[0].Value.ToString() == aranan)
                {
                    bulunanYetki = row.Cells[1].Value.ToString();
                    break;
                }
            }

            if (bulunanYetki == "Süper Yönetici" || guna2ComboBox2.Text == "Süper Yönetici")
            {
                guna2Button1.Enabled = false;
            }
            else
            {
                guna2Button1.Enabled = true;
            }
        }

        private void guna2ComboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GlobalData.kullaniciYetki == "Yönetici" && (guna2ComboBox3.Text == "Süper Yönetici" || guna2ComboBox3.Text == "Yönetici"))
            {
                guna2Button2.Enabled = false;
            }
            else
            { guna2Button2.Enabled = true; }
        }
    }
}
