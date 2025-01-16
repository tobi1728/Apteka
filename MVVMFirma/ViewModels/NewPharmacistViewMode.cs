using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVVMFirma.ViewModels
{
    public class NewPharmacistViewModel : OneViewModel<Farmaceuci>
    {
        #region Constructor
        public NewPharmacistViewModel()
            : base("Nowy farmaceuta")
        {
            aptekaEntities = new AptekaEntities();
            item = new Farmaceuci();
        }
        #endregion

        #region Properties

        public string Imie
        {
            get
            {
                return item.Imię;
            }
            set
            {
                item.Imię = value;
                OnPropertyChanged(() => Imie);
            }
        }

        public string Nazwisko
        {
            get
            {
                return item.Nazwisko;
            }
            set
            {
                item.Nazwisko = value;
                OnPropertyChanged(() => Nazwisko);
            }
        }

        public string NumerLicencji
        {
            get
            {
                return item.Numer_Licencji;
            }
            set
            {
                item.Numer_Licencji = value;
                OnPropertyChanged(() => NumerLicencji);
            }
        }


        #endregion

        #region Helpers
        public override void Save()
        {
            aptekaEntities.Farmaceuci.Add(item); // Dodanie rekordu do kolekcji lokalnej
            aptekaEntities.SaveChanges(); // Zapisanie zmian do bazy danych
        }
        #endregion
    }
}
