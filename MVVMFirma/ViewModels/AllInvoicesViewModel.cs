using MVVMFirma.Helper;
using MVVMFirma.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace MVVMFirma.ViewModels
{
    public class AllInvoicesViewModel : AllViewModel<InvoiceForAllView>
    {
        // -------------------------------
        // 1) Przechowuj oryginalną listę
        // -------------------------------
        private List<InvoiceForAllView> _allInvoices;

        // -------------------------------
        // 2) Właściwości do filtrowania
        // -------------------------------
        private DateTime? _dataOd;
        public DateTime? DataOd
        {
            get => _dataOd;
            set
            {
                _dataOd = value;
                // Zgłaszamy zmianę właściwości
                OnPropertyChanged(() => DataOd);
                // Filtruj po ustawieniu wartości (opcjonalnie)
                Filter();
            }
        }

        private DateTime? _dataDo;
        public DateTime? DataDo
        {
            get => _dataDo;
            set
            {
                _dataDo = value;
                OnPropertyChanged(() => DataDo);
                Filter();
            }
        }

        private decimal? _kwotaOd;
        public decimal? KwotaOd
        {
            get => _kwotaOd;
            set
            {
                _kwotaOd = value;
                OnPropertyChanged(() => KwotaOd);
                Filter();
            }
        }

        private decimal? _kwotaDo;
        public decimal? KwotaDo
        {
            get => _kwotaDo;
            set
            {
                _kwotaDo = value;
                OnPropertyChanged(() => KwotaDo);
                Filter();
            }
        }


        // -------------------------------
        // 3) Komenda "Filtruj" (opcjonalna)
        // -------------------------------
        public ICommand FilterCommand { get; set; }

        #region Konstruktor
        public AllInvoicesViewModel()
            : base("Wszystkie faktury dostawców")
        {
            // Jeśli chcesz wywoływać Filter() przyciskiem, a nie w seterach:
            FilterCommand = new BaseCommand(() => Filter());
        }
        #endregion

        #region Sortowanie i wyszukiwanie
        public override List<string> GetComboboxSortList()
        {
            return new List<string> { "Numer Faktury", "Nazwa Dostawcy", "Data Wystawienia", "Kwota" };
        }

        public override void Sort()
        {
            if (SortField == "Numer Faktury")
            {
                List = new ObservableCollection<InvoiceForAllView>(
                    List.OrderBy(item => item.Numer_Faktury).ToList()
                );
            }
            else if (SortField == "Nazwa Dostawcy")
            {
                List = new ObservableCollection<InvoiceForAllView>(
                    List.OrderBy(item => item.Nazwa_Dostawcy).ToList()
                );
            }
            else if (SortField == "Data Wystawienia")
            {
                List = new ObservableCollection<InvoiceForAllView>(
                    List.OrderBy(item => item.Data_Wystawienia).ToList()
                );
            }
            else if (SortField == "Kwota")
            {
                List = new ObservableCollection<InvoiceForAllView>(
                    List.OrderBy(item => item.Kwota).ToList()
                );
            }
        }

        public override List<string> GetComboboxFindList()
        {
            return new List<string> { "Numer Faktury", "Nazwa Dostawcy" };
        }

        public override void Find()
        {
            // Przywracamy pełną listę przed wyszukiwaniem (z bazy)
            Load();

            if (FindField == "Numer Faktury")
            {
                List = new ObservableCollection<InvoiceForAllView>(
                    List.Where(item => item.Numer_Faktury != null &&
                                       item.Numer_Faktury.StartsWith(FindTextBox, StringComparison.OrdinalIgnoreCase))
                        .ToList()
                );
            }
            else if (FindField == "Nazwa Dostawcy")
            {
                List = new ObservableCollection<InvoiceForAllView>(
                    List.Where(item => item.Nazwa_Dostawcy != null &&
                                       item.Nazwa_Dostawcy.StartsWith(FindTextBox, StringComparison.OrdinalIgnoreCase))
                        .ToList()
                );
            }
        }
        #endregion

        #region Metody pomocnicze
        // Wczytanie listy z bazy i zapamiętanie w _allInvoices
        public override void Load()
        {
            var invoicesQuery = from invoice in aptekaEntities.Faktury_Dostawców
                                select new InvoiceForAllView
                                {
                                    ID_Faktury = invoice.ID_Faktury,
                                    Numer_Faktury = invoice.Numer_Faktury,
                                    Nazwa_Dostawcy = invoice.Dostawcy.Nazwa,
                                    Data_Wystawienia = invoice.Data_Wystawienia,
                                    Kwota = invoice.Kwota,
                                    Numer_Zamówienia = invoice.Zamówienia.ID_Zamówienia.ToString(),
                                };

            // Zapisujemy pełną listę w pamięci (bez filtrów)
            _allInvoices = invoicesQuery.ToList();

            // Domyślnie wyświetlamy pełną listę
            List = new ObservableCollection<InvoiceForAllView>(_allInvoices);
        }

        // -------------------------------
        // 4) Metoda Filter() – filtruje po DataOd, DataDo, KwotaOd, KwotaDo
        // -------------------------------
        private void Filter()
        {
            if (_allInvoices == null)
                return;

            // Startujemy od oryginalnej listy
            var filtered = _allInvoices.AsEnumerable();

            // Filtr daty OD
            if (DataOd.HasValue)
                filtered = filtered.Where(f => f.Data_Wystawienia >= DataOd.Value);

            // Filtr daty DO
            if (DataDo.HasValue)
                filtered = filtered.Where(f => f.Data_Wystawienia <= DataDo.Value);

            // Filtr kwoty OD
            if (KwotaOd.HasValue)
                filtered = filtered.Where(f => f.Kwota >= KwotaOd.Value);

            // Filtr kwoty DO
            if (KwotaDo.HasValue)
                filtered = filtered.Where(f => f.Kwota <= KwotaDo.Value);

            // Tworzymy nową ObservableCollection i przypisujemy do List
            List = new ObservableCollection<InvoiceForAllView>(filtered.ToList());
        }
        #endregion
    }
}
