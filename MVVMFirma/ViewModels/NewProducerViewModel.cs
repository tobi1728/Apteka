using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace MVVMFirma.ViewModels
{
    public class NewProducerViewModel : OneViewModel<Producent_Leków>
    {
        #region Constructor
        public NewProducerViewModel()
            : base("Nowy producent")
        {
            aptekaEntities = new AptekaEntities();
            item = new Producent_Leków();
        }
        #endregion

        #region Properties
        public string NazwaProducenta
        {
            get
            {
                return item.Nazwa_Producenta;
            }
            set
            {
                item.Nazwa_Producenta = value;
                OnPropertyChanged(() => NazwaProducenta);
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
            if (string.IsNullOrEmpty(NazwaProducenta))
            {
                throw new InvalidOperationException("Musisz podać nazwę producenta.");
            }

            aptekaEntities.Producent_Leków.Add(item); // Dodaje do lokalnej kolekcji
            aptekaEntities.SaveChanges(); // Zapisuje zmiany do bazy danych
        }
        #endregion
    }
}
