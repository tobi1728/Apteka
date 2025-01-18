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
    public class NewSaleViewModel : OneViewModel<Sprzedaż>, IDataErrorInfo
    {
        #region Constructor
        public NewSaleViewModel()
            : base("Nowa sprzedaż")
        {
            aptekaEntities = new AptekaEntities();
            item = new Sprzedaż();

            DataSprzedazy = DateTime.Today;
            LoadLeki();
            LoadPacjenci();
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
                    case nameof(IDLeku):
                        _validationMessage = ValueValidator.ValidateSelection(IDLeku);
                        break;
                    case nameof(IDPacjenta):
                        _validationMessage = ValueValidator.ValidateOptionalSelection(IDPacjenta);
                        break;
                    case nameof(DataSprzedazy):
                        _validationMessage = ValueValidator.ValidatePastOrTodayDate(DataSprzedazy);
                        break;
                    case nameof(Kwota):
                        _validationMessage = ValueValidator.ValidatePositiveDecimal(Kwota);
                        break;
                    case nameof(FormaPlatnosci):
                        _validationMessage = ValueValidator.ValidateString(FormaPlatnosci, 3);
                        break;
                }
                return _validationMessage;
            }
        }

        public override bool IsValid()
        {
            return string.IsNullOrEmpty(this[nameof(IDLeku)]) &&
                   string.IsNullOrEmpty(this[nameof(IDPacjenta)]) &&
                   string.IsNullOrEmpty(this[nameof(DataSprzedazy)]) &&
                   string.IsNullOrEmpty(this[nameof(Kwota)]) &&
                   string.IsNullOrEmpty(this[nameof(FormaPlatnosci)]);
        }

        public string Error => string.Empty;
        #endregion

        #region Properties

        private List<Leki> _Leki;
        public List<Leki> Leki
        {
            get => _Leki;
            set
            {
                _Leki = value;
                OnPropertyChanged(() => Leki);
            }
        }

        private List<Pacjenci> _Pacjenci;
        public List<Pacjenci> Pacjenci
        {
            get => _Pacjenci;
            set
            {
                _Pacjenci = value;
                OnPropertyChanged(() => Pacjenci);
            }
        }

        public int IDLeku
        {
            get => item.ID_Leku;
            set
            {
                item.ID_Leku = value;
                OnPropertyChanged(() => IDLeku);
            }
        }

        public int? IDPacjenta
        {
            get => item.ID_Pacjenta;
            set
            {
                item.ID_Pacjenta = value;
                OnPropertyChanged(() => IDPacjenta);
            }
        }

        public DateTime DataSprzedazy
        {
            get => item.Data_Sprzedaży;
            set
            {
                item.Data_Sprzedaży = value;
                OnPropertyChanged(() => DataSprzedazy);
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

        public string FormaPlatnosci
        {
            get => item.Forma_Płatności;
            set
            {
                item.Forma_Płatności = value;
                OnPropertyChanged(() => FormaPlatnosci);
            }
        }

        #endregion

        #region Helpers
        public void LoadLeki()
        {
            Leki = aptekaEntities.Leki.ToList();
        }

        public void LoadPacjenci()
        {
            Pacjenci = aptekaEntities.Pacjenci.ToList();
        }

        public override void Save()
        {
            if (IsValid())
            {
                aptekaEntities.Sprzedaż.Add(item);
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
