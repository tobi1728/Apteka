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
    public class NewSalesReportViewModel : OneViewModel<Raporty_Sprzedaży>, IDataErrorInfo
    {
        #region Constructor
        public NewSalesReportViewModel()
            : base("Nowy raport sprzedaży")
        {
            aptekaEntities = new AptekaEntities();
            item = new Raporty_Sprzedaży();
            DataRozpoczecia = DateTime.Today;
            DataZakonczenia = DateTime.Today;
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
                    case nameof(DataRozpoczecia):
                        _validationMessage = ValueValidator.ValidatePastOrTodayDate(DataRozpoczecia);
                        break;
                    case nameof(DataZakonczenia):
                        _validationMessage = ValueValidator.ValidateEndAfterStartDate(DataRozpoczecia, DataZakonczenia);
                        break;
                    case nameof(LacznaSprzedaz):
                        _validationMessage = ValueValidator.ValidatePositiveDecimal(LacznaSprzedaz);
                        break;
                    case nameof(LiczbaTransakcji):
                        _validationMessage = ValueValidator.ValidateNonNegativeInteger(LiczbaTransakcji);
                        break;
                }
                return _validationMessage;
            }
        }

        public override bool IsValid()
        {
            return string.IsNullOrEmpty(this[nameof(DataRozpoczecia)]) &&
                   string.IsNullOrEmpty(this[nameof(DataZakonczenia)]) &&
                   string.IsNullOrEmpty(this[nameof(LacznaSprzedaz)]) &&
                   string.IsNullOrEmpty(this[nameof(LiczbaTransakcji)]);
        }

        public string Error => string.Empty;
        #endregion

        #region Properties

        public DateTime DataRozpoczecia
        {
            get => item.Data_Rozpoczęcia;
            set
            {
                item.Data_Rozpoczęcia = value;
                OnPropertyChanged(() => DataRozpoczecia);
            }
        }

        public DateTime DataZakonczenia
        {
            get => item.Data_Zakończenia;
            set
            {
                item.Data_Zakończenia = value;
                OnPropertyChanged(() => DataZakonczenia);
            }
        }

        public decimal LacznaSprzedaz
        {
            get => item.Łączna_Sprzedaż;
            set
            {
                item.Łączna_Sprzedaż = value;
                OnPropertyChanged(() => LacznaSprzedaz);
            }
        }

        public int LiczbaTransakcji
        {
            get => item.Liczba_Transakcji;
            set
            {
                item.Liczba_Transakcji = value;
                OnPropertyChanged(() => LiczbaTransakcji);
            }
        }

        #endregion

        #region Helpers
        public override void Save()
        {
            if (IsValid())
            {
                aptekaEntities.Raporty_Sprzedaży.Add(item);
                aptekaEntities.SaveChanges();
            }
            else
            {
                ShowMessageBox("Popraw błędy w formularzu.");
            }
        }
        #endregion
    }
}
