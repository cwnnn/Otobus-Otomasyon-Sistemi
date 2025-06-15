using QRCoder;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace OtobusOtomasyonSistemi.KullaniciArayuz._1seferler
{
    public partial class FormSatinAlmaOnaylama : Form
    {
        static string baglanti = "Data Source=LAPTOP-R03\\SQLEXPRESS;Initial Catalog=otobusOtomasyonu;Integrated Security=True;TrustServerCertificate=True";
        SqlConnection baglan = new SqlConnection(baglanti);

        private FormKullaniciArayuz _kullaniciArayuz;
        public FormSatinAlmaOnaylama(FormKullaniciArayuz kullaniciArayuz)
        {
            InitializeComponent();
            _kullaniciArayuz = kullaniciArayuz;
        }
        string eposta = "";
        int kod ;
        private void FormSatinAlmaOnaylama_Load(object sender, EventArgs e)
        {
            this.Size = new Size(436, 389);
            AnaFonkisyon();

            if (GlobalData.kullaniciID != null && GlobalData.kullaniciID > 0)
            {
                this.Close();
            }

        }

        private void AnaFonkisyon()
        {
            List<Biletler> silinecekRowlar = new List<Biletler>();
            List<Image> biletresimleri = new List<Image>();
            int yolcuID =-1;
            bool biletEklendiMi = false;
            string eposta="";
            
            foreach (Biletler b in GlobalData.biletListesi)
            {
                if (!b.biletAktifMi)
                {
                    continue;
                }

                yolcuID = -1;
                biletEklendiMi = false;
                eposta = "";

                if (GlobalData.kullaniciID == null)
                {
                    if (guna2TextBoxEposta.Text == "E-posta" || guna2TextBoxEposta.Text.Replace(" ", "") == "" || !guna2TextBoxEposta.Text.Contains("@") || !guna2TextBoxEposta.Text.Contains("."))
                    {
                        break;
                    }
                }
                MessageBox.Show(b.tur.ToString());

                if (b.kendineBiletAldiMi)
                {
                    yolcuID = Convert.ToInt32(GlobalData.kullaniciID);
                    eposta = GlobalData.kullaniciEposta;

                    label15.Text = MetotlarVeFonksiyonlar.BiletNoOlustur(b.seferID, b.koltukNo);
                    biletEklendiMi  = sqlBiletEkleme(label15.Text, yolcuID, b.seferID, b.koltukNo, (b.Fiyat * (1 - (b.IndirimOrani / 100))), b.seferAdiK, b.seferAdiV);

                }
                else
                {
                    int yolcuVarMiID = sqldeYolcuVarMi(b.tc);

                    if (yolcuVarMiID != -1)  
                    {
                        yolcuID = yolcuVarMiID;
                        eposta = SqldeYolcununEpostasi(yolcuID.ToString());

                        label15.Text = MetotlarVeFonksiyonlar.BiletNoOlustur(b.seferID, b.koltukNo);
                        biletEklendiMi = sqlBiletEkleme(label15.Text, yolcuID, b.seferID, b.koltukNo, (b.Fiyat * (1 - (b.IndirimOrani / 100))), b.seferAdiK, b.seferAdiV);

                    }
                    else
                    {
                        if (GlobalData.kullaniciID!=null)
                        {
                            eposta = GlobalData.kullaniciEposta;
                        }
                        else
                        {
                            eposta = guna2TextBoxEposta.Text;
                        }

                        yolcuID = sqlYolcuEkle(b.Ad, b.Soyad, b.Cinsiyet, eposta, b.turIndex, b.tc, b.pasaportMu, false);

                        if (yolcuID > 0)
                        {
                            label15.Text = MetotlarVeFonksiyonlar.BiletNoOlustur(b.seferID, b.koltukNo);
                            biletEklendiMi = sqlBiletEkleme(label15.Text, yolcuID, b.seferID, b.koltukNo, (b.Fiyat * (1 - (b.IndirimOrani / 100))), b.seferAdiK,b.seferAdiV);
                        }
                    }
                }

                if (biletEklendiMi)
                {
                    labelAdSoyad.Text = b.Ad + " " + b.Soyad;
                    label10.Text = b.tc;
                    label9.Text = b.tur;
                    
                    label14.Text = b.kalkisSaati;
                    label13.Text = b.tarihi;
                    labelKalkisYeri.Text = b.koltukNo.ToString();
                    labelVarisYeri.Text = b.seferAdiK;
                    label8.Text = b.seferAdiV;

                    silinecekRowlar.Add(b);

                    string qrBilgisi = label15.Text + "|Woot Turizm|RCŞ";
                    QRkodOlusturucu(qrBilgisi);
                    biletresimleri.Add(paneliImageCevir(guna2Panel2));
                }




            }

            foreach (var item in silinecekRowlar)
            {

                GlobalData.biletListesi.Remove(item);
            }
            if (yolcuID >0)
            {
               
                EpostayaBiletGonder(eposta, biletresimleri);
                _kullaniciArayuz.SayfaDegistir(new FormSatinAlmaIslemi(_kullaniciArayuz));

            }
            
            
        }
        private int sqldeYolcuVarMi(string tcNo)
        {
            string sorgu = "SELECT yolcuID FROM Yolcular WHERE tc = @tc";

            using (SqlCommand cmd = new SqlCommand(sorgu, baglan))
            {
                cmd.Parameters.AddWithValue("@tc", tcNo);

                try
                {
                    baglan.Open();
                    object result = cmd.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                        return Convert.ToInt32(result);
                    else
                        return -1; // kullanıcı yok
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message);
                    return -1; // hata varsa da -1 döndür
                }
                finally
                {
                    if (baglan.State == ConnectionState.Open)
                        baglan.Close();
                }
            }

        }
        private string SqldeYolcununEpostasi(string id)
        {
            string sorgu = "SELECT ePosta FROM Yolcular WHERE yolcuID = @id";

            using (SqlCommand cmd = new SqlCommand(sorgu, baglan))
            {
                cmd.Parameters.AddWithValue("@id", id);

                try
                {
                    baglan.Open();
                    object result = cmd.ExecuteScalar();

                    if (result != null && result != DBNull.Value)
                        return result.ToString();
                    else
                        return "";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message);
                    return "";
                }
                finally
                {
                    if (baglan.State == ConnectionState.Open)
                        baglan.Close();
                }
            }
        }

        private int sqlYolcuEkle(string ad, string soyad, string cinsiyet, string ePosta, int kullaniciTuruID, string tc, bool pasaportMu, bool kayitOlduMu)
        {
            string sorgu = "INSERT INTO Yolcular (ad, soyad, cinsiyet, ePosta, kullaniciTuruID, tc, pasaportMu, kayitOlduMu) " +
                           "VALUES (@ad, @soyad, @cinsiyet, @ePosta, @kullaniciTuruID, @tc, @pasaportMu, @kayitOlduMu); " +
                           "SELECT SCOPE_IDENTITY();";

            using (SqlCommand cmd = new SqlCommand(sorgu, baglan))
            {
                cmd.Parameters.AddWithValue("@ad", ad);
                cmd.Parameters.AddWithValue("@soyad", soyad);
                cmd.Parameters.AddWithValue("@cinsiyet", cinsiyet.Substring(0, 1));
                cmd.Parameters.AddWithValue("@ePosta", ePosta);
                cmd.Parameters.AddWithValue("@kullaniciTuruID", kullaniciTuruID);
                cmd.Parameters.AddWithValue("@tc", tc);
                cmd.Parameters.AddWithValue("@pasaportMu", pasaportMu);
                cmd.Parameters.AddWithValue("@kayitOlduMu", kayitOlduMu);

                try
                {
                    baglan.Open();
                    object sonuc = cmd.ExecuteScalar();
                    return Convert.ToInt32(sonuc);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message);
                    return -1;
                }
                finally
                {
                    if (baglan.State == System.Data.ConnectionState.Open)
                        baglan.Close();
                }
            }
        }

        private bool sqlBiletEkleme(string biletNo, int yolcuID, int seferID, int koltukNo, decimal ucret, string binisTerminalAdi, string varisTerminalAdi)
        {

            int binisTerminalID = GetTerminalIDByName(binisTerminalAdi);
            int varisTerminalID = GetTerminalIDByName(varisTerminalAdi);

            if (binisTerminalID == -1 || varisTerminalID == -1)
            {
                MessageBox.Show("Terminal adı geçersiz!");
                return false;
            }

            string sorgu = "INSERT INTO Biletler (biletNo, yolcuID, seferID, koltukNo, ucret, iptalEdildiMi, binisTerminalID, varisTerminalID) " +
                           "VALUES (@biletNO, @yolcuID, @seferID, @koltukNo, @ucret, 0, @binisTerminalID, @varisTerminalID);";

            using (SqlCommand cmd = new SqlCommand(sorgu, baglan))
            {
                cmd.Parameters.AddWithValue("@biletNO", biletNo);
                cmd.Parameters.AddWithValue("@yolcuID", yolcuID);
                cmd.Parameters.AddWithValue("@seferID", seferID);
                cmd.Parameters.AddWithValue("@koltukNo", koltukNo);
                cmd.Parameters.AddWithValue("@ucret", ucret);
                cmd.Parameters.AddWithValue("@binisTerminalID", binisTerminalID);
                cmd.Parameters.AddWithValue("@varisTerminalID", varisTerminalID);

                try
                {
                    baglan.Open();
                    int sonuc = cmd.ExecuteNonQuery();
                    return sonuc > 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex.Message);
                    return false;
                }
                finally
                {
                    if (baglan.State == ConnectionState.Open)
                        baglan.Close();
                    MessageBox.Show("Bilet Satın Alındı, Lütfen E-postanızı kontrol ediniz.", "Başarılı");
                }
            }
        }
        private int GetTerminalIDByName(string terminalAdi)
        {
            string sorgu = "SELECT terminalID FROM Terminaller WHERE terminalAdi = @terminalAdi";
            using (SqlCommand cmd = new SqlCommand(sorgu, baglan))
            {
                cmd.Parameters.AddWithValue("@terminalAdi", terminalAdi);
                try
                {
                    baglan.Open();
                    object result = cmd.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : -1;
                }
                catch
                {
                    return -1;
                }
                finally
                {
                    if (baglan.State == ConnectionState.Open)
                        baglan.Close();
                }
            }
        }



        private async void EpostayaBiletGonder(string eposta, List<Image> biletResimleri)
        {
            try
            {
                await Task.Run(() =>
                {
                    using (SmtpClient smtpServer = new SmtpClient("smtp.gmail.com")
                    {
                        Port = 587,
                        EnableSsl = true,
                        Credentials = new NetworkCredential("wootturizm@gmail.com", "vcor thxn vsnw pgch")
                    })
                    using (MailMessage mail = new MailMessage()
                    {
                        From = new MailAddress("wootturizm@gmail.com"),
                        Subject = "WOOT TURİZM - Biletleriniz",
                        Body =
                            $"Merhaba,\n\n" +
                            $"Bilet satın alma işleminiz başarıyla tamamlandı. Aşağıda görsel formatında bilet(ler)iniz ekte yer almaktadır.\n\n" +
                            "İyi yolculuklar dileriz!\nWoot Turizm Destek Ekibi"
                    })
                    {
                        mail.To.Add(eposta);

                        int sayac = 1;
                        foreach (var bilet in biletResimleri)
                        {
                            MemoryStream ms = new MemoryStream();
                            bilet.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                            ms.Position = 0;

                            Attachment attachment = new Attachment(ms, $"bilet_{sayac}.png", "image/png");
                            mail.Attachments.Add(attachment);
                            sayac++;
                        }

                        smtpServer.Send(mail);
                    }

                    
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
        }

        private void SendEmailWithImage(Image image, string recipientEmail)
        {
            try
            {
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(""); // Gönderen
                mail.To.Add(recipientEmail); // Alıcı parametre olarak alındı
                mail.Subject = "Panel Görseli"; // Sabit başlık
                mail.Body = "Aşağıda form panelinden alınan görsel bulunmaktadır."; // Sabit içerik

                using (MemoryStream ms = new MemoryStream())
                {
                    image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                    ms.Position = 0;

                    Attachment attachment = new Attachment(ms, "panel.png", "image/png");
                    mail.Attachments.Add(attachment);

                    SmtpClient smtpClient = new SmtpClient("smtp.example.com")
                    {
                        Port = 587,
                        Credentials = new NetworkCredential("your-email@example.com", "your-email-password"),
                        EnableSsl = true
                    };

                    smtpClient.Send(mail);
                }

                MessageBox.Show("E-posta başarıyla gönderildi!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("E-posta gönderilirken hata oluştu: " + ex.Message);
            }
        }

        public void QRkodOlusturucu(string qrText)
        {
            using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
            {
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrText, QRCodeGenerator.ECCLevel.H); // Q = %25 hata düzeltme
                QRCode qrCode = new QRCode(qrCodeData);
                Bitmap qrCodeImage = qrCode.GetGraphic(20, Color.Black, Color.White, Properties.Resources.logo, 25, 4, true);

                pictureBox3.Image = qrCodeImage;
            }
        }
        public Image paneliImageCevir(Guna.UI2.WinForms.Guna2Panel panel)
        {
            Bitmap bmp = new Bitmap(panel.Width, panel.Height);
            panel.DrawToBitmap(bmp, new Rectangle(0, 0, panel.Width, panel.Height));
            return bmp;
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (guna2TextBoxEposta.Text == "E-posta"|| guna2TextBoxEposta.Text.Replace(" ", "") == "" || !guna2TextBoxEposta.Text.Contains("@") || !guna2TextBoxEposta.Text.Contains("."))
            {
                labelUyari.Text = "lütfen geçerli bir e posta giriniz";
            }
            else
            {
                AnaFonkisyon();
                this.Close();
            }

        }
        private void label4_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void label4_MouseMove(object sender, MouseEventArgs e)
        {
            label4.ForeColor = Color.FromArgb(215, 41, 13);
        }

        private void label4_MouseLeave(object sender, EventArgs e)
        {
            label4.ForeColor = Color.FromArgb(255, 152, 0);

        }
        private void guna2TextBoxKullanıcıAdi_Enter(object sender, EventArgs e)
        {
            if (guna2TextBoxEposta.Text == "E-posta")
            {
                guna2TextBoxEposta.Text = "";
                guna2TextBoxEposta.ForeColor = Color.White;
            }
        }

        private void guna2TextBoxKullanıcıAdi_Leave(object sender, EventArgs e)
        {
            if (guna2TextBoxEposta.Text == "" || guna2TextBoxEposta.Text == " ")
            {
                guna2TextBoxEposta.Text = "E-posta";
                guna2TextBoxEposta.ForeColor = Color.FromArgb(44, 61, 94);
            }

        }


    }
}
