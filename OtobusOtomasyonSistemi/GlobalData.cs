using Guna.UI2.WinForms;
using OtobusOtomasyonSistemi.KullaniciArayuz._1seferler;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OtobusOtomasyonSistemi
{
    public static class GlobalData
    {
        public static int? kullaniciID { get; set; }
        public static string kullaniciAd { get; set; }
        public static string kullaniciSoyad { get; set; }
        public static string kullaniciCinsiyet { get; set; } // Kadın, Erkek
        public static string kullaniciTuru { get; set; }
        public static int kullaniciTurIndex { get; set; }
        public static string kullaniciTelefon { get; set; }
        public static string kullaniciEposta { get; set; }
        public static string kullaniciTC { get; set; }
        public static bool kullaniciPasaportMu { get; set; }


        //----------------------------------------------------------- ^ yolcu bilgileri
        public static string biletID { get; set; }

        
        public static DataTable seferlerListesi { get; set; }
        public static DataTable seferIDveYolcuKoltukNo { get; set; }


        public static Dictionary<byte, bool> paneller { get; set; } = new Dictionary<byte, bool>(); //otobüsler(40-46) için 4 adet seçilen biletler
        public static byte secmeSayisi { get; set; } = 0; // otobüsler(40-46) için 4 adet seçilen biletler seçildi mi
        public static string kalkisSehir { get; set; }
        public static string varisSehir { get; set; }
        public static int koltukSecmeSeferID { get; set; }
        public static string koltukSecmeseferAdiK { get; set; }
        public static string koltukSecmeseferAdiV { get; set; }
        public static string seferKalkisSaati { get; set; }
        public static int gecicikoltukNo { get; set; }
        public static TimeSpan TvarisSaati { get; set; } //km cinsinden




        public static decimal geciciFiyatToplam { get; set; } //bilet satın alma bölümündeki toplamı göstermek için
                                                              //---------------------------------------------------------------------------------------------------------------- ^ sefer bilgileri




        public static string otobusPlaka { get; set; }
        //--------------------------------------------------^harita için bilet bilgileri












        public static bool gidisDonusMu { get; set; }
        public static DateTime kalkisTarihi { get; set; }
        public static DateTime donusTarihi { get; set; }
        public static BindingList<Biletler> biletListesi { get; set; } = new BindingList<Biletler>();
        //-------------------------------------------------------------------------------->admin Bilgileri  
        public static int? adminID { get; set; }
        public static string kullaniciYetki { get; set; }

        public static string adminAdi{ get; set; }
        public static string adminSoyadi { get; set; }


    }
   

}
