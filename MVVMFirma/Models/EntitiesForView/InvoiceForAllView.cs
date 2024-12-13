namespace MVVMFirma.Models.EntitiesForView
{
    public class InvoiceForAllView
    {
        public int ID_Faktury { get; set; }
        public string Numer_Faktury { get; set; }
        public System.DateTime Data_Wystawienia { get; set; }
        public System.DateTime Data_Platnosci { get; set; }
        public decimal Kwota { get; set; }
        public string Nazwa_Dostawcy { get; set; } // Zamieniono klucz obcy na nazwę dostawcy
    }
}
