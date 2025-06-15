using Guna.UI2.WinForms;
using OtobusOtomasyonSistemi.AdminArayuz._1YetkilendimeSistemi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using OtobusOtomasyonSistemi.AdminArayuz._5SeferYonetimi;
using OtobusOtomasyonSistemi.AdminArayuz._7Raporlamaİstatistikler;
using OtobusOtomasyonSistemi.AdminArayuz._3KullaniciYönetimi;


namespace OtobusOtomasyonSistemi.AdminArayuz
{
    public partial class FormAdminArayuz1 : Form
    {

        public FormAdminArayuz1()
        {
            InitializeComponent();
        }

        bool menuAcildiMi = true;

        static string baglanti = "Data Source=LAPTOP-R03\\SQLEXPRESS;Initial Catalog=otobusOtomasyonu;Integrated Security=True;TrustServerCertificate=True";
        SqlConnection baglan = new SqlConnection(baglanti);



        private void FormAdminArayuz1_Load(object sender, EventArgs e)
        {
            baglan.Open();
            //GlobalData.adminID = 1;
            sqlYukleme();
            labelKullaniciAdi.Text = GlobalData.adminAdi + " " + GlobalData.adminSoyadi;
            labelKullaniciYetkisi.Text = GlobalData.kullaniciYetki;


            TuslariListeleme tuslar = new TuslariListeleme();
            tuslar.yetkilendirmeSistemi();


            hangiButonSecili();
            guna2Button1.FillColor = Color.FromArgb(163, 181, 201);
            guna2Button1.HoverState.FillColor = Color.FromArgb(163, 181, 201);
        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }



        private void pictureBoxMenu_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            TuslariListeleme tuslar = new TuslariListeleme();
            tuslar.yetkilendirmeSistemi();
            

            hangiButonSecili();
            guna2Button1.FillColor = Color.FromArgb(163, 181, 201);
            guna2Button1.HoverState.FillColor = Color.FromArgb(163, 181, 201);
          
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            TuslariListeleme tuslar = new TuslariListeleme();
            tuslar.gostergeVeIstatistik();
            hangiButonSecili();
            guna2Button2.FillColor = Color.FromArgb(163, 181, 201);
            guna2Button2.HoverState.FillColor = Color.FromArgb(163, 181, 201);
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            TuslariListeleme tuslar = new TuslariListeleme();
            tuslar.kullaniciYonetimi();
            hangiButonSecili();
            guna2Button3.FillColor = Color.FromArgb(163, 181, 201);
            guna2Button3.HoverState.FillColor = Color.FromArgb(163, 181, 201);
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            TuslariListeleme tuslar = new TuslariListeleme();
            tuslar.otobusYonetimi();

            hangiButonSecili();
            guna2Button4.FillColor = Color.FromArgb(163, 181, 201);
            guna2Button4.HoverState.FillColor = Color.FromArgb(163, 181, 201);
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            TuslariListeleme tuslar = new TuslariListeleme();
            tuslar.SeferEkleme();   
            hangiButonSecili();
            guna2Button5.FillColor = Color.FromArgb(163, 181, 201);
            guna2Button5.HoverState.FillColor = Color.FromArgb(163, 181, 201);
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            hangiButonSecili();
            guna2Button6.FillColor = Color.FromArgb(163, 181, 201);
            guna2Button6.HoverState.FillColor = Color.FromArgb(163, 181, 201);
        }

        private void guna2Button7_Click(object sender, EventArgs e)
        {
            TuslariListeleme tuslar = new TuslariListeleme();
            tuslar.gostergeVeIstatistik();
            hangiButonSecili();
            guna2Button7.FillColor = Color.FromArgb(163, 181, 201);
            guna2Button7.HoverState.FillColor = Color.FromArgb(163, 181, 201);
        }

        private void guna2Button8_Click(object sender, EventArgs e)
        {
            hangiButonSecili();
            guna2Button8.FillColor = Color.FromArgb(163, 181, 201);
            guna2Button8.HoverState.FillColor = Color.FromArgb(163, 181, 201);
        }

        private void guna2Button9_Click(object sender, EventArgs e)
        {
            hangiButonSecili();
            guna2Button9.FillColor = Color.FromArgb(163, 181, 201);
            guna2Button9.HoverState.FillColor = Color.FromArgb(163, 181, 201);

        }
        private void hangiButonSecili()
        {
            foreach (System.Windows.Forms.Control ctrl in panel1.Controls)
            {
                if (ctrl is Guna2Button button)
                {
                    button.FillColor = Color.FromArgb(19, 30, 43);
                    button.HoverState.FillColor = default;
                }
            }
        }
        public void SayfaDegistir(Form frm)
        {

            if (panel3.Controls.Count > 0)
            {
                panel3.Controls.Clear();
            }
            panel3.AutoScroll = true;
            Form fm = frm as Form;
            fm.TopLevel = false;
            fm.FormBorderStyle = FormBorderStyle.None;
            fm.Dock = DockStyle.Top;

            panel3.Controls.Add(fm);
            panel3.Tag = fm;
            fm.Show();
        }
        private void sqlYukleme()
        {



            string sorgu = "select ad, soyad, y.yetkiAdi as yetki, c.yetkiID as yetkiID from Calisanlar c join Yetkiler y on c.yetkiID=y.yetkiID where c.calisanID =" + GlobalData.adminID + "; ";
            SqlCommand command = new SqlCommand(sorgu, baglan);

            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            GlobalData.adminAdi = table.Rows[0]["ad"].ToString();
            GlobalData.adminSoyadi = table.Rows[0]["soyad"].ToString();
            GlobalData.kullaniciYetki = table.Rows[0]["yetki"].ToString();

            ButonlariYetkiyeGoreSirala(table.Rows[0]["yetkiID"].ToString());

        }

        private void ButonlariYetkiyeGoreSirala(string kullaniciYetkisi)
        {
            List<Guna2Button> butonlar = new List<Guna2Button>();

            string sorgu = "select k.kapsamAdi as kapsamlar from Kapsamlar k join YetkilerinKapsamlari yk on k.ID = yk.kapsamID join Yetkiler y on y.yetkiID = yk.yetkiID join Calisanlar c on y.yetkiID=c.yetkiID where c.calisanID =" + GlobalData.adminID + ";";
            SqlCommand command = new SqlCommand(sorgu, baglan);
            SqlDataAdapter adapter = new SqlDataAdapter(command);
            DataTable table = new DataTable();
            adapter.Fill(table);
            List<string> yetkiler = new List<string>();
            foreach (DataRow row in table.Rows)
            {
                string yetki = row["kapsamlar"].ToString();
                yetkiler.Add(yetki);
            }

            foreach (Control ctrl in panel1.Controls)
            {
                if (ctrl is Guna2Button btn)
                {
                    ctrl.Visible = false;

                    if (yetkiler.Contains(btn.Text))
                    {
                        butonlar.Add(btn);
                    }

                }
            }
            butonlar = butonlar.OrderBy(b => b.Name).ToList();

            int y = 98;
            foreach (var btn in butonlar)
            {
                btn.ImageAlign = HorizontalAlignment.Left;
                btn.TextOffset = new Point(0, 0);
                btn.Visible = true;
                btn.Location = new Point(-5, y);
                y += btn.Height + 6;
            }

        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (menuAcildiMi)
            {
                panel1.Width += 10;
                if (panel1.Width == panel1.MaximumSize.Width)
                {
                    timer1.Stop();
                    menuAcildiMi = !menuAcildiMi;
                    pictureBoxMenu.Image = ımageListMenu.Images[1];
                }
            }
            else
            {
                panel1.Width -= 10;
                if (panel1.Width == panel1.MinimumSize.Width)
                {
                    timer1.Stop();
                    menuAcildiMi = !menuAcildiMi;
                    pictureBoxMenu.Image = ımageListMenu.Images[0];

                }
            }
        }
        private void guna2Button10_Click(object sender, EventArgs e)
        {
            

        }


   
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HTCAPTION = 0x2;
        private void DragForm()
        {
            ReleaseCapture();
            SendMessage(this.Handle, WM_NCLBUTTONDOWN, HTCAPTION, 0);
        }

        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            DragForm();
        }
        private void panel4_MouseDown(object sender, MouseEventArgs e)
        {
            DragForm();
        }


        // form boyutlandırma işlemleri
        bool resizing = false;
        string resizeDirection = "";
        Point lastMousePos;

        private void panelSag_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                resizing = true;
                resizeDirection = "right";
                lastMousePos = PointToScreen(e.Location);
                Cursor.Current = Cursors.SizeWE;
            }
        }

        private void panelSag_MouseMove(object sender, MouseEventArgs e)
        {
            if (!resizing)
            {
                panelSag.Cursor = Cursors.SizeWE;
                return;
            }

            if (resizeDirection == "right")
            {
                Point currentPos = PointToScreen(e.Location);
                int diff = currentPos.X - lastMousePos.X;
                if (diff != 0)
                {
                    this.Width += diff;
                    lastMousePos = currentPos;
                    this.Update();
                }
            }
        }

        private void panelSag_MouseUp(object sender, MouseEventArgs e)
        {
            resizing = false;
            resizeDirection = "";
            Cursor.Current = Cursors.Default;
        }

        private void panelAlt_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                resizing = true;
                resizeDirection = "bottom";
                lastMousePos = PointToScreen(e.Location);
                Cursor.Current = Cursors.SizeNS;
            }
        }

        private void panelAlt_MouseMove(object sender, MouseEventArgs e)
        {
            if (!resizing)
            {
                panelAlt.Cursor = Cursors.SizeNS;
                return;
            }

            if (resizeDirection == "bottom")
            {
                Point currentPos = PointToScreen(e.Location);
                int diff = currentPos.Y - lastMousePos.Y;
                if (diff != 0)
                {
                    this.Height += diff;
                    lastMousePos = currentPos;
                    this.Update();
                }
            }
        }

        private void panelAlt_MouseUp(object sender, MouseEventArgs e)
        {
            resizing = false;
            resizeDirection = "";
            Cursor.Current = Cursors.Default;
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {
            FormYukleme formYukleme = Application.OpenForms["FormYukleme"] as FormYukleme;
            formYukleme?.Show();
            formYukleme.timer1.Start();
        }

        private void guna2ImageButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
