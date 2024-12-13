using System;

namespace MVVMFirma.Models.EntitiesForView
{
    public class OrderForAllView
    {
        public int ID_Zamówienia { get; set; }
        public string Nazwa_Dostawcy { get; set; }
        public DateTime Data_Zamówienia { get; set; }
        public DateTime? Data_Dostawy { get; set; }
        public string Status { get; set; }
    }
}
