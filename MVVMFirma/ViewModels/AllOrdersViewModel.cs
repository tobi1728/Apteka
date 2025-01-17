using MVVMFirma.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MVVMFirma.ViewModels
{
    public class AllOrdersViewModel : AllViewModel<OrderForAllView>
    {
        #region Constructor
        public AllOrdersViewModel() : base("Wszystkie zamówienia") { }
        #endregion
        #region Sort & Find 
        // tu decydujemy po czym sortowac
        public override List<string> GetComboboxSortList()
        {
            return new List<string> { "Nazwa Dostawcy", "Data Zamówienia", "Status" };
        }

        public override void Sort()
        {
            if (SortField == "Nazwa Dostawcy")
                List = new ObservableCollection<OrderForAllView>(List.OrderBy(item => item.Nazwa_Dostawcy).ToList());
            else if (SortField == "Data Zamówienia")
                List = new ObservableCollection<OrderForAllView>(List.OrderBy(item => item.Data_Zamówienia).ToList());
            else if (SortField == "Status")
                List = new ObservableCollection<OrderForAllView>(List.OrderBy(item => item.Status).ToList());
        }

        public override List<string> GetComboboxFindList()
        {
            return new List<string> { "Nazwa Dostawcy", "Status" };
        }

        public override void Find()
        {
            Load();
            if (FindField == "Nazwa Dostawcy")
                List = new ObservableCollection<OrderForAllView>(List.Where(item => item.Nazwa_Dostawcy != null && item.Nazwa_Dostawcy.StartsWith(FindTextBox, StringComparison.OrdinalIgnoreCase)).ToList());
            else if (FindField == "Status")
                List = new ObservableCollection<OrderForAllView>(List.Where(item => item.Status != null && item.Status.StartsWith(FindTextBox, StringComparison.OrdinalIgnoreCase)).ToList());
        }


        #endregion
        #region Helpers
        public override void Load()
        {
            List = new ObservableCollection<OrderForAllView>
                (
                    from order in aptekaEntities.Zamówienia
                    select new OrderForAllView
                    {
                        ID_Zamówienia = order.ID_Zamówienia,
                        Nazwa_Dostawcy = order.Dostawcy.Nazwa,
                        Data_Zamówienia = order.Data_Zamówienia,
                        Data_Dostawy = order.Data_Dostawy,
                        Status = order.Status
                    }
                );
        }
        #endregion
    }
}
