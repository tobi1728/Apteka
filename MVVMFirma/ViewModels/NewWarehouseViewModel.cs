using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using MVVMFirma.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Input;

namespace MVVMFirma.ViewModels
{
    public class NewWarehouseViewModel : OneViewModel<Magazyn>, IDataErrorInfo
    {
        private AptekaEntities aptekaEntities;

        public NewWarehouseViewModel()
            : base("Nowy Magazyn")
        {
            aptekaEntities = new AptekaEntities();
            item = new Magazyn();

            // Domyślnie:
            Quantity = 1; // np. domyślna ilość

            // Zamiast LoadProducts – rejestrujemy nasłuch na ProductForAllView
            Messenger.Default.Register<ProductForAllView>(this, getSelectedProduct);
        }

        // --- Komenda ShowProducts ---
        private ICommand _showProducts;
        public ICommand ShowProducts
        {
            get
            {
                if (_showProducts == null)
                {
                    _showProducts = new BaseCommand(() => Messenger.Default.Send("ShowProducts"));
                }
                return _showProducts;
            }
        }

        // Odbiór wybranego produktu (leka)
        private void getSelectedProduct(ProductForAllView product)
        {
            if (product != null)
            {
                // Ustawiamy ID leku w tabeli Magazyn
                IDLeku = product.ID_Leku;
                // Wyświetlamy w polu 'ProductName'
                ProductName = product.Nazwa_Leku;
            }
        }

        // --- Właściwość do wyświetlenia nazwy leku w polu ---
        private string _productName;
        public string ProductName
        {
            get => _productName;
            set
            {
                _productName = value;
                OnPropertyChanged(() => ProductName);
            }
        }

        // Klucz obcy do Leki
        public int IDLeku
        {
            get => item.ID_Leku;
            set
            {
                item.ID_Leku = value;
                OnPropertyChanged(() => IDLeku);
            }
        }

        // Ilość
        public int Quantity
        {
            get => item.Ilość;
            set
            {
                item.Ilość = value;
                OnPropertyChanged(() => Quantity);
            }
        }

        // Adres, telefon, etc.
        public string Street
        {
            get => item.Ulica;
            set
            {
                item.Ulica = value;
                OnPropertyChanged(() => Street);
            }
        }

        public string City
        {
            get => item.Miasto;
            set
            {
                item.Miasto = value;
                OnPropertyChanged(() => City);
            }
        }

        public string PostalCode
        {
            get => item.Kod_Pocztowy;
            set
            {
                item.Kod_Pocztowy = value;
                OnPropertyChanged(() => PostalCode);
            }
        }

        public string Phone
        {
            get => item.Telefon;
            set
            {
                item.Telefon = value;
                OnPropertyChanged(() => Phone);
            }
        }

        public override void Save()
        {
            if (IsValid())
            {
                aptekaEntities.Magazyn.Add(item);
                aptekaEntities.SaveChanges();
                ShowMessageBox("Zapisano nowy magazyn.");
            }
            else
            {
                ShowMessageBox("Popraw błędy w formularzu");
            }
        }

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

    }
}
