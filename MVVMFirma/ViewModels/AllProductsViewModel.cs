using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Helper;
using MVVMFirma.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MVVMFirma.ViewModels
{
    public class AllProductsViewModel : AllViewModel<ProductForAllView>
    {
        // --------------------------------------
        // 1) Pola i właściwości do mechanizmu modalnego
        // --------------------------------------

        private ProductForAllView _selectedProduct;
        public ProductForAllView SelectedProduct
        {
            get => _selectedProduct;
            set
            {
                _selectedProduct = value;
                OnPropertyChanged(() => SelectedProduct);

                // Jeśli okno jest uruchomione w trybie modalnym i user wybrał wiersz
                if (_selectedProduct != null && IsModal)
                {
                    // Wysyłamy wybrany produkt do VM, który nas otworzył (np. NewWarehouseViewModel)
                    Messenger.Default.Send(_selectedProduct);
                    // Zamykamy okno
                    OnRequestClose();
                }
            }
        }

        public bool IsModal { get; set; } = false;

        // --------------------------------------
        // 2) Konstruktor
        // --------------------------------------

        public AllProductsViewModel()
            : base("Wszystkie leki")
        {
        }

        // --------------------------------------
        // 3) Implementacja abstrakcyjnych metod z AllViewModel<T>
        // --------------------------------------

        /// <summary>
        /// Wczytanie listy produktów z bazy.
        /// </summary>
        public override void Load()
        {
            // Minimalna implementacja: np. wczytaj listę do List
            // Jeżeli chcesz – zaimplementuj logikę analogiczną do starej wersji,
            // np.:
            List = new ObservableCollection<ProductForAllView>(
                from product in aptekaEntities.Leki
                select new ProductForAllView
                {
                    ID_Leku = product.ID_Leku,
                    Nazwa_Leku = product.Nazwa_Leku,
                    Opis = product.Opis,
                    Nazwa_Kategorii = product.Kategorie_Leków.Nazwa_Kategorii,
                    Cena_Zakupu = product.Cena_Zakupu,
                    Cena_Sprzedaży = product.Cena_Sprzedaży,
                    Data_Waznosci = product.Data_Waznosci,
                    Nazwa_Producenta = product.Producent_Leków.Nazwa_Producenta,
                    Na_Recepte = product.Na_Recepte,
                    Refundacja = product.Refundacja
                }
            );
        }

        /// <summary>
        /// Metoda Sort – decyduje wg jakiego pola sortujemy.
        /// </summary>
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
        }

        /// <summary>
        /// Metoda Find – wyszukiwanie wg wybranego pola.
        /// </summary>
        public override void Find()
        {
            // Dla prostoty, minimalna implementacja:
            // Przywracamy pierwotną listę
            Load();

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
        }

        /// <summary>
        /// Lista pól, wg których można sortować.
        /// </summary>
        public override List<string> GetComboboxSortList()
        {
            return new List<string> { "Nazwa", "Kategoria", "Cena sprzedaży", "Cena zakupu", "Data ważności" };
        }

        /// <summary>
        /// Lista pól, wg których można wyszukiwać.
        /// </summary>
        public override List<string> GetComboboxFindList()
        {
            return new List<string> { "Nazwa", "Kategoria", "Producent" };
        }

        // --------------------------------------
        // 4) Ewentualne dodatkowe metody/komendy
        //     (ExportCsvCommand czy FilterCommand) 
        //     możesz tu zaimplementować wedle potrzeb.
        // --------------------------------------
    }
}
