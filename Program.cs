using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
     
        var request = new APIRequest
        {
            Tren = new Tren
            {
                Ad = "Başkent Ekspres",
                Vagonlar = new List<Vagon>
                {
                    new Vagon { Ad = "Vagon 1", Kapasite = 100, DoluKoltukAdet = 68 },
                    new Vagon { Ad = "Vagon 2", Kapasite = 90, DoluKoltukAdet = 50 },
                    new Vagon { Ad = "Vagon 3", Kapasite = 80, DoluKoltukAdet = 80 }
                }
            },
            RezervasyonYapilacakKisiSayisi = 30,
            KisilerFarkliVagonlaraYerlestirilebilir = true
        };

        
        APIResponse response = RezervasyonYap(request);

     
        Console.WriteLine("Rezervasyon Yapilabilir: " + response.RezervasyonYapilabilir);
        if (response.RezervasyonYapilabilir)
        {
            Console.WriteLine("Yerleşim Ayrintilari:");
            foreach (var yerlesim in response.YerlesimAyrinti)
            {
                Console.WriteLine($"Vagon Adi: {yerlesim.VagonAdi}, Kişi Sayisi: {yerlesim.KisiSayisi}");
            }
        }
    }

    static APIResponse RezervasyonYap(APIRequest request)
    {
        bool rezervasyonYapilabilir = false;
        var yerlesimAyrinti = new List<YerlesimAyrinti>();

        foreach (var vagon in request.Tren.Vagonlar)
        {
            int kalanKapasite = vagon.Kapasite - vagon.DoluKoltukAdet;

           
            double yuzdeDolulukOrani = (double)(vagon.DoluKoltukAdet + request.RezervasyonYapilacakKisiSayisi) / vagon.Kapasite;
            if (yuzdeDolulukOrani <= 0.7 && kalanKapasite >= request.RezervasyonYapilacakKisiSayisi)
            {
                if (request.KisilerFarkliVagonlaraYerlestirilebilir)
                {
                    yerlesimAyrinti.Add(new YerlesimAyrinti { VagonAdi = vagon.Ad, KisiSayisi = request.RezervasyonYapilacakKisiSayisi });
                    rezervasyonYapilabilir = true;
                    break;
                }
                else
                {
                    yerlesimAyrinti.Add(new YerlesimAyrinti { VagonAdi = vagon.Ad, KisiSayisi = request.RezervasyonYapilacakKisiSayisi });
                    request.RezervasyonYapilacakKisiSayisi -= kalanKapasite;
                    rezervasyonYapilabilir = true;
                    if (request.RezervasyonYapilacakKisiSayisi == 0)
                        break;
                }
            }
        }

        return new APIResponse { RezervasyonYapilabilir = rezervasyonYapilabilir, YerlesimAyrinti = yerlesimAyrinti };
    }
}

class APIRequest
{
    public required Tren Tren { get; set; }
    public int RezervasyonYapilacakKisiSayisi { get; set; }
    public bool KisilerFarkliVagonlaraYerlestirilebilir { get; set; }
}

class Tren
{
    public required string Ad { get; set; }
    public required List<Vagon> Vagonlar { get; set; }
}

class Vagon
{
    public required string Ad { get; set; }
    public int Kapasite { get; set; }
    public int DoluKoltukAdet { get; set; }
}

class APIResponse
{
    public bool RezervasyonYapilabilir { get; set; }
    public required List<YerlesimAyrinti> YerlesimAyrinti { get; set; }
}

class YerlesimAyrinti
{
    public required string VagonAdi { get; set; }
    public int KisiSayisi { get; set; }
}
