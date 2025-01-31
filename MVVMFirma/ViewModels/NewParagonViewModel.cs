using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using MVVMFirma.Validators;
using System;
using System.ComponentModel;
using System.Windows.Input;

namespace MVVMFirma.ViewModels
{
    public class NewParagonViewModel : OneViewModel<Paragony>, IDataErrorInfo
    {
        private AptekaEntities aptekaEntities;

        public NewParagonViewModel()
            : base("Nowy paragon")
        {
            aptekaEntities = new AptekaEntities();
            item = new Paragony();
            DataWystawienia = DateTime.Today;

            // Rejestr Messenger do odbioru SaleForAllView
            Messenger.Default.Register<SaleForAllView>(this, getSelectedSale);
        }

        // --- Komenda otwierająca listę sprzedaży w trybie modalnym ---
        private ICommand _showSales;
        public ICommand ShowSales
        {
            get
            {
                if (_showSales == null)
                    _showSales = new BaseCommand(() => Messenger.Default.Send("ShowSales"));
                return _showSales;
            }
        }

        // --- Odbiór wybranej sprzedaży ---
        private void getSelectedSale(SaleForAllView sale)
        {
            if (sale != null)
            {
                IDSprzedazy = sale.ID_Sprzedaży;
                // np. wyświetlamy datę + kwotę
                SaleInfo = $"Sprz. {sale.Data_Sprzedaży:d}, kwota = {sale.Kwota}";
            }
        }

        // --- Zamiast ComboBox, mamy SaleInfo do wyświetlenia
        private string _saleInfo;
        public string SaleInfo
        {
            get => _saleInfo;
            set
            {
                _saleInfo = value;
                OnPropertyChanged(() => SaleInfo);
            }
        }

        // Klucz obcy w bazie
        public int IDSprzedazy
        {
            get => item.ID_Sprzedaży;
            set
            {
                item.ID_Sprzedaży = value;
                OnPropertyChanged(() => IDSprzedazy);
            }
        }

        // Numer paragonu
        public string NumerParagonu
        {
            get => item.Numer_Paragonu;
            set
            {
                item.Numer_Paragonu = value;
                OnPropertyChanged(() => NumerParagonu);
            }
        }

        // Data wystawienia
        public DateTime DataWystawienia
        {
            get => item.Data_Wystawienia;
            set
            {
                item.Data_Wystawienia = value;
                OnPropertyChanged(() => DataWystawienia);
            }
        }

        // Kwota
        public decimal Kwota
        {
            get => item.Kwota;
            set
            {
                item.Kwota = value;
                OnPropertyChanged(() => Kwota);
            }
        }

        // Walidacja
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

        public override void Save()
        {
            if (IsValid())
            {
                aptekaEntities.Paragony.Add(item);
                aptekaEntities.SaveChanges();
                ShowMessageBox("Zapisano nowy paragon!");
            }
            else
            {
                ShowMessageBox("Popraw błędy w formularzu");
            }
        }
    }
}
