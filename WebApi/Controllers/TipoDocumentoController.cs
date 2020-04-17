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

        // GET: api/TipoDocumento/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/TipoDocumento
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/TipoDocumento/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
