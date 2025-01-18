using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using MVVMFirma.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace MVVMFirma.ViewModels
{
    public class NewParagonViewModel : OneViewModel<Paragony>, IDataErrorInfo
    {
        #region Constructor
        public NewParagonViewModel()
            : base("Nowy paragon")
        {
            aptekaEntities = new AptekaEntities();
            item = new Paragony();
            DataWystawienia = DateTime.Today;

            LoadSprzedaze();
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
                    case nameof(NumerParagonu):
                        _validationMessage = ValueValidator.ValidateString(NumerParagonu, 3);
                        break;
                    case nameof(IDSprzedazy):
                        _validationMessage = ValueValidator.ValidateSelection(IDSprzedazy);
                        break;
                    case nameof(DataWystawienia):
                        _validationMessage = ValueValidator.ValidatePastOrTodayDate(DataWystawienia);
                        break;
                    case nameof(Kwota):
                        _validationMessage = ValueValidator.ValidatePositiveDecimal(Kwota);
                        break;
                }
                return _validationMessage;
            }
        }

        public override bool IsValid()
        {
            return string.IsNullOrEmpty(this[nameof(NumerParagonu)]) &&
                   string.IsNullOrEmpty(this[nameof(IDSprzedazy)]) &&
                   string.IsNullOrEmpty(this[nameof(DataWystawienia)]) &&
                   string.IsNullOrEmpty(this[nameof(Kwota)]);
        }

        public string Error => string.Empty;
        #endregion

        #region Properties
        private List<Sprzedaż> _Sprzedaze;
        public List<Sprzedaż> Sprzedaze
        {
            get => _Sprzedaze;
            set
            {
                _Sprzedaze = value;
                OnPropertyChanged(() => Sprzedaze);
            }
        }

        public string NumerParagonu
        {
            get => item.Numer_Paragonu;
            set
            {
                item.Numer_Paragonu = value;
                OnPropertyChanged(() => NumerParagonu);
            }
        }

        public int IDSprzedazy
        {
            get => item.ID_Sprzedaży;
            set
            {
                item.ID_Sprzedaży = value;
                OnPropertyChanged(() => IDSprzedazy);
            }
        }

        public DateTime DataWystawienia
        {
            get => item.Data_Wystawienia;
            set
            {
                item.Data_Wystawienia = value;
                OnPropertyChanged(() => DataWystawienia);
            }
        }

        public decimal Kwota
        {
            get => item.Kwota;
            set
            {
                item.Kwota = value;
                OnPropertyChanged(() => Kwota);
            }
        }
        #endregion

        #region Helpers
        public void LoadSprzedaze()
        {
            Sprzedaze = aptekaEntities.Sprzedaż.ToList();
        }

        public override void Save()
        {
            if (IsValid())
            {
                aptekaEntities.Paragony.Add(item);
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
