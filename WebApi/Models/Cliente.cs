using System;
using System.Collections.Generic;

namespace WebApi.Models
{
    public partial class Cliente
    {
        public Cliente()
        {
            Factura = new HashSet<Factura>();
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
        public virtual ICollection<Factura> Factura { get; set; }
    }
}
