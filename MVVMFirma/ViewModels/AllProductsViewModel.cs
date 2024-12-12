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
            :base("Leki")
        {
        }
        #endregion

        #region Helpers
        // metoda load pobierze wszystkie produkty z bazy danych
        public override void Load()
        {
            List = new ObservableCollection<ProductForAllView>
                (
                    from product in aptekaEntities.Leki // dla kazdej faktury z bazy danych faktur
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