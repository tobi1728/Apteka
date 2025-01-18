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
    public class NewOrderProductViewModel : OneViewModel<Produkty_Zamówienia>, IDataErrorInfo
    {
        #region Constructor
        public NewOrderProductViewModel()
            : base("Nowy produkt zamówienia")
        {
            aptekaEntities = new AptekaEntities();
            item = new Produkty_Zamówienia();
            LoadLeki();
            LoadZamowienia();
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
                    case nameof(ID_Leku):
                        _validationMessage = ValueValidator.ValidateSelection(ID_Leku);
                        break;
                    case nameof(ID_Zamówienia):
                        _validationMessage = ValueValidator.ValidateSelection(ID_Zamówienia);
                        break;
                    case nameof(Ilość):
                        _validationMessage = ValueValidator.ValidatePositiveInteger(Ilość);
                        break;
                }
                return _validationMessage;
            }
        }

        public override bool IsValid()
        {
            return string.IsNullOrEmpty(this[nameof(ID_Leku)]) &&
                   string.IsNullOrEmpty(this[nameof(ID_Zamówienia)]) &&
                   string.IsNullOrEmpty(this[nameof(Ilość)]);
        }

        public string Error => string.Empty;
        #endregion

        #region Properties

        // Lista leków
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

        // Lista zamówień
        private List<Zamówienia> _Zamowienia;
        public List<Zamówienia> Zamowienia
        {
            get => _Zamowienia;
            set
            {
                _Zamowienia = value;
                OnPropertyChanged(() => Zamowienia);
            }
        }

        public int ID_Leku
        {
            get => item.ID_Leku;
            set
            {
                item.ID_Leku = value;
                OnPropertyChanged(() => ID_Leku);
            }
        }

        public int ID_Zamówienia
        {
            get => item.ID_Zamówienia;
            set
            {
                item.ID_Zamówienia = value;
                OnPropertyChanged(() => ID_Zamówienia);
            }
        }

        public int Ilość
        {
            get => item.Ilość;
            set
            {
                item.Ilość = value;
                OnPropertyChanged(() => Ilość);
            }
        }

        #endregion

        #region Helpers
        public void LoadLeki()
        {
            Leki = aptekaEntities.Leki.ToList();
        }

        public void LoadZamowienia()
        {
            Zamowienia = aptekaEntities.Zamówienia.ToList();
        }

        public override void Save()
        {
            if (IsValid())
            {
                aptekaEntities.Produkty_Zamówienia.Add(item);
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
