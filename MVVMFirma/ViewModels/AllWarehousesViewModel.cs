using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MVVMFirma.ViewModels
{
    public class AllWarehousesViewModel : AllViewModel<WarehouseForAllView>
    {
        #region Constructor
        public AllWarehousesViewModel()
            : base("Wszystkie magazyny")
        {
        }
        #endregion
        #region Sort & Find 
        // tu decydujemy po czym sortowac
        public override List<string> GetComboboxSortList()
        {
            return new List<string> { "Nazwa Leku", "Miasto", "Kod Pocztowy", "Ilość" };
        }

        public override void Sort()
        {
            if (SortField == "Nazwa Leku")
                List = new ObservableCollection<WarehouseForAllView>(List.OrderBy(item => item.Nazwa_Leku).ToList());
            else if (SortField == "Miasto")
                List = new ObservableCollection<WarehouseForAllView>(List.OrderBy(item => item.Miasto).ToList());
            else if (SortField == "Kod Pocztowy")
                List = new ObservableCollection<WarehouseForAllView>(List.OrderBy(item => item.Kod_Pocztowy).ToList());
            else if (SortField == "Ilość")
                List = new ObservableCollection<WarehouseForAllView>(List.OrderBy(item => item.Ilość).ToList());
        }

        public override List<string> GetComboboxFindList()
        {
            return new List<string> { "Nazwa Leku", "Miasto" };
        }

        public override void Find()
        {
            Load();
            if (FindField == "Nazwa Leku")
                List = new ObservableCollection<WarehouseForAllView>(List.Where(item => item.Nazwa_Leku != null && item.Nazwa_Leku.StartsWith(FindTextBox, StringComparison.OrdinalIgnoreCase)).ToList());
            else if (FindField == "Miasto")
                List = new ObservableCollection<WarehouseForAllView>(List.Where(item => item.Miasto != null && item.Miasto.StartsWith(FindTextBox, StringComparison.OrdinalIgnoreCase)).ToList());
        }


        #endregion
        #region Helpers
        public override void Load()
        {
            List = new ObservableCollection<WarehouseForAllView>
                (
                    from warehouse in aptekaEntities.Magazyn // dla kazdego magazynu z bazy danych magazyn
                    select new WarehouseForAllView // tworzymy nowy WarehouseForAllView
                    {
                        ID_Magazynu = warehouse.ID_Magazynu,
                        Nazwa_Leku = warehouse.Leki.Nazwa_Leku, // Użycie klucza obcego Leki
                        Ilość = warehouse.Ilość,
                        Ulica = warehouse.Ulica,
                        Miasto = warehouse.Miasto,
                        Kod_Pocztowy = warehouse.Kod_Pocztowy,
                        Telefon = warehouse.Telefon
                    }
                );
        }
        #endregion
    }
}
