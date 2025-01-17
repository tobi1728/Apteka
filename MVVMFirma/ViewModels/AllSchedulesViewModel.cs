using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MVVMFirma.ViewModels
{
    public class AllSchedulesViewModel : AllViewModel<ScheduleForAllView>
    {
        #region Constructor
        public AllSchedulesViewModel() : base("Grafiki pracowników") { }
        #endregion
        #region Sort & Find 
        // tu decydujemy po czym sortowac
        public override List<string> GetComboboxSortList()
        {
            return new List<string> { "Pracownik", "Data", "Godzina Rozpoczęcia", "Godzina Zakończenia" };
        }

        public override void Sort()
        {
            if (SortField == "Pracownik")
                List = new ObservableCollection<ScheduleForAllView>(List.OrderBy(item => item.Pracownik).ToList());
            else if (SortField == "Data")
                List = new ObservableCollection<ScheduleForAllView>(List.OrderBy(item => item.Data).ToList());
            else if (SortField == "Godzina Rozpoczęcia")
                List = new ObservableCollection<ScheduleForAllView>(List.OrderBy(item => item.Godzina_Rozpoczęcia).ToList());
            else if (SortField == "Godzina Zakończenia")
                List = new ObservableCollection<ScheduleForAllView>(List.OrderBy(item => item.Godzina_Zakończenia).ToList());
        }

        public override List<string> GetComboboxFindList()
        {
            return new List<string> { "Pracownik", "Data" };
        }

        public override void Find()
        {
            Load();
            if (FindField == "Pracownik")
                List = new ObservableCollection<ScheduleForAllView>(List.Where(item => item.Pracownik != null && item.Pracownik.StartsWith(FindTextBox, StringComparison.OrdinalIgnoreCase)).ToList());
            else if (FindField == "Data")
                List = new ObservableCollection<ScheduleForAllView>(List.Where(item => item.Data != null && item.Data.ToString().StartsWith(FindTextBox, StringComparison.OrdinalIgnoreCase)).ToList());
        }


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
