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

// iTextSharp do PDF
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace MVVMFirma.ViewModels
{
    public class AllInvoicesViewModel : AllViewModel<InvoiceForAllView>
    {
        #region Pola / Properties

        // Lista oryginalna (bez filtrów)
        private List<InvoiceForAllView> _allInvoices;

        // Filtrowanie: Data od/do
        private DateTime? _dataOd;
        public DateTime? DataOd
        {
            get => _dataOd;
            set
            {
                _dataOd = value;
                OnPropertyChanged(() => DataOd);
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

        // Filtrowanie: Kwota od/do
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

        // Zaznaczona faktura
        private InvoiceForAllView _selectedInvoice;
        public InvoiceForAllView SelectedInvoice
        {
            get => _selectedInvoice;
            set
            {
                _selectedInvoice = value;
                OnPropertyChanged(() => SelectedInvoice);
            }
        }

        // Statystyki
        private int _countOfInvoices;
        public int CountOfInvoices
        {
            get => _countOfInvoices;
            set
            {
                _countOfInvoices = value;
                OnPropertyChanged(() => CountOfInvoices);
            }
        }

        private decimal _sumOfInvoices;
        public decimal SumOfInvoices
        {
            get => _sumOfInvoices;
            set
            {
                _sumOfInvoices = value;
                OnPropertyChanged(() => SumOfInvoices);
            }
        }

        #endregion

        #region Konstruktor
        public AllInvoicesViewModel()
            : base("Wszystkie faktury dostawców")
        {
            PrintCommand = new BaseCommand(() => PrintSelectedInvoice());
            FilterCommand = new BaseCommand(() => Filter());
            ExportCsvCommand = new BaseCommand(() => ExportCsv());
        }
        #endregion

        #region Komendy

        private ICommand _printCommand;
        public ICommand PrintCommand
        {
            get => _printCommand;
            set
            {
                _printCommand = value;
                OnPropertyChanged(() => PrintCommand);
            }
        }

        private ICommand _filterCommand;
        public ICommand FilterCommand
        {
            get => _filterCommand;
            set
            {
                _filterCommand = value;
                OnPropertyChanged(() => FilterCommand);
            }
        }

        // NOWA komenda: Export CSV
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

        #endregion

        #region Metody wirtualne z AllViewModel<T>

        public override List<string> GetComboboxSortList()
        {
            return new List<string> { "Numer Faktury", "Nazwa Dostawcy", "Data Wystawienia", "Kwota" };
        }

        public override void Sort()
        {
            if (SortField == "Numer Faktury")
            {
                List = new ObservableCollection<InvoiceForAllView>(List.OrderBy(item => item.Numer_Faktury));
            }
            else if (SortField == "Nazwa Dostawcy")
            {
                List = new ObservableCollection<InvoiceForAllView>(List.OrderBy(item => item.Nazwa_Dostawcy));
            }
            else if (SortField == "Data Wystawienia")
            {
                List = new ObservableCollection<InvoiceForAllView>(List.OrderBy(item => item.Data_Wystawienia));
            }
            else if (SortField == "Kwota")
            {
                List = new ObservableCollection<InvoiceForAllView>(List.OrderBy(item => item.Kwota));
            }

            UpdateStatistics();
        }

        public override List<string> GetComboboxFindList()
        {
            return new List<string> { "Numer Faktury", "Nazwa Dostawcy" };
        }

        public override void Find()
        {
            Load();

            if (FindField == "Numer Faktury")
            {
                List = new ObservableCollection<InvoiceForAllView>(
                    List.Where(item => item.Numer_Faktury != null &&
                                       item.Numer_Faktury.StartsWith(FindTextBox, StringComparison.OrdinalIgnoreCase))
                );
            }
            else if (FindField == "Nazwa Dostawcy")
            {
                List = new ObservableCollection<InvoiceForAllView>(
                    List.Where(item => item.Nazwa_Dostawcy != null &&
                                       item.Nazwa_Dostawcy.StartsWith(FindTextBox, StringComparison.OrdinalIgnoreCase))
                );
            }

            UpdateStatistics();
        }

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
                                    Numer_Zamówienia = invoice.Zamówienia.ID_Zamówienia.ToString()
                                };

            _allInvoices = invoicesQuery.ToList();
            List = new ObservableCollection<InvoiceForAllView>(_allInvoices);

            UpdateStatistics();
        }

        #endregion

        #region Filtrowanie + Statystyki

        private void Filter()
        {
            if (_allInvoices == null)
                return;

            var filtered = _allInvoices.AsEnumerable();

            // Data od
            if (DataOd.HasValue)
                filtered = filtered.Where(f => f.Data_Wystawienia >= DataOd.Value);

            // Data do
            if (DataDo.HasValue)
                filtered = filtered.Where(f => f.Data_Wystawienia <= DataDo.Value);

            // Kwota od
            if (KwotaOd.HasValue)
                filtered = filtered.Where(f => f.Kwota >= KwotaOd.Value);

            // Kwota do
            if (KwotaDo.HasValue)
                filtered = filtered.Where(f => f.Kwota <= KwotaDo.Value);

            List = new ObservableCollection<InvoiceForAllView>(filtered.ToList());

            UpdateStatistics();
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
            decimal sum = 0;
            foreach (var invoice in List)
            {
                sum += invoice.Kwota;
            }
            SumOfInvoices = sum;
        }

        #endregion

        #region Export CSV

        private void ExportCsv()
        {
            try
            {
                // Przykładowa ścieżka: Pulpit, plik "invoices_export.csv"
                string csvPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    "invoices_export.csv"
                );

                using (var writer = new StreamWriter(csvPath))
                {
                    // Nagłówek (pierwsza linia)
                    writer.WriteLine("ID_Faktury;Numer_Faktury;Dostawca;Data_Wystawienia;Kwota;Numer_Zamowienia");

                    // Kolejne wiersze z danymi
                    foreach (var inv in List)
                    {
                        // Format CSV: dane rozdzielone ";"
                        // Data_Wystawienia d => np. 2023-05-01
                        // Ewentualnie .ToString("yyyy-MM-dd") itp. w zależności od potrzeb
                        writer.WriteLine($"{inv.ID_Faktury};{inv.Numer_Faktury};{inv.Nazwa_Dostawcy};{inv.Data_Wystawienia:d};{inv.Kwota};{inv.Numer_Zamówienia}");
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

        #region Drukowanie PDF

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

                doc.Add(new Paragraph($"Faktura numer: {SelectedInvoice.Numer_Faktury}"));
                doc.Add(new Paragraph($"Dostawca: {SelectedInvoice.Nazwa_Dostawcy}"));
                doc.Add(new Paragraph($"Data wystawienia: {SelectedInvoice.Data_Wystawienia:d}"));
                doc.Add(new Paragraph($"Kwota: {SelectedInvoice.Kwota}"));
                doc.Add(new Paragraph($"Numer Zamówienia: {SelectedInvoice.Numer_Zamówienia}"));

                doc.Close();
                writer.Close();
            }
        }

        #endregion
    }
}
