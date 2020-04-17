using System;
using System.Collections.Generic;

namespace DataLayer.Models.DB
{
    public partial class Persona
    {
        public Persona()
        {
            Cliente = new HashSet<Cliente>();
            Usuario = new HashSet<Usuario>();
        }

        public int Id { get; set; }
        public string NumeroDocumento { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Direccion { get; set; }
        public int? IdTipoDocumento { get; set; }

        public virtual TipoDocumento IdTipoDocumentoNavigation { get; set; }
        public virtual ICollection<Cliente> Cliente { get; set; }
        public virtual ICollection<Usuario> Usuario { get; set; }
    }
}
