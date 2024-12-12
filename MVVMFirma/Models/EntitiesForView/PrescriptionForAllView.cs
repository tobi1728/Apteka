using System;

namespace MVVMFirma.Models.EntitiesForView
{
    public class PrescriptionForAllView
    {
        public int ID_Recepty { get; set; }
        public string PatientName { get; set; }
        public string PharmacistName { get; set; }
        public DateTime Data_Wystawienia { get; set; }
        public DateTime? Data_Realizacji { get; set; }
    }
}
