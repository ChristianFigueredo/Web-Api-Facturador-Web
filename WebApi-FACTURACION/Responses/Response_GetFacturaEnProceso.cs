using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi_FACTURACION.Responses
{
    public class Response_GetFacturaEnProceso
    {
        public string numeroFactura { get; set; }
        public string acronimo { get; set; }
        public string numerodocumento { get; set; }
        public RespuestaTransaccion respuestaTransaccion { set; get; }

    }
}
