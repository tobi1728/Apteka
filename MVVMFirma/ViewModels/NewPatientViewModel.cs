using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace MVVMFirma.ViewModels
{
    public class NewPatientViewModel : OneViewModel<Pacjenci>
    {
        #region Constructor
        public NewPatientViewModel()
            : base("Nowy Pacjent")
        {
            aptekaEntities = new AptekaEntities();
            item = new Pacjenci();

            DataUrodzenia = DateTime.Today; // Ustaw aktualną datę jako domyślną datę urodzenia
        }
        #endregion

        #region Properties

        public string Imię
        {
            get
            {
                return item.Imię;
            }
            set
            {
                item.Imię = value;
                OnPropertyChanged(() => Imię);
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

        public DateTime DataUrodzenia
        {
            get
            {
                return item.Data_Urodzenia;
            }
            set
            {
                item.Data_Urodzenia = value;
                OnPropertyChanged(() => DataUrodzenia);
            }
        }

        public string PESEL
        {
            get
            {
                return item.PESEL;
            }
            set
            {
                item.PESEL = value;
                OnPropertyChanged(() => PESEL);
            }
        }

        #endregion

        #region Helpers
        public override void Save()
        {
            if (string.IsNullOrWhiteSpace(Imię) || string.IsNullOrWhiteSpace(Nazwisko) || string.IsNullOrWhiteSpace(PESEL))
            {
                throw new InvalidOperationException("Imię, Nazwisko i PESEL muszą być wypełnione.");
            }

            aptekaEntities.Pacjenci.Add(item); // Dodaje do lokalnej kolekcji
            aptekaEntities.SaveChanges(); // Zapisuje zmiany do bazy danych
        }
        #endregion
    }
}
