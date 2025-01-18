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
    public class NewWarehouseViewModel : OneViewModel<Magazyn>, IDataErrorInfo
    {
        #region Constructor
        public NewWarehouseViewModel()
            : base("Nowy Magazyn")
        {
            aptekaEntities = new AptekaEntities();
            item = new Magazyn();

            LoadProducts();
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
                    case nameof(Quantity):
                        _validationMessage = ValueValidator.ValidatePositiveInteger(Quantity);
                        break;
                    case nameof(Street):
                        _validationMessage = ValueValidator.ValidateString(Street, 3);
                        break;
                    case nameof(City):
                        _validationMessage = ValueValidator.ValidateString(City, 3);
                        break;
                    case nameof(PostalCode):
                        _validationMessage = ValueValidator.ValidatePostalCode(PostalCode);
                        break;
                    case nameof(Phone):
                        _validationMessage = ValueValidator.ValidatePhoneNumber(Phone);
                        break;
                }
                return _validationMessage;
            }
        }

        public override bool IsValid()
        {
            return string.IsNullOrEmpty(this[nameof(IDLeku)]) &&
                   string.IsNullOrEmpty(this[nameof(Quantity)]) &&
                   string.IsNullOrEmpty(this[nameof(Street)]) &&
                   string.IsNullOrEmpty(this[nameof(City)]) &&
                   string.IsNullOrEmpty(this[nameof(PostalCode)]) &&
                   string.IsNullOrEmpty(this[nameof(Phone)]);
        }

        public string Error => string.Empty;
        #endregion

        #region Properties

        // Lista leków (klucz obcy)
        private List<Leki> _Products;
        public List<Leki> Products
        {
            get => _Products;
            set
            {
                _Products = value;
                OnPropertyChanged(() => Products);
            }
        }

        // Nazwa leku (klucz obcy)
        public int IDLeku
        {
            get
            {
                return item.ID_Leku;
            }
            set
            {
                item.ID_Leku = value;
                OnPropertyChanged(() => IDLeku);
            }
        }

        public int Quantity
        {
            get
            {
                return item.Ilość;
            }
            set
            {
                item.Ilość = value;
                OnPropertyChanged(() => Quantity);
            }
        }

        public string Street
        {
            get
            {
                return item.Ulica;
            }
            set
            {
                item.Ulica = value;
                OnPropertyChanged(() => Street);
            }
        }

        public string City
        {
            get
            {
                return item.Miasto;
            }
            set
            {
                item.Miasto = value;
                OnPropertyChanged(() => City);
            }
        }

        public string PostalCode
        {
            get
            {
                return item.Kod_Pocztowy;
            }
            set
            {
                item.Kod_Pocztowy = value;
                OnPropertyChanged(() => PostalCode);
            }
        }

        public string Phone
        {
            get
            {
                return item.Telefon;
            }
            set
            {
                item.Telefon = value;
                OnPropertyChanged(() => Phone);
            }
        }

        #endregion

        #region Helpers
        public void LoadProducts()
        {
            Products = aptekaEntities.Leki.ToList();
        }

        public override void Save()
        {
            if (IsValid())
            {
                aptekaEntities.Magazyn.Add(item);
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
