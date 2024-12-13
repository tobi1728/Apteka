using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMFirma.Models.EntitiesForView
{
    public class SupplierForAllView
    {
        public int ID_Dostawcy { get; set; }
        public string Nazwa { get; set; }
        public string Telefon { get; set; }
        public string Ulica { get; set; }
        public string Miasto { get; set; }
        public string Kod_Pocztowy { get; set; }
    }
}
