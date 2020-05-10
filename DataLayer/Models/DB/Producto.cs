using System;
using System.Collections.Generic;

namespace DataLayer.Models.DB
{
    public partial class Producto
    {
        public int Id { get; set; }
        public decimal? Cantidad { get; set; }
        public int? IdInventario { get; set; }
        public int? IdFactura { get; set; }

        public virtual Factura IdFacturaNavigation { get; set; }
        public virtual Inventario IdInventarioNavigation { get; set; }
    }
}
