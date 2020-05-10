using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi_INVENTARIO.Request
{
    public class PostRequest : PutRequest
    {
        public decimal? TotalRecibidos { get; set; }
        public DateTime? FechaRegistro { get; set; }
    }
}
