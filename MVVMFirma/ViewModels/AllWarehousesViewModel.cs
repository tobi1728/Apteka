using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
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
