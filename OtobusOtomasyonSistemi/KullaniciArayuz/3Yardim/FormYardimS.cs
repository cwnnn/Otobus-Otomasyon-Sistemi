using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI;
using System.Windows.Forms;

namespace OtobusOtomasyonSistemi
{
    public partial class FormYardimS : Form
    {
        public FormYardimS()
        {
            InitializeComponent();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            hangiButonSecili();
            guna2Button1.BackColor = Color.FromArgb(46, 58, 71);
            guna2Button1.HoverState.FillColor = Color.FromArgb(46, 58, 71);
            SayfaDegistir(new FormYarBagaj());
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            hangiButonSecili();
            guna2Button2.BackColor = Color.FromArgb(46, 58, 71);
            guna2Button2.HoverState.FillColor = Color.FromArgb(46, 58, 71);
            SayfaDegistir(new FormYarBiletIslemleri());
        }
        private void guna2Button3_Click(object sender, EventArgs e)
        {
            hangiButonSecili();
            guna2Button3.BackColor = Color.FromArgb(46, 58, 71);
            guna2Button3.HoverState.FillColor = Color.FromArgb(46, 58, 71);
            SayfaDegistir(new FormYarBiletSatisN());
        }
        private void guna2Button4_Click(object sender, EventArgs e)
        {
            hangiButonSecili();
            guna2Button4.BackColor = Color.FromArgb(46, 58, 71);
            guna2Button4.HoverState.FillColor = Color.FromArgb(46, 58, 71);
            SayfaDegistir(new FormYarIletisim());
        }

        private void guna2Button5_Click(object sender, EventArgs e)
        {
            hangiButonSecili();
            guna2Button5.BackColor = Color.FromArgb(46, 58, 71);
            guna2Button5.HoverState.FillColor = Color.FromArgb(46, 58, 71);
            SayfaDegistir(new FormYarKoltukKatag());
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            hangiButonSecili();
            guna2Button6.BackColor = Color.FromArgb(46, 58, 71);
            guna2Button6.HoverState.FillColor = Color.FromArgb(46, 58, 71);
            SayfaDegistir(new FormYarUrunVeHizmetler());
        }

        private void guna2Button7_Click(object sender, EventArgs e)
        {
            hangiButonSecili();
            guna2Button7.BackColor = Color.FromArgb(163, 181, 201);
            guna2Button7.HoverState.FillColor = Color.FromArgb(46, 58, 71);
            SayfaDegistir(new FormYarBizeUlasin());
        }

        private void hangiButonSecili()
        {
            foreach (System.Windows.Forms.Control ctrl in panel1.Controls)
            {
                if (ctrl is Guna2Button button)
                {
                    button.BackColor = Color.FromArgb(21, 31, 46);
                    button.HoverState.FillColor = default;
                }
            }
        }

        private void SayfaDegistir(Form frm)
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
            fm.Height = fm.PreferredSize.Height;
            panel2.Controls.Add(fm);
            panel2.Tag = fm;
            fm.Show();
        }

        
        
    }
}
