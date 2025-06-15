using Guna.UI2.WinForms;
using OtobusOtomasyonSistemi.AdminArayuz._1YetkilendimeSistemi;
using OtobusOtomasyonSistemi.AdminArayuz._3KullaniciYönetimi;
using OtobusOtomasyonSistemi.AdminArayuz._4OtobusYonetimi;
using OtobusOtomasyonSistemi.AdminArayuz._5SeferYonetimi;
using OtobusOtomasyonSistemi.AdminArayuz._7Raporlamaİstatistikler;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OtobusOtomasyonSistemi.AdminArayuz
{
    public class TuslariListeleme
    {


        FormAdminArayuz1 faa1 = (FormAdminArayuz1)Application.OpenForms["FormAdminArayuz1"];
        
        public void yetkilendirmeSistemi()
        {
            faa1.panel4.Controls.Clear();
            int locationY = 3;

            Guna2Button btn1 = new Guna2Button();
            btn1.Text = "Çalışanların Yetkileri";
            btn1.Location = new Point(locationY, 2);
            btn1.Size = faa1.guna2Button10.Size;
            btn1.Font = faa1.guna2Button10.Font;
            btn1.ForeColor = faa1.guna2Button10.ForeColor;
            btn1.FillColor = faa1.guna2Button10.FillColor;
            btn1.BackgroundImage = faa1.guna2Button10.BackgroundImage;
            btn1.Click += yetkilendirmeSistemi_Click;

            locationY += btn1.Size.Width;

            Guna2Button btn2 = new Guna2Button();
            btn2.Text = "Yetki ve Kapsam Ayarları";
            btn2.Location = new Point(locationY, 2);
            btn2.Size = faa1.guna2Button10.Size;
            btn2.Font = faa1.guna2Button10.Font;
            btn2.ForeColor = faa1.guna2Button10.ForeColor;
            btn2.FillColor = faa1.guna2Button10.FillColor;
            btn2.BackgroundImage = faa1.guna2Button10.BackgroundImage;
            btn2.Click += yetkilendirmeSistemi_Click;

            faa1.panel4.Controls.Add(btn1);
            faa1.panel4.Controls.Add(btn2);

            tusSecildiginde(btn1);
            faa1.SayfaDegistir(new FormCalisanYetkilendirme());
        }
        public void yetkilendirmeSistemi_Click(object sender, EventArgs e)
        {
            Guna2Button basilan = sender as Guna2Button;
            tusSecilmediginde();
            tusSecildiginde(basilan);

            if (basilan.Text == "Çalışanların Yetkileri")
            {
                faa1.SayfaDegistir(new FormCalisanYetkilendirme());
            }
            else if (basilan.Text == "Yetki ve Kapsam Ayarları")
            {
                faa1.SayfaDegistir(new FormYetkilendirmeSistemi());
            }

        }

        public void otobusYonetimi()
        {
            faa1.panel4.Controls.Clear();

            int locationY = 3;

            Guna2Button btn1 = new Guna2Button();
            btn1.Text = "Otobus Ekle";
            btn1.Location = new Point(locationY, 2);
            btn1.Size = faa1.guna2Button10.Size;
            btn1.Font = faa1.guna2Button10.Font;
            btn1.ForeColor = faa1.guna2Button10.ForeColor;
            btn1.FillColor = faa1.guna2Button10.FillColor;
            btn1.BackgroundImage = faa1.guna2Button10.BackgroundImage;
            btn1.Click += OtobusYonetimi_Click;

            locationY += btn1.Size.Width;

            Guna2Button btn2 = new Guna2Button();
            btn2.Text = "Marka Ekle";
            btn2.Location = new Point(locationY, 2);
            btn2.Size = faa1.guna2Button10.Size;
            btn2.Font = faa1.guna2Button10.Font;
            btn2.ForeColor = faa1.guna2Button10.ForeColor;
            btn2.FillColor = faa1.guna2Button10.FillColor;
            btn2.BackgroundImage = faa1.guna2Button10.BackgroundImage;
            btn2.Click += OtobusYonetimi_Click;

            faa1.panel4.Controls.Add(btn1);
            faa1.panel4.Controls.Add(btn2);

            tusSecildiginde(btn1);
            faa1.SayfaDegistir(new FormOtobussEkleme());
        }
        public void OtobusYonetimi_Click(object sender, EventArgs e)
        {
            Guna2Button basilan = sender as Guna2Button;
            tusSecilmediginde();
            tusSecildiginde(basilan);

            if (basilan.Text == "Otobus Ekle")
            {
                faa1.SayfaDegistir(new FormOtobussEkleme());
            }
            else if (basilan.Text == "Marka Ekle")
            {
                faa1.SayfaDegistir(new FormOtobusMarkaEkle());
            }

        }

        public void SeferEkleme()
        {
            faa1.panel4.Controls.Clear();

            int locationY = 3;

            Guna2Button btn1 = new Guna2Button();
            btn1.Text = "Sefer Ekle";
            btn1.Location = new Point(locationY, 2);
            btn1.Size = faa1.guna2Button10.Size;
            btn1.Font = faa1.guna2Button10.Font;
            btn1.ForeColor = faa1.guna2Button10.ForeColor;
            btn1.FillColor = faa1.guna2Button10.FillColor;
            btn1.BackgroundImage = faa1.guna2Button10.BackgroundImage;


            faa1.panel4.Controls.Add(btn1);

            tusSecildiginde(btn1);
            faa1.SayfaDegistir(new FormSeferEkleme());
        }
        public void gostergeVeIstatistik()
        {
            faa1.panel4.Controls.Clear();

            int locationY = 3;

            Guna2Button btn1 = new Guna2Button();
            btn1.Text = "İstatistikler";
            btn1.Location = new Point(locationY, 2);
            btn1.Size = faa1.guna2Button10.Size;
            btn1.Font = faa1.guna2Button10.Font;
            btn1.ForeColor = faa1.guna2Button10.ForeColor;
            btn1.FillColor = faa1.guna2Button10.FillColor;
            btn1.BackgroundImage = faa1.guna2Button10.BackgroundImage;




            faa1.panel4.Controls.Add(btn1);

            tusSecildiginde(btn1);
            faa1.SayfaDegistir(new FormIsatistikler());
        }
        
        public void kullaniciYonetimi()
        {
            faa1.panel4.Controls.Clear();

            int locationY = 3;

            Guna2Button btn1 = new Guna2Button();
            btn1.Text = "Yolcu Düzenle";
            btn1.Location = new Point(locationY, 2);
            btn1.Size = faa1.guna2Button10.Size;
            btn1.Font = faa1.guna2Button10.Font;
            btn1.ForeColor = faa1.guna2Button10.ForeColor;
            btn1.FillColor = faa1.guna2Button10.FillColor;
            btn1.BackgroundImage = faa1.guna2Button10.BackgroundImage;
            btn1.Click += KullaniciYonetimi_Click;

            locationY += btn1.Size.Width;

            Guna2Button btn2 = new Guna2Button();
            btn2.Text = "Çalışan Düzenle";
            btn2.Location = new Point(locationY, 2);
            btn2.Size = faa1.guna2Button10.Size;
            btn2.Font = faa1.guna2Button10.Font;
            btn2.ForeColor = faa1.guna2Button10.ForeColor;
            btn2.FillColor = faa1.guna2Button10.FillColor;
            btn2.BackgroundImage = faa1.guna2Button10.BackgroundImage;
            btn2.Click += KullaniciYonetimi_Click;

            faa1.panel4.Controls.Add(btn1);
            faa1.panel4.Controls.Add(btn2);

            tusSecildiginde(btn1);
            faa1.SayfaDegistir(new FormYolcuDuzenleme());
        }
        public void KullaniciYonetimi_Click(object sender, EventArgs e)
        {
            Guna2Button basilan = sender as Guna2Button;
            tusSecilmediginde();
            tusSecildiginde(basilan);

            if (basilan.Text == "Yolcu Düzenle")
            {
                faa1.SayfaDegistir(new FormYolcuDuzenleme());
            }
            else if (basilan.Text == "Çalışan Düzenle")
            {
                faa1.SayfaDegistir(new FormCalisanDuzenleme());
            }

        }




























        public void tusSecildiginde(Guna2Button btn)
        {
            btn.BackgroundImageLayout = ImageLayout.Zoom;
            btn.ForeColor = Color.FromArgb(245, 124, 0);
        }
        public void tusSecilmediginde()
        {
            foreach (Control ctrl in faa1.panel4.Controls)
            {
                if (ctrl is Guna2Button button)
                {
                    button.BackgroundImageLayout = ImageLayout.None;
                    button.ForeColor = Color.White;
                }
            }
           
        }




    }
}
