using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace MVVMFirma.ViewModels
{
    public class NewCategoryViewModel : OneViewModel<Kategorie_Leków>
    {
        #region Constructor
        public NewCategoryViewModel()
            : base("Nowa kategoria")
        {
            aptekaEntities = new AptekaEntities();
            item = new Kategorie_Leków();
        }
        #endregion

        #region Properties

        public string NazwaKategorii
        {
            get
            {
                return item.Nazwa_Kategorii;
            }
            set
            {
                item.Nazwa_Kategorii = value;
                OnPropertyChanged(() => NazwaKategorii);
            }
        }

        public string Opis
        {
            get
            {
                return item.Opis;
            }
            set
            {
                item.Opis = value;
                OnPropertyChanged(() => Opis);
            }
        }

        #endregion

        #region Helpers
        public override void Save()
        {
            if (string.IsNullOrWhiteSpace(NazwaKategorii))
            {
                throw new InvalidOperationException("Nazwa kategorii nie może być pusta.");
            }

            aptekaEntities.Kategorie_Leków.Add(item);
            aptekaEntities.SaveChanges();
        }
        #endregion
    }
}
