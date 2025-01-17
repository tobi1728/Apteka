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
    public class AllPrescriptionsViewModel : AllViewModel<PrescriptionForAllView>
    {
        #region Constructor
        public AllPrescriptionsViewModel()
            : base("Wszystkie recepty")
        {
        }
        #endregion
        #region Sort & Find 
        // tu decydujemy po czym sortowac
        public override List<string> GetComboboxSortList()
        {
            return new List<string> { "PESEL", "Numer Licencji", "Data Wystawienia" };
        }

        public override void Sort()
        {
            if (SortField == "PESEL")
                List = new ObservableCollection<PrescriptionForAllView>(List.OrderBy(item => item.PESEL).ToList());
            else if (SortField == "Numer Licencji")
                List = new ObservableCollection<PrescriptionForAllView>(List.OrderBy(item => item.Numer_Licencji).ToList());
            else if (SortField == "Data Wystawienia")
                List = new ObservableCollection<PrescriptionForAllView>(List.OrderBy(item => item.Data_Wystawienia).ToList());
        }

        public override List<string> GetComboboxFindList()
        {
            return new List<string> { "PESEL", "Numer Licencji" };
        }

        public override void Find()
        {
            Load();
            if (FindField == "PESEL")
                List = new ObservableCollection<PrescriptionForAllView>(List.Where(item => item.PESEL != null && item.PESEL.StartsWith(FindTextBox, StringComparison.OrdinalIgnoreCase)).ToList());
            else if (FindField == "Numer Licencji")
                List = new ObservableCollection<PrescriptionForAllView>(List.Where(item => item.Numer_Licencji != null && item.Numer_Licencji.StartsWith(FindTextBox, StringComparison.OrdinalIgnoreCase)).ToList());
        }


        #endregion
        #region Helpers
        public override void Load()
        {
            List = new ObservableCollection<PrescriptionForAllView>
                (
                    from prescription in aptekaEntities.Recepty // dla kazdej recepty z bazy danych recept
                    select new PrescriptionForAllView // tworzymy nowy PresriptionForAllView
                    {
                        ID_Recepty = prescription.ID_Recepty,
                        PESEL = prescription.Pacjenci.PESEL,
                        Numer_Licencji = prescription.Farmaceuci.Numer_Licencji,
                        Data_Wystawienia = prescription.Data_Wystawienia,
                        Data_Realizacji = prescription.Data_Realizacji                        
                    }

                );
        }
        #endregion

    }
}