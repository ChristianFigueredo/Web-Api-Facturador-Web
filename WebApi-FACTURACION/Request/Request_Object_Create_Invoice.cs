using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi_FACTURACION.Request
{
    public class Request_Object_Create_Invoice
    {
        public int userId { get; set; }
        public string clientDocumentType { set; get; }
        public string clientDocumentNumber { set; get; }
    }
}
