using System;
using System.Collections.Generic;

namespace DataLayer.Models.DB
{
    public partial class TipoDocumento
    {
        public TipoDocumento()
        {
            Persona = new HashSet<Persona>();
        }

        public int Id { get; set; }
        public string Descripcion { get; set; }
        public string Acronimo { get; set; }

        public virtual ICollection<Persona> Persona { get; set; }
    }
}
