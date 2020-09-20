using DataLayer.Models.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi_FACTURACION.Responses;
using WebApi_FACTURACION.Responses.Factura;

namespace WebApi_FACTURACION.Logic
{
    public class GetFacturaDetailResponse
    {
        public Response_GetFacturaWithDetailscs ConsultarDetalleFactura(Factura factura) 
        {
            Response_GetFacturaWithDetailscs facturaResponse = new Response_GetFacturaWithDetailscs();
            RespuestaTransaccion RT = new RespuestaTransaccion();
            try
            {
                using (FacturadorWebContext bd = new FacturadorWebContext())
                {
                    List<Productos> ListadoProductos = new List<Productos>();
                    Datoscliente cliente = new Datoscliente();

                    var query1 = from C in bd.Cliente
                                 join P in bd.Persona on C.IdPersona equals P.Id
                                 join T in bd.TipoDocumento on P.IdTipoDocumento equals T.Id
                                 where C.Id == factura.IdCliente

                                 select new Datoscliente
                                 {
                                     nombreCliente = P.Nombre,
                                     apellidoCliente = P.Apellido,
                                     acronimoCliente = T.Acronimo,
                                     documentoCliente = P.NumeroDocumento
                                 };
                    cliente = query1.FirstOrDefault();
                    var productos = bd.Producto.Where(x => x.IdFactura == factura.Id && x.Cantidad > 0).ToList();

                    facturaResponse.valorSubtotal = 0;
                    facturaResponse.valorIva = 0;
                    facturaResponse.valorDescuento = 0;
                    facturaResponse.valorTotal = 0;
                    foreach (Producto p in productos)
                    {
                        var inventario = bd.Inventario.FirstOrDefault(x => x.Id == p.IdInventario);
                        Productos prod = new Productos();
                        prod.Codigo = Convert.ToInt32(p.IdInventario);
                        prod.nombre = inventario.Nombre;
                        prod.descripcion = inventario.Descripcion;
                        prod.Cantidad = p.Cantidad;
                        prod.ValorUnitario = p.ValorUnitario;
                        prod.ValorTotal = p.ValorTotal;
                        prod.PorcentajeDescuento = p.PorcentajeDescuento;
                        prod.ValorTotalDescuento = p.ValorTotalDescuento;
                        prod.PorcentajeIva = p.PorcentajeIva;
                        prod.ValorTotalIva = p.ValorTotalIva;

                        /* valores en factura */
                        facturaResponse.valorSubtotal = facturaResponse.valorSubtotal + p.ValorTotal;
                        facturaResponse.valorIva = facturaResponse.valorIva + p.ValorTotalIva;
                        facturaResponse.valorDescuento = facturaResponse.valorDescuento + p.ValorTotalDescuento;
                        ListadoProductos.Add(prod);
                    }
                    facturaResponse.valorTotal = facturaResponse.valorSubtotal - facturaResponse.valorDescuento + facturaResponse.valorIva;
                    facturaResponse.numeroFactura = factura.Numero.ToString();
                    facturaResponse.cliente = cliente;
                    facturaResponse.respuestaTransaccion = RT.GenerarRespuesta("Consulta exitosa", "0000");
                    facturaResponse.productos = ListadoProductos;
                }
            }
            catch (Exception ex) 
            {
                facturaResponse.respuestaTransaccion = RT.GenerarRespuesta("Ocurrio un error: " + ex.Message, "9090");
                return facturaResponse;
            }
            
            return facturaResponse;
        }
    }
}
