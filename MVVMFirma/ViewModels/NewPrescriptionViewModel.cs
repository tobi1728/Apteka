using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVVMFirma.ViewModels
{
    public class NewPrescriptionViewModel : OneViewModel<Recepty>
    {
        #region Constructor
        public NewPrescriptionViewModel()
            : base("Nowa recepta")
        {
            aptekaEntities = new AptekaEntities();
            item = new Recepty();

            DataWystawienia = DateTime.Today;

            LoadPacjenci();
            LoadFarmaceuci();
        }
        #endregion

        #region Properties

        // Lista pacjentów
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

        // Lista farmaceutów
        private List<Farmaceuci> _Farmaceuci;
        public List<Farmaceuci> Farmaceuci
        {
            get => _Farmaceuci;
            set
            {
                _Farmaceuci = value;
                OnPropertyChanged(() => Farmaceuci);
            }
        }

        // ID Pacjenta
        public int IDPacjenta
        {
            get
            {
                return item.ID_Pacjenta;
            }
            set
            {
                item.ID_Pacjenta = value;
                OnPropertyChanged(() => IDPacjenta);
            }
        }

        // ID Farmaceuty
        public int IDFarmaceuty
        {
            get
            {
                return item.ID_Farmaceuty;
            }
            set
            {
                item.ID_Farmaceuty = value;
                OnPropertyChanged(() => IDFarmaceuty);
            }
        }

        // Data wystawienia
        public DateTime DataWystawienia
        {
            get
            {
                return item.Data_Wystawienia;
            }
            set
            {
                item.Data_Wystawienia = value;
                OnPropertyChanged(() => DataWystawienia);
            }
        }

        // Data realizacji
        public DateTime? DataRealizacji
        {
            get
            {
                return item.Data_Realizacji;
            }
            set
            {
                item.Data_Realizacji = value;
                OnPropertyChanged(() => DataRealizacji);
            }
        }

        #endregion

        #region Helpers
        public void LoadPacjenci()
        {
            Pacjenci = aptekaEntities.Pacjenci.ToList();
        }

        public void LoadFarmaceuci()
        {
            Farmaceuci = aptekaEntities.Farmaceuci.ToList();
        }

        public override void Save()
        {
            if (IDPacjenta == 0 || IDFarmaceuty == 0)
            {
                throw new InvalidOperationException("Musisz wybrać pacjenta i farmaceutę.");
            }

            aptekaEntities.Recepty.Add(item);
            aptekaEntities.SaveChanges();
        }
        #endregion
    }
}
