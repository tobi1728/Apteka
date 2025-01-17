using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace MVVMFirma.ViewModels
{
    public class NewInvoiceViewModel : OneViewModel<Faktury_Dostawców>
    {
        #region Constructor
        public NewInvoiceViewModel()
            : base("Nowa faktura")
        {
            aptekaEntities = new AptekaEntities();
            item = new Faktury_Dostawców();
            DataWystawienia = DateTime.Today; 
            LoadDostawcy();
            LoadZamowienia();
            //Messenger.Default.Register<Dostawcy>(this, getSelectedSupplier);
        }
        #endregion

        #region Command
        private BaseCommand _ShowSuppliers;
        public ICommand ShowSuppliers
        {
            get
            {
                if (_ShowSuppliers == null)
                    _ShowSuppliers = new BaseCommand(() => showSuppliers());
                return _ShowSuppliers;
            }
        }
        private void showSuppliers()
        {
            Messenger.Default.Send<string>("KontrahenciAll");
        }
        #endregion

        #region Properties

        // Lista dostawców
        private List<Dostawcy> _Dostawcy;
        public List<Dostawcy> Dostawcy
        {
            get => _Dostawcy;
            set
            {
                _Dostawcy = value;
                OnPropertyChanged(() => Dostawcy);
            }
        }

        // Lista zamówień
        private List<Zamówienia> _Zamowienia;
        public List<Zamówienia> Zamowienia
        {
            get => _Zamowienia;
            set
            {
                _Zamowienia = value;
                OnPropertyChanged(() => Zamowienia);
            }
        }

        public string NumerFaktury
        {
            get
            {
                return item.Numer_Faktury;
            }
            set
            {
                item.Numer_Faktury = value;
                OnPropertyChanged(() => NumerFaktury);
            }
        }

        public int IDDostawcy
        {
            get
            {
                return item.ID_Dostawcy;
            }
            set
            {
                item.ID_Dostawcy = value;
                OnPropertyChanged(() => IDDostawcy);
            }
        }

        public string SupplierName { get; set; }   
        public string SupplierPhone{ get; set; }

        public DateTime DataWystawienia
        {
            get
            {
                return item.Data_Wystawienia;
            }
            set
            {
                item.Data_Wystawienia = value;
                OnPropertyChanged(() => DataWystawienia);
            }
        }

        public decimal Kwota
        {
            get
            {
                return item.Kwota;
            }
            set
            {
                item.Kwota = value;
                OnPropertyChanged(() => Kwota);
            }
        }

        public int IDZamowienia
        {
            get
            {
                return item.ID_Zamówienia;
            }
            set
            {
                item.ID_Zamówienia = value;
                OnPropertyChanged(() => IDZamowienia);
            }
        }



        #endregion

        #region Helpers

        //private void getSelectedSupplier(Dostawcy supplier)
        //{
        //    IDDostawcy = supplier.ID_Dostawcy;
        //    SupplierName = supplier.Nazwa;
        //    SupplierPhone = supplier.Telefon;
        //}
        
        public void LoadDostawcy()
        {
            Dostawcy = aptekaEntities.Dostawcy.ToList();
        }

        public void LoadZamowienia()
        {
            Zamowienia = aptekaEntities.Zamówienia.ToList();
        }

        public override void Save()
        {
            if (IDDostawcy == 0)
            {
                throw new InvalidOperationException("Musisz wybrać dostawcę.");
            }

            aptekaEntities.Faktury_Dostawców.Add(item); // Dodaje do lokalnej kolekcji
            aptekaEntities.SaveChanges(); // Zapisuje zmiany do bazy danych
        }

        #endregion
    }
}
