using GalaSoft.MvvmLight.Messaging;
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
    public class AllSuppliersViewModel : AllViewModel<SupplierForAllView>
    {
        #region Constructor
        public AllSuppliersViewModel()
            : base("Wszyscy dostawcy")
        {
        }
        #endregion
        #region Properties
        private Dostawcy _SelectedSupplier;
        public Dostawcy SelectedSupplier
        {
            get
            {
                return _SelectedSupplier;
            }
            set
            {
                _SelectedSupplier = value;
                //Messengerem wysyalmy wybranego Kontrahenta do okna z Faktura
                Messenger.Default.Send(_SelectedSupplier);
                //Zamykamy okno po wybraniu
                OnRequestClose();
            }
        }
        #endregion
        #region Helpers
        public override void Load()
        {
            List = new ObservableCollection<SupplierForAllView>
            (
                from supplier in aptekaEntities.Dostawcy // Dla każdego dostawcy z bazy
                select new SupplierForAllView
                {
                    ID_Dostawcy = supplier.ID_Dostawcy,
                    Nazwa = supplier.Nazwa,
                    Telefon = supplier.Telefon,
                    Ulica = supplier.Ulica,
                    Miasto = supplier.Miasto,
                    Kod_Pocztowy = supplier.Kod_Pocztowy
                }
            );
        }
        #endregion



    }
}
