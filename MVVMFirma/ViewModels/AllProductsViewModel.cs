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
        private List<ProductForAllView> _allProducts;

        // ------------------ Filtry ------------------
        private decimal? _cenaZakupuOd;
        public decimal? CenaZakupuOd
        {
            get => _cenaZakupuOd;
            set { _cenaZakupuOd = value; OnPropertyChanged(() => CenaZakupuOd); Filter(); }
        }

        private decimal? _cenaZakupuDo;
        public decimal? CenaZakupuDo
        {
            get => _cenaZakupuDo;
            set { _cenaZakupuDo = value; OnPropertyChanged(() => CenaZakupuDo); Filter(); }
        }

        private decimal? _cenaSprzedazyOd;
        public decimal? CenaSprzedazyOd
        {
            get => _cenaSprzedazyOd;
            set { _cenaSprzedazyOd = value; OnPropertyChanged(() => CenaSprzedazyOd); Filter(); }
        }

        private decimal? _cenaSprzedazyDo;
        public decimal? CenaSprzedazyDo
        {
            get => _cenaSprzedazyDo;
            set { _cenaSprzedazyDo = value; OnPropertyChanged(() => CenaSprzedazyDo); Filter(); }
        }

        private bool _tylkoNaRecepte;
        public bool TylkoNaRecepte
        {
            get => _tylkoNaRecepte;
            set { _tylkoNaRecepte = value; OnPropertyChanged(() => TylkoNaRecepte); Filter(); }
        }

        private bool _tylkoRefundacja;
        public bool TylkoRefundacja
        {
            get => _tylkoRefundacja;
            set { _tylkoRefundacja = value; OnPropertyChanged(() => TylkoRefundacja); Filter(); }
        }

        private bool _tylkoPrzeterminowane;
        public bool TylkoPrzeterminowane
        {
            get => _tylkoPrzeterminowane;
            set { _tylkoPrzeterminowane = value; OnPropertyChanged(() => TylkoPrzeterminowane); Filter(); }
        }

        // ------------------ Statystyki ------------------
        private int _countOfProducts;
        public int CountOfProducts
        {
            get => _countOfProducts;
            set { _countOfProducts = value; OnPropertyChanged(() => CountOfProducts); }
        }

        private int _countExpired;
        public int CountExpired
        {
            get => _countExpired;
            set { _countExpired = value; OnPropertyChanged(() => CountExpired); }
        }

        private decimal _avgPurchasePrice;
        public decimal AvgPurchasePrice
        {
            get => _avgPurchasePrice;
            set { _avgPurchasePrice = value; OnPropertyChanged(() => AvgPurchasePrice); }
        }

        private decimal _avgSellPrice;
        public decimal AvgSellPrice
        {
            get => _avgSellPrice;
            set { _avgSellPrice = value; OnPropertyChanged(() => AvgSellPrice); }
        }

        // ------------------ Komenda ExportCsv ------------------
        private ICommand _exportCsvCommand;
        public ICommand ExportCsvCommand
        {
            get => _exportCsvCommand;
            set
            {
                _exportCsvCommand = value;
                OnPropertyChanged(() => ExportCsvCommand);
            }
        }

        // ------------------ Konstruktor ------------------
        public AllProductsViewModel() : base("Wszystkie leki")
        {
            // Inicjujemy komendę ExportCsv
            ExportCsvCommand = new BaseCommand(() => ExportCsv());
        }

        // ------------------ Sort & Find ------------------
        public override List<string> GetComboboxSortList()
        {
            return new List<string> { "Nazwa", "Kategoria", "Cena sprzedaży", "Cena zakupu", "Data ważności" };
        }

        public override void Sort()
        {
            if (SortField == "Nazwa")
            {
                List = new ObservableCollection<ProductForAllView>(List.OrderBy(p => p.Nazwa_Leku));
            }
            else if (SortField == "Kategoria")
            {
                List = new ObservableCollection<ProductForAllView>(List.OrderBy(p => p.Nazwa_Kategorii));
            }
            else if (SortField == "Cena sprzedaży")
            {
                List = new ObservableCollection<ProductForAllView>(List.OrderBy(p => p.Cena_Sprzedaży));
            }
            else if (SortField == "Cena zakupu")
            {
                List = new ObservableCollection<ProductForAllView>(List.OrderBy(p => p.Cena_Zakupu));
            }
            else if (SortField == "Data ważności")
            {
                List = new ObservableCollection<ProductForAllView>(List.OrderBy(p => p.Data_Waznosci));
            }
            UpdateStatistics();
        }

        public override List<string> GetComboboxFindList()
        {
            return new List<string> { "Nazwa", "Kategoria", "Producent" };
        }

        public override void Find()
        {
            // Przywracamy pełną listę
            Load();

            if (FindField == "Nazwa")
            {
                List = new ObservableCollection<ProductForAllView>(
                    List.Where(p => p.Nazwa_Leku != null &&
                                    p.Nazwa_Leku.StartsWith(FindTextBox, StringComparison.OrdinalIgnoreCase))
                );
            }
            else if (FindField == "Kategoria")
            {
                List = new ObservableCollection<ProductForAllView>(
                    List.Where(p => p.Nazwa_Kategorii != null &&
                                    p.Nazwa_Kategorii.StartsWith(FindTextBox, StringComparison.OrdinalIgnoreCase))
                );
            }
            else if (FindField == "Producent")
            {
                List = new ObservableCollection<ProductForAllView>(
                    List.Where(p => p.Nazwa_Producenta != null &&
                                    p.Nazwa_Producenta.StartsWith(FindTextBox, StringComparison.OrdinalIgnoreCase))
                );
            }
            UpdateStatistics();
        }

        // ------------------ Load & Filter ------------------
        public override void Load()
        {
            _allProducts = (
                from product in aptekaEntities.Leki
                select new ProductForAllView
                {
                    ID_Leku = product.ID_Leku,
                    Nazwa_Leku = product.Nazwa_Leku,
                    Nazwa_Kategorii = product.Kategorie_Leków.Nazwa_Kategorii,
                    Cena_Zakupu = product.Cena_Zakupu,
                    Cena_Sprzedaży = product.Cena_Sprzedaży,
                    Data_Waznosci = product.Data_Waznosci,
                    Nazwa_Producenta = product.Producent_Leków.Nazwa_Producenta,
                    Na_Recepte = product.Na_Recepte,
                    Refundacja = product.Refundacja,
                    Opis = product.Opis
                }
            ).ToList();

            List = new ObservableCollection<ProductForAllView>(_allProducts);
            UpdateStatistics();
        }

        private void Filter()
        {
            if (_allProducts == null) return;

            var filtered = _allProducts.AsEnumerable();

            // Cena zakupu
            if (CenaZakupuOd.HasValue)
                filtered = filtered.Where(p => p.Cena_Zakupu >= CenaZakupuOd.Value);

            if (CenaZakupuDo.HasValue)
                filtered = filtered.Where(p => p.Cena_Zakupu <= CenaZakupuDo.Value);

            // Cena sprzedaży
            if (CenaSprzedazyOd.HasValue)
                filtered = filtered.Where(p => p.Cena_Sprzedaży >= CenaSprzedazyOd.Value);

            if (CenaSprzedazyDo.HasValue)
                filtered = filtered.Where(p => p.Cena_Sprzedaży <= CenaSprzedazyDo.Value);

            // Tylko na receptę
            if (TylkoNaRecepte)
                filtered = filtered.Where(p => p.Na_Recepte);

            // Tylko refundacja
            if (TylkoRefundacja)
                filtered = filtered.Where(p => p.Refundacja);

            // Przeterminowane => Data_Waznosci < Today
            if (TylkoPrzeterminowane)
                filtered = filtered.Where(p => p.Data_Waznosci < DateTime.Today);

            List = new ObservableCollection<ProductForAllView>(filtered.ToList());
            UpdateStatistics();
        }

        // ------------------ UpdateStatistics (z zaokrągleniem) ------------------
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

            // Liczymy średnie i zaokrąglamy do 2 miejsc
            decimal avgPurchase = (decimal)List.Average(p => p.Cena_Zakupu);
            decimal avgSell = (decimal)List.Average(p => p.Cena_Sprzedaży);

            AvgPurchasePrice = Math.Round(avgPurchase, 2);
            AvgSellPrice = Math.Round(avgSell, 2);
        }

        // ------------------ ExportCsv ------------------
        private void ExportCsv()
        {
            try
            {
                string csvPath = System.IO.Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    "products_export.csv"
                );

                using (var writer = new StreamWriter(csvPath))
                {
                    // Nagłówek CSV
                    writer.WriteLine("ID_Leku;Nazwa;Opis;Kategoria;Cena_Zakupu;Cena_Sprzedazy;Data_Waznosci;Producent;Na_Recepte;Refundacja");

                    foreach (var item in List)
                    {
                        writer.WriteLine($"{item.ID_Leku};" +
                                         $"{item.Nazwa_Leku};" +
                                         $"{item.Opis};" +
                                         $"{item.Nazwa_Kategorii};" +
                                         $"{item.Cena_Zakupu};" +
                                         $"{item.Cena_Sprzedaży};" +
                                         $"{item.Data_Waznosci:d};" +
                                         $"{item.Nazwa_Producenta};" +
                                         $"{item.Na_Recepte};" +
                                         $"{item.Refundacja}");
                    }
                }

                ShowMessageBox($"Zapisano plik CSV: {csvPath}");
            }
            catch (Exception ex)
            {
                ShowMessageBox($"Błąd przy eksporcie CSV: {ex.Message}");
            }
        }
    }
}
