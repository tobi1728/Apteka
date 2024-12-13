using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

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
                        //Numer_Zamówienia = invoice.Zamówienia != null ? invoice.Zamówienia.Numer_Zamówienia : "Brak"
                    }
                );
        }
        #endregion
    }
}
