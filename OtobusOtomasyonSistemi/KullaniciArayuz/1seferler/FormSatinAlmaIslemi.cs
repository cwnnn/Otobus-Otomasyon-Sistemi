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

namespace OtobusOtomasyonSistemi.KullaniciArayuz._1seferler
{
    public partial class FormSatinAlmaIslemi : Form
    {
        FormKullaniciArayuz _kullaniciArayuz;
        public FormSatinAlmaIslemi(FormKullaniciArayuz kullaniciArayuz)
        {
            InitializeComponent();
            _kullaniciArayuz = kullaniciArayuz;
        }
        double fiyat = 0;
        private void FormSatinAlmaIslemi_Load(object sender, EventArgs e)
        {
            SayfaDegistir(new FormSatinAlinacakBiletleriListele(this));
            labelFiyat.Text = "Fiyat: " +GlobalData.geciciFiyatToplam + " TL";
        }
        int koltukNo = 17;
        private void guna2ButtonOnayla_Click(object sender, EventArgs e)
        {
            FormSatinAlmaOnaylama satinAlmaOnaylama = new FormSatinAlmaOnaylama(_kullaniciArayuz);
            satinAlmaOnaylama.ShowDialog();
        }
        public void SayfaDegistir(Form frm)
        {
            if (panel3.Controls.Count > 0)
            {
                panel3.Controls.Clear();    
            }
            panel3.AutoScroll = false;
            Form fm = frm as Form;
            fm.TopLevel = false;
            fm.FormBorderStyle = FormBorderStyle.None;
            fm.Dock = DockStyle.None;
            fm.Height = fm.PreferredSize.Height;
            panel3.Controls.Add(fm);
            panel3.Tag = fm;
            fm.Show();


            //bilet listeleme işleminde 8 bilet den sonra paneller formu aşıyor. muhtemelen auto scrolldan kaynaklı
        }

        private void guna2ButtonIptal_Click(object sender, EventArgs e)
        {
            _kullaniciArayuz.SayfaDegistir(new FormSeferListeleme(_kullaniciArayuz));
        }
    }
}

