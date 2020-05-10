using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi_INVENTARIO.Response
{
    public class ProductoInvetarioRespuestaTransaccion : ProductoInventario
    {
        public RespuestaTransaccion Respuesta { set; get; }
    }
}
