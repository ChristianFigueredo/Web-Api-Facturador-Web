using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi_INVENTARIO.Response
{
    public class ProductoInventario 
    {
        public int Codigo { get; set; }
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
    }
}
