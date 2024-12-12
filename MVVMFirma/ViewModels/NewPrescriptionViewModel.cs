using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using System;

namespace MVVMFirma.ViewModels
{
    public class NewPrescriptionViewModel : OneViewModel
    {
        private AptekaEntities _aptekaEntities;
        private Recepty _recepta;

        public NewPrescriptionViewModel()
        {
            DisplayName = "Dodaj Receptę";
            _aptekaEntities = new AptekaEntities();
            _recepta = new Recepty();
        }

        public int ID_Pacjenta
        {
            get => _recepta.ID_Pacjenta;
            set
            {
                _recepta.ID_Pacjenta = value;
                OnPropertyChanged(() => ID_Pacjenta);
            }
        }

        public int ID_Farmaceuty
        {
            get => _recepta.ID_Farmaceuty;
            set
            {
                _recepta.ID_Farmaceuty = value;
                OnPropertyChanged(() => ID_Farmaceuty);
            }
        }

        public DateTime Data_Wystawienia
        {
            get => _recepta.Data_Wystawienia;
            set
            {
                _recepta.Data_Wystawienia = value;
                OnPropertyChanged(() => Data_Wystawienia);
            }
        }

        public void Save()
        {
            _aptekaEntities.Recepty.Add(_recepta);
            _aptekaEntities.SaveChanges();
        }

        public void SaveAndClose()
        {
            Save();
            OnRequestClose();
        }
    }
}
