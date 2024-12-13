using MVVMFirma.Models.EntitiesForView;
using System.Collections.ObjectModel;
using System.Linq;

namespace MVVMFirma.ViewModels
{
    public class AllOrdersViewModel : AllViewModel<OrderForAllView>
    {
        #region Constructor
        public AllOrdersViewModel() : base("Wszystkie zamówienia") { }
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
