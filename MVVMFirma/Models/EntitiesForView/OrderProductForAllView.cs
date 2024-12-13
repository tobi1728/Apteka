using System;

namespace MVVMFirma.Models.EntitiesForView
{
    public class OrderProductForAllView
    {
        public int ID_Produktu_Zamówienia { get; set; }
        public int ID_Zamówienia { get; set; }
        public int Numer_Zamówienia { get; set; } // Zamówienia -> Numer_Zamówienia
        public string Nazwa_Leku { get; set; } // Leki -> Nazwa_Leku
        public int Ilość { get; set; }
    }
}
