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
    public class NewProducerViewModel : OneViewModel<Producent_Leków>, IDataErrorInfo
    {
        #region Constructor
        public NewProducerViewModel()
            : base("Nowy producent")
        {
            aptekaEntities = new AptekaEntities();
            item = new Producent_Leków();
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
                    case nameof(NazwaProducenta):
                        _validationMessage = ValueValidator.ValidateString(NazwaProducenta, 3);
                        break;
                    case nameof(Telefon):
                        _validationMessage = ValueValidator.ValidatePhoneNumber(Telefon);
                        break;
                    case nameof(Ulica):
                        _validationMessage = ValueValidator.ValidateString(Ulica, 3);
                        break;
                    case nameof(Miasto):
                        _validationMessage = ValueValidator.ValidateString(Miasto, 3);
                        break;
                    case nameof(KodPocztowy):
                        _validationMessage = ValueValidator.ValidatePostalCode(KodPocztowy);
                        break;
                }
                return _validationMessage;
            }
        }

        public override bool IsValid()
        {
            return string.IsNullOrEmpty(this[nameof(NazwaProducenta)]) &&
                   string.IsNullOrEmpty(this[nameof(Telefon)]) &&
                   string.IsNullOrEmpty(this[nameof(Ulica)]) &&
                   string.IsNullOrEmpty(this[nameof(Miasto)]) &&
                   string.IsNullOrEmpty(this[nameof(KodPocztowy)]);
        }

        public string Error => string.Empty;
        #endregion

        #region Properties
        public string NazwaProducenta
        {
            get
            {
                return item.Nazwa_Producenta;
            }
            set
            {
                item.Nazwa_Producenta = value;
                OnPropertyChanged(() => NazwaProducenta);
            }
        }

        public string Telefon
        {
            get
            {
                return item.Telefon;
            }
            set
            {
                item.Telefon = value;
                OnPropertyChanged(() => Telefon);
            }
        }

        public string Ulica
        {
            get
            {
                return item.Ulica;
            }
            set
            {
                item.Ulica = value;
                OnPropertyChanged(() => Ulica);
            }
        }

        public string Miasto
        {
            get
            {
                return item.Miasto;
            }
            set
            {
                item.Miasto = value;
                OnPropertyChanged(() => Miasto);
            }
        }

        public string KodPocztowy
        {
            get
            {
                return item.Kod_Pocztowy;
            }
            set
            {
                item.Kod_Pocztowy = value;
                OnPropertyChanged(() => KodPocztowy);
            }
        }
        #endregion

        #region Helpers
        public override void Save()
        {
            if (IsValid())
            {
                aptekaEntities.Producent_Leków.Add(item);
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
