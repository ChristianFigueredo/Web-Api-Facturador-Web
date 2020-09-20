using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using DataLayer.Models.DB;
using WebApi.Models.Response;

namespace WebApi.Properties
{
    [Route("api/[controller]")]
    [ApiController]
    public class TipoDocumentoController : ControllerBase
    {
        // GET: api/TipoDocumento
        [HttpGet]
        public List<GetTiposDocumentos> Get()
        {
            List<TipoDocumento> consulta = null;
            List<GetTiposDocumentos> respuesta = new List<GetTiposDocumentos>();
            using (FacturadorWebContext bd = new FacturadorWebContext()) 
            {
                consulta = bd.TipoDocumento.ToList();
            }

            foreach (TipoDocumento td in consulta)
            {
                GetTiposDocumentos tipo_documento = new GetTiposDocumentos();
                tipo_documento.DESCRIPCION = td.Descripcion;
                tipo_documento.ACRONIMO = td.Acronimo;
                respuesta.Add(tipo_documento);
            }
            return respuesta;
        }
    }
}
