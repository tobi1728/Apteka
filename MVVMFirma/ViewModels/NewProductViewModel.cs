using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using MVVMFirma.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace MVVMFirma.ViewModels
{
    public class NewProductViewModel : OneViewModel<Leki>, IDataErrorInfo
    {

        #region Constructor
        public NewProductViewModel()
            :base("Nowy lek")
        {
            aptekaEntities = new AptekaEntities();
            item = new Leki();

            DataWaznosci = DateTime.Today;

            LoadKategorie();
            LoadProducenci();
            Messenger.Default.Register<ProducerForAllView>(this, getSelectedProducer);

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
                    case nameof(Nazwa):
                        _validationMessage = ValueValidator.ValidateString(Nazwa, 3);
                        break;
                    case nameof(IDKategorii):
                        _validationMessage = ValueValidator.ValidateSelection(IDKategorii);
                        break;
                    case nameof(IDProducenta):
                        _validationMessage = ValueValidator.ValidateSelection(IDProducenta);
                        break;
                    case nameof(CenaZakupu):
                        _validationMessage = ValueValidator.ValidatePositiveDecimal(CenaZakupu);
                        break;
                    case nameof(CenaSprzedazy):
                        _validationMessage = ValueValidator.ValidatePositiveDecimal(CenaSprzedazy);
                        break;
                    case nameof(DataWaznosci):
                        _validationMessage = ValueValidator.ValidateFutureDate(DataWaznosci);
                        break;
                }
                return _validationMessage;
            }
        }

        public override bool IsValid()
        {
            return string.IsNullOrEmpty(this[nameof(Nazwa)]) &&
                   string.IsNullOrEmpty(this[nameof(IDKategorii)]) &&
                   string.IsNullOrEmpty(this[nameof(IDProducenta)]) &&
                   string.IsNullOrEmpty(this[nameof(CenaZakupu)]) &&
                   string.IsNullOrEmpty(this[nameof(CenaSprzedazy)]) &&
                   string.IsNullOrEmpty(this[nameof(DataWaznosci)]);
        }

        public string Error => string.Empty;
        #endregion

        #region Properties

        public ICommand ShowProducers
        {
            get
            {
                return new BaseCommand(() => Messenger.Default.Send("Wszyscy producenci"));
            }
        }

        void getSelectedProducer(ProducerForAllView producer)
        {
            if (producer != null)
            {
                IDProducenta = producer.ID_Producenta;
                ProducerName = producer.Nazwa_Producenta;

            }
        }

        private string _ProducerName;
        public string ProducerName
        {
            get => _ProducerName;
            set
            {
                _ProducerName = value;
            }
        }




        // Lista kategorii leków
        private List<Kategorie_Leków> _KategorieLekow;
        public List<Kategorie_Leków> KategorieLekow
        {
            get => _KategorieLekow;
            set
            {
                _KategorieLekow = value;
                OnPropertyChanged(() => KategorieLekow);
            }
        }

        // Lista producentów
        private List<Producent_Leków> _Producenci;
        public List<Producent_Leków> Producenci
        {
            get => _Producenci;
            set
            {
                _Producenci = value;
                OnPropertyChanged(() => Producenci);
            }
        }

        public string Nazwa
        {
            get
            {
                return item.Nazwa_Leku;
            }
            set
            {
                item.Nazwa_Leku = value;
                OnPropertyChanged(() => Nazwa);
            }
        }

        // ID Kategorii
        public int IDKategorii
        {
            get
            {
                return item.ID_Kategorii;
            }
            set
            {
                item.ID_Kategorii = value;
                OnPropertyChanged(() => IDKategorii);
            }
        }

        // ID Producenta
        private int _IDProducenta;
        public int IDProducenta
        {
            get => _IDProducenta;
            set
            {
                _IDProducenta = value;
            }
        }

        // Cena zakupu
        public decimal CenaZakupu
        {
            get
            {
                return item.Cena_Zakupu;
            }
            set
            {
                item.Cena_Zakupu = value;
                OnPropertyChanged(() => CenaZakupu);
            }
        }

        // Cena sprzedaży
        public decimal CenaSprzedazy
        {
            get
            {
                return item.Cena_Sprzedaży;
            }
            set
            {
                item.Cena_Sprzedaży = value;
                OnPropertyChanged(() => CenaSprzedazy);
            }
        }

        // Data ważności
        public DateTime DataWaznosci
        {
            get
            {
                return item.Data_Waznosci;
            }
            set
            {
                item.Data_Waznosci = value;
                OnPropertyChanged(() => DataWaznosci);
            }
        }

        // Czy wymaga recepty
        public bool Recepta
        {
            get
            {
                return item.Na_Recepte;
            }
            set
            {
                item.Na_Recepte = value;
                OnPropertyChanged(() => Recepta);
            }
        }

        // Czy lek jest refundowany
        public bool Refundacja
        {
            get
            {
                return item.Refundacja;
            }
            set
            {
                item.Refundacja = value;
                OnPropertyChanged(() => Refundacja);
            }
        }

        // Opis leku
        public string Opis
        {
            get
            {
                return item.Opis;
            }
            set
            {
                item.Opis = value;
                OnPropertyChanged(() => Opis);
            }
        }

        #endregion

        #region Helpers
        public void LoadKategorie()
        {
            KategorieLekow = aptekaEntities.Kategorie_Leków.ToList();
        }

        public void LoadProducenci()
        {
            Producenci = aptekaEntities.Producent_Leków.ToList();
        }
        public override void Save()
        {
            if (IsValid())
            {
                item.ID_Producenta = IDProducenta;
                item.ID_Kategorii = IDKategorii;

                aptekaEntities.Leki.Add(item);
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
