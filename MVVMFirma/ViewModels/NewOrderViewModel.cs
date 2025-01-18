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
    public class NewOrderViewModel : OneViewModel<Zamówienia>, IDataErrorInfo
    {
        #region Constructor
        public NewOrderViewModel()
            : base("Nowe zamówienie")
        {
            aptekaEntities = new AptekaEntities();
            item = new Zamówienia();

            DataZamowienia = DateTime.Today;
            DataDostawy = DateTime.Today;
            LoadDostawcy();
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
                    case nameof(IDDostawcy):
                        _validationMessage = ValueValidator.ValidateSelection(IDDostawcy);
                        break;
                    case nameof(DataZamowienia):
                        _validationMessage = ValueValidator.ValidatePastOrTodayDate(DataZamowienia);
                        break;
                    case nameof(DataDostawy):
                        _validationMessage = ValueValidator.ValidateEndAfterStartDate(DataZamowienia, DataDostawy);
                        break;
                    case nameof(Status):
                        _validationMessage = ValueValidator.ValidateString(Status, 3);
                        break;
                }
                return _validationMessage;
            }
        }

        public override bool IsValid()
        {
            return string.IsNullOrEmpty(this[nameof(IDDostawcy)]) &&
                   string.IsNullOrEmpty(this[nameof(DataZamowienia)]) &&
                   string.IsNullOrEmpty(this[nameof(DataDostawy)]) &&
                   string.IsNullOrEmpty(this[nameof(Status)]);
        }

        public string Error => string.Empty;
        #endregion

        #region Properties

        private List<Dostawcy> _Dostawcy;
        public List<Dostawcy> Dostawcy
        {
            get => _Dostawcy;
            set
            {
                _Dostawcy = value;
                OnPropertyChanged(() => Dostawcy);
            }
        }

        public int IDDostawcy
        {
            get => item.ID_Dostawcy;
            set
            {
                item.ID_Dostawcy = value;
                OnPropertyChanged(() => IDDostawcy);
            }
        }

        public DateTime DataZamowienia
        {
            get => item.Data_Zamówienia;
            set
            {
                item.Data_Zamówienia = value;
                OnPropertyChanged(() => DataZamowienia);
            }
        }

        public DateTime DataDostawy
        {
            get => item.Data_Dostawy;
            set
            {
                item.Data_Dostawy = value;
                OnPropertyChanged(() => DataDostawy);
            }
        }

        public string Status
        {
            get => item.Status;
            set
            {
                item.Status = value;
                OnPropertyChanged(() => Status);
            }
        }

        #endregion

        #region Helpers
        public void LoadDostawcy()
        {
            Dostawcy = aptekaEntities.Dostawcy.ToList();
        }

        public override void Save()
        {
            if (IsValid())
            {
                aptekaEntities.Zamówienia.Add(item);
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
