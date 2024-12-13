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
    public class AllProducersViewModel : AllViewModel<ProducerForAllView>
    {
        #region Constructor
        public AllProducersViewModel()
            : base("Wszyscy Producenci")
        {
        }
        #endregion

        #region Helpers
        public override void Load()
        {
            List = new ObservableCollection<ProducerForAllView>
            (
                from producer in aptekaEntities.Producent_Leków
                select new ProducerForAllView
                {
                    ID_Producenta = producer.ID_Producenta,
                    Nazwa_Producenta = producer.Nazwa_Producenta,
                    Telefon = producer.Telefon,
                    Ulica = producer.Ulica,
                    Miasto = producer.Miasto,
                    Kod_Pocztowy = producer.Kod_Pocztowy
                }
            );
        }
        #endregion
    }
}
