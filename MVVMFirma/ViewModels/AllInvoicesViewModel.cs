using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using MVVMFirma.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

// Dla PDF (przykład z iTextSharp):
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Diagnostics;

namespace MVVMFirma.ViewModels
{
    public class AllInvoicesViewModel : AllViewModel<InvoiceForAllView>
    {
        #region Pola / Properties

        // Przechowujemy pełną listę (bez filtrów)
        private List<InvoiceForAllView> _allInvoices;

        // Filtrowanie – data od/do
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

        // Filtrowanie – kwota od/do
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

        // Zaznaczona faktura (do drukowania)
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

        #endregion

        #region Konstruktor
        public AllInvoicesViewModel()
            : base("Wszystkie faktury dostawców")
        {
            // Komenda Drukuj
            PrintCommand = new BaseCommand(() => PrintSelectedInvoice());
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
        #endregion

        #region Metody wirtualne z AllViewModel<T>

        // 1) Lista dostępnych pól do sortowania
        public override List<string> GetComboboxSortList()
        {
            return new List<string> { "Numer Faktury", "Nazwa Dostawcy", "Data Wystawienia", "Kwota" };
        }

        // 2) Sortowanie wg wybranego pola
        public override void Sort()
        {
            if (SortField == "Numer Faktury")
            {
                List = new ObservableCollection<InvoiceForAllView>(
                    List.OrderBy(item => item.Numer_Faktury)
                );
            }
            else if (SortField == "Nazwa Dostawcy")
            {
                List = new ObservableCollection<InvoiceForAllView>(
                    List.OrderBy(item => item.Nazwa_Dostawcy)
                );
            }
            else if (SortField == "Data Wystawienia")
            {
                List = new ObservableCollection<InvoiceForAllView>(
                    List.OrderBy(item => item.Data_Wystawienia)
                );
            }
            else if (SortField == "Kwota")
            {
                List = new ObservableCollection<InvoiceForAllView>(
                    List.OrderBy(item => item.Kwota)
                );
            }
        }

        // 3) Lista dostępnych pól do wyszukiwania
        public override List<string> GetComboboxFindList()
        {
            return new List<string> { "Numer Faktury", "Nazwa Dostawcy" };
        }

        // 4) Wyszukiwanie (Find)
        public override void Find()
        {
            // Przywracamy pełną listę
            Load();

            if (FindField == "Numer Faktury")
            {
                List = new ObservableCollection<InvoiceForAllView>(
                    List.Where(item => item.Numer_Faktury != null
                        && item.Numer_Faktury.StartsWith(FindTextBox, StringComparison.OrdinalIgnoreCase))
                );
            }
            else if (FindField == "Nazwa Dostawcy")
            {
                List = new ObservableCollection<InvoiceForAllView>(
                    List.Where(item => item.Nazwa_Dostawcy != null
                        && item.Nazwa_Dostawcy.StartsWith(FindTextBox, StringComparison.OrdinalIgnoreCase))
                );
            }
        }

        // 5) Wczytanie danych z bazy
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

            // Pełna lista (bez filtrów)
            _allInvoices = invoicesQuery.ToList();

            // Domyślnie – wyświetlamy wszystko
            List = new ObservableCollection<InvoiceForAllView>(_allInvoices);
        }

        #endregion

        #region Filtrowanie
        private void Filter()
        {
            if (_allInvoices == null)
                return;

            // Zaczynamy od pełnej listy
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

            // Wynik do List
            List = new ObservableCollection<InvoiceForAllView>(filtered.ToList());
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
                // Ścieżka docelowa pliku – przykładowo Pulpit
                string pdfPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    $"Faktura_{SelectedInvoice.Numer_Faktury}.pdf"
                );

                // Generujemy PDF
                GeneratePdf(pdfPath);

                // Otwieramy plik w domyślnym programie do PDF
                Process.Start(pdfPath);
            }
            catch (Exception ex)
            {
                ShowMessageBox($"Błąd przy generowaniu PDF: {ex.Message}");
            }
        }

        // Metoda tworzy przykładowy PDF z danymi o fakturze
        private void GeneratePdf(string pdfPath)
        {
            using (var fs = new FileStream(pdfPath, FileMode.Create, FileAccess.Write))
            {
                var doc = new Document(PageSize.A4, 50, 50, 25, 25);
                var writer = PdfWriter.GetInstance(doc, fs);
                doc.Open();

                // Proste dane faktury
                doc.Add(new Paragraph($"Faktura numer: {SelectedInvoice.Numer_Faktury}"));
                doc.Add(new Paragraph($"Dostawca: {SelectedInvoice.Nazwa_Dostawcy}"));
                doc.Add(new Paragraph($"Data wystawienia: {SelectedInvoice.Data_Wystawienia:d}"));
                doc.Add(new Paragraph($"Kwota: {SelectedInvoice.Kwota}"));
                doc.Add(new Paragraph($"Numer Zamówienia: {SelectedInvoice.Numer_Zamówienia}"));

                // Rozbudowane wydruki: doc.Add( ... ) Tabele, style, itd.

                doc.Close();
                writer.Close();
            }
        }

        #endregion
    }
}
