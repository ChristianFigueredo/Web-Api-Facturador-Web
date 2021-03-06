﻿using System;
using System.Collections.Generic;

namespace DataLayer.Models.DB
{
    public partial class Producto
    {
        public int Id { get; set; }
        public decimal? ValorUnitario { get; set; }
        public decimal? ValorTotal { get; set; }
        public decimal? Cantidad { get; set; }
        public decimal? PorcentajeIva { get; set; }
        public decimal? ValorTotalIva { get; set; }
        public decimal? PorcentajeDescuento { get; set; }
        public decimal? ValorTotalDescuento { get; set; }
        public int? IdInventario { get; set; }
        public int? IdFactura { get; set; }

        public virtual Factura IdFacturaNavigation { get; set; }
        public virtual Inventario IdInventarioNavigation { get; set; }
    }
}
