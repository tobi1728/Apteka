using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace MVVMFirma.ViewModels
{
    public class NewWarehouseViewModel : OneViewModel<Magazyn>
    {
        #region Constructor
        public NewWarehouseViewModel()
            : base("Nowy Magazyn")
        {
            aptekaEntities = new AptekaEntities();
            item = new Magazyn();

            LoadProducts();
        }
        #endregion

        #region Properties

        // Lista leków (klucz obcy)
        private List<Leki> _Products;
        public List<Leki> Products
        {
            get => _Products;
            set
            {
                _Products = value;
                OnPropertyChanged(() => Products);
            }
        }

        // Nazwa leku (klucz obcy)
        public int IDLeku
        {
            get
            {
                return item.ID_Leku;
            }
            set
            {
                item.ID_Leku = value;
                OnPropertyChanged(() => IDLeku);
            }
        }

        public int Quantity
        {
            get
            {
                return item.Ilość;
            }
            set
            {
                item.Ilość = value;
                OnPropertyChanged(() => Quantity);
            }
        }

        public string Street
        {
            get
            {
                return item.Ulica;
            }
            set
            {
                item.Ulica = value;
                OnPropertyChanged(() => Street);
            }
        }

        public string City
        {
            get
            {
                return item.Miasto;
            }
            set
            {
                item.Miasto = value;
                OnPropertyChanged(() => City);
            }
        }

        public string PostalCode
        {
            get
            {
                return item.Kod_Pocztowy;
            }
            set
            {
                item.Kod_Pocztowy = value;
                OnPropertyChanged(() => PostalCode);
            }
        }

        public string Phone
        {
            get
            {
                return item.Telefon;
            }
            set
            {
                item.Telefon = value;
                OnPropertyChanged(() => Phone);
            }
        }

        #endregion

        #region Helpers
        public void LoadProducts()
        {
            Products = aptekaEntities.Leki.ToList();
        }

        public override void Save()
        {
            aptekaEntities.Magazyn.Add(item);
            aptekaEntities.SaveChanges();
        }
        #endregion
    }
}
