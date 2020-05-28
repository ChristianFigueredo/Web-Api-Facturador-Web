using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi_FACTURACION.Responses.Factura
{
    public class Productos
    {
        public int Codigo { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public decimal? ValorUnitario { get; set; }
        public decimal? ValorTotal { get; set; }
        public decimal? Cantidad { get; set; }
        public decimal? PorcentajeIva { get; set; }
        public decimal? ValorTotalIva { get; set; }
        public decimal? PorcentajeDescuento { get; set; }
        public decimal? ValorTotalDescuento { get; set; }
    }
}
