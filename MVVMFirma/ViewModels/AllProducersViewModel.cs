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
    public class AllProducersViewModel : AllViewModel<ProducerForAllView>
    {
        #region Constructor
        public AllProducersViewModel()
            : base("Wszyscy Producenci")
        {
        }
        #endregion
        #region Sort & Find 
        // tu decydujemy po czym sortowac
        public override List<string> GetComboboxSortList()
        {
            return new List<string> { "Nazwa Producenta", "Miasto", "Kod Pocztowy" };
        }

        private ProducerForAllView _SelectedProducer;
        public ProducerForAllView SelectedProducer
        {
            get => _SelectedProducer;
            set
            {
                _SelectedProducer = value;
                if (_SelectedProducer != null)
                {
                    Messenger.Default.Send(_SelectedProducer);
                    OnRequestClose();
                }
            }
        }



        public override void Sort()
        {
            if (SortField == "Nazwa Producenta")
                List = new ObservableCollection<ProducerForAllView>(List.OrderBy(item => item.Nazwa_Producenta).ToList());
            else if (SortField == "Miasto")
                List = new ObservableCollection<ProducerForAllView>(List.OrderBy(item => item.Miasto).ToList());
            else if (SortField == "Kod Pocztowy")
                List = new ObservableCollection<ProducerForAllView>(List.OrderBy(item => item.Kod_Pocztowy).ToList());
        }

        public override List<string> GetComboboxFindList()
        {
            return new List<string> { "Nazwa Producenta", "Miasto" };
        }

        public override void Find()
        {
            Load();
            if (FindField == "Nazwa Producenta")
                List = new ObservableCollection<ProducerForAllView>(List.Where(item => item.Nazwa_Producenta != null && item.Nazwa_Producenta.StartsWith(FindTextBox, StringComparison.OrdinalIgnoreCase)).ToList());
            else if (FindField == "Miasto")
                List = new ObservableCollection<ProducerForAllView>(List.Where(item => item.Miasto != null && item.Miasto.StartsWith(FindTextBox, StringComparison.OrdinalIgnoreCase)).ToList());
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
