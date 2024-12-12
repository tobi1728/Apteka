using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMFirma.ViewModels
{
    public class NewInvoiceViewModel : WorkspaceViewModel // wszystkie zakladki dziedzicza po WVM
    {
        public NewInvoiceViewModel() 
        {
            base.DisplayName = "Nowa faktura"; // nazwa zakladki
        }
    }
}
