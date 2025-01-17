using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MVVMFirma.ViewModels
{
    public class AllSuppliersViewModel : AllViewModel<SupplierForAllView>
    {
        private SupplierForAllView _SelectedSupplier;
        public SupplierForAllView SelectedSupplier
        {
            get => _SelectedSupplier;
            set
            {
                _SelectedSupplier = value;
                if (_SelectedSupplier != null)
                {
                    Messenger.Default.Send(_SelectedSupplier);
                    OnRequestClose();
                }
            }
        }


        #region Sort & Find 
        // tu decydujemy po czym sortowac
        public override List<string> GetComboboxSortList()
        {
            return new List<string> { "Nazwa", "Miasto", "Kod Pocztowy" };
        }

        public override void Sort()
        {
            if (SortField == "Nazwa")
                List = new ObservableCollection<SupplierForAllView>(List.OrderBy(item => item.Nazwa).ToList());
            else if (SortField == "Miasto")
                List = new ObservableCollection<SupplierForAllView>(List.OrderBy(item => item.Miasto).ToList());
            else if (SortField == "Kod Pocztowy")
                List = new ObservableCollection<SupplierForAllView>(List.OrderBy(item => item.Kod_Pocztowy).ToList());
        }

        public override List<string> GetComboboxFindList()
        {
            return new List<string> { "Nazwa", "Miasto" };
        }

        public override void Find()
        {
            Load();
            if (FindField == "Nazwa")
                List = new ObservableCollection<SupplierForAllView>(List.Where(item => item.Nazwa != null && item.Nazwa.StartsWith(FindTextBox, StringComparison.OrdinalIgnoreCase)).ToList());
            else if (FindField == "Miasto")
                List = new ObservableCollection<SupplierForAllView>(List.Where(item => item.Miasto != null && item.Miasto.StartsWith(FindTextBox, StringComparison.OrdinalIgnoreCase)).ToList());
        }


        #endregion

        public AllSuppliersViewModel() : base("Wszyscy dostawcy") { }

        public override void Load()
        {
            List = new ObservableCollection<SupplierForAllView>
            (
                from supplier in aptekaEntities.Dostawcy
                select new SupplierForAllView
                {
                    ID_Dostawcy = supplier.ID_Dostawcy,
                    Nazwa = supplier.Nazwa,
                    Telefon = supplier.Telefon,
                    Ulica = supplier.Ulica,
                    Miasto = supplier.Miasto,
                    Kod_Pocztowy = supplier.Kod_Pocztowy
                }
            );
        }
    }
}
