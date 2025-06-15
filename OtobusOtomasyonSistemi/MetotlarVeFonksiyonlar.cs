using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OtobusOtomasyonSistemi
{
    public class MetotlarVeFonksiyonlar
    {
        public static string BiletNoOlustur(int seferId,int koltukNo)
        {
            string biletNO = GlobalData.kalkisTarihi.ToString("yy"+"MM"+"dd")+"-"+seferId.ToString()+"-"+koltukNo.ToString()+"-"+RastgeleKodUret();
            return biletNO;
        }

        public static string RastgeleKodUret()
        {
            const string karakterler = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random rnd = new Random();
            return new string(Enumerable.Range(0, 4)
                .Select(i => karakterler[rnd.Next(karakterler.Length)]).ToArray());
        }
    }
}
