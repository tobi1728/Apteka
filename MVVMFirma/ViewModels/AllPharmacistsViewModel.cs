using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMFirma.ViewModels
{
    public class AllPharmacistsViewModel : AllViewModel<PharmacistForAllView>
    {
        #region Constructor
        public AllPharmacistsViewModel()
            : base("Wszyscy Farmaceuci")
        {
        }
        #endregion

        #region Helpers
        public override void Load()
        {
            List = new ObservableCollection<PharmacistForAllView>
            (
                from pharmacist in aptekaEntities.Farmaceuci
                select new PharmacistForAllView
                {
                    ID_Farmaceuty = pharmacist.ID_Farmaceuty,
                    Imię = pharmacist.Imię,
                    Nazwisko = pharmacist.Nazwisko,
                    Numer_Licencji = pharmacist.Numer_Licencji
                }
            );
        }
        #endregion
    }
}
