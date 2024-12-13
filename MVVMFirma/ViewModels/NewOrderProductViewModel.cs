using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace MVVMFirma.ViewModels
{
    public class NewOrderProductViewModel : OneViewModel<Produkty_Zamówienia>
    {
        #region Constructor
        public NewOrderProductViewModel()
            : base("Nowy produkt zamówienia")
        {
            aptekaEntities = new AptekaEntities();
            item = new Produkty_Zamówienia();
            LoadLeki();
            LoadZamowienia();
        }
        #endregion

        #region Properties

        // Lista leków
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

        // Lista zamówień
        private List<Zamówienia> _Zamowienia;
        public List<Zamówienia> Zamowienia
        {
            get => _Zamowienia;
            set
            {
                _Zamowienia = value;
                OnPropertyChanged(() => Zamowienia);
            }
        }

        public int ID_Leku
        {
            get => item.ID_Leku;
            set
            {
                item.ID_Leku = value;
                OnPropertyChanged(() => ID_Leku);
            }
        }

        public int ID_Zamówienia
        {
            get => item.ID_Zamówienia;
            set
            {
                item.ID_Zamówienia = value;
                OnPropertyChanged(() => ID_Zamówienia);
            }
        }

        public int Ilość
        {
            get => item.Ilość;
            set
            {
                item.Ilość = value;
                OnPropertyChanged(() => Ilość);
            }
        }

        #endregion

        #region Helpers
        public void LoadLeki()
        {
            Leki = aptekaEntities.Leki.ToList();
        }

        public void LoadZamowienia()
        {
            Zamowienia = aptekaEntities.Zamówienia.ToList();
        }

        public override void Save()
        {
            if (ID_Leku == 0 || ID_Zamówienia == 0)
            {
                throw new InvalidOperationException("Musisz wybrać lek i zamówienie.");
            }

            aptekaEntities.Produkty_Zamówienia.Add(item); // Dodaje do lokalnej kolekcji
            aptekaEntities.SaveChanges(); // Zapisuje zmiany do bazy danych
        }
        #endregion
    }
}
