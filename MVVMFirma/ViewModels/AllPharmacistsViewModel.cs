using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMFirma.ViewModels
{
    public class AllPharmacistsViewModel : AllViewModel<PharmacistForAllView>
    {
        #region Constructor
        public AllPharmacistsViewModel()
            : base("Wszyscy farmaceuci")
        {
        }
        #endregion
        #region Sort & Find 
        // tu decydujemy po czym sortowac
        public override List<string> GetComboboxSortList()
        {
            return new List<string> { "Imię", "Nazwisko", "Numer Licencji" };
        }
        private PharmacistForAllView _SelectedPharmacist;
        public PharmacistForAllView SelectedPharmacist
        {
            get => _SelectedPharmacist;
            set
            {
                _SelectedPharmacist = value;
                if (_SelectedPharmacist != null)
                {
                    Messenger.Default.Send(_SelectedPharmacist);
                    OnRequestClose();
                }
            }
        }



        public override void Sort()
        {
            if (SortField == "Imię")
                List = new ObservableCollection<PharmacistForAllView>(List.OrderBy(item => item.Imię).ToList());
            else if (SortField == "Nazwisko")
                List = new ObservableCollection<PharmacistForAllView>(List.OrderBy(item => item.Nazwisko).ToList());
            else if (SortField == "Numer Licencji")
                List = new ObservableCollection<PharmacistForAllView>(List.OrderBy(item => item.Numer_Licencji).ToList());
        }

        public override List<string> GetComboboxFindList()
        {
            return new List<string> { "Imię", "Nazwisko" };
        }

        public override void Find()
        {
            Load();
            if (FindField == "Imię")
                List = new ObservableCollection<PharmacistForAllView>(List.Where(item => item.Imię != null && item.Imię.StartsWith(FindTextBox, StringComparison.OrdinalIgnoreCase)).ToList());
            else if (FindField == "Nazwisko")
                List = new ObservableCollection<PharmacistForAllView>(List.Where(item => item.Nazwisko != null && item.Nazwisko.StartsWith(FindTextBox, StringComparison.OrdinalIgnoreCase)).ToList());
        }


        #endregion
        #region Helpers
        public override void Load()
        {
            List = new ObservableCollection<PharmacistForAllView>
            (
                from pharmacist in aptekaEntities.Farmaceuci
                select new PharmacistForAllView
                {
                    ID_Farmaceuty = pharmacist.ID_Farmaceuty,
                    Imię = pharmacist.Imię,
                    Nazwisko = pharmacist.Nazwisko,
                    Numer_Licencji = pharmacist.Numer_Licencji
                }
            );
        }
        #endregion
    }
}
