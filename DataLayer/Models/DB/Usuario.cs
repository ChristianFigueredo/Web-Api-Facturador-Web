using System;
using System.Collections.Generic;

namespace DataLayer.Models.DB
{
    public partial class Usuario
    {
        public Usuario()
        {
            Factura = new HashSet<Factura>();
        }

        public int Id { get; set; }
        public string Nickname { get; set; }
        public string Clave { get; set; }
        public string Perfil { get; set; }
        public int? IdPersona { get; set; }
        public bool Estado { get; set; }

        public virtual Persona IdPersonaNavigation { get; set; }
        public virtual ICollection<Factura> Factura { get; set; }
    }
}
