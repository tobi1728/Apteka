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
