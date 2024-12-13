using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace MVVMFirma.ViewModels
{
    public class NewSaleViewModel : OneViewModel<Sprzedaż>
    {
        #region Constructor
        public NewSaleViewModel()
            : base("Nowa sprzedaż")
        {
            aptekaEntities = new AptekaEntities();
            item = new Sprzedaż();

            DataSprzedazy = DateTime.Today;
            LoadLeki();
            LoadPacjenci();
        }
        #endregion

        #region Properties

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

        public int IDLeku
        {
            get => item.ID_Leku;
            set
            {
                item.ID_Leku = value;
                OnPropertyChanged(() => IDLeku);
            }
        }

        public int? IDPacjenta
        {
            get => item.ID_Pacjenta;
            set
            {
                item.ID_Pacjenta = value;
                OnPropertyChanged(() => IDPacjenta);
            }
        }

        public DateTime DataSprzedazy
        {
            get => item.Data_Sprzedaży;
            set
            {
                item.Data_Sprzedaży = value;
                OnPropertyChanged(() => DataSprzedazy);
            }
        }

        public decimal Kwota
        {
            get => item.Kwota;
            set
            {
                item.Kwota = value;
                OnPropertyChanged(() => Kwota);
            }
        }

        public string FormaPlatnosci
        {
            get => item.Forma_Płatności;
            set
            {
                item.Forma_Płatności = value;
                OnPropertyChanged(() => FormaPlatnosci);
            }
        }

        #endregion

        #region Helpers
        public void LoadLeki()
        {
            Leki = aptekaEntities.Leki.ToList();
        }

        public void LoadPacjenci()
        {
            Pacjenci = aptekaEntities.Pacjenci.ToList();
        }

        public override void Save()
        {
            aptekaEntities.Sprzedaż.Add(item);
            aptekaEntities.SaveChanges();
        }
        #endregion
    }
}
