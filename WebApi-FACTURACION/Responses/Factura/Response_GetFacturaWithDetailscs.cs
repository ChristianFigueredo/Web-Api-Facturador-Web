using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi_FACTURACION.Responses.Factura;

namespace WebApi_FACTURACION.Responses
{
    public class Response_GetFacturaWithDetailscs
    {
        public string numeroFactura { get; set; }
        public decimal? valorTotal { get; set; }
        public decimal? valorSubtotal { get; set; }
        public decimal? valorIva { get; set; }
        public decimal? valorDescuento { get; set; }
        public Datoscliente cliente { get; set; }
        public List<Productos> productos { get; set; }
        public RespuestaTransaccion respuestaTransaccion { get; set; }

    }
}
