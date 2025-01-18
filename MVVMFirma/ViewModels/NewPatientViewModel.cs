using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using MVVMFirma.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace MVVMFirma.ViewModels
{
    public class NewPatientViewModel : OneViewModel<Pacjenci>, IDataErrorInfo
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
        #region Validation
        private string _validationMessage = string.Empty;

        public string this[string propertyName]
        {
            get
            {
                _validationMessage = string.Empty;
                switch (propertyName)
                {
                    case nameof(Imię):
                        _validationMessage = ValueValidator.ValidateString(Imię, 2);
                        break;
                    case nameof(Nazwisko):
                        _validationMessage = ValueValidator.ValidateString(Nazwisko, 2);
                        break;
                    case nameof(PESEL):
                        _validationMessage = ValueValidator.ValidatePESEL(PESEL);
                        break;
                    case nameof(DataUrodzenia):
                        _validationMessage = ValueValidator.ValidatePastDate(DataUrodzenia);
                        break;
                }
                return _validationMessage;
            }
        }

        public override bool IsValid()
        {
            return string.IsNullOrEmpty(this[nameof(Imię)]) &&
                   string.IsNullOrEmpty(this[nameof(Nazwisko)]) &&
                   string.IsNullOrEmpty(this[nameof(PESEL)]) &&
                   string.IsNullOrEmpty(this[nameof(DataUrodzenia)]);
        }

        public string Error => string.Empty;
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
