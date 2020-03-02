using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.Response
{
    public class RespuestaTransaccion
    { 
        public string CODIGO { set; get; }
        public string MENSAJE { set; get; }
        public Exception EXCEPTION_TRACE { set; get; }
    }
}
