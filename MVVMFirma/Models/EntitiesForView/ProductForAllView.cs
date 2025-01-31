using MVVMFirma.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVVMFirma.Models.EntitiesForView
{
    // ta klasa jest wzorowana na klasie product tylko zamiast kluczy obcych ma czytelne dla klienta pola, klucze obce beda zastapione przez dane zrozumiale dla czlowieka
    public class ProductForAllView
    {
        public int ID_Leku { get; set; }
        public string Nazwa_Leku { get; set; }
        public string Nazwa_Kategorii { get; set; }
        // to jest pole zamiast klucza obcego ID_Kategorii
        public decimal Cena_Zakupu { get; set; }
        public decimal Cena_Sprzedaży { get; set; }
        public System.DateTime Data_Waznosci { get; set; }
        public string Nazwa_Producenta { get; set; }
        // to jest pole zamiast klucza obcego ID_Producenta
        public bool Na_Recepte { get; set; }
        public bool Refundacja { get; set; }
        public string Opis { get; set; }

        public bool IsExpired
        {
            get
            {
                return Data_Waznosci < DateTime.Today;
            }
        }
    }
}
