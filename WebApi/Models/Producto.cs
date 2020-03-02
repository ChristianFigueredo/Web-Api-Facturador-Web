using System;
using System.Collections.Generic;

namespace WebApi.Models
{
    public partial class Producto
    {
        public Producto()
        {
            Inventario = new HashSet<Inventario>();
        }

        public int Id { get; set; }
        public string Descripcion { get; set; }
        public int? Cantidad { get; set; }
        public int? ValorUnitario { get; set; }
        public int? ValorTotal { get; set; }
        public int? IdFactura { get; set; }

        public virtual Factura IdFacturaNavigation { get; set; }
        public virtual ICollection<Inventario> Inventario { get; set; }
    }
}
