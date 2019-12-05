using DadisService.Models;
using DadisService.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace DadisService.Controllers
{
    public class QuedadaController : ApiController
    {
        [Route("api/Quedada/GetQuedadas")]
        [HttpGet]
        public List<Quedada> GetQuedadas(string textoBusqueda)
        {
            List<Quedada> resultado = new List<Quedada>();

            QuedadaService quedadaService = new QuedadaService();
            resultado = quedadaService.GetQuedadas(textoBusqueda);

            return resultado;
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}