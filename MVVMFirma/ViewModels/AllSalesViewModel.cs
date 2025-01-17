using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MVVMFirma.ViewModels
{
    public class AllSalesViewModel : AllViewModel<SaleForAllView>
    {
        #region Constructor
        public AllSalesViewModel()
            : base("Wszystkie sprzedaże")
        {
        }
        #endregion
        #region Sort & Find 
        // tu decydujemy po czym sortowac
        public override List<string> GetComboboxSortList()
        {
            return new List<string> { "Nazwa Leku", "Data Sprzedaży", "Kwota", "Forma Płatności" };
        }

        public override void Sort()
        {
            if (SortField == "Nazwa Leku")
                List = new ObservableCollection<SaleForAllView>(List.OrderBy(item => item.Nazwa_Leku).ToList());
            else if (SortField == "Data Sprzedaży")
                List = new ObservableCollection<SaleForAllView>(List.OrderBy(item => item.Data_Sprzedaży).ToList());
            else if (SortField == "Kwota")
                List = new ObservableCollection<SaleForAllView>(List.OrderBy(item => item.Kwota).ToList());
            else if (SortField == "Forma Płatności")
                List = new ObservableCollection<SaleForAllView>(List.OrderBy(item => item.Forma_Płatności).ToList());
        }

        public override List<string> GetComboboxFindList()
        {
            return new List<string> { "Nazwa Leku", "Forma Płatności" };
        }

        public override void Find()
        {
            Load();
            if (FindField == "Nazwa Leku")
                List = new ObservableCollection<SaleForAllView>(List.Where(item => item.Nazwa_Leku != null && item.Nazwa_Leku.StartsWith(FindTextBox, StringComparison.OrdinalIgnoreCase)).ToList());
            else if (FindField == "Forma Płatności")
                List = new ObservableCollection<SaleForAllView>(List.Where(item => item.Forma_Płatności != null && item.Forma_Płatności.StartsWith(FindTextBox, StringComparison.OrdinalIgnoreCase)).ToList());
        }


        #endregion
        #region Helpers
        public override void Load()
        {
            List = new ObservableCollection<SaleForAllView>
            (
                from sale in aptekaEntities.Sprzedaż
                select new SaleForAllView
                {
                    ID_Sprzedaży = sale.ID_Sprzedaży,
                    Nazwa_Leku = sale.Leki.Nazwa_Leku, // Użycie klucza obcego z tabeli Leki
                    PESEL_Pacjenta = sale.Pacjenci != null ? sale.Pacjenci.PESEL : "Brak pacjenta", // Obsługa wartości null
                    Data_Sprzedaży = sale.Data_Sprzedaży,
                    Kwota = sale.Kwota,
                    Forma_Płatności = sale.Forma_Płatności
                }
            );
        }
        #endregion
    }
}
