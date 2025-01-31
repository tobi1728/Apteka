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
    public class NewScheduleViewModel : OneViewModel<Grafiki_Pracowników>, IDataErrorInfo
    {
        private AptekaEntities aptekaEntities;

        public  NewScheduleViewModel()
            : base("Nowy grafik")
        {
            aptekaEntities = new AptekaEntities();
            item = new Grafiki_Pracowników();

            Data = DateTime.Today;
            GodzinaRozpoczecia = new TimeSpan(8, 0, 0);
            GodzinaZakonczenia = new TimeSpan(16, 0, 0);

            LoadFarmaceuci();
            // Rejestrujemy nasłuch farmaceuty wybranego w oknie modalnym
            Messenger.Default.Register<PharmacistForAllView>(this, getSelectedPharmacist);
        }

        // Komenda "Wybierz" - otwarcie listy farmaceutów
        private ICommand _showPharmacists;
        public ICommand ShowPharmacists
        {
            get
            {
                if (_showPharmacists == null)
                    _showPharmacists = new BaseCommand(() => Messenger.Default.Send("ShowPharmacists"));
                return _showPharmacists;
            }
        }

        private void getSelectedPharmacist(PharmacistForAllView pharmacist)
        {
            if (pharmacist != null)
            {
                PharmacistName = pharmacist.Imię + " " + pharmacist.Nazwisko;
                IDFarmaceuty = pharmacist.ID_Farmaceuty;
            }
        }

        // Właściwość wyświetlana w polu "Farmaceuta"
        private string _pharmacistName;
        public string PharmacistName
        {
            get => _pharmacistName;
            set
            {
                _pharmacistName = value;
                OnPropertyChanged(() => PharmacistName);
            }
        }

        // Lista farmaceutów z bazy (opcjonalnie)
        private List<Farmaceuci> _farmaceuci;
        public List<Farmaceuci> Farmaceuci
        {
            get => _farmaceuci;
            set
            {
                _farmaceuci = value;
                OnPropertyChanged(() => Farmaceuci);
            }
        }

        public int IDFarmaceuty
        {
            get => item.ID_Farmaceuty;
            set
            {
                item.ID_Farmaceuty = value;
                OnPropertyChanged(() => IDFarmaceuty);
            }
        }

        public DateTime Data
        {
            get => item.Data;
            set
            {
                item.Data = value;
                OnPropertyChanged(() => Data);
            }
        }

        public TimeSpan GodzinaRozpoczecia
        {
            get => item.Godzina_Rozpoczęcia;
            set
            {
                item.Godzina_Rozpoczęcia = value;
                OnPropertyChanged(() => GodzinaRozpoczecia);
            }
        }

        public TimeSpan GodzinaZakonczenia
        {
            get => item.Godzina_Zakończenia;
            set
            {
                item.Godzina_Zakończenia = value;
                OnPropertyChanged(() => GodzinaZakonczenia);
            }
        }

        // Wczytanie listy farmaceutów (opcjonalne)
        public void LoadFarmaceuci()
        {
            Farmaceuci = aptekaEntities.Farmaceuci.ToList();
        }

        public override void Save()
        {
            if (IsValid())
            {
                aptekaEntities.Grafiki_Pracowników.Add(item);
                aptekaEntities.SaveChanges();
                ShowMessageBox("Zapisano grafik pracownika");
            }
            else
            {
                ShowMessageBox("Popraw błędy w formularzu");
            }
        }

        #region Walidacja (IDataErrorInfo)

        public string Error => string.Empty;

        private string _validationMessage = string.Empty;
        public string this[string propertyName]
        {
            get
            {
                _validationMessage = string.Empty;
                switch (propertyName)
                {
                    case nameof(Data):
                        _validationMessage = ValueValidator.ValidateFutureDate(Data);
                        break;
                    case nameof(GodzinaRozpoczecia):
                        _validationMessage = ValueValidator.ValidateTime(GodzinaRozpoczecia);
                        break;
                    case nameof(GodzinaZakonczenia):
                        _validationMessage = ValueValidator.ValidateEndTime(GodzinaRozpoczecia, GodzinaZakonczenia);
                        break;
                    case nameof(IDFarmaceuty):
                        _validationMessage = ValueValidator.ValidatePharmacist(IDFarmaceuty);
                        break;
                }
                return _validationMessage;
            }
        }

        public override bool IsValid()
        {
            return string.IsNullOrEmpty(this[nameof(Data)]) &&
                   string.IsNullOrEmpty(this[nameof(GodzinaRozpoczecia)]) &&
                   string.IsNullOrEmpty(this[nameof(GodzinaZakonczenia)]) &&
                   string.IsNullOrEmpty(this[nameof(IDFarmaceuty)]);
        }

        #endregion
    }
}
