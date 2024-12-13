using System;

namespace MVVMFirma.Models.EntitiesForView
{
    public class SaleForAllView
    {
        public int ID_Sprzedaży { get; set; }
        public string Nazwa_Leku { get; set; } // Klucz obcy z tabeli Leki
        public string PESEL_Pacjenta { get; set; } // Klucz obcy z tabeli Pacjenci
        public DateTime Data_Sprzedaży { get; set; }
        public decimal Kwota { get; set; }
        public string Forma_Płatności { get; set; }
    }
}
