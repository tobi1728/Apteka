using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using MVVMFirma.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace MVVMFirma.ViewModels
{
    public class NewPharmacistViewModel : OneViewModel<Farmaceuci>, IDataErrorInfo
    {
        #region Constructor
        public NewPharmacistViewModel()
            : base("Nowy farmaceuta")
        {
            aptekaEntities = new AptekaEntities();
            item = new Farmaceuci();
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
                    case nameof(Imie):
                        _validationMessage = ValueValidator.ValidateString(Imie, 2);
                        break;
                    case nameof(Nazwisko):
                        _validationMessage = ValueValidator.ValidateString(Nazwisko, 2);
                        break;
                    case nameof(NumerLicencji):
                        _validationMessage = ValueValidator.ValidateLicenseNumber(NumerLicencji);
                        break;

                }
                return _validationMessage;
            }
        }

        public override bool IsValid()
        {
            return string.IsNullOrEmpty(this[nameof(Imie)]) &&
                   string.IsNullOrEmpty(this[nameof(Nazwisko)]) &&
                   string.IsNullOrEmpty(this[nameof(NumerLicencji)]);

        }

        public string Error => string.Empty;
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
            if (IsValid())
            {
                aptekaEntities.Farmaceuci.Add(item);
                aptekaEntities.SaveChanges();
            }
            else
            {
                ShowMessageBox("Popraw błędy w formularzu");
            }
        }

        #endregion
    }
}
