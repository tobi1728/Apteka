using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MVVMFirma.ViewModels
{
    public class AllPatientsViewModel : AllViewModel<PatientForAllView>
    {
        #region Constructor
        public AllPatientsViewModel()
            : base("Wszyscy pacjenci")
        {
        }
        #endregion

        #region Helpers
        public override void Load()
        {
            List = new ObservableCollection<PatientForAllView>
            (
                from patient in aptekaEntities.Pacjenci
                select new PatientForAllView
                {
                    ID_Pacjenta = patient.ID_Pacjenta,
                    Imię = patient.Imię,
                    Nazwisko = patient.Nazwisko,
                    Data_Urodzenia = patient.Data_Urodzenia,
                    PESEL = patient.PESEL
                }
            );
        }
        #endregion
    }
}
