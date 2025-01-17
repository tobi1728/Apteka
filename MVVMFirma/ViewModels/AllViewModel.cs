using GalaSoft.MvvmLight.Messaging;
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

        #region Command
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

        private BaseCommand _AddCommand; // to jest komenda ktora bedzie wywolywala funkcje Add wywolujaca okno do dodawania i bedzie podpieta pod dodaj

        public ICommand AddCommand
        {
            get
            {
                if (_AddCommand == null)
                    _AddCommand = new BaseCommand(() => add());
                return _AddCommand;
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
        public AllViewModel(string displayName)
        {
            aptekaEntities = new AptekaEntities();
            base.DisplayName = displayName;
        }
        #endregion

        #region Sort & Filter
        public string SortField { get; set; }
        public List<string> SortComboboxItems
        {
            get
            {
                return GetComboboxSortList();
            }
        }
        public abstract List<string> GetComboboxSortList();

        private BaseCommand _SortCommand;

        public ICommand SortCommand
        {
            get
            {
                if (_SortCommand == null)
                    _SortCommand = new BaseCommand(() => Sort());
                return _SortCommand;

            }
        }


        public abstract void Sort();

        public string FindField { get; set; }
        public List<string>FindComboboxItems
        {
            get
            {
                return GetComboboxFindList();
            }
        }

        public abstract List<string> GetComboboxFindList();
        public string FindTextBox { get; set; }
        private BaseCommand _FindCommand;

        public ICommand FindCommand
        {
            get
            {
                if (_FindCommand == null)
                    _FindCommand = new BaseCommand(() => Find());
                return _FindCommand;

            }
        }
        public abstract void Find();

        #endregion

        #region Helpers
        private void add()
        {
            // messenger jest z biblioteki MVVM light, dzieki jej wysylami do innych obiektow komunikat Display Name Add gdzie display name jest nazwa widoku
            // ten komunikat odbierze mainwindowviewmodel, ktore jest odpowiedzialne za otwieranie okien
            Messenger.Default.Send(DisplayName + "Add");
        }
        public abstract void Load();
        #endregion
    }
}
