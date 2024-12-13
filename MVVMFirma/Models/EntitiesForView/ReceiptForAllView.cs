using System;

namespace MVVMFirma.Models.EntitiesForView
{
    public class ReceiptForAllView
    {
        public int ID_Paragonu { get; set; }
        public string Numer_Paragonu { get; set; }
        public int ID_Sprzedaży { get; set; }
        public DateTime Data_Wystawienia { get; set; }
        public decimal Kwota { get; set; }
    }
}
