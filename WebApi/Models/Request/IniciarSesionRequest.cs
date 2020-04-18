using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.Request
{
    public class IniciarSesionRequest
    {
        public string NICKNAME { set; get; }
        public string PASSWORD { set; get; }
    }
}
