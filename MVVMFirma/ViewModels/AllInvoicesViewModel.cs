using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace MVVMFirma.ViewModels
{
    public class AllInvoicesViewModel : AllViewModel<InvoiceForAllView>
    {
        #region Constructor
        public AllInvoicesViewModel()
            : base("Wszystkie faktury dostawców")
        {
        }
        #endregion

        #region Sort & Find 
        // tu decydujemy po czym sortowac
        public override List<string> GetComboboxSortList()
        {
            return new List<string> { "Numer Faktury", "Nazwa Dostawcy", "Data Wystawienia", "Kwota" };
        }

        public override void Sort()
        {
            if (SortField == "Numer Faktury")
            {
                List = new ObservableCollection<InvoiceForAllView>(
                    List.OrderBy(item => item.Numer_Faktury).ToList()
                );
            }
            else if (SortField == "Nazwa Dostawcy")
            {
                List = new ObservableCollection<InvoiceForAllView>(
                    List.OrderBy(item => item.Nazwa_Dostawcy).ToList()
                );
            }
            else if (SortField == "Data Wystawienia")
            {
                List = new ObservableCollection<InvoiceForAllView>(
                    List.OrderBy(item => item.Data_Wystawienia).ToList()
                );
            }
            else if (SortField == "Kwota")
            {
                List = new ObservableCollection<InvoiceForAllView>(
                    List.OrderBy(item => item.Kwota).ToList()
                );
            }
        }

        // tu decydujemy po czym wyszukiwac
        public override List<string> GetComboboxFindList()
        {
            return new List<string> { "Numer Faktury", "Nazwa Dostawcy" };
        }

        public override void Find()
        {
            Load(); // Przywracamy pełną listę przed wyszukiwaniem
            if (FindField == "Numer Faktury")
            {
                List = new ObservableCollection<InvoiceForAllView>(
                    List.Where(item => item.Numer_Faktury != null && item.Numer_Faktury.StartsWith(FindTextBox, StringComparison.OrdinalIgnoreCase)).ToList()
                );
            }
            else if (FindField == "Nazwa Dostawcy")
            {
                List = new ObservableCollection<InvoiceForAllView>(
                    List.Where(item => item.Nazwa_Dostawcy != null && item.Nazwa_Dostawcy.StartsWith(FindTextBox, StringComparison.OrdinalIgnoreCase)).ToList()
                );
            }
        }

        #endregion

        #region Helpers
        public override void Load()
        {
            List = new ObservableCollection<InvoiceForAllView>
                (
                    from invoice in aptekaEntities.Faktury_Dostawców
                    select new InvoiceForAllView
                    {
                        ID_Faktury = invoice.ID_Faktury,
                        Numer_Faktury = invoice.Numer_Faktury,
                        Nazwa_Dostawcy = invoice.Dostawcy.Nazwa,
                        Data_Wystawienia = invoice.Data_Wystawienia,
                        Kwota = invoice.Kwota,
                        Numer_Zamówienia = invoice.Zamówienia.ID_Zamówienia.ToString(),
                    }
                );
        }
        #endregion
    }
}
