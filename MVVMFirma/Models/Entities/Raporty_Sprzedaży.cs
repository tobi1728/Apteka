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
    
    public partial class Raporty_Sprzedaży
    {
        public int ID_Raportu { get; set; }
        public System.DateTime Data_Rozpoczęcia { get; set; }
        public System.DateTime Data_Zakończenia { get; set; }
        public decimal Łączna_Sprzedaż { get; set; }
        public int Liczba_Transakcji { get; set; }
    }
}
