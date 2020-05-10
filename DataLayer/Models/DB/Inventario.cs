using System;
using System.Collections.Generic;

namespace DataLayer.Models.DB
{
    public partial class Inventario
    {
        public Inventario()
        {
            Producto = new HashSet<Producto>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal? TotalRecibidos { get; set; }
        public decimal? TotalVendidos { get; set; }
        public decimal? TotalDesincorporados { get; set; }
        public decimal? TotalDevueltos { get; set; }
        public decimal? TotalProceso { get; set; }
        public decimal? PrecioCompra { get; set; }
        public decimal? PrecioVenta { get; set; }
        public DateTime? FechaRegistro { get; set; }
        public decimal? PorcentajeIva { get; set; }
        public decimal? PorcentajeDescuento { get; set; }

        public virtual ICollection<Producto> Producto { get; set; }
    }
}
