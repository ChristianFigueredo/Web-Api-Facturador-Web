using System;
using System.Collections.Generic;

namespace DataLayer.Models.DB
{
    public partial class Inventario
    {
        public int Id { get; set; }
        public int? IdProducto { get; set; }
        public int? TotalUnidades { get; set; }
        public int? TotalMinimoUnidades { get; set; }
        public int? TotalMaximoUnidades { get; set; }

        public virtual Producto IdProductoNavigation { get; set; }
    }
}
