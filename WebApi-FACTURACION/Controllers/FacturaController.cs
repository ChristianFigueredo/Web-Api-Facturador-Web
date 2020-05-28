using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Models.DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi_FACTURACION.Logic;
using WebApi_FACTURACION.Request;
using WebApi_FACTURACION.Responses;
using WebApi_FACTURACION.Responses.Factura;

namespace WebApi_FACTURACION.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FacturaController : ControllerBase
    {
        // GET: api/Factura
        [HttpGet]
        public List<Response_GetAllFacturas> Get()
        {
            List<Factura> ListadoFactura = null;
            List<Response_GetAllFacturas> ListaFacturasResponse = new List<Response_GetAllFacturas>();

            using (FacturadorWebContext context = new FacturadorWebContext())
            {
                ListadoFactura = context.Factura.ToList();
                if (ListadoFactura != null)
                {
                    foreach (Factura c in ListadoFactura)
                    {
                        var query = from F in context.Factura
                                    join E in context.EstadosFactura on F.IdEstado equals E.Id

                                    select new Response_GetAllFacturas
                                    {
                                        Id = F.Id,
                                        Numero = F.Numero,
                                        ValorTotal = F.ValorTotal,
                                        ValorSubtotal = F.ValorSubtotal,
                                        ValorIva = F.ValorIva,
                                        ValorDescuento = F.ValorDescuento,
                                        FechaApertura = F.FechaApertura,
                                        FechaCierre = F.FechaCierre,
                                        Estado = E.Descripcion
                                    };
                        ListaFacturasResponse = query.ToList();
                    }
                }
            }
            return ListaFacturasResponse;
        }

        // GET: api/Factura/5
        [HttpGet("{userId}", Name = "Get")]
        public Response_GetFacturaEnProceso Get(int userId)
        {
            Response_GetFacturaEnProceso resultado = new Response_GetFacturaEnProceso();
            RespuestaTransaccion RT = new RespuestaTransaccion();

            using (FacturadorWebContext bd = new FacturadorWebContext()) 
            {
                var usuario = bd.Usuario.FirstOrDefault(x => x.Id == userId && x.Estado == true);
                if (usuario != null)
                {
                    var query = from F in bd.Factura
                                join C in bd.Cliente on F.IdCliente equals C.Id
                                join P in bd.Persona on C.IdPersona equals P.Id
                                join T in bd.TipoDocumento on P.IdTipoDocumento equals T.Id
                                where F.IdUsuario == userId && F.IdEstado == 1

                                select new Response_GetFacturaEnProceso
                                {
                                    numeroFactura = F.Numero.ToString(),
                                    numerodocumento = P.NumeroDocumento,
                                    acronimo = T.Acronimo
                                };

                    resultado = query.FirstOrDefault();

                    if (resultado != null)
                    {
                        resultado.respuestaTransaccion = RT.GenerarRespuesta("Error, el operador ya tiene una factura en proceso", "0023");
                    }
                    else
                    {
                        resultado = new Response_GetFacturaEnProceso();
                        resultado.respuestaTransaccion = RT.GenerarRespuesta("Continuar proceso de facturacion - crear nueva factura", "0000");
                    }
                }
                else
                {
                     resultado.respuestaTransaccion = RT.GenerarRespuesta("Error, el operador no existe o no tiene permisos para ejecutar el proceso de facturacion", "0024");
                }
            }
            return resultado;
        }

        // POST: api/Factura
        [HttpPost]
        public RespuestaTransaccion Post([FromBody] Request_Object_Create_Invoice objeto_entrada)
        {
            RespuestaTransaccion RG = new RespuestaTransaccion();
            Cliente cliente = null;
            using (FacturadorWebContext bd = new FacturadorWebContext()) 
            {
                var usuario = bd.Usuario.FirstOrDefault(x => x.Id == objeto_entrada.userId && x.Estado == true);

                if (usuario != null)
                {

                    var TD = bd.TipoDocumento.SingleOrDefault(x => x.Acronimo == objeto_entrada.clientDocumentType);
                    if (TD != null)
                    {
                        // BUSCAR EN PERSONAS - CLIENTES
                        var query_clientes = from C in bd.Cliente
                                             join P in bd.Persona on C.IdPersona equals P.Id
                                             join T in bd.TipoDocumento on P.IdTipoDocumento equals T.Id
                                             where T.Acronimo == objeto_entrada.clientDocumentType && P.NumeroDocumento == objeto_entrada.clientDocumentNumber
                                             select new Cliente { Id = C.Id };
                        cliente = query_clientes.FirstOrDefault();

                        if (cliente != null)
                        {
                            var facturaEnProceso = bd.Factura.FirstOrDefault(x => x.IdUsuario == objeto_entrada.userId && x.IdEstado == 1);

                            if (facturaEnProceso != null)
                            {
                                return RG.GenerarRespuesta("Error, El operador ya tiene una factura en proceso", "0022");
                            }
                            else
                            {
                                Factura factura = new Factura();
                                factura.FechaApertura = DateTime.Now;
                                factura.IdEstado = 1;
                                factura.IdCliente = cliente.Id;
                                factura.IdUsuario = usuario.Id;
                                bd.Factura.Add(factura);
                                bd.SaveChanges();
                                return RG.GenerarRespuesta("Factura creada con exito", "0000");
                            }
                        }
                        else
                        {
                            return RG.GenerarRespuesta("Error. El cliente(" + objeto_entrada.clientDocumentType + " - " + objeto_entrada.clientDocumentNumber + ") no existe", "0020");
                        }
                    }
                    else
                    {
                        return RG.GenerarRespuesta("Error. Tipo de documento (" + objeto_entrada.clientDocumentType + ") no existe", "0019");
                    }
                }
                else 
                {
                    return RG.GenerarRespuesta("Error. El cajero no esta registrado en base de datos o no esta autorizado para realizar el proceso de facturacion", "0021");
                }
            }
        }

        // PUT: api/Factura/5
        [HttpPut]
        public RespuestaTransaccion Put( [FromBody] UpdateFactura datos)
        {
            RespuestaTransaccion RG = new RespuestaTransaccion();
            Response_GetFacturaWithDetailscs facturaResponse = new Response_GetFacturaWithDetailscs();
            GetFacturaDetailResponse consulta = new GetFacturaDetailResponse();
            using (FacturadorWebContext bd = new FacturadorWebContext())
            {
                var facturaEnProceso = bd.Factura.FirstOrDefault(x => x.IdUsuario == datos.idUsuario && x.IdEstado == 1);
                facturaResponse = consulta.ConsultarDetalleFactura(facturaEnProceso);
                if (facturaEnProceso != null)
                {
                    facturaEnProceso.ValorTotal = facturaResponse.valorTotal;
                    facturaEnProceso.ValorSubtotal = facturaResponse.valorSubtotal;
                    facturaEnProceso.ValorIva = facturaResponse.valorIva;
                    facturaEnProceso.ValorDescuento = facturaResponse.valorDescuento;
                    facturaEnProceso.FechaCierre = DateTime.Now;
                    if (datos.operacion == 1) {  facturaEnProceso.IdEstado = 2; }
                    if (datos.operacion == 2) {  facturaEnProceso.IdEstado = 3; }
                    bd.SaveChanges();

                    foreach (Productos p in facturaResponse.productos) 
                    {
                        var inventario = bd.Inventario.SingleOrDefault(x => x.Id == p.Codigo);
                        if (datos.operacion == 1) 
                        {
                            inventario.TotalVendidos = inventario.TotalVendidos + p.Cantidad;
                        }
                        inventario.TotalProceso = inventario.TotalProceso - p.Cantidad;
                        bd.SaveChanges();
                    }
                    return RG.GenerarRespuesta("Operacion Exitosa", "0000");
                }
                else
                {
                    return RG.GenerarRespuesta("Error, El operador no tiene una factura en proceso", "0029");
                }
            }
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
