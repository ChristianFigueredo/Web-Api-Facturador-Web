using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi_INVENTARIO.Request
{
    public class PutRequest
    {
        public int Codigo { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal? PrecioCompra { get; set; }
        public decimal? PrecioVenta { get; set; }
        public decimal? PorcentajeIva { get; set; }
        public decimal? PorcentajeDescuento { get; set; }
    }
}
