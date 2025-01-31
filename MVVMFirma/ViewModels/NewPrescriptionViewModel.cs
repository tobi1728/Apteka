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
    public class NewPrescriptionViewModel : OneViewModel<Recepty>, IDataErrorInfo
    {
        private AptekaEntities aptekaEntities;

        public NewPrescriptionViewModel()
            : base("Nowa recepta")
        {
            aptekaEntities = new AptekaEntities();
            item = new Recepty();

            DataWystawienia = DateTime.Today;

            // Rejestr Messengerów – odbiór wybranego pacjenta i farmaceuty
            Messenger.Default.Register<PatientForAllView>(this, getSelectedPatient);
            Messenger.Default.Register<PharmacistForAllView>(this, getSelectedPharmacist);
        }

        // ----------------------- Komendy: ShowPatients, ShowPharmacists -----------------------
        private ICommand _showPatients;
        public ICommand ShowPatients
        {
            get
            {
                if (_showPatients == null)
                    _showPatients = new BaseCommand(() => Messenger.Default.Send("ShowPatients"));
                return _showPatients;
            }
        }

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

        // ----------------------- Odbiór obiektów modalnych -----------------------
        private void getSelectedPatient(PatientForAllView patient)
        {
            if (patient != null)
            {
                IDPacjenta = patient.ID_Pacjenta;
                PatientName = $"{patient.Imię} {patient.Nazwisko}";
            }
        }

        private void getSelectedPharmacist(PharmacistForAllView pharmacist)
        {
            if (pharmacist != null)
            {
                IDFarmaceuty = pharmacist.ID_Farmaceuty;
                PharmacistName = $"{pharmacist.Imię} {pharmacist.Nazwisko}";
            }
        }

        // ----------------------- Pola do wyświetlania wybranych obcych kluczy -----------------------
        private string _patientName;
        public string PatientName
        {
            get => _patientName;
            set
            {
                _patientName = value;
                OnPropertyChanged(() => PatientName);
            }
        }

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

        // ----------------------- Rzeczywiste klucze obce w tabeli Recepty -----------------------
        public int IDPacjenta
        {
            get => item.ID_Pacjenta;
            set
            {
                item.ID_Pacjenta = value;
                OnPropertyChanged(() => IDPacjenta);
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

        // ----------------------- Daty: wystawienia, realizacji -----------------------
        public DateTime DataWystawienia
        {
            get => item.Data_Wystawienia;
            set
            {
                item.Data_Wystawienia = value;
                OnPropertyChanged(() => DataWystawienia);
            }
        }

        public DateTime? DataRealizacji
        {
            get => item.Data_Realizacji;
            set
            {
                item.Data_Realizacji = value;
                OnPropertyChanged(() => DataRealizacji);
            }
        }

        // ----------------------- Walidacja i Save -----------------------
        private string _validationMessage = string.Empty;
        public string this[string propertyName]
        {
            get
            {
                _validationMessage = string.Empty;
                switch (propertyName)
                {
                    case nameof(IDPacjenta):
                        _validationMessage = ValueValidator.ValidateSelection(IDPacjenta);
                        break;
                    case nameof(IDFarmaceuty):
                        _validationMessage = ValueValidator.ValidateSelection(IDFarmaceuty);
                        break;
                    case nameof(DataWystawienia):
                        // Walidacja np. DataWystawienia nie może być z przyszłości:
                        _validationMessage = ValueValidator.ValidateNotToday(DataWystawienia);
                        break;
                    case nameof(DataRealizacji):
                        // Walidacja DataRealizacji >= DataWystawienia
                        if (DataRealizacji.HasValue)
                        {
                            _validationMessage = ValueValidator.ValidateEndAfterStartDate(DataWystawienia, DataRealizacji.Value);
                        }
                        break;
                }
                return _validationMessage;
            }
        }

        public override bool IsValid()
        {
            return string.IsNullOrEmpty(this[nameof(IDPacjenta)]) &&
                   string.IsNullOrEmpty(this[nameof(IDFarmaceuty)]) &&
                   string.IsNullOrEmpty(this[nameof(DataWystawienia)]) &&
                   string.IsNullOrEmpty(this[nameof(DataRealizacji)]);
        }

        public string Error => string.Empty;

        public override void Save()
        {
            if (IsValid())
            {
                aptekaEntities.Recepty.Add(item);
                aptekaEntities.SaveChanges();
                ShowMessageBox("Zapisano nową receptę!");
            }
            else
            {
                ShowMessageBox("Popraw błędy w formularzu");
            }
        }
    }
}
