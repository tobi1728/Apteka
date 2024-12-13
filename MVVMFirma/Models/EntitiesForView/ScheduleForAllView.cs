using System;

namespace MVVMFirma.Models.EntitiesForView
{
    public class ScheduleForAllView
    {
        public int ID_Grafiku { get; set; }
        public string Numer_Licencji { get; set; } // Z tabeli Farmaceuci
        public DateTime Data { get; set; }
        public TimeSpan Godzina_Rozpoczęcia { get; set; }
        public TimeSpan Godzina_Zakończenia { get; set; }
    }
}
