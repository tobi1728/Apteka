using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
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
    public class AllPrescriptionsViewModel : AllViewModel<PrescriptionForAllView>
    {
        #region Constructor
        public AllPrescriptionsViewModel()
            : base("Wszystkie recepty")
        {
        }
        #endregion

        #region Helpers
        public override void Load()
        {
            List = new ObservableCollection<PrescriptionForAllView>
                (
                    from prescription in aptekaEntities.Recepty // dla kazdej recepty z bazy danych recept
                    select new PrescriptionForAllView // tworzymy nowy PresriptionForAllView
                    {
                        ID_Recepty = prescription.ID_Recepty,
                        PESEL = prescription.Pacjenci.PESEL,
                        Numer_Licencji = prescription.Farmaceuci.Numer_Licencji,
                        Data_Wystawienia = prescription.Data_Wystawienia,
                        Data_Realizacji = prescription.Data_Realizacji                        
                    }

                );
        }
        #endregion

    }
}