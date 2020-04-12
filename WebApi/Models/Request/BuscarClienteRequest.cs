using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.Models.Response;

namespace WebApi.Models.Request
{
    public class BuscarClienteRequest
    {
        public string NUMERO_DOCUMENTO { set; get; }
        public string ACRONIMO_TIPO_DOCUMENTO { set; get; }
    }
}
