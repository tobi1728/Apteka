using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace MVVMFirma.ViewModels
{
    public class NewProductViewModel : WorkspaceViewModel
    {
        #region DB
        private AptekaEntities aptekaEntities;
        #endregion

        #region Item
        private Leki leki;
        #endregion

        #region Command
        //to jest komenda, ktora zostanie podpieta pod przycisk zapisz i zamknij
        private BaseCommand _SaveCommand;
        public ICommand SaveCommand
        { get 
            {
                if (_SaveCommand == null)
                    _SaveCommand = new BaseCommand(() => SaveAndClose());
                return _SaveCommand; 
            } 
        }
        #endregion
        #region Constructor
        public NewProductViewModel()
        {
            base.DisplayName = "Leki";
            aptekaEntities = new AptekaEntities();
            leki = new Leki();

            LoadKategorie();
            LoadProducenci();
        }
        #endregion

        #region Properties
        //dla kazdego pola na interface towrzymy properties

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

        #endregion
        public string Nazwa
        {
            get
            {
                return leki.Nazwa_Leku;
            }
            set
            {
                leki.Nazwa_Leku = value;
                OnPropertyChanged(() => Nazwa);
            }
        }

        // ID Kategorii
        public int? IDKategorii
        {
            get
            {
                return leki.ID_Kategorii;
            }
            set
            {
                leki.ID_Kategorii = value ?? 0;
                OnPropertyChanged(() => IDKategorii);
            }
        }

        // ID Producenta
        public int? IDProducenta
        {
            get
            {
                return leki.ID_Producenta;
            }
            set
            {
                leki.ID_Producenta = value ?? 0;
                OnPropertyChanged(() => IDProducenta);
            }
        }

        // Cena zakupu
        public decimal CenaZakupu
        {
            get
            {
                return leki.Cena_Zakupu;
            }
            set
            {
                leki.Cena_Zakupu = value;
                OnPropertyChanged(() => CenaZakupu);
            }
        }

        // Cena sprzedaży
        public decimal CenaSprzedazy
        {
            get
            {
                return leki.Cena_Sprzedaży;
            }
            set
            {
                leki.Cena_Sprzedaży = value;
                OnPropertyChanged(() => CenaSprzedazy);
            }
        }

        // Data ważności
        public DateTime DataWaznosci
        {
            get
            {
                return leki.Data_Waznosci;
            }
            set
            {
                leki.Data_Waznosci = value;
                OnPropertyChanged(() => DataWaznosci);
            }
        }

        // Czy wymaga recepty
        public bool Recepta
        {
            get
            {
                return leki.Na_Recepte;
            }
            set
            {
                leki.Na_Recepte = value;
                OnPropertyChanged(() => Recepta);
            }
        }

        // Czy lek jest refundowany
        public bool Refundacja
        {
            get
            {
                return leki.Refundacja;
            }
            set
            {
                leki.Refundacja = value;
                OnPropertyChanged(() => Refundacja);
            }
        }

        // Opis leku
        public string Opis
        {
            get
            {
                return leki.Opis;
            }
            set
            {
                leki.Opis = value;
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
        public void Save()
        {
            if (!IDKategorii.HasValue || !IDProducenta.HasValue || IDKategorii == 0 || IDProducenta == 0)
            {
                throw new InvalidOperationException("Musisz wybrać kategorię i producenta.");
            }

            aptekaEntities.Leki.Add(leki); // Dodaje do lokalnej kolekcji
            aptekaEntities.SaveChanges(); // Zapisuje zmiany do bazy danych
        }
        public void SaveAndClose()
        {
            Save();
            OnRequestClose();
        }
        #endregion
    }
}
