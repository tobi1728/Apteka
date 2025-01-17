using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMFirma.ViewModels
{
    public class AllDrugCategoriesViewModel : AllViewModel<DrugCategoryForAllView>
    {
        #region Constructor
        public AllDrugCategoriesViewModel()
            : base("Wszystkie kategorie leków")
        {
        }
        #endregion

        #region Sort & Find 
        // tu decydujemy po czym sortowac
        public override List<string> GetComboboxSortList()
        {
            return new List<string> { "Nazwa Kategorii", "ID" };
        }

        public override void Sort()
        {
            if (SortField == "Nazwa Kategorii")
            {
                List = new ObservableCollection<DrugCategoryForAllView>(
                    List.OrderBy(item => item.Nazwa_Kategorii).ToList()
                );
            }
            else if (SortField == "ID")
            {
                List = new ObservableCollection<DrugCategoryForAllView>(
                    List.OrderBy(item => item.ID_Kategorii).ToList()
                );
            }
        }

        // tu decydujemy po czym wyszukiwac
        public override List<string> GetComboboxFindList()
        {
            return new List<string> { "Nazwa Kategorii", "Opis" };
        }

        public override void Find()
        {
            Load(); // Przywracamy pełną listę przed wyszukiwaniem
            if (FindField == "Nazwa Kategorii")
            {
                List = new ObservableCollection<DrugCategoryForAllView>(
                    List.Where(item => item.Nazwa_Kategorii != null && item.Nazwa_Kategorii.StartsWith(FindTextBox, StringComparison.OrdinalIgnoreCase)).ToList()
                );
            }
            else if (FindField == "Opis")
            {
                List = new ObservableCollection<DrugCategoryForAllView>(
                    List.Where(item => item.Opis != null && item.Opis.StartsWith(FindTextBox, StringComparison.OrdinalIgnoreCase)).ToList()
                );
            }
        }

        #endregion

        #region Helpers
        public override void Load()
        {
            List = new ObservableCollection<DrugCategoryForAllView>
            (
                from category in aptekaEntities.Kategorie_Leków
                select new DrugCategoryForAllView
                {
                    ID_Kategorii = category.ID_Kategorii,
                    Nazwa_Kategorii = category.Nazwa_Kategorii,
                    Opis = category.Opis
                }
            );
        }
        #endregion
    }
}
