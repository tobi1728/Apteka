//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MVVMFirma.Models.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class Faktury_Dostawców
    {
        public int ID_Faktury { get; set; }
        public string Numer_Faktury { get; set; }
        public int ID_Dostawcy { get; set; }
        public System.DateTime Data_Wystawienia { get; set; }
        public decimal Kwota { get; set; }
        public int ID_Zamówienia { get; set; }
    
        public virtual Dostawcy Dostawcy { get; set; }
        public virtual Zamówienia Zamówienia { get; set; }
    }
}
