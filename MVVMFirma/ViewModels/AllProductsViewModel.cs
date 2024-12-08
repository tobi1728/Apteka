using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using MVVMFirma.Models.Entities;

namespace MVVMFirma.ViewModels
{
    public class AllProductsViewModel : WorkspaceViewModel
    {
        #region Fields
        private readonly AptekaEntities aptekaEntities; // to jest pole, ktore reprezentuje baze danych
        #endregion

        #region Constructor
        public AllProductsViewModel()
        {
            base.DisplayName = "Products";

        }
        #endregion

        #region Helpers

        #endregion

    }
}