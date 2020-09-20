using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
using WebApi.Models.Request;
using WebApi.Models.Response;
using DataLayer.Models.DB;
using Microsoft.AspNetCore.Cors;

namespace WebApi.Controllers
{
    [EnableCors("PoliticaCors")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        // POST: api/Usuario
        [HttpPost]
        public IniciarSesionResponse Post([FromBody] IniciarSesionRequest SessionObject)
        {
            var RG = new RespuestaTransaccion();
            IniciarSesionResponse ObjetoRespuesta = null;
            RespuestaTransaccion RTN = new RespuestaTransaccion();

            using ( FacturadorWebContext context = new FacturadorWebContext() ) 
            {
                var query_sesion = from U in context.Usuario join P in context.Persona on U.IdPersona equals P.Id where U.Nickname == SessionObject.NICKNAME && U.Clave == SessionObject.PASSWORD
                select new IniciarSesionResponse { NOMBRE = P.Nombre, APELLIDO = P.Apellido, ID_USUARIO = U.Id, PERFIL = U.Perfil, ESTADO = U.Estado };
                ObjetoRespuesta = query_sesion.FirstOrDefault();

                if (ObjetoRespuesta != null)
                {
                    if (ObjetoRespuesta.ESTADO)
                    {
                        ObjetoRespuesta.RESPUESTA_TRANSACCION = RTN.GenerarRespuesta("Transaccion exitosa", "0000");
                    }
                    else 
                    {
                        ObjetoRespuesta.RESPUESTA_TRANSACCION = RTN.GenerarRespuesta("Error. Usuario inactivo, por favor contacte al administrador del sistema", "0008");
                    }
                }
                else 
                {
                    ObjetoRespuesta = new IniciarSesionResponse();
                    ObjetoRespuesta.RESPUESTA_TRANSACCION = RTN.GenerarRespuesta("Error. Inicio de sesion fallido, usuario o password incorrectos", "0007");
                }
                return ObjetoRespuesta;
            }
        }     
    }
}
