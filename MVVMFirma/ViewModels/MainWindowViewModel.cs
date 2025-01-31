using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Data;
using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Helper;
using MVVMFirma.Views;
using System.Windows;
using MVVMFirma.Models.Entities;
using System.Runtime.CompilerServices;

namespace MVVMFirma.ViewModels
{
    public class MainWindowViewModel : BaseViewModel
    {
        private ReadOnlyCollection<CommandViewModel> _Commands;
        private ObservableCollection<WorkspaceViewModel> _Workspaces;

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
        
        public MainWindowViewModel()
        {
            Messenger.Default.Register<string>(this, open);
        }

        private List<CommandViewModel> CreateCommands()
        {
            return new List<CommandViewModel>
            {
                new CommandViewModel(
                    "Dostawcy",
                    new BaseCommand(() => this.ShowAll<AllSuppliersViewModel>())),

                new CommandViewModel(
                    "Faktury dostawców",
                    new BaseCommand(() => this.ShowAll<AllInvoicesViewModel>())),

                new CommandViewModel(
                    "Farmaceuci",
                    new BaseCommand(() => this.ShowAll<AllPharmacistsViewModel>())),

                new CommandViewModel(
                    "Grafiki pracowników",
                    new BaseCommand(() => this.ShowAll<AllSchedulesViewModel>())),

                new CommandViewModel(
                    "Kategorie leków",
                    new BaseCommand(() => this.ShowAll<AllDrugCategoriesViewModel>())),

                new CommandViewModel(
                    "Leki",
                    new BaseCommand(() => this.ShowAll<AllProductsViewModel>())),

                new CommandViewModel(
                    "Magazyny",
                    new BaseCommand(() => this.ShowAll<AllWarehousesViewModel>())),

                new CommandViewModel(
                    "Pacjenci",
                    new BaseCommand(() => this.ShowAll<AllPatientsViewModel>())),

                new CommandViewModel(
                    "Paragony",
                    new BaseCommand(() => this.ShowAll<AllReceiptsViewModel>())),

                new CommandViewModel(
                    "Producenci",
                    new BaseCommand(() => this.ShowAll<AllProducersViewModel>())),

                new CommandViewModel(
                    "Raporty sprzedaży",
                    new BaseCommand(() => this.ShowAll<AllSalesReportsViewModel>())),

                new CommandViewModel(
                    "Recepty",
                    new BaseCommand(() => this.ShowAll<AllPrescriptionsViewModel>())),

                new CommandViewModel(
                    "Sprzedaże",
                    new BaseCommand(() => this.ShowAll<AllSalesViewModel>())),

                new CommandViewModel(
                    "Zamówienia",
                    new BaseCommand(() => this.ShowAll<AllOrdersViewModel>())),

                new CommandViewModel(
                    "Produkty zamówienia",
                    new BaseCommand(() => this.ShowAll<AllOrderProductsViewModel>())),
            };
        }

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
            this.Workspaces.Remove(workspace);
        }

        private void CreateView(WorkspaceViewModel newView)
        {
            this.Workspaces.Add(newView);
            this.SetActiveWorkspace(newView);
        }

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



        private void SetActiveWorkspace(WorkspaceViewModel workspace)
        {
            Debug.Assert(this.Workspaces.Contains(workspace));

            ICollectionView collectionView = CollectionViewSource.GetDefaultView(this.Workspaces);
            if (collectionView != null)
                collectionView.MoveCurrentTo(workspace);
        }

        private void open(string name)
        {
            if (name == "ShowSuppliers")
            {
                var suppliersViewModel = new AllSuppliersViewModel
                {
                    IsModal = true // Otwieramy jako modalne okno
                };
                CreateView(suppliersViewModel);
            }
            else if (name == "Wszyscy dostawcy")
            {
                var suppliersViewModel = new AllSuppliersViewModel
                {
                    IsModal = false // Standardowe otwieranie z listy linków
                };
                CreateView(suppliersViewModel);
            }
            if (name == "ShowPharmacists")
            {
                var pharmacistsViewModel = new AllPharmacistsViewModel
                {
                    IsModal = true // KLUCZOWE
                };
                CreateView(pharmacistsViewModel);
            }
            else if (name == "Wszyscy farmaceuci")
            {
                var pharmacistsViewModel = new AllPharmacistsViewModel
                {
                    IsModal = false // Standardowe otwieranie z listy linków
                };
                CreateView(pharmacistsViewModel);
             } 
            else if (name == "ShowProducts")
            {
                var productsViewModel = new AllProductsViewModel
                {
                    IsModal = true
                };
                CreateView(productsViewModel);
            }
            else if (name == "ShowSales")
            {
                var salesVM = new AllSalesViewModel
                {
                    IsModal = true
                };
                CreateView(salesVM);
            }



            // Obsługa komunikatów typu string — np. otwieranie formularzy "Add" lub list "All".
            if (name == "Wszystkie lekiAdd")
                CreateView(new NewProductViewModel());
            if (name == "Wszystkie kategorie lekówAdd")
                CreateView(new NewCategoryViewModel());
            if (name == "Wszyscy dostawcyAdd")
                CreateView(new NewSupplierViewModel());
            if (name == "Wszyscy farmaceuciAdd")
                CreateView(new NewPharmacistViewModel());
            if (name == "Wszystkie faktury dostawcówAdd")
                CreateView(new NewInvoiceViewModel());
            if (name == "Grafiki pracownikówAdd")
                CreateView(new NewScheduleViewModel());
            if (name == "Wszystkie magazynyAdd")
                CreateView(new NewWarehouseViewModel());
            if (name == "Wszyscy pacjenciAdd")
                CreateView(new NewPatientViewModel());
            if (name == "Wszystkie paragonyAdd")
                CreateView(new NewParagonViewModel());
            if (name == "Wszyscy ProducenciAdd")
                CreateView(new NewProducerViewModel());
            if (name == "Produkty zamówieniaAdd")
                CreateView(new NewOrderProductViewModel());
            if (name == "Wszystkie raporty sprzedażyAdd")
                CreateView(new NewSalesReportViewModel());
            if (name == "Wszystkie receptyAdd")
                CreateView(new NewPrescriptionViewModel());
            if (name == "Wszystkie zamówieniaAdd")
                CreateView(new NewOrderViewModel());
            if (name == "Wszystkie sprzedażeAdd")
                CreateView(new NewSaleViewModel());
            if (name == "Wszyscy producenci")
                CreateView(new AllProducersViewModel());

        }


    }
}
