using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace MVVMFirma.ViewModels
{
    public class NewOrderViewModel : OneViewModel<Zamówienia>
    {
        #region Constructor
        public NewOrderViewModel()
            : base("Nowe zamówienie")
        {
            aptekaEntities = new AptekaEntities();
            item = new Zamówienia();

            DataZamowienia = DateTime.Today;
            DataDostawy = DateTime.Today;
            LoadDostawcy();
        }
        #endregion

        #region Properties

        private List<Dostawcy> _Dostawcy;
        public List<Dostawcy> Dostawcy
        {
            get => _Dostawcy;
            set
            {
                _Dostawcy = value;
                OnPropertyChanged(() => Dostawcy);
            }
        }

        public int IDDostawcy
        {
            get => item.ID_Dostawcy;
            set
            {
                item.ID_Dostawcy = value;
                OnPropertyChanged(() => IDDostawcy);
            }
        }

        public DateTime DataZamowienia
        {
            get => item.Data_Zamówienia;
            set
            {
                item.Data_Zamówienia = value;
                OnPropertyChanged(() => DataZamowienia);
            }
        }

        public DateTime DataDostawy
        {
            get => item.Data_Dostawy;
            set
            {
                item.Data_Dostawy = value;
                OnPropertyChanged(() => DataDostawy);
            }
        }

        public string Status
        {
            get => item.Status;
            set
            {
                item.Status = value;
                OnPropertyChanged(() => Status);
            }
        }

        #endregion

        #region Helpers
        public void LoadDostawcy()
        {
            Dostawcy = aptekaEntities.Dostawcy.ToList();
        }

        public override void Save()
        {
            aptekaEntities.Zamówienia.Add(item);
            aptekaEntities.SaveChanges();
        }
        #endregion
    }
}
