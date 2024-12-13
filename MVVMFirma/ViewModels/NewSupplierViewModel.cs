using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MVVMFirma.ViewModels
{
    public class NewSupplierViewModel : OneViewModel<Dostawcy>
    {
        #region Constructor
        public NewSupplierViewModel()
            : base("Nowy dostawca")
        {
            aptekaEntities = new AptekaEntities();
            item = new Dostawcy();
        }
        #endregion

        #region Properties

        public string Nazwa
        {
            get
            {
                return item.Nazwa;
            }
            set
            {
                item.Nazwa = value;
                OnPropertyChanged(() => Nazwa);
            }
        }

        public string Telefon
        {
            get
            {
                return item.Telefon;
            }
            set
            {
                item.Telefon = value;
                OnPropertyChanged(() => Telefon);
            }
        }

        public string Ulica
        {
            get
            {
                return item.Ulica;
            }
            set
            {
                item.Ulica = value;
                OnPropertyChanged(() => Ulica);
            }
        }

        public string Miasto
        {
            get
            {
                return item.Miasto;
            }
            set
            {
                item.Miasto = value;
                OnPropertyChanged(() => Miasto);
            }
        }

        public string KodPocztowy
        {
            get
            {
                return item.Kod_Pocztowy;
            }
            set
            {
                item.Kod_Pocztowy = value;
                OnPropertyChanged(() => KodPocztowy);
            }
        }

        #endregion

        #region Helpers
        public override void Save()
        {
            aptekaEntities.Dostawcy.Add(item); // Dodanie rekordu do kolekcji lokalnej
            aptekaEntities.SaveChanges(); // Zapisanie zmian do bazy danych
        }
        #endregion
    }
}
