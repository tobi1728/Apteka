using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System.Collections.ObjectModel;
using System.Linq;

namespace MVVMFirma.ViewModels
{
    public class AllSchedulesViewModel : AllViewModel<ScheduleForAllView>
    {
        #region Constructor
        public AllSchedulesViewModel() : base("Grafiki pracowników") { }
        #endregion

        #region Helpers
        public override void Load()
        {
            List = new ObservableCollection<ScheduleForAllView>
            (
                from schedule in aptekaEntities.Grafiki_Pracowników
                select new ScheduleForAllView
                {
                    ID_Grafiku = schedule.ID_Grafiku,
                    Pracownik = schedule.Farmaceuci.Imię + " " + schedule.Farmaceuci.Nazwisko,
                    Data = schedule.Data,
                    Godzina_Rozpoczęcia = schedule.Godzina_Rozpoczęcia,
                    Godzina_Zakończenia = schedule.Godzina_Zakończenia
                }
            );
        }
        #endregion
    }
}
