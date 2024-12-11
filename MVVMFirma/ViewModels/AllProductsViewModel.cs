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
    public class AllProductsViewModel : AllViewModel<Leki>
    {
        #region Constructor
        public AllProductsViewModel()
            :base("Leki")
        {
        }
        #endregion

        #region Helpers
        // metoda load pobierze wszystkie produkty z bazy danych
        public override void Load()
        {
            List = new ObservableCollection<Leki>
                (
                    aptekaEntities.Leki.ToList()
                // z bazdy danych pobieram Faktury_Dostawców i zamieniam wszystkie rekordy na liste
                );
        }
        #endregion

    }
}