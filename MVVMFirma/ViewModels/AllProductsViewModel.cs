using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace MVVMFirma.ViewModels
{
    public class AllProductsViewModel : AllViewModel<ProductForAllView>
    {
        #region Pola do mechanizmu modalnego (wybór produktu)
        private ProductForAllView _selectedProduct;
        public ProductForAllView SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                OnPropertyChanged(() => SelectedProduct);

                // Jeśli okno otwarte w trybie modalnym i user wybrał wiersz:
                if (_selectedProduct != null && IsModal)
                {
                    Messenger.Default.Send(_selectedProduct);
                    OnRequestClose();
                }
            }
        }

        public bool IsModal { get; set; } = false;
        #endregion

        #region Pola i właściwości do filtrowania

        private decimal? _cenaZakupuOd;
        public decimal? CenaZakupuOd
        {
            get => _cenaZakupuOd;
            set
            {
                _cenaZakupuOd = value;
                OnPropertyChanged(() => CenaZakupuOd);
            }
        }

        private decimal? _cenaZakupuDo;
        public decimal? CenaZakupuDo
        {
            get => _cenaZakupuDo;
            set
            {
                _cenaZakupuDo = value;
                OnPropertyChanged(() => CenaZakupuDo);
            }
        }

        private decimal? _cenaSprzedazyOd;
        public decimal? CenaSprzedazyOd
        {
            get => _cenaSprzedazyOd;
            set
            {
                _cenaSprzedazyOd = value;
                OnPropertyChanged(() => CenaSprzedazyOd);
            }
        }

        private decimal? _cenaSprzedazyDo;
        public decimal? CenaSprzedazyDo
        {
            get => _cenaSprzedazyDo;
            set
            {
                _cenaSprzedazyDo = value;
                OnPropertyChanged(() => CenaSprzedazyDo);
            }
        }

        private bool _tylkoNaRecepte;
        public bool TylkoNaRecepte
        {
            get => _tylkoNaRecepte;
            set
            {
                _tylkoNaRecepte = value;
                OnPropertyChanged(() => TylkoNaRecepte);
            }
        }

        private bool _tylkoRefundacja;
        public bool TylkoRefundacja
        {
            get => _tylkoRefundacja;
            set
            {
                _tylkoRefundacja = value;
                OnPropertyChanged(() => TylkoRefundacja);
            }
        }

        private bool _tylkoPrzeterminowane;
        public bool TylkoPrzeterminowane
        {
            get => _tylkoPrzeterminowane;
            set
            {
                _tylkoPrzeterminowane = value;
                OnPropertyChanged(() => TylkoPrzeterminowane);
            }
        }

        #endregion

        #region Pola i właściwości do statystyk (liczba, przeterminowane, średnie ceny)

        private int _countOfProducts;
        public int CountOfProducts
        {
            get => _countOfProducts;
            set
            {
                _countOfProducts = value;
                OnPropertyChanged(() => CountOfProducts);
            }
        }

        private int _countExpired;
        public int CountExpired
        {
            get => _countExpired;
            set
            {
                _countExpired = value;
                OnPropertyChanged(() => CountExpired);
            }
        }

        private decimal _avgPurchasePrice;
        public decimal AvgPurchasePrice
        {
            get => _avgPurchasePrice;
            set
            {
                _avgPurchasePrice = value;
                OnPropertyChanged(() => AvgPurchasePrice);
            }
        }

        private decimal _avgSellPrice;
        public decimal AvgSellPrice
        {
            get => _avgSellPrice;
            set
            {
                _avgSellPrice = value;
                OnPropertyChanged(() => AvgSellPrice);
            }
        }

        #endregion

        #region Komendy

        private ICommand _filterCommand;
        public ICommand FilterCommand
        {
            get
            {
                if (_filterCommand == null)
                    _filterCommand = new BaseCommand(() => Filter());
                return _filterCommand;
            }
        }

        private ICommand _exportCsvCommand;
        public ICommand ExportCsvCommand
        {
            get
            {
                if (_exportCsvCommand == null)
                    _exportCsvCommand = new BaseCommand(() => ExportCsv());
                return _exportCsvCommand;
            }
            set
            {
                _exportCsvCommand = value;
                OnPropertyChanged(() => ExportCsvCommand);
            }
        }

        #endregion

        #region Konstruktor
        public AllProductsViewModel()
            : base("Wszystkie leki")
        {
            // Domyślnie ustawiamy komendę ExportCsv,
            // jeśli chcesz od razu z tego korzystać:
            // ExportCsvCommand = new BaseCommand(() => ExportCsv());
        }
        #endregion

        #region Implementacja abstrakcyjnych metod

        /// <summary>
        /// Wczytanie listy produktów z bazy.
        /// </summary>
        public override void Load()
        {
            // Pobieramy z bazy i zamieniamy na listę
            List = new ObservableCollection<ProductForAllView>
            (
                from product in aptekaEntities.Leki
                select new ProductForAllView
                {
                    ID_Leku = product.ID_Leku,
                    Nazwa_Leku = product.Nazwa_Leku,
                    Opis = product.Opis,
                    Nazwa_Kategorii = product.Kategorie_Leków.Nazwa_Kategorii,
                    Cena_Zakupu = product.Cena_Zakupu,
                    Cena_Sprzedaży = product.Cena_Sprzedaży,
                    Data_Waznosci = product.Data_Waznosci,  // zwykły DateTime
                    Nazwa_Producenta = product.Producent_Leków.Nazwa_Producenta,
                    Na_Recepte = product.Na_Recepte,
                    Refundacja = product.Refundacja
                }
            );

            UpdateStatistics();
        }

        public override void Sort()
        {
            if (SortField == "Nazwa")
            {
                List = new ObservableCollection<ProductForAllView>(
                    List.OrderBy(p => p.Nazwa_Leku)
                );
            }
            else if (SortField == "Kategoria")
            {
                List = new ObservableCollection<ProductForAllView>(
                    List.OrderBy(p => p.Nazwa_Kategorii)
                );
            }
            else if (SortField == "Cena sprzedaży")
            {
                List = new ObservableCollection<ProductForAllView>(
                    List.OrderBy(p => p.Cena_Sprzedaży)
                );
            }
            else if (SortField == "Cena zakupu")
            {
                List = new ObservableCollection<ProductForAllView>(
                    List.OrderBy(p => p.Cena_Zakupu)
                );
            }
            else if (SortField == "Data ważności")
            {
                List = new ObservableCollection<ProductForAllView>(
                    List.OrderBy(p => p.Data_Waznosci)
                );
            }

            UpdateStatistics();
        }

        public override void Find()
        {
            Load(); // przywracamy pełną listę

            if (FindField == "Nazwa")
            {
                List = new ObservableCollection<ProductForAllView>(
                    List.Where(p => p.Nazwa_Leku != null
                        && p.Nazwa_Leku.StartsWith(FindTextBox, StringComparison.OrdinalIgnoreCase))
                );
            }
            else if (FindField == "Kategoria")
            {
                List = new ObservableCollection<ProductForAllView>(
                    List.Where(p => p.Nazwa_Kategorii != null
                        && p.Nazwa_Kategorii.StartsWith(FindTextBox, StringComparison.OrdinalIgnoreCase))
                );
            }
            else if (FindField == "Producent")
            {
                List = new ObservableCollection<ProductForAllView>(
                    List.Where(p => p.Nazwa_Producenta != null
                        && p.Nazwa_Producenta.StartsWith(FindTextBox, StringComparison.OrdinalIgnoreCase))
                );
            }

            UpdateStatistics();
        }

        public override List<string> GetComboboxSortList()
        {
            return new List<string> { "Nazwa", "Kategoria", "Cena sprzedaży", "Cena zakupu", "Data ważności" };
        }

        public override List<string> GetComboboxFindList()
        {
            return new List<string> { "Nazwa", "Kategoria", "Producent" };
        }

        #endregion

        #region Filter()
        private void Filter()
        {
            // Przywróć pełną listę
            Load();

            var filtered = List.AsEnumerable();

            // Cena zakupu od/do
            if (CenaZakupuOd.HasValue)
                filtered = filtered.Where(p => p.Cena_Zakupu >= CenaZakupuOd.Value);

            if (CenaZakupuDo.HasValue)
                filtered = filtered.Where(p => p.Cena_Zakupu <= CenaZakupuDo.Value);

            // Cena sprzedaży od/do
            if (CenaSprzedazyOd.HasValue)
                filtered = filtered.Where(p => p.Cena_Sprzedaży >= CenaSprzedazyOd.Value);

            if (CenaSprzedazyDo.HasValue)
                filtered = filtered.Where(p => p.Cena_Sprzedaży <= CenaSprzedazyDo.Value);

            // Na receptę
            if (TylkoNaRecepte)
                filtered = filtered.Where(p => p.Na_Recepte);

            // Refundacja
            if (TylkoRefundacja)
                filtered = filtered.Where(p => p.Refundacja);

            // Przeterminowane
            if (TylkoPrzeterminowane)
                filtered = filtered.Where(p => p.Data_Waznosci < DateTime.Today);

            // Gotowa lista
            List = new ObservableCollection<ProductForAllView>(filtered.ToList());

            UpdateStatistics();
        }
        #endregion

        #region UpdateStatistics()
        private void UpdateStatistics()
        {
            if (List == null || List.Count == 0)
            {
                CountOfProducts = 0;
                CountExpired = 0;
                AvgPurchasePrice = 0;
                AvgSellPrice = 0;
                return;
            }

            CountOfProducts = List.Count;
            CountExpired = List.Count(p => p.Data_Waznosci < DateTime.Today);

            AvgPurchasePrice = Math.Round((decimal)List.Average(p => p.Cena_Zakupu), 2);
            AvgSellPrice = Math.Round((decimal)List.Average(p => p.Cena_Sprzedaży), 2);
        }
        #endregion

        #region ExportCsv()
        private void ExportCsv()
        {
            try
            {
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string csvPath = Path.Combine(desktopPath, "products_export.csv");

                using (var writer = new StreamWriter(csvPath))
                {
                    // Nagłówek CSV
                    writer.WriteLine("ID_Leku;Nazwa;Opis;Kategoria;CenaZakupu;CenaSprzedazy;DataWaznosci;Producent;NaRecepte;Refundacja");

                    foreach (var p in List)
                    {
                        // Jeżeli Data_Waznosci to zwykły DateTime, wystarczy:
                        // $"{p.Data_Waznosci:yyyy-MM-dd}"
                        // O ile w ProductForAllView jest "public DateTime Data_Waznosci { get; set; }"
                        writer.WriteLine(
                            $"{p.ID_Leku};" +
                            $"{p.Nazwa_Leku};" +
                            $"{p.Opis};" +
                            $"{p.Nazwa_Kategorii};" +
                            $"{p.Cena_Zakupu};" +
                            $"{p.Cena_Sprzedaży};" +
                            $"{p.Data_Waznosci:yyyy-MM-dd};" +
                            $"{p.Nazwa_Producenta};" +
                            $"{p.Na_Recepte};" +
                            $"{p.Refundacja}"
                        );
                    }
                }

                ShowMessageBox($"Zapisano plik CSV: {csvPath}");
            }
            catch (Exception ex)
            {
                ShowMessageBox($"Błąd przy eksporcie CSV: {ex.Message}");
            }
        }
        #endregion
    }
}
