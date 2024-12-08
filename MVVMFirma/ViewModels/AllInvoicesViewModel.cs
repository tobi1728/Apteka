using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
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
    public class AllInvoicesViewModel : WorkspaceViewModel
    {
        #region DB
        private readonly AptekaEntities aptekaEntities; // to jest pole, ktore reprezentuje baze danych

        #endregion
        #region LoadCommand
        private BaseCommand _LoadCommand; // to jest komenda ktora bedzie wywolywala funkcje Load pobierajaca z bazy danych 

        public ICommand LoadCommand
        {
            get
            { if (_LoadCommand == null)
                    _LoadCommand = new BaseCommand(() => load());
            return _LoadCommand;
            }
        }
        #endregion
        #region List
        private ObservableCollection<Faktury_Dostawców> _List; // tu beda przechowywane produkty pobrane z DB

        public ObservableCollection<Faktury_Dostawców> List
        {
            get
            {
                if (_List == null)
                    load();
                return _List;
            }
            set
            {
                _List = value;
                OnPropertyChanged(() => List); // to jest zlecenie odswiezenia listy na interfejsie
            }
        }
        #endregion

        #region Constructor
        public AllInvoicesViewModel()
        {
            base.DisplayName = "Faktury";
            aptekaEntities = new AptekaEntities();
        }
        #endregion

        #region Helpers
        // metoda load pobierze wszystkie produkty z bazy danych
        private void load()
        {
            List = new ObservableCollection<Faktury_Dostawców>
                (
                    aptekaEntities.Faktury_Dostawców.ToList()
                // z bazdy danych pobieram Faktury_Dostawców i zamieniam wszystkie rekordy na liste
                );
        }
        #endregion

    }
}
