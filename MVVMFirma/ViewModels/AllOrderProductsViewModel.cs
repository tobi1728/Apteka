using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MVVMFirma.ViewModels
{
    public class AllOrderProductsViewModel : AllViewModel<OrderProductForAllView>
    {
        #region Constructor
        public AllOrderProductsViewModel()
            : base("Produkty zamówienia")
        {
        }
        #endregion

        #region Sort & Find 
        // tu decydujemy po czym sortowac
        public override List<string> GetComboboxSortList()
        {
            return new List<string> { "Nazwa Leku", "ID Zamówienia", "Ilość" };
        }

        public override void Sort()
        {
            if (SortField == "Nazwa Leku")
            {
                List = new ObservableCollection<OrderProductForAllView>(
                    List.OrderBy(item => item.Nazwa_Leku).ToList()
                );
            }
            else if (SortField == "ID Zamówienia")
            {
                List = new ObservableCollection<OrderProductForAllView>(
                    List.OrderBy(item => item.ID_Zamówienia).ToList()
                );
            }
            else if (SortField == "Ilość")
            {
                List = new ObservableCollection<OrderProductForAllView>(
                    List.OrderBy(item => item.Ilość).ToList()
                );
            }
        }

        // tu decydujemy po czym wyszukiwac
        public override List<string> GetComboboxFindList()
        {
            return new List<string> { "Nazwa Leku" };
        }

        public override void Find()
        {
            Load(); // Przywracamy pełną listę przed wyszukiwaniem
            if (FindField == "Nazwa Leku")
            {
                List = new ObservableCollection<OrderProductForAllView>(
                    List.Where(item => item.Nazwa_Leku != null && item.Nazwa_Leku.StartsWith(FindTextBox, System.StringComparison.OrdinalIgnoreCase)).ToList()
                );
           
            }
        }

        #endregion

        #region Helpers
        public override void Load()
        {
            List = new ObservableCollection<OrderProductForAllView>
            (
                from orderProduct in aptekaEntities.Produkty_Zamówienia
                select new OrderProductForAllView
                {
                    ID_Produktu_Zamówienia = orderProduct.ID_Produktu_Zamówienia,
                    ID_Zamówienia = orderProduct.ID_Zamówienia,
                    Nazwa_Leku = orderProduct.Leki.Nazwa_Leku,
                    Ilość = orderProduct.Ilość
                }
            );
        }
        #endregion
    }
}
