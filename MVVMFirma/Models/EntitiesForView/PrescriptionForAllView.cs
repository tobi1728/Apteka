using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMFirma.Models.EntitiesForView
{
    public class PrescriptionForAllView
    {

        public int ID_Recepty { get; set; }
        public string PESEL { get; set; }
        public string Numer_Licencji { get; set; }
        public System.DateTime Data_Wystawienia { get; set; }
        public Nullable<System.DateTime> Data_Realizacji { get; set; }


    }
}
