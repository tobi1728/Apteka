using System;

namespace MVVMFirma.Models.EntitiesForView
{
    public class SalesReportForAllView
    {
        public int ID_Raportu { get; set; }
        public DateTime Data_Rozpoczęcia { get; set; }
        public DateTime Data_Zakończenia { get; set; }
        public decimal Łączna_Sprzedaż { get; set; }
        public int Liczba_Transakcji { get; set; }
    }
}
