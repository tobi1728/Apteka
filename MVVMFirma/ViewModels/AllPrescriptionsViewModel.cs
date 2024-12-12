using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System.Collections.ObjectModel;
using System.Linq;

namespace MVVMFirma.ViewModels
{
    public class AllPrescriptionsViewModel : AllViewModel<PrescriptionForAllView>
    {
        public AllPrescriptionsViewModel() : base("Recepty") { }

        public override void Load()
        {
            List = new ObservableCollection<PrescriptionForAllView>
            (
                from recepta in AptekaEntities.Recepty
                select new PrescriptionForAllView
                {
                    ID_Recepty = recepta.ID_Recepty,
                    PatientName = recepta.Pacjenci.Imię + " " + recepta.Pacjenci.Nazwisko,
                    PharmacistName = recepta.Farmaceuci.Imię + " " + recepta.Farmaceuci.Nazwisko,
                    Data_Wystawienia = recepta.Data_Wystawienia,
                    Data_Realizacji = recepta.Data_Realizacji
                }
            );
        }
    }
}
