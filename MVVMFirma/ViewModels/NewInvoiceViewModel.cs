﻿using GalaSoft.MvvmLight.Messaging;
using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using MVVMFirma.Models.EntitiesForView;
using MVVMFirma.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace MVVMFirma.ViewModels
{
    public class NewInvoiceViewModel : OneViewModel<Faktury_Dostawców>, IDataErrorInfo
    {
        private AptekaEntities aptekaEntities;
        private BaseCommand _ShowSuppliers;

        public NewInvoiceViewModel() : base("Nowa faktura")
        {
            aptekaEntities = new AptekaEntities();
            item = new Faktury_Dostawców();
            DataWystawienia = DateTime.Today;
            LoadDostawcy();
            LoadZamowienia();

            // Nasłuchujemy na obiekt Dostawcy przesyłany przez Messenger
            Messenger.Default.Register<SupplierForAllView>(this, getSelectedSupplier);
        }

        // Komenda wywoływana przez przycisk "Wybierz" (Dostawca)
        public ICommand ShowSuppliers
        {
            get
            {
                if (_ShowSuppliers == null)
                    _ShowSuppliers = new BaseCommand(() => Messenger.Default.Send("ShowSuppliers"));
                return _ShowSuppliers;
            }
        }

        void getSelectedSupplier(SupplierForAllView supplier)
        {
            if (supplier != null)
            {
                IDDostawcy = supplier.ID_Dostawcy;
                SupplierName = supplier.Nazwa;
                SupplierPhone = supplier.Telefon;

            }
        }


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
            get => item.Numer_Faktury;
            set
            {
                item.Numer_Faktury = value;
                OnPropertyChanged(() => NumerFaktury);
            }
        }

        public int IDDostawcy
        {
            get => item.ID_Dostawcy;
            set
            {
                item.ID_Dostawcy = value;
                OnPropertyChanged(() => IDDostawcy);
            }
        }

        public string SupplierName { get; set; }
        public string SupplierPhone { get; set; }

        public DateTime DataWystawienia
        {
            get => item.Data_Wystawienia;
            set
            {
                item.Data_Wystawienia = value;
                OnPropertyChanged(() => DataWystawienia);
            }
        }

        public decimal Kwota
        {
            get => item.Kwota;
            set
            {
                item.Kwota = value;
                OnPropertyChanged(() => Kwota);
            }
        }

        public int IDZamowienia
        {
            get => item.ID_Zamówienia;
            set
            {
                item.ID_Zamówienia = value;
                OnPropertyChanged(() => IDZamowienia);
            }
        }

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
            if (IsValid())
            {
                aptekaEntities.Faktury_Dostawców.Add(item);
                aptekaEntities.SaveChanges();
            }
            else
            {
                ShowMessageBox("Popraw błędy w formularzu");
            }
        }


        #region Validation
        private string _validationMessage = string.Empty;

        public string this[string propertyName]
        {
            get
            {
                _validationMessage = string.Empty;
                switch (propertyName)
                {
                    case nameof(Kwota):
                        _validationMessage = ValueValidator.ValidatePositiveDecimal(Kwota);
                        break;
                    case nameof(DataWystawienia):
                        _validationMessage = ValueValidator.ValidateDate(DataWystawienia);
                        break;
                    case nameof(NumerFaktury):
                        _validationMessage = ValueValidator.ValidateString(NumerFaktury);
                        break;
                    case nameof(IDDostawcy):
                        _validationMessage = IDDostawcy > 0 ? string.Empty : "Musisz wybrać dostawcę.";
                        break;
                }
                return _validationMessage;
            }
        }

        public override bool IsValid()
        {
            return string.IsNullOrEmpty(this[nameof(Kwota)]) &&
                   string.IsNullOrEmpty(this[nameof(DataWystawienia)]) &&
                   string.IsNullOrEmpty(this[nameof(NumerFaktury)]) &&
                   string.IsNullOrEmpty(this[nameof(IDDostawcy)]);
        }

        public string Error => string.Empty;
        #endregion
    }
}