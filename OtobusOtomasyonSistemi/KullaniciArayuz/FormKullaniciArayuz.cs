using OtobusOtomasyonSistemi.AdminArayuz;
using OtobusOtomasyonSistemi.KullaniciArayuz._1seferler;
using OtobusOtomasyonSistemi.KullaniciArayuz._2OtobusumNerede;
using OtobusOtomasyonSistemi.KullaniciArayuz._4Giris;
using OtobusOtomasyonSistemi.KullaniciArayuz._4Profilim;
using OtobusOtomasyonSistemi.KullaniciArayuz._4Profilim.ProfilimDetay;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OtobusOtomasyonSistemi
{
    public partial class FormKullaniciArayuz: Form
    {

        bool move;
        int mouse_x, mouse_y;

        public FormKullaniciArayuz()
        {
            InitializeComponent();
            this.AutoScaleMode = AutoScaleMode.None;
        }

        private void FormKullaniciArayuz_Load(object sender, EventArgs e)
        {
            //GlobalData.kullaniciID = 36;GlobalData.kullaniciEposta = "resulcwn@gmail.com"; GlobalData.kullaniciAd = "Resul Can"; GlobalData.kullaniciSoyad = "Sabancı"; GlobalData.kullaniciTC = "12345678901"; GlobalData.kullaniciTuru = "admin";
            SayfaDegistir(new kSeferAramaFormu(this));
            
            
            
           
            kullaniciGirisYaptiysa();
        }


        private void guna2Button3_Click(object sender, EventArgs e)
        {
            SayfaDegistir(new kSeferAramaFormu(this));
        }

        private void guna2ButtonYardim_Click(object sender, EventArgs e)
        {
            //SayfaDegistir(new kYardimForm());
            SayfaDegistir(new FormYardimS());
        }


        private void guna2ButtonG_Click(object sender, EventArgs e)
        {
            
            kGirisForm girisf = new kGirisForm();
            girisf.ShowDialog();
          
        }

        public void SayfaDegistir(Form frm)
        {
            if (guna2Panel3.Controls.Count > 0)
            {
               guna2Panel3.Controls.Clear();
            }
            guna2Panel3.AutoScroll = true;
            Form fm = frm as Form;
            fm.TopLevel = false;
            fm.FormBorderStyle = FormBorderStyle.None;
            fm.Dock = DockStyle.Top;

            guna2Panel3.Controls.Add(fm);
            guna2Panel3.Tag = fm;
            fm.Show();
        }

        private void guna2Panel2_MouseDown(object sender, MouseEventArgs e)
        {
            move = true;
            mouse_x = e.X;
            mouse_y = e.Y;
            this.Opacity = 0.7;
        }

        private void guna2Panel2_MouseMove(object sender, MouseEventArgs e)
        {
            if (move)
            {
                this.SetDesktopLocation(MousePosition.X - mouse_x, MousePosition.Y - mouse_y);
            }
        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void guna2ButtonOtobusNerede_Click(object sender, EventArgs e)
        {
            // guna2Panel2.Enabled = false;
             SayfaDegistir(new FormBiletSorgu(this));
            //FormAdminArayuz1 faa1 = new FormAdminArayuz1();
            //FormProfilim faa1 = new FormProfilim();
            //FormKullaniciBiletleri faa1 = new FormKullaniciBiletleri();
            //faa1.Show();
        }


        private void guna2Panel2_MouseUp(object sender, MouseEventArgs e)
        {
            move = false;
            this.Opacity = 1;
        }

        private void guna2ButtonProfil_Click(object sender, EventArgs e)
        {
            SayfaDegistir(new FormProfilim(this));
        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void kullaniciGirisYaptiysa()
        {
            if (GlobalData.kullaniciID != null || GlobalData.kullaniciID >0 )
            {
                guna2ButtonProfil.Visible = true;
                guna2ButtonProfil.Location = guna2ButtonGiris.Location;
                guna2ButtonGiris.Visible = false;
            }
            else
            {
                guna2ButtonProfil.Visible = false;
                guna2ButtonGiris.Visible = true;
            }
        }
    }
}
