using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Models.Response
{
    public class IniciarSesionResponse
    {
        public int ID_USUARIO { set; get; }
        public string NOMBRE { set; get; }
        public string APELLIDO { set; get; }
        public string PERFIL { set; get; }
        public bool ESTADO { get; set; }
        public RespuestaTransaccion RESPUESTA_TRANSACCION { set; get; }
    }
}
