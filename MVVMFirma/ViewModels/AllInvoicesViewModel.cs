using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MVVMFirma.ViewModels
{
    public class AllInvoicesViewModel : AllViewModel<Faktury_Dostawców>
    {

        #region Constructor
        public AllInvoicesViewModel()
            :base("Faktury dostawcow")
        {
        }
        #endregion

        #region Helpers
        // metoda load pobierze wszystkie produkty z bazy danych
        public override void Load()
        {
            List = new ObservableCollection<Faktury_Dostawców>
                (
                    aptekaEntities.Faktury_Dostawców.ToList()
                // z bazdy danych pobieram Faktury_Dostawców i zamieniam wszystkie rekordy na liste
                );
        }
        #endregion

    }
}