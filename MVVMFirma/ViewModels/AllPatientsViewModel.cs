using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MVVMFirma.ViewModels
{
    public class AllPatientsViewModel : AllViewModel<PatientForAllView>
    {
        #region Constructor
        public AllPatientsViewModel()
            : base("Wszyscy pacjenci")
        {
        }
        #endregion
        #region Sort & Find 
        // tu decydujemy po czym sortowac
        public override List<string> GetComboboxSortList()
        {
            return new List<string> { "Imię", "Nazwisko", "PESEL" };
        }

        public override void Sort()
        {
            if (SortField == "Imię")
                List = new ObservableCollection<PatientForAllView>(List.OrderBy(item => item.Imię).ToList());
            else if (SortField == "Nazwisko")
                List = new ObservableCollection<PatientForAllView>(List.OrderBy(item => item.Nazwisko).ToList());
            else if (SortField == "PESEL")
                List = new ObservableCollection<PatientForAllView>(List.OrderBy(item => item.PESEL).ToList());
        }

        public override List<string> GetComboboxFindList()
        {
            return new List<string> { "Imię", "Nazwisko" };
        }

        public override void Find()
        {
            Load();
            if (FindField == "Imię")
                List = new ObservableCollection<PatientForAllView>(List.Where(item => item.Imię != null && item.Imię.StartsWith(FindTextBox, StringComparison.OrdinalIgnoreCase)).ToList());
            else if (FindField == "Nazwisko")
                List = new ObservableCollection<PatientForAllView>(List.Where(item => item.Nazwisko != null && item.Nazwisko.StartsWith(FindTextBox, StringComparison.OrdinalIgnoreCase)).ToList());
        }


        #endregion
        #region Helpers
        public override void Load()
        {
            List = new ObservableCollection<PatientForAllView>
            (
                from patient in aptekaEntities.Pacjenci
                select new PatientForAllView
                {
                    ID_Pacjenta = patient.ID_Pacjenta,
                    Imię = patient.Imię,
                    Nazwisko = patient.Nazwisko,
                    Data_Urodzenia = patient.Data_Urodzenia,
                    PESEL = patient.PESEL
                }
            );
        }
        #endregion
    }
}
