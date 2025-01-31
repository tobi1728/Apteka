using LiveCharts;
using LiveCharts.Wpf;
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
    public class AllSalesViewModel : AllViewModel<SaleForAllView>
    {
        private List<SaleForAllView> _allSales;

        #region Filtry
        public DateTime? DataOd { get; set; }
        public DateTime? DataDo { get; set; }
        public decimal? KwotaOd { get; set; }
        public decimal? KwotaDo { get; set; }
        private string _wybranaFormaPlatnosci;
        public string WybranaFormaPlatnosci
        {
            get => _wybranaFormaPlatnosci;
            set
            {
                _wybranaFormaPlatnosci = value;
                OnPropertyChanged(() => WybranaFormaPlatnosci);
            }
        }
        #endregion

        #region Statystyki
        private int _countOfSales;
        public int CountOfSales
        {
            get => _countOfSales;
            set
            {
                _countOfSales = value;
                OnPropertyChanged(() => CountOfSales);
            }
        }

        private decimal _totalAmount;
        public decimal TotalAmount
        {
            get => _totalAmount;
            set
            {
                _totalAmount = value;
                OnPropertyChanged(() => TotalAmount);
            }
        }

        private decimal _avgAmount;
        public decimal AvgAmount
        {
            get => _avgAmount;
            set
            {
                _avgAmount = value;
                OnPropertyChanged(() => AvgAmount);
            }
        }
        #endregion

        #region Wykres form płatności (PieChart)
        private SeriesCollection _seriesCollectionPayment;
        public SeriesCollection SeriesCollectionPayment
        {
            get => _seriesCollectionPayment;
            set
            {
                _seriesCollectionPayment = value;
                OnPropertyChanged(() => SeriesCollectionPayment);
            }
        }
        #endregion

        #region Komendy
        public ICommand FilterCommand { get; set; }
        public ICommand ExportCsvCommand { get; set; }
        #endregion

        #region Konstruktor
        public AllSalesViewModel()
            : base("Wszystkie sprzedaże")
        {
            FilterCommand = new BaseCommand(() => Filter());
            ExportCsvCommand = new BaseCommand(() => ExportCsv());

            WybranaFormaPlatnosci = "Wszystkie";  // domyślnie

            SeriesCollectionPayment = new SeriesCollection();  // pusty wykres na start

            Load(); // wczytanie danych od razu
        }
        #endregion

        #region Sort & Find
        public override List<string> GetComboboxSortList()
        {
            return new List<string>
            {
                "Nazwa Leku",
                "Data Sprzedaży",
                "Kwota",
                "Forma Płatności"
            };
        }

        public override void Sort()
        {
            if (SortField == "Nazwa Leku")
            {
                List = new ObservableCollection<SaleForAllView>(
                    List.OrderBy(item => item.Nazwa_Leku)
                );
            }
            else if (SortField == "Data Sprzedaży")
            {
                List = new ObservableCollection<SaleForAllView>(
                    List.OrderBy(item => item.Data_Sprzedaży)
                );
            }
            else if (SortField == "Kwota")
            {
                List = new ObservableCollection<SaleForAllView>(
                    List.OrderBy(item => item.Kwota)
                );
            }
            else if (SortField == "Forma Płatności")
            {
                List = new ObservableCollection<SaleForAllView>(
                    List.OrderBy(item => item.Forma_Płatności)
                );
            }

            UpdateStatistics();
        }

        public override List<string> GetComboboxFindList()
        {
            return new List<string>
            {
                "Nazwa Leku",
                "Forma Płatności"
            };
        }

        public override void Find()
        {
            Load(); // przywracamy pełną listę

            if (FindField == "Nazwa Leku")
            {
                List = new ObservableCollection<SaleForAllView>(
                    List.Where(item => item.Nazwa_Leku != null
                        && item.Nazwa_Leku
                           .StartsWith(FindTextBox, StringComparison.OrdinalIgnoreCase))
                );
            }
            else if (FindField == "Forma Płatności")
            {
                List = new ObservableCollection<SaleForAllView>(
                    List.Where(item => item.Forma_Płatności != null
                        && item.Forma_Płatności
                           .StartsWith(FindTextBox, StringComparison.OrdinalIgnoreCase))
                );
            }

            UpdateStatistics();
        }
        #endregion

        #region Load & Filter
        public override void Load()
        {
            var salesQuery =
                from s in aptekaEntities.Sprzedaż
                select new SaleForAllView
                {
                    ID_Sprzedaży = s.ID_Sprzedaży,
                    Nazwa_Leku = s.Leki.Nazwa_Leku,
                    PESEL_Pacjenta = s.Pacjenci != null ? s.Pacjenci.PESEL : "",
                    Data_Sprzedaży = s.Data_Sprzedaży,
                    Kwota = s.Kwota,
                    Forma_Płatności = s.Forma_Płatności
                };

            _allSales = salesQuery.ToList();
            List = new ObservableCollection<SaleForAllView>(_allSales);

            UpdateStatistics();
        }

        private void Filter()
        {
            if (_allSales == null)
                return;

            var filtered = _allSales.AsEnumerable();

            // Zamieniamy DataOd i DataDo jeśli user podał odwrotnie
            if (DataOd.HasValue && DataDo.HasValue && DataOd > DataDo)
            {
                var tmp = DataOd.Value;
                DataOd = DataDo;
                DataDo = tmp;
            }

            // Data od/do (koniec dnia w DataDo)
            if (DataOd.HasValue)
                filtered = filtered.Where(s => s.Data_Sprzedaży >= DataOd.Value.Date);

            if (DataDo.HasValue)
            {
                var dataDoEnd = DataDo.Value.Date.AddDays(1).AddTicks(-1);
                filtered = filtered.Where(s => s.Data_Sprzedaży <= dataDoEnd);
            }

            // Kwota od/do
            if (KwotaOd.HasValue)
                filtered = filtered.Where(s => s.Kwota >= KwotaOd.Value);

            if (KwotaDo.HasValue)
                filtered = filtered.Where(s => s.Kwota <= KwotaDo.Value);

            // Forma płatności (pomijamy "Wszystkie")
            if (!string.IsNullOrEmpty(WybranaFormaPlatnosci)
                && WybranaFormaPlatnosci != "Wszystkie")
            {
                filtered = filtered.Where(s =>
                    s.Forma_Płatności != null
                    && s.Forma_Płatności.Trim().Equals(
                           WybranaFormaPlatnosci.Trim(),
                           StringComparison.OrdinalIgnoreCase)
                );
            }

            List = new ObservableCollection<SaleForAllView>(filtered.ToList());

            UpdateStatistics(); // liczymy statystyki i budujemy wykres
        }

        private void UpdateStatistics()
        {
            if (List == null || List.Count == 0)
            {
                CountOfSales = 0;
                TotalAmount = 0;
                AvgAmount = 0;
                BuildPaymentPieChart(); // Wyczyści wykres (pusta kolekcja)
                return;
            }

            CountOfSales = List.Count;
            TotalAmount = List.Sum(s => s.Kwota);
            AvgAmount = Math.Round(List.Average(s => s.Kwota), 2);

            BuildPaymentPieChart(); // Budujemy pie chart
        }
        #endregion

        #region PieChart - formy płatności
        private void BuildPaymentPieChart()
        {
            // sumujemy kwoty w podziale na formę płatności
            if (List == null || List.Count == 0)
            {
                SeriesCollectionPayment = new SeriesCollection();
                return;
            }

            var grouped = List
                .GroupBy(s => s.Forma_Płatności)
                .Select(g => new
                {
                    Forma = g.Key,
                    Suma = g.Sum(x => x.Kwota)
                })
                .Where(x => !string.IsNullOrEmpty(x.Forma))
                .ToList();

            var newSeries = new SeriesCollection();

            foreach (var item in grouped)
            {
                newSeries.Add(
                    new PieSeries
                    {
                        Title = item.Forma,              // "karta", "gotówka", "blik"
                        Values = new LiveCharts.ChartValues<decimal> { item.Suma },
                        DataLabels = true
                    }
                );
            }

            SeriesCollectionPayment = newSeries;
        }
        #endregion

        #region Export CSV
        private void ExportCsv()
        {
            try
            {
                string csvPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    "sprzedaz_export.csv"
                );

                using (var writer = new StreamWriter(csvPath))
                {
                    writer.WriteLine("ID_Sprzedaży;Nazwa_Leku;PESEL_Pacjenta;Data_Sprzedaży;Kwota;Forma_Płatności");

                    foreach (var sale in List)
                    {
                        writer.WriteLine(
                            $"{sale.ID_Sprzedaży};" +
                            $"{sale.Nazwa_Leku};" +
                            $"{sale.PESEL_Pacjenta};" +
                            $"{sale.Data_Sprzedaży:yyyy-MM-dd};" +
                            $"{sale.Kwota};" +
                            $"{sale.Forma_Płatności}"
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
