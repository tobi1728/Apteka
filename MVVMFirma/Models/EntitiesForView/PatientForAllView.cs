using System;

namespace MVVMFirma.Models.EntitiesForView
{
    public class PatientForAllView
    {
        public int ID_Pacjenta { get; set; }
        public string Imię { get; set; }
        public string Nazwisko { get; set; }
        public DateTime Data_Urodzenia { get; set; }
        public string PESEL { get; set; }
    }
}
