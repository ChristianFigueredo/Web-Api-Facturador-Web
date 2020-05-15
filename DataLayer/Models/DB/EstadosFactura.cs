using System;
using System.Collections.Generic;

namespace DataLayer.Models.DB
{
    public partial class EstadosFactura
    {
        public EstadosFactura()
        {
            Factura = new HashSet<Factura>();
        }

        public int Id { get; set; }
        public string Descripcion { get; set; }

        public virtual ICollection<Factura> Factura { get; set; }
    }
}
