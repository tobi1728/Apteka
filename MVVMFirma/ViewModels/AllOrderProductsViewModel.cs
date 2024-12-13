using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
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
