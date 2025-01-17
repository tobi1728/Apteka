using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace MVVMFirma.ViewModels
{
    public class AllSalesReportsViewModel : AllViewModel<SalesReportForAllView>
    {
        #region Constructor
        public AllSalesReportsViewModel()
            : base("Wszystkie raporty sprzedaży")
        {
        }
        #endregion
        #region Sort & Find 
        // tu decydujemy po czym sortowac
        public override List<string> GetComboboxSortList()
        {
            return new List<string> { "Data Rozpoczęcia", "Data Zakończenia", "Łączna Sprzedaż", "Liczba Transakcji" };
        }

        public override void Sort()
        {
            if (SortField == "Data Rozpoczęcia")
                List = new ObservableCollection<SalesReportForAllView>(List.OrderBy(item => item.Data_Rozpoczęcia).ToList());
            else if (SortField == "Data Zakończenia")
                List = new ObservableCollection<SalesReportForAllView>(List.OrderBy(item => item.Data_Zakończenia).ToList());
            else if (SortField == "Łączna Sprzedaż")
                List = new ObservableCollection<SalesReportForAllView>(List.OrderBy(item => item.Łączna_Sprzedaż).ToList());
            else if (SortField == "Liczba Transakcji")
                List = new ObservableCollection<SalesReportForAllView>(List.OrderBy(item => item.Liczba_Transakcji).ToList());
        }

        public override List<string> GetComboboxFindList()
        {
            return new List<string> { "Data Rozpoczęcia", "Data Zakończenia" };
        }

        public override void Find()
        {
            Load();
            if (FindField == "Data Rozpoczęcia")
                List = new ObservableCollection<SalesReportForAllView>(List.Where(item => item.Data_Rozpoczęcia != null && item.Data_Rozpoczęcia.ToString().StartsWith(FindTextBox, StringComparison.OrdinalIgnoreCase)).ToList());
            else if (FindField == "Data Zakończenia")
                List = new ObservableCollection<SalesReportForAllView>(List.Where(item => item.Data_Zakończenia != null && item.Data_Zakończenia.ToString().StartsWith(FindTextBox, StringComparison.OrdinalIgnoreCase)).ToList());
        }


        #endregion
        #region Helpers
        public override void Load()
        {
            List = new ObservableCollection<SalesReportForAllView>
            (
                from report in aptekaEntities.Raporty_Sprzedaży
                select new SalesReportForAllView
                {
                    ID_Raportu = report.ID_Raportu,
                    Data_Rozpoczęcia = report.Data_Rozpoczęcia,
                    Data_Zakończenia = report.Data_Zakończenia,
                    Łączna_Sprzedaż = report.Łączna_Sprzedaż,
                    Liczba_Transakcji = report.Liczba_Transakcji
                }
            );
        }
        #endregion
    }
}
