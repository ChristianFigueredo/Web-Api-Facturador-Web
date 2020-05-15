using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLayer.Models.DB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WebApi_FACTURACION.Hubs;
using WebApi_FACTURACION.Request;
using WebApi_FACTURACION.Responses;
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


        // POST: api/Productos
        [HttpPost]
        public async Task<RespuestaTransaccion> PostAsync([FromBody] Request_Objetc_Add_Producto_To_Factura entrada)
        {
            RespuestaTransaccion RG = new RespuestaTransaccion();
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
                            productoInventario.TotalRecibidos--;
                            bd.SaveChanges();

                            var productoProductoEnProceso = bd.Producto.FirstOrDefault(x => x.Id == entrada.codBarras && x.IdFactura == facturaEnProceso.Id);
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
                            await _hubContext.Clients.All.SendAsync("SignalrFacturaReceived", entrada.codBarras.ToString());
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
                    return RG.GenerarRespuesta("Error, El facturador existe una factura en proceso", "0018");
                }
            }
        }

        // PUT: api/Facturacion/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }
    }
}
