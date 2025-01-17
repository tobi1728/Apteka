using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MVVMFirma.ViewModels
{
    public class AllProductsViewModel : AllViewModel<ProductForAllView>
    {
        #region Constructor
        public AllProductsViewModel()
            :base("Wszystkie leki")
        {
        }
        #endregion
        #region Sort & Find 
        // tu decydujemy po czym sortowac
        public override List<string> GetComboboxSortList()
        {
            return new List<string> { "Nazwa", "Kategoria", "Cena sprzedaży", "Cena zakupu", "Data ważności" };
        }

        // tu decydujemy jak sortowac
        public override void Sort()
        {
            if (SortField == "Nazwa")
            {
                List = new ObservableCollection<ProductForAllView>(
                    List.OrderBy(item => item.Nazwa_Leku).ToList()
                );
            }
            else if (SortField == "Kategoria")
            {
                List = new ObservableCollection<ProductForAllView>(
                    List.OrderBy(item => item.Nazwa_Kategorii).ToList()
                );
            }
            else if (SortField == "Cena sprzedaży")
            {
                List = new ObservableCollection<ProductForAllView>(
                    List.OrderBy(item => item.Cena_Sprzedaży).ToList()
                );
            }
            else if (SortField == "Cena zakupu")
            {
                List = new ObservableCollection<ProductForAllView>(
                    List.OrderBy(item => item.Cena_Zakupu).ToList()
                );
            }
            else if (SortField == "Data ważności")
            {
                List = new ObservableCollection<ProductForAllView>(
                    List.OrderBy(item => item.Data_Waznosci).ToList()
                );
            }
        }

        // tu decydujemy po czym wyszukiwac
        public override List<string> GetComboboxFindList()
        {
            return new List<string> { "Nazwa", "Kategoria", "Producent" };
        }

        // tu decydujemy jak wyszukiwac
        public override void Find()
        {
            Load();
            if (FindField == "Nazwa")
            {
                List = new ObservableCollection<ProductForAllView>(
                    List.Where(item => item.Nazwa_Leku != null && item.Nazwa_Leku.StartsWith(FindTextBox, StringComparison.OrdinalIgnoreCase)).ToList()
                );
            }
            else if (FindField == "Kategoria")
            {
                List = new ObservableCollection<ProductForAllView>(
                    List.Where(item => item.Nazwa_Kategorii != null && item.Nazwa_Kategorii.StartsWith(FindTextBox, StringComparison.OrdinalIgnoreCase)).ToList()
                );
            }
            else if (FindField == "Producent")
            {
                List = new ObservableCollection<ProductForAllView>(
                    List.Where(item => item.Nazwa_Producenta != null && item.Nazwa_Producenta.StartsWith(FindTextBox, StringComparison.OrdinalIgnoreCase)).ToList()
                );
            }
        }

        #endregion
        #region Helpers
        public override void Load()
        {
            List = new ObservableCollection<ProductForAllView>
                (
                    from product in aptekaEntities.Leki // dla kazdego leku z bazy danych lekow
                    select new ProductForAllView // tworzymy nowy ProductForAllView
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

                );
        }
        #endregion

    }
}