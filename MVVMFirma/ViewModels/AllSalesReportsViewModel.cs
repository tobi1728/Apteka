using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
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
