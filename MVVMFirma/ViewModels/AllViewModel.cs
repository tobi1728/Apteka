using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MVVMFirma.ViewModels
{
    public abstract class AllViewModel<T>:WorkspaceViewModel // pod T beda podstawiane konkretne typy elementow listy
    {
        #region DB
        protected readonly AptekaEntities aptekaEntities; // to jest pole, ktore reprezentuje baze danych

        #endregion

        #region LoadCommand
        private BaseCommand _LoadCommand; // to jest komenda ktora bedzie wywolywala funkcje Load pobierajaca z bazy danych 

        public ICommand LoadCommand
        {
            get
            {
                if (_LoadCommand == null)
                    _LoadCommand = new BaseCommand(() => Load());
                return _LoadCommand;
            }
        }
        #endregion

        #region List
        private ObservableCollection<T> _List; // tu beda przechowywane produkty pobrane z DB

        public ObservableCollection<T> List
        {
            get
            {
                if (_List == null)
                    Load();
                return _List;
            }
            set
            {
                _List = value;
                OnPropertyChanged(() => List);  // to jest zlecenie odswiezenia listy na interfejsie
            }
        }
        #endregion

        #region Constructor
        public AllViewModel()
        {
            aptekaEntities = new AptekaEntities();
        }
        #endregion

        #region Helpers
        public abstract void Load();
        #endregion
    }
}
