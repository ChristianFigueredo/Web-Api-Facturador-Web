using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi_FACTURACION.Responses
{
    public class Response_GetAllFacturas
    {
        public int Id { get; set; }
        public Guid? Numero { get; set; }
        public decimal? ValorTotal { get; set; }
        public decimal? ValorSubtotal { get; set; }
        public decimal? ValorIva { get; set; }
        public decimal? ValorDescuento { get; set; }
        public DateTime? FechaApertura { get; set; }
        public DateTime? FechaCierre { get; set; }
        public string Estado { get; set; }

    }
}
