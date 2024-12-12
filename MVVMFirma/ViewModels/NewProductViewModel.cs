using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace MVVMFirma.ViewModels
{
    public class NewProductViewModel : OneViewModel<Leki>
    {
       
        #region Constructor
        public NewProductViewModel()
            :base("Leki")
        {
            aptekaEntities = new AptekaEntities();
            item = new Leki();

            DataWaznosci = DateTime.Today;

            LoadKategorie();
            LoadProducenci();
        }
        #endregion

        #region Properties

        // Lista kategorii leków
        private List<Kategorie_Leków> _KategorieLekow;
        public List<Kategorie_Leków> KategorieLekow
        {
            get => _KategorieLekow;
            set
            {
                _KategorieLekow = value;
                OnPropertyChanged(() => KategorieLekow);
            }
        }

        // Lista producentów
        private List<Producent_Leków> _Producenci;
        public List<Producent_Leków> Producenci
        {
            get => _Producenci;
            set
            {
                _Producenci = value;
                OnPropertyChanged(() => Producenci);
            }
        }

        public string Nazwa
        {
            get
            {
                return item.Nazwa_Leku;
            }
            set
            {
                item.Nazwa_Leku = value;
                OnPropertyChanged(() => Nazwa);
            }
        }

        // ID Kategorii
        public int IDKategorii
        {
            get
            {
                return item.ID_Kategorii;
            }
            set
            {
                item.ID_Kategorii = value;
                OnPropertyChanged(() => IDKategorii);
            }
        }

        // ID Producenta
        public int IDProducenta
        {
            get
            {
                return item.ID_Producenta;
            }
            set
            {
                item.ID_Producenta = value;
                OnPropertyChanged(() => IDProducenta);
            }
        }

        // Cena zakupu
        public decimal CenaZakupu
        {
            get
            {
                return item.Cena_Zakupu;
            }
            set
            {
                item.Cena_Zakupu = value;
                OnPropertyChanged(() => CenaZakupu);
            }
        }

        // Cena sprzedaży
        public decimal CenaSprzedazy
        {
            get
            {
                return item.Cena_Sprzedaży;
            }
            set
            {
                item.Cena_Sprzedaży = value;
                OnPropertyChanged(() => CenaSprzedazy);
            }
        }

        // Data ważności
        public DateTime DataWaznosci
        {
            get
            {
                return item.Data_Waznosci;
            }
            set
            {
                item.Data_Waznosci = value;
                OnPropertyChanged(() => DataWaznosci);
            }
        }

        // Czy wymaga recepty
        public bool Recepta
        {
            get
            {
                return item.Na_Recepte;
            }
            set
            {
                item.Na_Recepte = value;
                OnPropertyChanged(() => Recepta);
            }
        }

        // Czy lek jest refundowany
        public bool Refundacja
        {
            get
            {
                return item.Refundacja;
            }
            set
            {
                item.Refundacja = value;
                OnPropertyChanged(() => Refundacja);
            }
        }

        // Opis leku
        public string Opis
        {
            get
            {
                return item.Opis;
            }
            set
            {
                item.Opis = value;
                OnPropertyChanged(() => Opis);
            }
        }

        #endregion

        #region Helpers
        public void LoadKategorie()
        {
            KategorieLekow = aptekaEntities.Kategorie_Leków.ToList();
            Console.WriteLine($"Załadowano {KategorieLekow.Count} kategorii.");
        }

        public void LoadProducenci()
        {
            Producenci = aptekaEntities.Producent_Leków.ToList();
            Console.WriteLine($"Załadowano {Producenci.Count} producentów.");
        }
        public override void Save()
        {
            if (IDKategorii == 0 || IDProducenta == 0)
            {
                throw new InvalidOperationException("Musisz wybrać kategorię i producenta.");
            }

            aptekaEntities.Leki.Add(item); // Dodaje do lokalnej kolekcji
            aptekaEntities.SaveChanges(); // Zapisuje zmiany do bazy danych
        }

        #endregion
    }
}
