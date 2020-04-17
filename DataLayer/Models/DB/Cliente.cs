using System;
using System.Collections.Generic;

namespace DataLayer.Models.DB
{
    public partial class Cliente
    {
        public Cliente()
        {
            Factura = new HashSet<Factura>();
        }

        public int Id { get; set; }
        public int? Puntos { get; set; }
        public int? IdPersona { get; set; }

        public virtual Persona IdPersonaNavigation { get; set; }
        public virtual ICollection<Factura> Factura { get; set; }
    }
}
