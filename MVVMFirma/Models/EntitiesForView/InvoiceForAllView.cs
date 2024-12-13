using System;

namespace MVVMFirma.Models.EntitiesForView
{
    public class InvoiceForAllView
    {
        public int ID_Faktury { get; set; }
        public string Numer_Faktury { get; set; }
        public string Nazwa_Dostawcy { get; set; } // Zamiast ID_Dostawcy
        public DateTime Data_Wystawienia { get; set; }
        public decimal Kwota { get; set; }
        public string Numer_Zamówienia { get; set; } // Zamiast ID_Zamówienia
    }
}
