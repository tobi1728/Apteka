using MVVMFirma.Helper;
using MVVMFirma.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace MVVMFirma.ViewModels
{
    public abstract class OneViewModel<T>:WorkspaceViewModel
    {
        #region DB
        protected AptekaEntities aptekaEntities;
        #endregion


        #region Item
        protected T item;
        #endregion

        #region Command
        //to jest komenda, ktora zostanie podpieta pod przycisk zapisz i zamknij
        private BaseCommand _SaveAndCloseCommand;
        public ICommand SaveAndCloseCommand
        {
            get
            {
                if (_SaveAndCloseCommand == null)
                    _SaveAndCloseCommand = new BaseCommand(() => SaveAndClose());
                return _SaveAndCloseCommand;
            }
        }

        private BaseCommand _SaveCommand;

        public ICommand SaveCommand
        {
            get
            {
                if (_SaveCommand == null)
                    _SaveCommand = new BaseCommand(() => Save());
                return _SaveCommand;
            }
        }
        #endregion

        #region Constructor
        public OneViewModel(string displayName)
        {
            base.DisplayName = displayName;
            aptekaEntities = new AptekaEntities();
        }
        #endregion

        #region Helpers


        public abstract void Save();
        public void SaveAndClose()
        {
            if (IsValid())
            { 
                Save();
                OnRequestClose();
            }
            else
            {
                ShowMessageBox("Popraw błędy w formularzu");
            }
        }
        public virtual bool IsValid() => true;
        #endregion
    }
}
