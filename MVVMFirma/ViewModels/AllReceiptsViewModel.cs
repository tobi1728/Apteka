using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MVVMFirma.ViewModels
{
    public class AllReceiptsViewModel : AllViewModel<ReceiptForAllView>
    {
        #region Constructor
        public AllReceiptsViewModel()
            : base("Wszystkie paragony")
        {
        }
        #endregion
        #region Sort & Find 
        // tu decydujemy po czym sortowac
        public override List<string> GetComboboxSortList()
        {
            return new List<string> { "Numer Paragonu", "Data Wystawienia", "Kwota" };
        }

        public override void Sort()
        {
            if (SortField == "Numer Paragonu")
                List = new ObservableCollection<ReceiptForAllView>(List.OrderBy(item => item.Numer_Paragonu).ToList());
            else if (SortField == "Data Wystawienia")
                List = new ObservableCollection<ReceiptForAllView>(List.OrderBy(item => item.Data_Wystawienia).ToList());
            else if (SortField == "Kwota")
                List = new ObservableCollection<ReceiptForAllView>(List.OrderBy(item => item.Kwota).ToList());
        }

        public override List<string> GetComboboxFindList()
        {
            return new List<string> { "Numer Paragonu" };
        }

        public override void Find()
        {
            Load();
            if (FindField == "Numer Paragonu")
                List = new ObservableCollection<ReceiptForAllView>(List.Where(item => item.Numer_Paragonu != null && item.Numer_Paragonu.StartsWith(FindTextBox, StringComparison.OrdinalIgnoreCase)).ToList());
        }


        #endregion
        #region Helpers
        public override void Load()
        {
            List = new ObservableCollection<ReceiptForAllView>
                (
                    from receipt in aptekaEntities.Paragony
                    select new ReceiptForAllView
                    {
                        ID_Paragonu = receipt.ID_Paragonu,
                        Numer_Paragonu = receipt.Numer_Paragonu,
                        ID_Sprzedaży = receipt.ID_Sprzedaży,
                        Data_Wystawienia = receipt.Data_Wystawienia,
                        Kwota = receipt.Kwota
                    }
                );
        }
        #endregion
    }
}
