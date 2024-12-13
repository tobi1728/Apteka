using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MVVMFirma.ViewModels
{
    public class NewParagonViewModel : OneViewModel<Paragony>
    {
        #region Constructor
        public NewParagonViewModel()
            : base("Nowy paragon")
        {
            aptekaEntities = new AptekaEntities();
            item = new Paragony();
            DataWystawienia = DateTime.Today;

            LoadSprzedaze();
        }
        #endregion

        #region Properties
        private List<Sprzedaż> _Sprzedaze;
        public List<Sprzedaż> Sprzedaze
        {
            get => _Sprzedaze;
            set
            {
                _Sprzedaze = value;
                OnPropertyChanged(() => Sprzedaze);
            }
        }

        public string NumerParagonu
        {
            get => item.Numer_Paragonu;
            set
            {
                item.Numer_Paragonu = value;
                OnPropertyChanged(() => NumerParagonu);
            }
        }

        public int IDSprzedazy
        {
            get => item.ID_Sprzedaży;
            set
            {
                item.ID_Sprzedaży = value;
                OnPropertyChanged(() => IDSprzedazy);
            }
        }

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
        #endregion

        #region Helpers
        public void LoadSprzedaze()
        {
            Sprzedaze = aptekaEntities.Sprzedaż.ToList();
        }

        public override void Save()
        {
            if (IDSprzedazy == 0)
            {
                throw new InvalidOperationException("Musisz wybrać sprzedaż.");
            }

            aptekaEntities.Paragony.Add(item);
            aptekaEntities.SaveChanges();
        }
        #endregion
    }
}
