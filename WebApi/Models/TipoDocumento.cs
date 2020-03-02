using System;
using System.Collections.Generic;

namespace WebApi.Models
{
    public partial class TipoDocumento
    {
        public TipoDocumento()
        {
            Cliente = new HashSet<Cliente>();
        }

        public int Id { get; set; }
        public string Descripcion { get; set; }
        public string Acronimo { get; set; }

        public virtual ICollection<Cliente> Cliente { get; set; }
    }
}
