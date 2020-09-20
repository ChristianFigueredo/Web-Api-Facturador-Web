using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DataLayer.Models.DB;
using WebApi_INVENTARIO.Response;
using Microsoft.AspNetCore.Cors;
using WebApi_INVENTARIO.Request;

namespace WebApi_INVENTARIO.Controllers
{
    [EnableCors("PoliticaCors")]
    [Route("api/[controller]")]
    [ApiController]
    public class InventarioController : ControllerBase
    {
        // GET: api/Inventario
        [HttpGet]
        public List<ProductoInventario> Get()
        {
            List<Inventario> resultado = null;
            List<ProductoInventario> listado_definitivo = new List<ProductoInventario>();
            using (FacturadorWebContext bd = new FacturadorWebContext()) 
            {
                resultado = bd.Inventario.ToList();
            }

            if (resultado != null)
            {
                foreach ( Inventario producto in resultado) 
                {
                    ProductoInventario PI = new ProductoInventario();
                    PI.Codigo = producto.Id;
                    PI.Descripcion = producto.Descripcion;
                    PI.Nombre = producto.Nombre;
                    PI.FechaRegistro = producto.FechaRegistro;
                    PI.TotalRecibidos = producto.TotalRecibidos;
                    PI.TotalVendidos = producto.TotalVendidos;
                    PI.TotalDevueltos = producto.TotalDevueltos;
                    PI.TotalDesincorporados = producto.TotalDesincorporados;
                    PI.TotalProceso = producto.TotalProceso;
                    PI.PrecioCompra = producto.PrecioCompra;
                    PI.PrecioVenta = producto.PrecioVenta;
                    PI.PorcentajeIva = producto.PorcentajeIva;
                    PI.PorcentajeDescuento = producto.PorcentajeDescuento;
                    listado_definitivo.Add(PI);
                }
            }
            return listado_definitivo;
        }

        // GET: api/Inventario/5
        [HttpGet("{id}", Name = "Get")]
        public ProductoInvetarioRespuestaTransaccion Get(int id)
        {
            Inventario Query_resultado = new Inventario();
            using (FacturadorWebContext bd = new FacturadorWebContext())
            {
                Query_resultado = bd.Inventario.SingleOrDefault(x => x.Id == id);
            }

            ProductoInvetarioRespuestaTransaccion resultado = new ProductoInvetarioRespuestaTransaccion();
            RespuestaTransaccion respuesta = new RespuestaTransaccion();
            if (Query_resultado != null)
            {
                resultado.Codigo = Query_resultado.Id;
                resultado.Descripcion = Query_resultado.Descripcion;
                resultado.Nombre = Query_resultado.Nombre;
                resultado.FechaRegistro = Query_resultado.FechaRegistro;
                resultado.TotalRecibidos = Query_resultado.TotalRecibidos;
                resultado.TotalVendidos = Query_resultado.TotalVendidos;
                resultado.TotalDevueltos = Query_resultado.TotalDevueltos;
                resultado.TotalDesincorporados = Query_resultado.TotalDesincorporados;
                resultado.TotalProceso = Query_resultado.TotalProceso;
                resultado.PrecioCompra = Query_resultado.PrecioCompra;
                resultado.PrecioVenta = Query_resultado.PrecioVenta;
                resultado.PorcentajeIva = Query_resultado.PorcentajeIva;
                resultado.PorcentajeDescuento = Query_resultado.PorcentajeDescuento;
                resultado.Respuesta = respuesta.GenerarRespuesta("Producto existe en nuestra base de datos", "0000");
            }
            else 
            {
                resultado.Respuesta = respuesta.GenerarRespuesta("Error al consultar inventario: el codigo de producto "+ id +" no existe en nuestra base de datos", "0009");
            }

            return resultado;
        }

        // POST: api/Inventario
        [HttpPost]
        public RespuestaTransaccion Post([FromBody] PostRequest producto)
        {
            RespuestaTransaccion RT = new RespuestaTransaccion();
            Inventario productoInventario = new Inventario();
            try
            {
                if (producto.Nombre != null && producto.PrecioCompra > 0 && producto.Descripcion != null && producto.PrecioVenta > producto.PrecioCompra && producto.TotalRecibidos > 0)
                {
                    productoInventario.Nombre = producto.Nombre;
                    productoInventario.Descripcion = producto.Descripcion;
                    productoInventario.PrecioCompra = producto.PrecioCompra;
                    productoInventario.PrecioVenta = producto.PrecioVenta;
                    productoInventario.TotalRecibidos = producto.TotalRecibidos;
                    productoInventario.FechaRegistro = DateTime.Now;
                    productoInventario.TotalDesincorporados = 0;
                    productoInventario.TotalDevueltos = 0;
                    productoInventario.TotalVendidos = 0;
                    productoInventario.TotalProceso = 0;
                    if (producto.PorcentajeDescuento == null) { productoInventario.PorcentajeDescuento = 0; } else { productoInventario.PorcentajeDescuento = producto.PorcentajeDescuento; }
                    if (producto.PorcentajeIva == null) { productoInventario.PorcentajeIva = 0; } else { productoInventario.PorcentajeIva = producto.PorcentajeIva; }

                    using (FacturadorWebContext bd = new FacturadorWebContext())
                    {
                        bd.Inventario.Add(productoInventario);
                        bd.SaveChanges();
                    }
                }
                else
                {
                    return RT.GenerarRespuesta("Error, registro fallido. Los datos en el registro no son validos. ", "0010");
                }

                if (productoInventario.Id != 0)
                {
                    return RT.GenerarRespuesta("El producto se registro con exito bajo el codigo " + productoInventario.Id + "", "0000");
                }
                else
                {
                    return RT.GenerarRespuesta("Error, registro fallido. Ocurrio un error inesperado crear el registro del producto", "0011");
                }
            }
            catch (Exception ex) 
            {
                return RT.GenerarRespuesta("Error, registro fallido. Ocurrio un error inesperado crear el registro del producto", "0012", ex);
            }
        }

        // PUT: api/Inventario/5
        [HttpPut]
        public RespuestaTransaccion Put([FromBody] PutRequest producto)
        {
            RespuestaTransaccion RT = new RespuestaTransaccion();
            Inventario inventario;
            try
            {
                using ( FacturadorWebContext bd = new FacturadorWebContext() ) 
                {
                    inventario = bd.Inventario.SingleOrDefault(x => x.Id == producto.Codigo);
                    if (inventario != null)
                    {
                        if (producto.Nombre != null && producto.PrecioCompra > 0 && producto.Descripcion != null && producto.PrecioVenta > producto.PrecioCompra)
                        {
                            inventario.Nombre = producto.Nombre;
                            inventario.Descripcion = producto.Descripcion;
                            inventario.PorcentajeDescuento = producto.PorcentajeDescuento;
                            inventario.PorcentajeIva = producto.PorcentajeIva;
                            inventario.PrecioCompra = producto.PrecioCompra;
                            inventario.PrecioVenta = producto.PrecioVenta;
                            bd.SaveChanges();
                            return RT.GenerarRespuesta("Actualizacion exitosa", "0000");
                        }
                        else
                        {
                            return RT.GenerarRespuesta("Error, registro fallido. Los datos en el registro no son validos. ", "0015");
                        }
                    }
                    else 
                    {
                        return RT.GenerarRespuesta("Error. No se encontro el codigo del producto " + producto.Codigo +  "", "0014");
                    }
                }
            }
            catch (Exception ex)
            {
                return RT.GenerarRespuesta("Error. Ocurrio un error inesperado", "0013", ex);
            }
        }
    }
}
