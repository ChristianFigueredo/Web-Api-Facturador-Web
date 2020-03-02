using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.Response
{
    public class GetClienteResponse
    {
        public string NUMERO_DOCUMENTO { set; get; }
        public string NOMBRE { set; get; }
        public string APELLIDO { set; get; }
        public string TELEFONO { set; get; }
        public string EMAIL { set; get; }
        public string DIRECCION { set; get; }
        public string TIPO_DOCUMENTO { set; get; }
    }
}
