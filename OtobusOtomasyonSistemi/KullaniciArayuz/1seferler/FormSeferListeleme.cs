using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OtobusOtomasyonSistemi.KullaniciArayuz._1seferler
{
    public partial class FormSeferListeleme : Form
    {
        FormKullaniciArayuz _formKullaniciArayuz;
        public FormSeferListeleme(FormKullaniciArayuz formKullaniciArayuz)
        {
            InitializeComponent();
            _formKullaniciArayuz = formKullaniciArayuz;

        }
        string baglanti = "Data Source=LAPTOP-R03\\SQLEXPRESS;Initial Catalog=otobusOtomasyonu;Integrated Security=True;TrustServerCertificate=True";

        Int16 koltukSayisi;

        FormKullaniciArayuz frmka = (FormKullaniciArayuz)Application.OpenForms["FormKullaniciArayuz"];
        private void FormSeferListeleme_Load(object sender, EventArgs e)
        {
            //Sqlbaglanma();
            panelUret(GlobalData.seferlerListesi);
        }

        private void DevamEt_Click(object sender, EventArgs e)
        {
            bool klks = true;

            var buton = sender as Guna2Button;
            var seferID = (int)buton.Tag;
            GlobalData.koltukSecmeSeferID = seferID;
            GlobalData.seferKalkisSaati = TimeSpan.Parse(GlobalData.seferlerListesi.Select("seferID =" + buton.Tag)[0]["kalkisSaati"].ToString()).ToString(@"hh\:mm");
             koltukSayisi =  Convert.ToInt16(GlobalData.seferlerListesi.Select("seferID =" + seferID)[0]["koltukSayisi"]);

            foreach (Control panel in panelAna.Controls)
            {
                if (panel is Guna2Panel)
                {
                    foreach (Control control in panel.Controls)
                    {
                        if (control is Label lbl && lbl.Tag != null)
                        {
                            if (lbl.Tag.ToString() == seferID.ToString())
                            {
                                if (klks)
                                {
                                    GlobalData.koltukSecmeseferAdiK = lbl.Text;
                                    klks = false;
                                }
                                else
                                {
                                    GlobalData.koltukSecmeseferAdiV = lbl.Text;
                                    break;
                                }


                            }
                        }
                    }
                }
            }

            if (koltukSayisi == 40)
            {
                frmka.SayfaDegistir(new otobus40KoltukF(_formKullaniciArayuz));
                this.Close();
            }
            else if (koltukSayisi == 46)
            {
                frmka.SayfaDegistir(new otobus46Koltuk(_formKullaniciArayuz));
                this.Close();
            }

            



        }

        private void panelUret(DataTable dt)
        {
            int yKonum = guna2Panel1.Location.Y;
            int yPanel = 210;
            TimeSpan varis, kalkis;


            if (GlobalData.seferlerListesi.Rows.Count > 0)
            {
                koltukSayisi = Convert.ToInt16(GlobalData.seferlerListesi.Rows[0]["koltukSayisi"].ToString());
            }
            else
            {
                label1.Visible = true;
                return;
            }


            foreach (DataRow row in dt.Rows)
            {


                Guna2Panel yeniPanel = new Guna2Panel();
                yeniPanel.Size = guna2Panel1.Size;
                yeniPanel.Location = new Point(69, yKonum);
                yeniPanel.BorderStyle = guna2Panel1.BorderStyle;
                yeniPanel.BorderThickness = guna2Panel1.BorderThickness;
                yeniPanel.BackColor = guna2Panel1.BackColor;
                yeniPanel.BorderColor = guna2Panel1.BorderColor;
                yeniPanel.FillColor = guna2Panel1.FillColor;
                yeniPanel.BorderRadius = guna2Panel1.BorderRadius;


                Label lblKalkisS = new Label();
                kalkis = TimeSpan.Parse(row["kalkisSaati"].ToString());
                lblKalkisS.Text = kalkis.ToString(@"hh\:mm");
                lblKalkisS.Location = labelKalkisSaati.Location;
                lblKalkisS.AutoSize = true;
                lblKalkisS.ForeColor = labelKalkisSaati.ForeColor;
                lblKalkisS.Font = labelKalkisSaati.Font;

                Label lblVarisS = new Label();
                varis = TimeSpan.Parse(row["varisSaati"].ToString());
                lblVarisS.Text = varis.ToString(@"hh\:mm");
                lblVarisS.Location = labelVarisaati.Location;
                lblVarisS.AutoSize = true;
                lblVarisS.ForeColor = labelVarisaati.ForeColor;
                lblVarisS.Font = labelVarisaati.Font;

                Label lblTahminiVaris = new Label();
                lblTahminiVaris.Text = (varis - kalkis).ToString(@"hh\:mm");
                lblTahminiVaris.Location = labelYolculukSuresi.Location;
                lblTahminiVaris.AutoSize = true;
                lblTahminiVaris.ForeColor = labelYolculukSuresi.ForeColor;
                lblTahminiVaris.Font = labelYolculukSuresi.Font;

                Label lblCizgi = new Label();
                lblCizgi.Text = label2.Text;
                lblCizgi.Location = label2.Location;
                lblCizgi.AutoSize = true;
                lblCizgi.ForeColor = label2.ForeColor;
                lblCizgi.Font = label2.Font;

                Label lblFiyat = new Label();
                lblFiyat.Text = row["fiyat"].ToString() + " TL";
                lblFiyat.Location = labelFiyat.Location;
                lblFiyat.AutoSize = true;
                lblFiyat.ForeColor = labelFiyat.ForeColor;
                lblFiyat.Font = labelFiyat.Font;

                Label lblKalkisT = new Label();
                lblKalkisT.Text = row["kalkisTerminalAdi"].ToString();
                lblKalkisT.Location = labelKalkisYeri.Location;
                lblKalkisT.AutoSize = true;
                lblKalkisT.ForeColor = labelKalkisYeri.ForeColor;
                lblKalkisT.Font = labelKalkisYeri.Font;
                lblKalkisT.Tag = row["seferID"];

                Label lblVarisT = new Label();
                lblVarisT.Text = row["varisTerminalAdi"].ToString();
                lblVarisT.Location = labelVarisYeri.Location;
                lblVarisT.AutoSize = true;
                lblVarisT.ForeColor = labelVarisYeri.ForeColor;
                lblVarisT.Font = labelVarisYeri.Font;
                lblVarisT.Tag = row["seferID"];

                Label lblUyari = new Label();
                lblUyari.Text = "";
                lblUyari.Location = labelUyari.Location;
                lblUyari.AutoSize = true;
                lblUyari.ForeColor = labelUyari.ForeColor;
                lblUyari.Font = labelUyari.Font;
                if (Convert.ToInt32(row["kacKoltukKaldi"]) <= 3)
                {
                    lblUyari.Text = row["kacKoltukKaldi"].ToString() + " Koltuk Kaldı!";
                }

                Guna2TextBox otobusBilgisi = new Guna2TextBox();
                otobusBilgisi.TextAlign = HorizontalAlignment.Center;
                otobusBilgisi.Margin = guna2TextBox1.Margin;
                otobusBilgisi.Size = new Size(170, 18);
                otobusBilgisi.Location = new Point(28, 63);
                otobusBilgisi.BorderRadius = guna2TextBox1.BorderRadius;
                otobusBilgisi.BorderThickness = guna2TextBox1.BorderThickness;
                otobusBilgisi.ReadOnly = true;
                otobusBilgisi.BackColor = guna2TextBox1.BackColor;
                otobusBilgisi.ForeColor = guna2TextBox1.ForeColor;
                otobusBilgisi.Font = guna2TextBox1.Font;

                if (row["koltukSayisi"].ToString() == "40")
                {
                    otobusBilgisi.Text = "Otobüs | 2 + 1";
                }
                else
                {
                    otobusBilgisi.Text = "Otobüs | 2 + 2";
                }


                PictureBox pictureBox = new PictureBox();
                pictureBox.Size = pictureBox1.Size;
                pictureBox.Location = new Point(pictureBox1.Location.X + 40, pictureBox1.Location.Y);
                pictureBox.Image = pictureBox1.Image;
                pictureBox.SizeMode = pictureBox1.SizeMode;
                pictureBox.BackColor = pictureBox1.BackColor;
                pictureBox.BorderStyle = pictureBox1.BorderStyle;

                Guna2Button devamEt = new Guna2Button();
                devamEt.Text = "DEVAM ET";
                devamEt.Location = guna2ButtonOnayla.Location;
                devamEt.Size = guna2ButtonOnayla.Size;
                devamEt.BorderRadius = guna2ButtonOnayla.BorderRadius;
                devamEt.Font = guna2ButtonOnayla.Font;
                devamEt.ForeColor = guna2ButtonOnayla.ForeColor;
                devamEt.FillColor = guna2ButtonOnayla.FillColor;
                devamEt.Tag = row["seferID"];
                devamEt.Click += DevamEt_Click;







                yeniPanel.Controls.Add(lblKalkisS);
                yeniPanel.Controls.Add(lblVarisS);
                yeniPanel.Controls.Add(lblTahminiVaris);
                yeniPanel.Controls.Add(lblCizgi);
                yeniPanel.Controls.Add(lblFiyat);
                yeniPanel.Controls.Add(lblKalkisT);
                yeniPanel.Controls.Add(lblVarisT);
                yeniPanel.Controls.Add(lblUyari);
                yeniPanel.Controls.Add(otobusBilgisi);
                yeniPanel.Controls.Add(pictureBox);
                yeniPanel.Controls.Add(devamEt);

                panelAna.Size = new Size(panelAna.Size.Width, yPanel);
      
                panelAna.Controls.Add(yeniPanel);

                yPanel += 150;
                yKonum += 150; 
            }


        }
    }
}
