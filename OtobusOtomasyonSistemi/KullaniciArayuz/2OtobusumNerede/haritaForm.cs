using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
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
    public partial class haritaForm : Form
    {
        static string baglanti = "Data Source=LAPTOP-R03\\SQLEXPRESS;Initial Catalog=otobusOtomasyonu;Integrated Security=True;TrustServerCertificate=True";
        GMapOverlay markersOverlay; 

        public haritaForm()
        {
            InitializeComponent();
            markersOverlay = new GMapOverlay("markers"); 
        }

        private void haritaForm_Load(object sender, EventArgs e)
        {
            gMapControl1.MapProvider = GMapProviders.GoogleMap;
            double lat = 39.9208;
            double lng = 32.8541;

            gMapControl1.Position = new PointLatLng(lat, lng);
            gMapControl1.MinZoom = 0;
            gMapControl1.MaxZoom = 24;
            gMapControl1.Zoom = 6;
            gMapControl1.ShowCenter = false;
            gMapControl1.DragButton = MouseButtons.Left;

            gMapControl1.MouseClick += new MouseEventHandler(GMapControl1_MouseClick);


            gMapControl1.Overlays.Add(markersOverlay);



            
            timer1.Start();
            
            seferinTerminalleri();

        }

        private void GMapControl1_MouseClick(object sender, MouseEventArgs e)
        {
            PointLatLng clickedPoint = gMapControl1.FromLocalToLatLng(e.X, e.Y);

            guna2TextBox1.Text = clickedPoint.Lat.ToString();
            guna2TextBox2.Text = clickedPoint.Lng.ToString();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            decimal enlemm = Convert.ToDecimal(guna2TextBox1.Text);
            decimal boylammm = Convert.ToDecimal(guna2TextBox2.Text);

            string sorgu = @" UPDATE c SET c.enlem = @enlem, c.boylam = @boylam
 from CanliKonumlar c join Otobusler o on o.otobusID = c.otobusID 
 where o.plaka = @plaka;";
            SqlConnection baglan = new SqlConnection(baglanti);
            SqlCommand komut = new SqlCommand(sorgu, baglan);

            komut.Parameters.AddWithValue("@enlem", enlemm);
            komut.Parameters.AddWithValue("@boylam", boylammm);
            komut.Parameters.AddWithValue("@plaka", GlobalData.otobusPlaka);

            try
            {
                baglan.Open();
                int sonuc = komut.ExecuteNonQuery(); 

                if (sonuc > 0)
                    MessageBox.Show("Konum başarıyla güncellendi.");
                else
                    MessageBox.Show("Güncellenecek kayıt bulunamadı.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
            finally
            {
                baglan.Close();
            }
        }

 
        private void seferinTerminalleri()
        {

            SqlConnection connection = new SqlConnection(baglanti);
            SqlCommand command = new SqlCommand(@"
       select t.terminalAdi,t.Enlem, t.Boylam 
from Terminaller t
join SeferTerminalleri st on t.terminalID= st.terminalID
where st.seferID =@seferID", connection);

            command.Parameters.AddWithValue("@seferID",GlobalData.koltukSecmeSeferID );

            try
            {
                connection.Open();
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable table = new DataTable();
                adapter.Fill(table);

                foreach (DataRow row in table.Rows)
                {
                    string terminalAdi = row["terminalAdi"].ToString();
                    string enlem = row["Enlem"].ToString();
                    string boylam = row["Boylam"].ToString();
                    // Marker oluştur
                    MarkerOlustur(1, enlem, boylam, terminalAdi);
                }
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

        private void MarkerOlustur(int iconIndex, string enlem, string boylam, string markerName)
        {
            Bitmap icon = (Bitmap)ımageList1.Images[iconIndex];
            icon = new Bitmap(icon, new Size(55, 55));

            PointLatLng point = new PointLatLng(Convert.ToDouble(enlem), Convert.ToDouble(boylam));

            GMarkerGoogle marker = new GMarkerGoogle(point, icon);

            marker.ToolTipText = markerName;

            markersOverlay.Markers.Add(marker);

            gMapControl1.Refresh();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            foreach (var marker in markersOverlay.Markers.ToList()) 
            {
                if (marker.ToolTipText == "Otobüs")
                {
                    markersOverlay.Markers.Remove(marker);
                    break; 
                }
            }

            SqlConnection connection = new SqlConnection(baglanti);
            SqlCommand command = new SqlCommand(@"
        SELECT enlem, boylam
        FROM CanliKonumlar c
        JOIN Otobusler o ON c.otobusID = o.otobusID
        WHERE plaka = @plaka", connection);

            command.Parameters.AddWithValue("@plaka", GlobalData.otobusPlaka);

            try
            {
                connection.Open(); 

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    string enlem = reader["enlem"].ToString();
                    string boylam = reader["boylam"].ToString();

                    
                    MarkerOlustur(0, enlem, boylam,"Otobüs");
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

            timer1.Interval = 5000; 
        }

        private void guna2CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (guna2CheckBox1.Checked)
            {
                panel1.Visible = true;
            }
            else
            {
                panel1.Visible = false;
            }
           
        }
    }
    }

