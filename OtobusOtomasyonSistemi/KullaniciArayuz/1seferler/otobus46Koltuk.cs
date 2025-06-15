using Guna.UI2.WinForms;
using OtobusOtomasyonSistemi.KullaniciArayuz._1seferler;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace OtobusOtomasyonSistemi
{
    public partial class otobus46Koltuk : Form
    {

        List<byte> tiklananButonlar = new List<byte>();
        static string baglanti = "Data Source=LAPTOP-R03\\SQLEXPRESS;Initial Catalog=otobusOtomasyonu;Integrated Security=True;TrustServerCertificate=True";

        SqlConnection baglan = new SqlConnection(baglanti);


        FormKullaniciArayuz _kullaniciArayuz;
        public otobus46Koltuk(FormKullaniciArayuz kullaniciArayuz)
        {
            InitializeComponent();

            this.AutoScaleMode = AutoScaleMode.None;
            _kullaniciArayuz = kullaniciArayuz;

        }

        private void otobus40Koltuk_Load(object sender, EventArgs e)
        {
            labelUyari.Text = "";

            panel1.BackgroundImageLayout = ImageLayout.Zoom;
            panel2.BackgroundImageLayout = ImageLayout.Zoom;
            panel3.BackgroundImageLayout = ImageLayout.Zoom;
            panel4.BackgroundImageLayout = ImageLayout.Zoom;

            labelPanel1.Text = "";
            labelPanel2.Text = "";
            labelPanel3.Text = "";
            labelPanel4.Text = "";

            panellerYenileme();
            SqlBaglan();
            buttonlariDoldurma(GlobalData.seferIDveYolcuKoltukNo);
        }


        private void Buton_Click(object sender, EventArgs e)
        {
  

            Guna2ImageButton tiklananButon = sender as Guna2ImageButton;
            FormKoltukSecmeSayfasi kSecme2 = new FormKoltukSecmeSayfasi();
            byte secilneKoltukNO = Convert.ToByte(tiklananButon.Name.Replace("guna2ImageButton", ""));
            GlobalData.gecicikoltukNo = Convert.ToInt32(tiklananButon.Name.Replace("guna2ImageButton", ""));

            if (GlobalData.biletListesi.Any(k => k.koltukNo == secilneKoltukNO && k.seferID == GlobalData.koltukSecmeSeferID))
            {
                GlobalData.biletListesi.Remove(GlobalData.biletListesi.FirstOrDefault(k => k.koltukNo == secilneKoltukNO));

                tiklananButon.Image = ımageList1.Images[0];

                panellerYenileme();
                GlobalData.secmeSayisi--;
                return;
            }
            if (GlobalData.biletListesi.Count(k => k.seferID == GlobalData.koltukSecmeSeferID) > 3)
            {
                labelUyari.Text = "En fazla 4 koltuk seçebilirsiniz.";
                timer1.Start();

                return;
            }
            if (tiklananButon != null && kSecme2.ShowDialog() == DialogResult.OK && kSecme2.koltukOnaylandiMi == true)
            {



                tiklananButon.Image = ımageList1.Images[1];

                tiklananButonlar.Add(Convert.ToByte(tiklananButon.Name.Replace("guna2ImageButton", "")));

  
                //label1.Text = kSecme2.ad;
                panellerYenileme();

                GlobalData.secmeSayisi++;

            }

        }

        private void guna2ImageButton43_Paint(object sender, PaintEventArgs e)
        {
            string koltukNumarasi = "";
            Guna2ImageButton secilenKoltuk = sender as Guna2ImageButton;
            if (secilenKoltuk != null)
            {
                string koltukAdi = secilenKoltuk.Name;
                koltukNumarasi = koltukAdi.Replace("guna2ImageButton", "");
            }

            using (Font font = new Font("Arial", 11, FontStyle.Bold))
            using (SolidBrush brush = new SolidBrush(Color.Black))
            {
                e.Graphics.DrawString(koltukNumarasi, font, brush, new PointF(9, 16));
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (labelUyari.Alpha > 0)
            {

                labelUyari.Alpha -= 2;
            }
            else
            {
                timer1.Stop();
                labelUyari.Text = "";
                labelUyari.Alpha = 250;
            }
        }
        private void panellerYenileme()
        {
            panel1.BackgroundImage = null;
            panel2.BackgroundImage = null;
            panel3.BackgroundImage = null;
            panel4.BackgroundImage = null;

            labelPanel1.Text = "";
            labelPanel2.Text = "";
            labelPanel3.Text = "";
            labelPanel4.Text = "";

            System.Windows.Forms.Panel[] panelListesi = { panel1, panel2, panel3, panel4 };
            System.Windows.Forms.Label[] labels = { labelPanel1, labelPanel2, labelPanel3, labelPanel4 };
            byte sayac = 0;

    
            var secilenKoltuklar = GlobalData.biletListesi
                                            .Where(k => k.seferID == GlobalData.koltukSecmeSeferID && k.seferAdiK == GlobalData.koltukSecmeseferAdiK)
                                            .ToList();

            if (!secilenKoltuklar.Any())
                return; 

            foreach (var item in secilenKoltuklar)
            {
                if (sayac >= panelListesi.Length)
                    break; 

                System.Windows.Forms.Panel panel = panelListesi[sayac];
                System.Windows.Forms.Label label = labels[sayac];

                
                if (item.Cinsiyet == "Erkek")
                {
                    if (ımageList1.Images.Count > 2)
                        panel.BackgroundImage = ımageList1.Images[2];
                }
                else
                {
                    if (ımageList1.Images.Count > 3)
                        panel.BackgroundImage = ımageList1.Images[3];
                }

                
                Guna2ImageButton btn = this.Controls.Find("guna2ImageButton" + item.koltukNo.ToString(), true).FirstOrDefault() as Guna2ImageButton;
                if (btn != null)
                {
                    if (ımageList1.Images.Count > 1)
                        btn.Image = ımageList1.Images[1];
                }

             
                label.Location = new Point(9, 16);
                label.Text = item.koltukNo.ToString();
                panel.Invalidate();
                sayac++;
            }
        }

        private void SqlBaglan()
        {
            baglan.Open();
            string sorgu = @"mssqlde otobüs otomasyonu için
 SELECT b.koltukNo, y.cinsiyet
FROM Biletler b
JOIN Yolcular y ON b.yolcuID = y.yolcuID
JOIN SeferTerminalleri ST1 ON b.binisTerminalID = ST1.terminalID AND b.seferID = ST1.seferID
JOIN SeferTerminalleri ST2 ON b.varisTerminalID = ST2.terminalID AND b.seferID = ST2.seferID
JOIN SeferTerminalleri yeniST1 ON b.seferID = yeniST1.seferID
JOIN Terminaller t ON yeniST1.terminalID = t.terminalID
WHERE b.seferID = @seferID
  AND t.terminalAdi = @yeniTerminalIsmi
  AND b.iptalEdildiMi = 0
  AND yeniST1.siralama < ST2.siralama
diye bir  dopru mu ";



            SqlCommand command = new SqlCommand(sorgu, baglan);

            command.Parameters.AddWithValue("@seferID", GlobalData.koltukSecmeSeferID);
            command.Parameters.AddWithValue("@yeniTerminalIsmi", GlobalData.koltukSecmeseferAdiK);

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            GlobalData.seferIDveYolcuKoltukNo = table;

        }
        private void buttonlariDoldurma(DataTable dt)
        {

            foreach (DataRow row in dt.Rows)
            {
                foreach (System.Windows.Forms.Control ctrl in this.Controls)
                {
                    if (ctrl is Guna2ImageButton)
                    {
                        Guna2ImageButton btn = (Guna2ImageButton)ctrl;
                        if (btn.Name != null && btn.Name.Replace("guna2ImageButton", "") == row["koltukNo"].ToString())
                        {
                            btn.Click -= Buton_Click;
                            btn.PressedState.ImageSize = btn.ImageSize;
                            btn.HoverState.ImageSize = btn.ImageSize;
                            if (row["cinsiyet"].ToString() == "E" || row["cinsiyet"].ToString() == "e")
                            {
                                btn.Image = ımageList1.Images[2];
                            }
                            else
                            {
                                btn.Image = ımageList1.Images[3];
                            }

                            break;
                        }
                    }
                }
            }

        }

        private void guna2ButtonSatinAl_Click(object sender, EventArgs e)
        {
            if (GlobalData.biletListesi.Count > 0)
            {
                if (GlobalData.gidisDonusMu)
                {
                    GlobalData.gidisDonusMu = false;
                    GlobalData.gidisDonusMu = false;
                    string sorgu = @" SELECT 
            s.seferID, 
            t1.terminalAdi AS kalkisTerminalAdi, 
            t2.terminalAdi AS varisTerminalAdi, 
            s.kalkisSaati, 
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
          AND CAST(s.tarih AS DATE) = @seferTarihi 
          AND s.aktifMi = 1;";


                    SqlCommand command = new SqlCommand(sorgu, baglan);

                    command.Parameters.AddWithValue("@kalkisSehirID", GlobalData.varisSehir);
                    command.Parameters.AddWithValue("@varisSehirID", GlobalData.kalkisSehir);
                    command.Parameters.AddWithValue("@seferTarihi", GlobalData.donusTarihi);


                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    GlobalData.seferlerListesi = new DataTable();
                    adapter.Fill(GlobalData.seferlerListesi);

                    GlobalData.seferlerListesi.Columns.Add("varisSaati", typeof(TimeSpan));

                    foreach (DataRow row in GlobalData.seferlerListesi.Rows)
                    {
                        TimeSpan kalkisSaati = (TimeSpan)row["kalkisSaati"];
                        TimeSpan varisSaati = kalkisSaati.Add(GlobalData.TvarisSaati);
                        row["varisSaati"] = varisSaati.ToString();
                    }

                    _kullaniciArayuz.SayfaDegistir(new FormSeferListeleme(_kullaniciArayuz));
                }
                else
                {
                    _kullaniciArayuz.SayfaDegistir(new FormSatinAlmaIslemi(_kullaniciArayuz));
                }

                this.Close();
            }
            else
            {
                labelUyari.Text = "Lütfen en az bir koltuk seçiniz.";
                timer1.Start();
            }
        }

        private void guna2ButtonIptal_Click(object sender, EventArgs e)
        {
            _kullaniciArayuz.SayfaDegistir(new FormSeferListeleme(_kullaniciArayuz));
        }
    }
    
}
