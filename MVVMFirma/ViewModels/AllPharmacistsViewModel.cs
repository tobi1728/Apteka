using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Helper;
using MVVMFirma.Models.EntitiesForView;
using System.Collections.ObjectModel;
using System.Linq;

namespace MVVMFirma.ViewModels
{
    public class AllPharmacistsViewModel : AllViewModel<PharmacistForAllView>
    {
        private PharmacistForAllView _selectedPharmacist;
        public PharmacistForAllView SelectedPharmacist
        {
            get => _selectedPharmacist;
            set
            {
                _selectedPharmacist = value;
                OnPropertyChanged(() => SelectedPharmacist);

                // Jeśli okno jest modalne, a user wybrał wiersz:
                if (_selectedPharmacist != null && IsModal)
                {
                    // Wysyłamy farmaceutę do VM, który nas otworzył (np. NewScheduleViewModel)
                    Messenger.Default.Send(_selectedPharmacist);
                    // Zamykamy okno (modalne)
                    OnRequestClose();
                }
            }
        }

        // Domyślnie false, ustawiamy w MainWindowViewModel jeśli chcemy modalnie
        public bool IsModal { get; set; } = false;

        public AllPharmacistsViewModel()
            : base("Wszyscy farmaceuci")
        {
        }

        public override void Load()
        {
            List = new ObservableCollection<PharmacistForAllView>
            (
                from p in aptekaEntities.Farmaceuci
                select new PharmacistForAllView
                {
                    ID_Farmaceuty = p.ID_Farmaceuty,
                    Imię = p.Imię,
                    Nazwisko = p.Nazwisko,
                    Numer_Licencji = p.Numer_Licencji
                }
            );
        }

        public override System.Collections.Generic.List<string> GetComboboxSortList()
        {
            return new System.Collections.Generic.List<string> { "Imię", "Nazwisko", "Numer Licencji" };
        }

        public override void Sort()
        {
            if (SortField == "Imię")
            {
                List = new ObservableCollection<PharmacistForAllView>(
                    List.OrderBy(item => item.Imię)
                );
            }
            else if (SortField == "Nazwisko")
            {
                List = new ObservableCollection<PharmacistForAllView>(
                    List.OrderBy(item => item.Nazwisko)
                );
            }
            else if (SortField == "Numer Licencji")
            {
                List = new ObservableCollection<PharmacistForAllView>(
                    List.OrderBy(item => item.Numer_Licencji)
                );
            }
        }

        public override System.Collections.Generic.List<string> GetComboboxFindList()
        {
            return new System.Collections.Generic.List<string> { "Imię", "Nazwisko" };
        }

        public override void Find()
        {
            Load();
            if (FindField == "Imię")
            {
                List = new ObservableCollection<PharmacistForAllView>(
                    List.Where(item => item.Imię != null
                        && item.Imię.StartsWith(FindTextBox, System.StringComparison.OrdinalIgnoreCase))
                );
            }
            else if (FindField == "Nazwisko")
            {
                List = new ObservableCollection<PharmacistForAllView>(
                    List.Where(item => item.Nazwisko != null
                        && item.Nazwisko.StartsWith(FindTextBox, System.StringComparison.OrdinalIgnoreCase))
                );
            }
        }
    }
}
