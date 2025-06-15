using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtobusOtomasyonSistemi.KullaniciArayuz._1seferler
{
    public class Biletler
    {
        public bool kendineBiletAldiMi { get; set; } //kullanıcı her otobüsten 1 tane bilet aldırabilinir
        public bool biletAktifMi { get; set; }
        public int seferID { get; set; }

        public string seferAdiK { get; set; }
        public string seferAdiV { get; set; }

        public string Ad { get; set; }
        public string Soyad { get; set; }
        public string Cinsiyet { get; set; }
        public string tur { get; set; }
        public int turIndex { get; set; }
        public bool pasaportMu {  get; set; }

        public string tc { get; set; }
        public int koltukNo { get; set; }
        public decimal Fiyat { get; set; }

        public decimal IndirimOrani { get; set; }

        //-------------------------------------bilet oluştururkenki bilgiler allta
        public string kalkisSehri { get; set; }
        public string varisSehri {  get; set; }
       
        public string tarihi { get; set; }
        public string kalkisSaati { get; set; }
    }
}
//1. hata profilim, biletlerim bölümünde bir bilet silinip sonra bir bilet satın alınınca tekrar giremiyoruz. muhtemelen çağırmadan kaynaklı parent olarak çağırmayı dene
