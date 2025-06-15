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
using System.Windows.Forms.DataVisualization.Charting;

namespace OtobusOtomasyonSistemi.AdminArayuz._7Raporlamaİstatistikler
{
    public partial class FormIsatistikler : Form
    {
        static string baglanti = "Data Source=LAPTOP-R03\\SQLEXPRESS;Initial Catalog=otobusOtomasyonu;Integrated Security=True;TrustServerCertificate=True";
        SqlConnection baglan = new SqlConnection(baglanti);

        public FormIsatistikler()
        {
            InitializeComponent();
        }

        private void FormIsatistikler_Load(object sender, EventArgs e)
        {
            


            guna2ComboBox1.DropDownHeight = 200;
            guna2ComboBox1.MaxDropDownItems = 10;
            sqllisteleme();
            sqlkarYukleme();
            LoadEnhancedChart();
        }

        private void sqllisteleme()
        {
            string sorguSatilanBiletler = "SELECT count(*)  FROM Biletler where YEAR(islemZamani) =YEAR(GETDATE())  and month(islemZamani) = @ay";

            string sorguKayitOlanlar = "SELECT count(*) FROM Yolcular WHERE kayitOlduMu =  1";

            string sorguKayitOlmayanlar = "SELECT count(*) FROM Yolcular WHERE kayitOlduMu =  0";

            string sorguIptalEdilenBiletler = "SELECT count(*) FROM Biletler where YEAR(islemZamani) =YEAR(GETDATE())  and iptalEdildiMi = 1 and month(islemZamani) = @ay";

            string sorguDolulukOrani = @"SELECT 
    sum(distinct(s.kacKoltukKaldi)) AS ToplamKoltukKapasitesi,
    COUNT(b.BiletID) AS SatilanBiletSayisi,
    CAST(COUNT(b.BiletID) AS FLOAT) * 100 / NULLIF(SUM(distinct(s.kacKoltukKaldi)), 0) AS DolulukOraniYuzde
FROM 
     Biletler b 
LEFT JOIN 
  Seferler s ON s.SeferID = b.SeferID
WHERE 
    YEAR(s.tarih) = YEAR(GETDATE()) and  MONTH(s.tarih) = @ay
";


            try
            {
                baglan.Open();

                using (SqlCommand command = new SqlCommand(sorguSatilanBiletler, baglan))
                {
                    command.Parameters.AddWithValue("@ay", guna2ComboBox1.SelectedIndex + 1);
                    int satilanBiletSayisi = Convert.ToInt32(command.ExecuteScalar());
                    s1 = satilanBiletSayisi;
                }

                using (SqlCommand command = new SqlCommand(sorguIptalEdilenBiletler, baglan))
                {
                    command.Parameters.AddWithValue("@ay", guna2ComboBox1.SelectedIndex + 1);
                    int iptalEdilenBiletler = Convert.ToInt32(command.ExecuteScalar());
                    s2 = iptalEdilenBiletler;
                }

                using (SqlCommand command = new SqlCommand(sorguKayitOlanlar, baglan))
                {
                    int kayitOlanlar = Convert.ToInt32(command.ExecuteScalar());
                    s3 = kayitOlanlar;
                }

                using (SqlCommand command = new SqlCommand(sorguKayitOlmayanlar, baglan))
                {
                    int kayitOlmayanlar = Convert.ToInt32(command.ExecuteScalar());
                    s4 = kayitOlmayanlar;
                }

                using (SqlCommand command = new SqlCommand(sorguDolulukOrani, baglan))
                {
                    command.Parameters.AddWithValue("@ay", guna2ComboBox1.SelectedIndex + 1);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            double dolulukOrani = reader.IsDBNull(2) ? 0 : reader.GetDouble(2);
                            label1.Text = $"%{dolulukOrani.ToString("N2")}";
                            oran = (int)dolulukOrani;
                        }
                    }
                }
            }


            catch (Exception ex)
            {
                MessageBox.Show("Hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                
            }
            finally
            {

                guna2CircleProgressBar1.Value = 0;
                label1.Text = "0";
                label9.Text = (s1-10).ToString();
                label12.Text = (s2 - 10).ToString(); ;
                label10.Text = (s3 - 10).ToString(); ;
                label11.Text = (s4 - 10).ToString(); ;


                baglan.Close();
                timer1.Start();
                
            }



        }

        private void sqlkarYukleme()
        {
            string sorgu = @"SELECT 
    MONTH(islemZamani) AS Ay,
    DATENAME(MONTH, islemZamani) AS AyAdi,
    SUM(ucret) AS ToplamFiyat
FROM 
    Biletler
WHERE 
    YEAR(islemZamani) = YEAR(GETDATE())
    AND iptalEdildiMi = 0
GROUP BY 
    MONTH(islemZamani), 
    DATENAME(MONTH, islemZamani)
ORDER BY 
    MONTH(islemZamani)";

            try
            {
                baglan.Open();

                SqlDataAdapter adapter = new SqlDataAdapter(sorgu, baglan);
                DataTable table = new DataTable();
                adapter.Fill(table);

                chart1.Series.Clear();
                chart1.Titles.Clear();

               
                chart1.Titles.Add("Aylık Bilet Gelirleri");
                chart1.Titles[0].Font = new Font("Arial", 12, FontStyle.Bold);

                
                Series series = new Series("Toplam Gelir");
                series.ChartType = SeriesChartType.Column;
                series.XValueMember = "AyAdi";
                series.YValueMembers = "ToplamFiyat";
                series.IsValueShownAsLabel = true;
                series.LabelFormat = "C2"; 
                series.Font = new Font("Arial", 8, FontStyle.Bold);
                series.Color = Color.SteelBlue;

                
                chart1.Series.Add(series);

           
                chart1.DataSource = table;
                chart1.DataBind();

                chart1.ChartAreas[0].AxisX.Title = "Aylar";
                chart1.ChartAreas[0].AxisY.Title = "Toplam Gelir (₺)";
                chart1.ChartAreas[0].AxisY.LabelStyle.Format = "C0";
                chart1.ChartAreas[0].AxisX.Interval = 1;
            
            }
            catch (Exception ex)
            {

                MessageBox.Show("Hata oluştu: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
            finally
            {
                baglan.Close();
            }
        }


        private void LoadEnhancedChart()
        {
         
            chart1.BackColor = Color.Transparent;
            chart1.ForeColor = Color.White;

      
            ChartArea chartArea = chart1.ChartAreas[0];
            chartArea.BackColor = Color.Transparent;
            chartArea.AxisX.IsMarginVisible = false;
            chartArea.AxisY.IsMarginVisible = false;

            
            chartArea.AxisX.LineColor = Color.White;
            chartArea.AxisX.MajorGrid.Enabled = false;
            chartArea.AxisX.LabelStyle.ForeColor = Color.White;
            chartArea.AxisX.TitleForeColor = Color.White;
            chartArea.AxisX.MajorTickMark.LineColor = Color.White;

            chartArea.AxisY.LineColor = Color.White;
            chartArea.AxisY.MajorGrid.Enabled = false;
            chartArea.AxisY.LabelStyle.ForeColor = Color.White;
            chartArea.AxisY.TitleForeColor = Color.White;
            chartArea.AxisY.MajorTickMark.LineColor = Color.White;

       
            chart1.Titles[0].ForeColor = Color.White;
            chart1.Titles[0].Font = new Font("Arial", 12, FontStyle.Bold);

           
            Series series = chart1.Series[0];
            series.ChartType = SeriesChartType.Spline;
            series.Color = Color.FromArgb(180, 120, 255); 
            series.BorderWidth = 3; 
            series.ShadowColor = Color.Transparent; 
            series.MarkerStyle = MarkerStyle.Circle;
            series.MarkerSize = 8;
            series.MarkerColor = Color.White;

            series.IsValueShownAsLabel = true;
            series.LabelForeColor = Color.White;
            series.LabelBackColor = Color.Transparent;
            series.LabelFormat = "C0";

           
            chart1.Legends[0].ForeColor = Color.White;
            chart1.Legends[0].BackColor = Color.Transparent;
            chart1.Legends[0].Font = new Font("Arial", 9, FontStyle.Bold);

            
            series.ToolTip = "Ay: #VALX\nToplam: #VALY{C2}";

            
            chart1.BorderlineColor = Color.Transparent;
            chartArea.BorderColor = Color.Transparent;

            chartArea.Area3DStyle.Enable3D = false; 
            chartArea.ShadowColor = Color.Transparent;

           
            chart1.Parent.BackColor = Color.Transparent;
            chart1.Invalidate(); 

        }

        private void guna2ComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            sqllisteleme();
        }


        int oran, s1,s2,s3,s4;
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (guna2CircleProgressBar1.Value+1 <= oran)
            {
                guna2CircleProgressBar1.Value++;
                label1.Text ="%" + guna2CircleProgressBar1.Value.ToString();
            }
            if (Convert.ToInt32(label9.Text)+1 <= s1)
            {
                label9.Text = (Convert.ToInt32(label9.Text) + 1).ToString();
            }
            if (Convert.ToInt32(label12.Text)+1 <= s2)
            {
                label12.Text = (Convert.ToInt32(label12.Text) + 1).ToString();
            }
            if (Convert.ToInt32(label10.Text)+1 <= s3)
            {
                label10.Text = (Convert.ToInt32(label10.Text) + 1).ToString();
            }
            if (Convert.ToInt32(label11.Text)+1 <= s4)
            {
                label11.Text = (Convert.ToInt32(label11.Text) + 1).ToString();
            }

            if (guna2CircleProgressBar1.Value == oran && Convert.ToInt32(label9.Text) == s1 &&
                Convert.ToInt32(label12.Text) == s2 && Convert.ToInt32(label10.Text) == s3 && Convert.ToInt32(label11.Text) == s4)
            {
                timer1.Stop();
            }
        }
    }
}
