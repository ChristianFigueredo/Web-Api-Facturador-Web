using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Models.DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebApi_FACTURACION.Hubs;
using WebApi_FACTURACION.Logic;
using WebApi_FACTURACION.Request;
using WebApi_FACTURACION.Responses;
using WebApi_FACTURACION.Responses.Factura;
using WebApi_FACTURACION.Services;

namespace WebApi_FACTURACION.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosController : ControllerBase
    {
        private readonly ISignalService _signalService;
        private readonly IHubContext<FacturaHub> _hubContext;

        public ProductosController(ISignalService signalService, IHubContext<FacturaHub> hubContext)
        {
            _signalService = signalService;
            _hubContext = hubContext;
        }
        // GET: api/Productos
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Productos/5
        [HttpGet("{userId}", Name = "GetFacturaDetails")]
        public Response_GetFacturaWithDetailscs Get(int userId)
        {
            Response_GetFacturaWithDetailscs facturaResponse = new Response_GetFacturaWithDetailscs();
            RespuestaTransaccion RT = new RespuestaTransaccion();
            GetFacturaDetailResponse consulta = new GetFacturaDetailResponse();
            using (FacturadorWebContext bd = new FacturadorWebContext())
            {
                var usuario = bd.Usuario.SingleOrDefault(x => x.Id == userId && x.Estado == true);
                if (usuario != null)
                {
                    var factura = bd.Factura.SingleOrDefault(x => x.IdUsuario == userId && x.IdEstado == 1);
                    if (factura != null)
                    {
                        facturaResponse = consulta.ConsultarDetalleFactura(factura);
                    }
                    else
                    {
                        facturaResponse.respuestaTransaccion = RT.GenerarRespuesta("Error. El operador no tiene facturas en proceso", "0025");
                    }
                }
                else
                {
                    facturaResponse.respuestaTransaccion = RT.GenerarRespuesta("Error. El operador no existe o no tiene permisos para facturar", "0026");
                }
            }
            return facturaResponse;
        }



        // POST: api/Productos
        [HttpPost]
        public async Task<RespuestaTransaccion> PostAsync([FromBody] Request_Objetc_Add_Producto_To_Factura entrada)
        {
            RespuestaTransaccion RG = new RespuestaTransaccion();
            GetFacturaDetailResponse consulta = new GetFacturaDetailResponse();
            Response_GetFacturaWithDetailscs facturaResponse = new Response_GetFacturaWithDetailscs();
            using (FacturadorWebContext bd = new FacturadorWebContext()) 
            {
                var facturaEnProceso = bd.Factura.FirstOrDefault(x  => x.IdUsuario == entrada.idUsuario && x.IdEstado == 1); 
                
                if (facturaEnProceso != null)
                {
                    var productoInventario = bd.Inventario.FirstOrDefault(x => x.Id == entrada.codBarras);
                    if (productoInventario != null)
                    {
                        //varificar si hay unidades disponibles y si hay unidades entonces descontar unidad del total disponible y aumentar unidades en proceso
                        if ((productoInventario.TotalRecibidos - productoInventario.TotalVendidos - productoInventario.TotalProceso - productoInventario.TotalDevueltos - productoInventario.TotalDesincorporados) > 0)
                        {
                            productoInventario.TotalProceso++;
                            bd.SaveChanges();

                            var productoProductoEnProceso = bd.Producto.FirstOrDefault(x => x.IdInventario == entrada.codBarras && x.IdFactura == facturaEnProceso.Id);
                            if (productoProductoEnProceso != null)
                            {
                                productoProductoEnProceso.Cantidad++;
                                productoProductoEnProceso.ValorTotal = productoProductoEnProceso.ValorUnitario * productoProductoEnProceso.Cantidad;
                                productoProductoEnProceso.ValorTotalDescuento = (productoProductoEnProceso.PorcentajeDescuento * productoProductoEnProceso.ValorTotal) / 100;
                                productoProductoEnProceso.ValorTotalIva = (productoProductoEnProceso.PorcentajeIva * productoProductoEnProceso.ValorTotal) / 100;
                            }
                            else
                            {
                                Producto producto = new Producto();
                                producto.ValorUnitario = productoInventario.PrecioVenta;
                                producto.ValorTotal = productoInventario.PrecioVenta;
                                producto.Cantidad = 1;
                                producto.PorcentajeIva = productoInventario.PorcentajeIva;
                                producto.ValorTotalIva = (productoInventario.PorcentajeIva * producto.ValorTotal) / 100;
                                producto.PorcentajeDescuento = productoInventario.PorcentajeDescuento;
                                producto.ValorTotalDescuento = (productoInventario.PorcentajeDescuento * producto.ValorTotal) / 100;
                                producto.IdFactura = facturaEnProceso.Id;
                                producto.IdInventario = entrada.codBarras;
                                bd.Producto.Add(producto);
                            }
                            bd.SaveChanges();
                            facturaResponse = consulta.ConsultarDetalleFactura(facturaEnProceso);
                            await _hubContext.Clients.All.SendAsync("SignalrFacturaReceived", facturaResponse);
                            return RG.GenerarRespuesta("Producto añadido con exito", "0000");
                        }
                        else
                        {
                            return RG.GenerarRespuesta("Error. el producto bajo el codigo indicado no tiene unidades disponibles en el inventario", "0017");
                        }
                    }
                    else
                    {
                        return RG.GenerarRespuesta("Error, el codigo de producto no existe en BD", "0016");
                    }
                }
                else 
                {
                    return RG.GenerarRespuesta("Error, El operador no tiene una factura en proceso", "0018"); 
                }
            }
        }

        // PUT: api/Facturacion/5
        [HttpPut]
        public async Task<RespuestaTransaccion> PutAsync([FromBody] Request_Objetc_Add_Producto_To_Factura entrada)
        {
            RespuestaTransaccion RG = new RespuestaTransaccion();
            GetFacturaDetailResponse consulta = new GetFacturaDetailResponse();
            Response_GetFacturaWithDetailscs facturaResponse = new Response_GetFacturaWithDetailscs();
            using (FacturadorWebContext bd = new FacturadorWebContext())
            {
                var facturaEnProceso = bd.Factura.FirstOrDefault(x => x.IdUsuario == entrada.idUsuario && x.IdEstado == 1);

                if (facturaEnProceso != null)
                {
                    var productoInventario = bd.Inventario.FirstOrDefault(x => x.Id == entrada.codBarras);
                    if (productoInventario != null)
                    {
                        productoInventario.TotalProceso--;
                        bd.SaveChanges();

                        var productoProductoEnProceso = bd.Producto.FirstOrDefault(x => x.IdInventario == entrada.codBarras && x.IdFactura == facturaEnProceso.Id);
                        if (productoProductoEnProceso != null)
                        {
                            if (productoProductoEnProceso.Cantidad > 0)
                            {
                                productoProductoEnProceso.Cantidad--;
                                productoProductoEnProceso.ValorTotal = productoProductoEnProceso.ValorUnitario * productoProductoEnProceso.Cantidad;
                                productoProductoEnProceso.ValorTotalDescuento = (productoProductoEnProceso.PorcentajeDescuento * productoProductoEnProceso.ValorTotal) / 100;
                                productoProductoEnProceso.ValorTotalIva = (productoProductoEnProceso.PorcentajeIva * productoProductoEnProceso.ValorTotal) / 100;
                                bd.SaveChanges();
                            }
                            else
                            {
                                return RG.GenerarRespuesta("Error, ya se desconto el total de unidades disponibles del producto para la factura", "0026");
                            }
                        }
                        else
                        {
                            return RG.GenerarRespuesta("Error, El producto no esta asociado a la factura", "0025");
                        }  
                        facturaResponse = consulta.ConsultarDetalleFactura(facturaEnProceso);
                        await _hubContext.Clients.All.SendAsync("SignalrFacturaReceived", facturaResponse);
                        return RG.GenerarRespuesta("Producto descontado de la factura. Transaccion exitosa", "0000");
                    }
                    else
                    {
                        return RG.GenerarRespuesta("Error, el codigo de producto no existe en BD", "0027");
                    }
                }
                else
                {
                    return RG.GenerarRespuesta("Error, El operador no tiene una factura en proceso", "0028");
                }
            }
        }
    }
}
