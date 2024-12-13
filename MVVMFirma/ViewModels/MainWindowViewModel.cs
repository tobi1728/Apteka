using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using MVVMFirma.Helper;
using System.Diagnostics;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows.Data;
using GalaSoft.MvvmLight.Messaging;

namespace MVVMFirma.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        #region Fields
        private ReadOnlyCollection<CommandViewModel> _Commands;
        private ObservableCollection<WorkspaceViewModel> _Workspaces;
        #endregion

        #region Commands
        public ReadOnlyCollection<CommandViewModel> Commands
        {
            get
            {
                if (_Commands == null)
                {
                    List<CommandViewModel> cmds = this.CreateCommands();
                    _Commands = new ReadOnlyCollection<CommandViewModel>(cmds);
                }
                return _Commands;
            }
        }
        private List<CommandViewModel> CreateCommands()
        {   
            Messenger.Default.Register<string>(this, open);
            return new List<CommandViewModel>
            {
                new CommandViewModel(
                    "Wszystkie leki",
                    new BaseCommand(() => this.ShowAll<AllProductsViewModel>())),

                new CommandViewModel(
                    "Wszystkie recepty",
                    new BaseCommand(() => this.ShowAll<AllPrescriptionsViewModel>())),

                new CommandViewModel(
                    "Wszyscy pacjenci",
                    new BaseCommand(() => this.ShowAll<AllPatientsViewModel>())),

                new CommandViewModel(
                    "Dodaj nowy lek",
                    new BaseCommand(() => this.CreateView(new NewProductViewModel()))),

                new CommandViewModel(
                    "Wszyscy Farmaceuci",
                    new BaseCommand(() => this.ShowAll<AllPharmacistsViewModel>())),



                };
        }
        #endregion

        #region Workspaces
        public ObservableCollection<WorkspaceViewModel> Workspaces
        {
            get
            {
                if (_Workspaces == null)
                {
                    _Workspaces = new ObservableCollection<WorkspaceViewModel>();
                    _Workspaces.CollectionChanged += this.OnWorkspacesChanged;
                }
                return _Workspaces;
            }
        }
        private void OnWorkspacesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
                foreach (WorkspaceViewModel workspace in e.NewItems)
                    workspace.RequestClose += this.OnWorkspaceRequestClose;

            if (e.OldItems != null && e.OldItems.Count != 0)
                foreach (WorkspaceViewModel workspace in e.OldItems)
                    workspace.RequestClose -= this.OnWorkspaceRequestClose;
        }
        private void OnWorkspaceRequestClose(object sender, EventArgs e)
        {
            WorkspaceViewModel workspace = sender as WorkspaceViewModel;
            //workspace.Dispos();
            this.Workspaces.Remove(workspace);
        }

        #endregion // Workspaces

        #region Private Helpers
        private void CreateView(WorkspaceViewModel newView)
        {
            this.Workspaces.Add(newView); //to jest dodanie zakladki do kolekcji zakladek
            this.SetActiveWorkspace(newView); //aktywowanie zakladki
        }

        //private void CreateInvoice()
        //{
        //    NewInvoiceViewModel workspace = new NewInvoiceViewModel();
        //    this.Workspaces.Add(workspace);
        //    this.SetActiveWorkspace(workspace);
        //}

        private void ShowAll<T>() where T : WorkspaceViewModel, new()
        {
            T workspace = this.Workspaces.FirstOrDefault(vm => vm is T) as T;

            if (workspace == null)
            {
                workspace = new T();
                this.Workspaces.Add(workspace);
            }

            this.SetActiveWorkspace(workspace);
        }

        //private void ShowAllProducts()
        //{
        //    AllProductsViewModel workspace =
        //        this.Workspaces.FirstOrDefault(vm => vm is AllProductsViewModel)
        //        as AllProductsViewModel;
        //    if (workspace == null)
        //    {
        //        workspace = new AllProductsViewModel();
        //        this.Workspaces.Add(workspace);
        //    }

        //    this.SetActiveWorkspace(workspace);
        //}

        //private void ShowAllInvoices()
        //{
        //    AllInvoicesViewModel workspace =
        //        this.Workspaces.FirstOrDefault(vm => vm is AllInvoicesViewModel)
        //        as AllInvoicesViewModel;
        //    if (workspace == null)
        //    {
        //        workspace = new AllInvoicesViewModel();
        //        this.Workspaces.Add(workspace);
        //    }

        //    this.SetActiveWorkspace(workspace);
        //}

        private void SetActiveWorkspace(WorkspaceViewModel workspace)
        {
            Debug.Assert(this.Workspaces.Contains(workspace));

            ICollectionView collectionView = CollectionViewSource.GetDefaultView(this.Workspaces);
            if (collectionView != null)
                collectionView.MoveCurrentTo(workspace);
        }

        private void open(string name) // name to wyslany komunikat
        {
            if (name == "Wszystkie lekiAdd")
                CreateView(new NewProductViewModel());
            if (name == "Wszystkie ReceptyAdd")
                CreateView(new NewProductViewModel());

        }
        #endregion
    }
}
