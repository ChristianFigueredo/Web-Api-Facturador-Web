using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
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
                        var TD = context.TipoDocumento.SingleOrDefault(x => x.Id == c.IdTipoDocumento);
                        GetClienteResponse GCR = new GetClienteResponse();
                        GCR.NOMBRE = c.Nombre;
                        GCR.APELLIDO = c.Apellido;
                        GCR.NUMERO_DOCUMENTO = c.NumeroDocumento;
                        GCR.TELEFONO = c.Telefono;
                        GCR.EMAIL = c.Email;
                        GCR.DIRECCION = c.Direccion;
                        GCR.TIPO_DOCUMENTO = TD.Acronimo;
                        ListaClientesResponse.Add(GCR);
                    }
                } 
            }
            return ListaClientesResponse;
        }

        // GET api/[controller]/ACRONIMO/NUMERO_DOCUMENTO
        [HttpGet("ACRONIMO/{ACRONIMO}/NUMERO_DOCUMENTO/{NUMERO_DOCUMENTO}")]
        public RespuestaTransaccion Get(string ACRONIMO, string NUMERO_DOCUMENTO) 
        {
            Cliente cliente = null;

            using (FacturadorWebContext context = new FacturadorWebContext()) 
            {
                var TD = context.TipoDocumento.SingleOrDefault(x => x.Acronimo == ACRONIMO);

                if (TD != null)
                {
                    cliente = context.Cliente.SingleOrDefault(c => c.NumeroDocumento == NUMERO_DOCUMENTO && c.IdTipoDocumento == TD.Id);
                    if (cliente != null)
                    {
                        return GenerarRespuesta("Cliente existe en nuestra base de datos", "0000");
                    }
                    else 
                    {
                        return GenerarRespuesta("Error. El cliente (" + ACRONIMO + "-" + NUMERO_DOCUMENTO + ") no existe", "0002");
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
                List<Cliente> resultado = null;

                using (FacturadorWebContext context = new FacturadorWebContext())
                {
                    var TD = context.TipoDocumento.SingleOrDefault(x => x.Acronimo == clienteRequest.TIPO_DOCUMENTO);
                    resultado = context.Cliente.Where(x => x.NumeroDocumento == clienteRequest.NUMERO_DOCUMENTO && x.IdTipoDocumento == TD.Id).ToList();
                    if (resultado.Count == 0)
                    {
                        Cliente cliente = new Cliente();

                        cliente.Nombre = clienteRequest.NOMBRE;
                        cliente.Apellido = clienteRequest.APELLIDO;
                        cliente.NumeroDocumento = clienteRequest.NUMERO_DOCUMENTO;
                        cliente.IdTipoDocumento = TD.Id;
                        cliente.Telefono = clienteRequest.TELEFONO;
                        cliente.Email = clienteRequest.EMAIL;
                        cliente.Direccion = clienteRequest.DIRECCION;

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
            Cliente resultado = null;
            try 
            {
                using (FacturadorWebContext context = new FacturadorWebContext())
                {
                    var TD = context.TipoDocumento.SingleOrDefault(x => x.Acronimo == clienteRequest.TIPO_DOCUMENTO);
                    resultado = context.Cliente.SingleOrDefault(x => x.NumeroDocumento == clienteRequest.NUMERO_DOCUMENTO && x.IdTipoDocumento == TD.Id);
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