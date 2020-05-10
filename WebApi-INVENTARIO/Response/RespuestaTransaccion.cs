using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi_INVENTARIO.Response
{
    public class RespuestaTransaccion
    {
        public string CODIGO { set; get; }
        public string MENSAJE { set; get; }
        public Exception EXCEPTION_TRACE { set; get; }

        public RespuestaTransaccion GenerarRespuesta(string mensaje, string codigo, Exception ex = null)
        {
            RespuestaTransaccion RG = new RespuestaTransaccion();
            RG.CODIGO = codigo;
            RG.MENSAJE = mensaje;
            RG.EXCEPTION_TRACE = ex;
            return RG;
        }
    }
}
