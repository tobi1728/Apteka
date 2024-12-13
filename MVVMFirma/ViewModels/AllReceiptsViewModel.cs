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
