using System;
using System.Collections.Generic;

namespace ProgettoFed.Models
{
    public partial class Utente
    {
        public Utente()
        {
            Nota = new HashSet<Notum>();
        }

        public int IdUtente { get; set; }
        public string Nome { get; set; } = null!;
        public string Psw { get; set; } = null!;
        public int? Biscotto { get; set; }

        public virtual ICollection<Notum> Nota { get; set; }
    }
}
