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
