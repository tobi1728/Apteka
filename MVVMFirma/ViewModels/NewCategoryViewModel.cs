using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using MVVMFirma.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace MVVMFirma.ViewModels
{
    public class NewCategoryViewModel : OneViewModel<Kategorie_Leków>, IDataErrorInfo
    {
        #region Constructor
        public NewCategoryViewModel()
            : base("Nowa kategoria")
        {
            aptekaEntities = new AptekaEntities();
            item = new Kategorie_Leków();
        }
        #endregion
        #region Validation
        private string _validationMessage = string.Empty;

        public string this[string propertyName]
        {
            get
            {
                _validationMessage = string.Empty;
                switch (propertyName)
                {
                    case nameof(NazwaKategorii):
                        _validationMessage = ValueValidator.ValidateString(NazwaKategorii, 3);
                        break;
                    case nameof(Opis):
                        _validationMessage = ValueValidator.ValidateOptionalString(Opis, 500);
                        break;
                }
                return _validationMessage;
            }
        }

        public override bool IsValid()
        {
            return string.IsNullOrEmpty(this[nameof(NazwaKategorii)]) &&
                   string.IsNullOrEmpty(this[nameof(Opis)]);
        }

        public string Error => string.Empty;
        #endregion

        #region Properties

        public string NazwaKategorii
        {
            get
            {
                return item.Nazwa_Kategorii;
            }
            set
            {
                item.Nazwa_Kategorii = value;
                OnPropertyChanged(() => NazwaKategorii);
            }
        }

        public string Opis
        {
            get
            {
                return item.Opis;
            }
            set
            {
                item.Opis = value;
                OnPropertyChanged(() => Opis);
            }
        }

        #endregion

        #region Helpers
        public override void Save()
        {
            if (IsValid())
            {
                aptekaEntities.Kategorie_Leków.Add(item);
                aptekaEntities.SaveChanges();
            }
            else
            {
                ShowMessageBox("Popraw błędy w formularzu");
            }
        }

        #endregion
    }
}
