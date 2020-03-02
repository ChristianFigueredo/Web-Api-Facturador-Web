using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi.Models;
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
                        var TD = context.TipoDocumento.SingleOrDefault(x => x.Id == c.Id);
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

        [HttpPost]
        public RespuestaTransaccion Post([FromBody]Cliente cliente)
        {
            try
            {
                List<Cliente> resultado = null;

                using (FacturadorWebContext context = new FacturadorWebContext())
                {
                    resultado = context.Cliente.Where(x => x.NumeroDocumento == cliente.NumeroDocumento && x.IdTipoDocumento == cliente.IdTipoDocumento).ToList();
                    if (resultado.Count == 0)
                    {
                        context.Cliente.Add(cliente);
                        context.SaveChanges();
                        return GenerarRespuesta("El cliente se registro con exito", "0000");
                    }
                    else
                    {
                        return GenerarRespuesta("Error. El cliente ya existe en nuestra base de datos", "0001");
                    }
                }
            }
            catch (Exception ex)
            {
                return GenerarRespuesta("Ocurrio un error durante el proceso de registro", "5562", ex);
            }
        }

        [HttpPut]
        public RespuestaTransaccion Put([FromBody] Cliente cliente) 
        {
            Cliente resultado = null;
            try 
            {
                using (FacturadorWebContext context = new FacturadorWebContext())
                {
                    resultado = context.Cliente.SingleOrDefault(x => x.NumeroDocumento == cliente.NumeroDocumento && x.IdTipoDocumento == cliente.IdTipoDocumento);
                    if (resultado != null)
                    {
                        resultado.Nombre = cliente.Nombre;
                        resultado.Apellido = cliente.Apellido;
                        resultado.Direccion = cliente.Direccion;
                        resultado.Email = cliente.Email;
                        resultado.Telefono = cliente.Telefono;
                        context.SaveChanges();
                        return GenerarRespuesta("Actualizacion exitosa", "0000");
                    }
                    else
                    {
                        return GenerarRespuesta("Error. El cliente no existe en nuestra base de datos", "0002");
                    }
                }
            }
            catch(Exception ex)            
            {
                return GenerarRespuesta("Ocurrio un error durante la actualizacion de los datos", "5562", ex);
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