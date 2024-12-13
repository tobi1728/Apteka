using System;

namespace MVVMFirma.Models.EntitiesForView
{
    public class WarehouseForAllView
    {
        public int ID_Magazynu { get; set; }
        public string Nazwa_Leku { get; set; } // Klucz obcy z tabeli Leki
        public int Ilość { get; set; }
        public string Ulica { get; set; }
        public string Miasto { get; set; }
        public string Kod_Pocztowy { get; set; }
        public string Telefon { get; set; }
    }
}
