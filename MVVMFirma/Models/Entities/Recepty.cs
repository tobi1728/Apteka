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
    
    public partial class Recepty
    {
        public int ID_Recepty { get; set; }
        public int ID_Pacjenta { get; set; }
        public int ID_Farmaceuty { get; set; }
        public System.DateTime Data_Wystawienia { get; set; }
        public Nullable<System.DateTime> Data_Realizacji { get; set; }
    
        public virtual Farmaceuci Farmaceuci { get; set; }
        public virtual Pacjenci Pacjenci { get; set; }
    }
}
