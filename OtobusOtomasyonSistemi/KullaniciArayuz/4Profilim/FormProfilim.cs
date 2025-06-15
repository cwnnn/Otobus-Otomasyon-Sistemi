using Guna.UI2.WinForms;
using OtobusOtomasyonSistemi.KullaniciArayuz._1seferler;
using OtobusOtomasyonSistemi.KullaniciArayuz._4Giris;
using OtobusOtomasyonSistemi.KullaniciArayuz._4Profilim.ProfilimDetay;
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

namespace OtobusOtomasyonSistemi.KullaniciArayuz._4Profilim
{
    public partial class FormProfilim : Form
    {

        FormKullaniciArayuz _formKullaniciArayuz;
        public FormProfilim(FormKullaniciArayuz formKullaniciArayuz)
        {
            InitializeComponent();
            _formKullaniciArayuz = formKullaniciArayuz;
        }

        private void FormProfilim_Load(object sender, EventArgs e)
        {
            SayfaDegistir2(new FormKullaniciBilgierli(this));
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            SayfaDegistir2(new FormKullaniciBilgierli(this));
        }
        public void SayfaDegistir2(Form frm)
        {
            if (panel2.Controls.Count > 0)
            {
                panel2.Controls.Clear();
            }
            panel2.AutoScroll = true;
            Form fm = frm as Form;
            fm.TopLevel = false;
            fm.FormBorderStyle = FormBorderStyle.None;
            fm.Dock = DockStyle.Top;
            panel2.Controls.Add(fm);
            panel2.Tag = fm;
            fm.Show();
        }


        public void SayfaDegistir(Form frm)
        {
            if (panel2.Controls.Count > 0)
            {
                panel2.Controls.Clear();
            }
            panel2.AutoScroll = false;
            Form fm = frm as Form;
            fm.TopLevel = false;
            fm.FormBorderStyle = FormBorderStyle.None;
            fm.Dock = DockStyle.Top;
            fm.Height = fm.PreferredSize.Height;
            panel2.Controls.Add(fm);
            panel2.Tag = fm;
            fm.Show();
        }

        private void guna2Button3_DoubleClick(object sender, EventArgs e)
        {
            DialogResult sonuc = MessageBox.Show("Çıkış Yapmak istediğinize emin misiniz?", "Çıkış", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (sonuc == DialogResult.Yes)
            {
                GlobalData.kullaniciID = null;
                foreach (Form form in Application.OpenForms.Cast<Form>().ToList())
                {
                    if (!(form is FormYukleme)) 
                    {
                        form.Close();
                    }
                }
                FormYukleme formYukleme = Application.OpenForms["FormYukleme"] as FormYukleme;
                formYukleme?.Show();
                formYukleme.timer1.Start();

            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            SayfaDegistir(new FormKullaniciBiletleri(this));
        }

        private void guna2Button4_Click(object sender, EventArgs e)
        {
            _formKullaniciArayuz.SayfaDegistir(new FormSatinAlmaIslemi(_formKullaniciArayuz));
        
    }
    }
}
