using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OtobusOtomasyonSistemi
{

    public partial class FormYukleme: Form
    {
        public FormYukleme()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Convert.ToInt32(labelYuklenme.Text)<100)
            {
                labelYuklenme.Text = Convert.ToString(Convert.ToInt32(labelYuklenme.Text) + 1);
            }
            else
            {
                pictureBox1.Enabled = false;
                Thread.Sleep(200);
                timer1.Stop();
                labelYuklenme.Text = "0";
                this.Hide();
                FormKullaniciArayuz frm = new FormKullaniciArayuz(); 
                frm.Show();
            }
        }

     
        private void Form1_Load(object sender, EventArgs e)
        {
            
            timer1.Start();
        }
    }
}
