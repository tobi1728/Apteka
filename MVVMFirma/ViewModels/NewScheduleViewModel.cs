using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MVVMFirma.ViewModels
{
    public class NewScheduleViewModel : OneViewModel<Grafiki_Pracowników>
    {
        #region Constructor
        public NewScheduleViewModel()
            : base("Nowy grafik")
        {
            aptekaEntities = new AptekaEntities();
            item = new Grafiki_Pracowników();
            Data = DateTime.Today; // Default date to today
            GodzinaRozpoczecia = new TimeSpan(8, 0, 0); // Default start time
            GodzinaZakonczenia = new TimeSpan(16, 0, 0); // Default end time
            LoadFarmaceuci();
        }
        #endregion

        #region Properties
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




        public DateTime Data
        {
            get
            {
                return item.Data;
            }
            set
            {
                item.Data = value;
                OnPropertyChanged(() => Data);
            }
        }

        public TimeSpan GodzinaRozpoczecia
        {
            get
            {
                return item.Godzina_Rozpoczęcia;
            }
            set
            {
                item.Godzina_Rozpoczęcia = value;
                OnPropertyChanged(() => GodzinaRozpoczecia);
            }
        }

        public TimeSpan GodzinaZakonczenia
        {
            get
            {
                return item.Godzina_Zakończenia;
            }
            set
            {
                item.Godzina_Zakończenia = value;
                OnPropertyChanged(() => GodzinaZakonczenia);
            }
        }
        #endregion

        #region Helpers
        public void LoadFarmaceuci()
        {
            Farmaceuci = aptekaEntities.Farmaceuci.ToList();
        }

        public override void Save()
        {
            if (IDFarmaceuty == 0)
            {
                throw new InvalidOperationException("Musisz wybrać farmaceutę.");
            }

            aptekaEntities.Grafiki_Pracowników.Add(item);
            aptekaEntities.SaveChanges();
        }
        #endregion
    }
}
