using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace MVVMFirma.ViewModels
{
    public class NewSalesReportViewModel : OneViewModel<Raporty_Sprzedaży>
    {
        #region Constructor
        public NewSalesReportViewModel()
            : base("Nowy raport sprzedaży")
        {
            aptekaEntities = new AptekaEntities();
            item = new Raporty_Sprzedaży();
            DataRozpoczecia = DateTime.Today;
            DataZakonczenia = DateTime.Today;
        }
        #endregion

        #region Properties

        public DateTime DataRozpoczecia
        {
            get => item.Data_Rozpoczęcia;
            set
            {
                item.Data_Rozpoczęcia = value;
                OnPropertyChanged(() => DataRozpoczecia);
            }
        }

        public DateTime DataZakonczenia
        {
            get => item.Data_Zakończenia;
            set
            {
                item.Data_Zakończenia = value;
                OnPropertyChanged(() => DataZakonczenia);
            }
        }

        public decimal LacznaSprzedaz
        {
            get => item.Łączna_Sprzedaż;
            set
            {
                item.Łączna_Sprzedaż = value;
                OnPropertyChanged(() => LacznaSprzedaz);
            }
        }

        public int LiczbaTransakcji
        {
            get => item.Liczba_Transakcji;
            set
            {
                item.Liczba_Transakcji = value;
                OnPropertyChanged(() => LiczbaTransakcji);
            }
        }

        #endregion

        #region Helpers
        public override void Save()
        {
            if (DataZakonczenia < DataRozpoczecia)
            {
                throw new InvalidOperationException("Data zakończenia nie może być wcześniejsza niż data rozpoczęcia.");
            }

            aptekaEntities.Raporty_Sprzedaży.Add(item); // Dodanie do lokalnej kolekcji
            aptekaEntities.SaveChanges(); // Zapisanie zmian w bazie danych
        }
        #endregion
    }
}
