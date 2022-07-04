using System;
using System.Collections.Generic;

namespace ProgettoFed.Models
{
    public partial class Notum
    {
        public int IdTask { get; set; }
        public int? IdUtente { get; set; }
        public DateTime DataCreazione { get; set; }
        public DateTime DataScadenza { get; set; }
        public string TitoloTask { get; set; } = null!;
        public string TestoTask { get; set; } = null!;
        public bool Flag { get; set; }

        public virtual Utente? IdUtenteNavigation { get; set; }
    }
}
