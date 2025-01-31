using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Input;

// Biblioteka LiveCharts
using LiveCharts;
using LiveCharts.Wpf;

// Biblioteka iTextSharp do PDF
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace MVVMFirma.ViewModels
{
    public class AllInvoicesViewModel : AllViewModel<InvoiceForAllView>
    {
        #region Pola i właściwości

        private List<InvoiceForAllView> _allInvoices;

        // Filtry
        private DateTime? _dataOd;
        public DateTime? DataOd
        {
            get => _dataOd;
            set { _dataOd = value; OnPropertyChanged(() => DataOd); Filter(); }
        }

        private DateTime? _dataDo;
        public DateTime? DataDo
        {
            get => _dataDo;
            set { _dataDo = value; OnPropertyChanged(() => DataDo); Filter(); }
        }

        private decimal? _kwotaOd;
        public decimal? KwotaOd
        {
            get => _kwotaOd;
            set { _kwotaOd = value; OnPropertyChanged(() => KwotaOd); Filter(); }
        }

        private decimal? _kwotaDo;
        public decimal? KwotaDo
        {
            get => _kwotaDo;
            set { _kwotaDo = value; OnPropertyChanged(() => KwotaDo); Filter(); }
        }

        // Wybrana faktura (do drukowania)
        private InvoiceForAllView _selectedInvoice;
        public InvoiceForAllView SelectedInvoice
        {
            get => _selectedInvoice;
            set { _selectedInvoice = value; OnPropertyChanged(() => SelectedInvoice); }
        }

        // Statystyki
        private int _countOfInvoices;
        public int CountOfInvoices
        {
            get => _countOfInvoices;
            set { _countOfInvoices = value; OnPropertyChanged(() => CountOfInvoices); }
        }

        private decimal _sumOfInvoices;
        public decimal SumOfInvoices
        {
            get => _sumOfInvoices;
            set { _sumOfInvoices = value; OnPropertyChanged(() => SumOfInvoices); }
        }

        // Wykres miesiące
        private SeriesCollection _seriesCollectionMonth;
        public SeriesCollection SeriesCollectionMonth
        {
            get => _seriesCollectionMonth;
            set { _seriesCollectionMonth = value; OnPropertyChanged(() => SeriesCollectionMonth); }
        }
        private string[] _labelsMonth;
        public string[] LabelsMonth
        {
            get => _labelsMonth;
            set { _labelsMonth = value; OnPropertyChanged(() => LabelsMonth); }
        }
        private Func<double, string> _yFormatterMonth;
        public Func<double, string> YFormatterMonth
        {
            get => _yFormatterMonth;
            set { _yFormatterMonth = value; OnPropertyChanged(() => YFormatterMonth); }
        }

        // Wykres dostawcy
        private SeriesCollection _seriesCollectionSuppliers;
        public SeriesCollection SeriesCollectionSuppliers
        {
            get => _seriesCollectionSuppliers;
            set { _seriesCollectionSuppliers = value; OnPropertyChanged(() => SeriesCollectionSuppliers); }
        }
        private string[] _labelsSuppliers;
        public string[] LabelsSuppliers
        {
            get => _labelsSuppliers;
            set { _labelsSuppliers = value; OnPropertyChanged(() => LabelsSuppliers); }
        }
        private Func<double, string> _yFormatterSuppliers;
        public Func<double, string> YFormatterSuppliers
        {
            get => _yFormatterSuppliers;
            set { _yFormatterSuppliers = value; OnPropertyChanged(() => YFormatterSuppliers); }
        }

        #endregion

        #region Konstruktor

        public AllInvoicesViewModel()
            : base("Wszystkie faktury dostawców")
        {
            PrintCommand = new BaseCommand(() => PrintSelectedInvoice());
            FilterCommand = new BaseCommand(() => Filter());
            ExportCsvCommand = new BaseCommand(() => ExportCsv());

            // Inicjowanie wykresów
            SeriesCollectionMonth = new SeriesCollection();
            LabelsMonth = new string[] { };
            YFormatterMonth = val => val.ToString("C");

            SeriesCollectionSuppliers = new SeriesCollection();
            LabelsSuppliers = new string[] { };
            YFormatterSuppliers = val => val.ToString("C");
        }

        #endregion

        #region Komendy

        private ICommand _printCommand;
        public ICommand PrintCommand
        {
            get => _printCommand;
            set { _printCommand = value; OnPropertyChanged(() => PrintCommand); }
        }

        private ICommand _filterCommand;
        public ICommand FilterCommand
        {
            get => _filterCommand;
            set { _filterCommand = value; OnPropertyChanged(() => FilterCommand); }
        }

        private ICommand _exportCsvCommand;
        public ICommand ExportCsvCommand
        {
            get => _exportCsvCommand;
            set { _exportCsvCommand = value; OnPropertyChanged(() => ExportCsvCommand); }
        }

        #endregion

        #region Nadpisanie metod z AllViewModel<InvoiceForAllView>

        public override List<string> GetComboboxSortList()
        {
            return new List<string> { "Numer Faktury", "Nazwa Dostawcy", "Data Wystawienia", "Kwota" };
        }

        public override void Sort()
        {
            if (SortField == "Numer Faktury")
            {
                List = new ObservableCollection<InvoiceForAllView>(List.OrderBy(i => i.Numer_Faktury));
            }
            else if (SortField == "Nazwa Dostawcy")
            {
                List = new ObservableCollection<InvoiceForAllView>(List.OrderBy(i => i.Nazwa_Dostawcy));
            }
            else if (SortField == "Data Wystawienia")
            {
                List = new ObservableCollection<InvoiceForAllView>(List.OrderBy(i => i.Data_Wystawienia));
            }
            else if (SortField == "Kwota")
            {
                List = new ObservableCollection<InvoiceForAllView>(List.OrderBy(i => i.Kwota));
            }

            UpdateStatistics();
            BuildChartData();
        }

        public override List<string> GetComboboxFindList()
        {
            return new List<string> { "Numer Faktury", "Nazwa Dostawcy" };
        }

        public override void Find()
        {
            // Przywracamy pełną listę
            Load();

            if (FindField == "Numer Faktury")
            {
                List = new ObservableCollection<InvoiceForAllView>(
                    List.Where(i => i.Numer_Faktury != null &&
                                    i.Numer_Faktury.StartsWith(FindTextBox, StringComparison.OrdinalIgnoreCase))
                );
            }
            else if (FindField == "Nazwa Dostawcy")
            {
                List = new ObservableCollection<InvoiceForAllView>(
                    List.Where(i => i.Nazwa_Dostawcy != null &&
                                    i.Nazwa_Dostawcy.StartsWith(FindTextBox, StringComparison.OrdinalIgnoreCase))
                );
            }

            UpdateStatistics();
            BuildChartData();
        }

        public override void Load()
        {
            var invoicesQuery = from inv in aptekaEntities.Faktury_Dostawców
                                select new InvoiceForAllView
                                {
                                    ID_Faktury = inv.ID_Faktury,
                                    Numer_Faktury = inv.Numer_Faktury,
                                    Nazwa_Dostawcy = inv.Dostawcy.Nazwa,
                                    Data_Wystawienia = inv.Data_Wystawienia,
                                    Kwota = inv.Kwota,
                                    Numer_Zamówienia = inv.Zamówienia.ID_Zamówienia.ToString()
                                };

            _allInvoices = invoicesQuery.ToList();

            List = new ObservableCollection<InvoiceForAllView>(_allInvoices);

            UpdateStatistics();
            BuildChartData();
        }

        #endregion

        #region Filtrowanie + Statystyki

        private void Filter()
        {
            if (_allInvoices == null) return;

            var filtered = _allInvoices.AsEnumerable();

            if (DataOd.HasValue)
                filtered = filtered.Where(f => f.Data_Wystawienia >= DataOd.Value);

            if (DataDo.HasValue)
                filtered = filtered.Where(f => f.Data_Wystawienia <= DataDo.Value);

            if (KwotaOd.HasValue)
                filtered = filtered.Where(f => f.Kwota >= KwotaOd.Value);

            if (KwotaDo.HasValue)
                filtered = filtered.Where(f => f.Kwota <= KwotaDo.Value);

            List = new ObservableCollection<InvoiceForAllView>(filtered.ToList());

            UpdateStatistics();
            BuildChartData();
        }

        private void UpdateStatistics()
        {
            if (List == null || List.Count == 0)
            {
                CountOfInvoices = 0;
                SumOfInvoices = 0;
                return;
            }

            CountOfInvoices = List.Count;
            SumOfInvoices = List.Sum(i => i.Kwota);
        }

        #endregion

        #region Generowanie danych do wykresów

        private void BuildChartData()
        {
            BuildChartMonth();
            BuildChartSuppliers();
        }

        private void BuildChartMonth()
        {
            var grouped = List
                .GroupBy(i => i.Data_Wystawienia.Month)
                .Select(g => new
                {
                    Miesiac = g.Key,
                    SumaKwota = g.Sum(x => x.Kwota)
                })
                .OrderBy(x => x.Miesiac)
                .ToList();

            var values = new ChartValues<decimal>();
            var labels = new List<string>();

            for (int m = 1; m <= 12; m++)
            {
                var data = grouped.FirstOrDefault(x => x.Miesiac == m);
                decimal kwotaMies = (data != null) ? data.SumaKwota : 0;
                values.Add(kwotaMies);

                labels.Add(System.Globalization.CultureInfo
                           .CurrentCulture
                           .DateTimeFormat
                           .GetAbbreviatedMonthName(m));
            }

            var colSeries = new ColumnSeries
            {
                Title = "Miesiące",
                Values = values
            };

            SeriesCollectionMonth = new SeriesCollection { colSeries };
            LabelsMonth = labels.ToArray();
        }

        private void BuildChartSuppliers()
        {
            var grouped = List
                .GroupBy(i => i.Nazwa_Dostawcy)
                .Select(g => new
                {
                    Dostawca = g.Key,
                    SumaKwota = g.Sum(x => x.Kwota)
                })
                .OrderByDescending(x => x.SumaKwota)
                .ToList();

            var values = new ChartValues<decimal>();
            var labels = new List<string>();

            foreach (var row in grouped)
            {
                values.Add(row.SumaKwota);
                labels.Add(row.Dostawca);
            }

            var colSeries = new ColumnSeries
            {
                Title = "Dostawcy",
                Values = values
            };

            SeriesCollectionSuppliers = new SeriesCollection { colSeries };
            LabelsSuppliers = labels.ToArray();
        }

        #endregion

        #region Export CSV

        private void ExportCsv()
        {
            try
            {
                string csvPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    "invoices_export.csv"
                );

                using (var writer = new StreamWriter(csvPath))
                {
                    writer.WriteLine("ID_Faktury;Numer_Faktury;Dostawca;Data_Wystawienia;Kwota;Numer_Zamowienia");
                    foreach (var inv in List)
                    {
                        writer.WriteLine($"{inv.ID_Faktury};" +
                                         $"{inv.Numer_Faktury};" +
                                         $"{inv.Nazwa_Dostawcy};" +
                                         $"{inv.Data_Wystawienia:d};" +
                                         $"{inv.Kwota};" +
                                         $"{inv.Numer_Zamówienia}"
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

        #region Drukowanie PDF (bardziej zbliżone do prawdziwej faktury)

        private ICommand _printPdfCommand;
        public ICommand PrintPdfCommand
        {
            get
            {
                if (_printPdfCommand == null)
                    _printPdfCommand = new BaseCommand(() => PrintSelectedInvoice());
                return _printPdfCommand;
            }
        }

        private void PrintSelectedInvoice()
        {
            if (SelectedInvoice == null)
            {
                ShowMessageBox("Nie zaznaczono faktury do wydruku!");
                return;
            }

            try
            {
                string pdfPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    $"Faktura_{SelectedInvoice.Numer_Faktury}.pdf"
                );

                GeneratePdf(pdfPath);
                Process.Start(pdfPath);
            }
            catch (Exception ex)
            {
                ShowMessageBox($"Błąd przy generowaniu PDF: {ex.Message}");
            }
        }

        private void GeneratePdf(string pdfPath)
        {
            using (var fs = new FileStream(pdfPath, FileMode.Create, FileAccess.Write))
            {
                var doc = new Document(PageSize.A4, 50, 50, 25, 25);
                var writer = PdfWriter.GetInstance(doc, fs);
                doc.Open();

                // Nagłówek faktury
                var fontTitle = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16);
                var titlePara = new Paragraph($"Faktura nr {SelectedInvoice.Numer_Faktury}\n", fontTitle);
                titlePara.Alignment = Element.ALIGN_CENTER;
                doc.Add(titlePara);

                doc.Add(new Paragraph($"Data wystawienia: {SelectedInvoice.Data_Wystawienia:yyyy-MM-dd}"));
                doc.Add(new Paragraph($"Numer zamówienia: {SelectedInvoice.Numer_Zamówienia}\n"));

                // Sprzedawca + Dostawca sekcja
                // (Tu w realnym systemie pobrałbyś info o sprzedawcy z bazy, my robimy “mock”)
                var chunkSprzedawca = new Chunk("Sprzedawca:\nApteka Sp. z o.o.\nul. Medyczna 12\n00-001 Warszawa\nNIP: 1234567890", FontFactory.GetFont(FontFactory.HELVETICA, 10));
                var chunkNabywca = new Chunk($"Nabywca (dostawca):\n{SelectedInvoice.Nazwa_Dostawcy}\n\n", FontFactory.GetFont(FontFactory.HELVETICA, 10));

                var sprzedawcaPara = new Paragraph(chunkSprzedawca);
                var nabywcaPara = new Paragraph(chunkNabywca);

                // Dodamy dwie kolumny w tabeli
                var tableHeader = new PdfPTable(2)
                {
                    WidthPercentage = 100
                };
                tableHeader.SetWidths(new float[] { 50, 50 });

                var cellLeft = new PdfPCell(sprzedawcaPara)
                {
                    Border = Rectangle.NO_BORDER
                };
                var cellRight = new PdfPCell(nabywcaPara)
                {
                    Border = Rectangle.NO_BORDER
                };
                tableHeader.AddCell(cellLeft);
                tableHeader.AddCell(cellRight);

                doc.Add(tableHeader);

                doc.Add(new Paragraph("\n"));

                // Sekcja "Pozycje faktury" – uproszczona (1 pozycja)
                var itemsTable = new PdfPTable(5)
                {
                    WidthPercentage = 100
                };
                itemsTable.SetWidths(new float[] { 10, 40, 15, 15, 20 }); // Lp, Nazwa, Ilość, Cena, Razem

                // Nagłówki
                itemsTable.AddCell(new PdfPCell(new Phrase("LP", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10))));
                itemsTable.AddCell(new PdfPCell(new Phrase("Nazwa uslugi/towaru", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10))));
                itemsTable.AddCell(new PdfPCell(new Phrase("Ilosc", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10))));
                itemsTable.AddCell(new PdfPCell(new Phrase("Cena", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10))));
                itemsTable.AddCell(new PdfPCell(new Phrase("Wartosc", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10))));

                // Przykładowa 1 pozycja na fakturze
                itemsTable.AddCell(new PdfPCell(new Phrase("1")));
                itemsTable.AddCell(new PdfPCell(new Phrase($"Faktura dostawcy\n{SelectedInvoice.Numer_Faktury}")));
                itemsTable.AddCell(new PdfPCell(new Phrase("1")));
                itemsTable.AddCell(new PdfPCell(new Phrase($"{SelectedInvoice.Kwota} zl")));
                itemsTable.AddCell(new PdfPCell(new Phrase($"{SelectedInvoice.Kwota} zl")));

                doc.Add(itemsTable);

                doc.Add(new Paragraph("\n"));

                // Podsumowanie
                var totalParagraph = new Paragraph(
                    $"Do zaplaty (brutto): {SelectedInvoice.Kwota:0.00} zl",
                    FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12)
                );
                totalParagraph.Alignment = Element.ALIGN_RIGHT;
                doc.Add(totalParagraph);

                // Informacje dodatkowe
                doc.Add(new Paragraph("\nDziekujemy za skorzystanie z uslug Apteka Sp. z o.o."));

                doc.Close();
                writer.Close();
            }
        }

        #endregion
    }
}
