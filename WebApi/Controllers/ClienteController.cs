using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DataLayer.Models.DB;
using WebApi.Models.Request;
using WebApi.Models.Response;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClienteController : ControllerBase
    {
        [HttpGet]
        public List<GetClienteResponse> Get()
        {
            List<Cliente> ListadoClientes = null;
            List<GetClienteResponse> ListaClientesResponse = new List<GetClienteResponse>();

            using (FacturadorWebContext context = new FacturadorWebContext())
            {
                ListadoClientes = context.Cliente.ToList();
                if (ListadoClientes != null)
                {
                    foreach (Cliente c in ListadoClientes)
                    {
                        var query = from P in context.Persona
                                    join C in context.Cliente on P.Id equals C.IdPersona
                                    join TD in context.TipoDocumento on P.IdTipoDocumento equals TD.Id

                                    select new GetClienteResponse
                                    {
                                        NOMBRE = P.Nombre,
                                        APELLIDO = P.Apellido,
                                        NUMERO_DOCUMENTO = P.NumeroDocumento,
                                        TELEFONO = P.Telefono,
                                        EMAIL = P.Email,
                                        DIRECCION = P.Direccion,
                                        TIPO_DOCUMENTO = TD.Acronimo
                                    };
                        ListaClientesResponse = query.ToList();
                    }
                } 
            }
            return ListaClientesResponse;
        }

        // GET 
        [HttpGet("ACRONIMO/{ACRONIMO}/NUMERO_DOCUMENTO/{NUMERO_DOCUMENTO}")]
        public RespuestaTransaccion Get(string ACRONIMO, string NUMERO_DOCUMENTO) 
        {
            Cliente cliente = null;

            using (FacturadorWebContext context = new FacturadorWebContext()) 
            {
                var TD = context.TipoDocumento.SingleOrDefault(x => x.Acronimo == ACRONIMO);
                if ( TD != null )
                {
                    // BUSCAR EN PERSONAS - CLIENTES
                    var query_clientes = from C in context.Cliente 
                                         join P in context.Persona on C.IdPersona equals P.Id 
                                         join T in context.TipoDocumento on P.IdTipoDocumento equals T.Id 
                                         where T.Acronimo == ACRONIMO && P.NumeroDocumento == NUMERO_DOCUMENTO
                    select new Cliente { Id = C.Id };
                    cliente = query_clientes.FirstOrDefault();

                    if ( cliente != null )
                    { 
                        return GenerarRespuesta("Cliente existe en nuestra base de datos", "0000");
                    }
                    else 
                    {
                        var query_personas = context.Persona.SingleOrDefault(x => x.NumeroDocumento == NUMERO_DOCUMENTO && x.IdTipoDocumento == TD.Id);
                        if ( query_personas != null )
                        {
                            Cliente nuevo_cliente = new Cliente();
                            nuevo_cliente.Puntos = 0;
                            nuevo_cliente.IdPersona = query_personas.Id;
                            context.Cliente.Add(nuevo_cliente);
                            context.SaveChanges();
                            return GenerarRespuesta("El usuario " + query_personas.Nombre + " " + query_personas.Apellido  +" ya existe en nuestra base de datos y se ha registrado como cliente para modificar su informacion de registro buscar en clientes y haga clic en la opcion editar", "0000");
                        }
                        else 
                        {
                            return GenerarRespuesta("Error. El cliente (" + ACRONIMO + "-" + NUMERO_DOCUMENTO + ") no existe", "0002");
                        } 
                    }
                }
                else
                {
                    return GenerarRespuesta("Error. Tipo de documento (" + ACRONIMO + ") no existe", "0001");
                }
            }
        }

        [HttpPost]
        public RespuestaTransaccion Post([FromBody]ClienteRequest clienteRequest)
        {
            try
            {
                using (FacturadorWebContext context = new FacturadorWebContext())
                {
                    var TD = context.TipoDocumento.SingleOrDefault(x => x.Acronimo == clienteRequest.TIPO_DOCUMENTO);
                    var resultado = context.Persona.Where(x => x.NumeroDocumento == clienteRequest.NUMERO_DOCUMENTO && x.IdTipoDocumento == TD.Id).ToList();
                    if (resultado.Count == 0)
                    {
                        Persona persona = new Persona();
                        persona.Nombre = clienteRequest.NOMBRE;
                        persona.Apellido = clienteRequest.APELLIDO;
                        persona.NumeroDocumento = clienteRequest.NUMERO_DOCUMENTO;
                        persona.IdTipoDocumento = TD.Id;
                        persona.Telefono = clienteRequest.TELEFONO;
                        persona.Email = clienteRequest.EMAIL;
                        persona.Direccion = clienteRequest.DIRECCION;
                        context.Persona.Add(persona);
                        context.SaveChanges();

                        int id_persona_transaccion = persona.Id;
                        Cliente cliente = new Cliente();
                        cliente.Puntos = 0;
                        cliente.IdPersona = id_persona_transaccion;
                        context.Cliente.Add(cliente);
                        context.SaveChanges();
                        return GenerarRespuesta("El cliente se registro con exito", "0000");
                    }
                    else
                    {
                        return GenerarRespuesta("Error. El cliente ya existe en nuestra base de datos", "0003");
                    }
                }
            }
            catch (Exception ex)
            {
                return GenerarRespuesta("Ocurrio un error durante el proceso de registro", "0004", ex);
            }
        }

        [HttpPut]
        public RespuestaTransaccion Put([FromBody] ClienteRequest clienteRequest) 
        {
            try 
            {
                using (FacturadorWebContext context = new FacturadorWebContext())
                {
                    var TD = context.TipoDocumento.SingleOrDefault(x => x.Acronimo == clienteRequest.TIPO_DOCUMENTO);
                    var resultado = context.Persona.SingleOrDefault(x => x.NumeroDocumento == clienteRequest.NUMERO_DOCUMENTO && x.IdTipoDocumento == TD.Id);
                    if (resultado != null)
                    {
                        resultado.Nombre = clienteRequest.NOMBRE;
                        resultado.Apellido = clienteRequest.APELLIDO;
                        resultado.Direccion = clienteRequest.DIRECCION;
                        resultado.Email = clienteRequest.EMAIL;
                        resultado.Telefono = clienteRequest.TELEFONO;
                        context.SaveChanges();
                        return GenerarRespuesta("Actualizacion exitosa", "0000");
                    }
                    else
                    {
                        return GenerarRespuesta("Error. El cliente no existe en nuestra base de datos", "0005");
                    }
                }
            }
            catch(Exception ex)            
            {
                return GenerarRespuesta("Ocurrio un error durante la actualizacion de los datos", "0006", ex);
            }
        }

        private RespuestaTransaccion GenerarRespuesta(string mensaje, string codigo, Exception ex = null)
        {
            RespuestaTransaccion RG = new RespuestaTransaccion();
            RG.CODIGO = codigo;
            RG.MENSAJE = mensaje;
            RG.EXCEPTION_TRACE = ex;
            return RG;
        }
    }
}