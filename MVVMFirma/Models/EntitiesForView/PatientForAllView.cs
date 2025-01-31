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

        // Sprawdza, czy Data_Urodzenia <= (dziś - 18 lat)
        public bool IsAdult
        {
            get
            {
                DateTime adultThreshold = DateTime.Today.AddYears(-18);
                return Data_Urodzenia <= adultThreshold;
            }
        }
    }
}
