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

        [Route("api/Quedada/Guardar")]
        [HttpPost]
        public int Guardar(Quedada value)
        {
            Quedada quedadaGuardado = new Quedada();

            int resultado = 0;

            // usuarioGuardado.Id = int.Parse(DateTime.Now.Ticks.ToString().Substring(0, 5));
            quedadaGuardado.Titulo = value.Titulo;
            quedadaGuardado.Resumen = value.Resumen;
            quedadaGuardado.Descripcion = value.Descripcion;
            quedadaGuardado.IdUsuarioAlta = value.IdUsuarioAlta;
            quedadaGuardado.IdUsuarioModificacion = value.IdUsuarioModificacion;
            quedadaGuardado.Locacion = value.Locacion;
            quedadaGuardado.MaximoAsistentes = value.MaximoAsistentes;

            int anioQuedada = value.FechaEvento.Year;
            int mesQuedada = value.FechaEvento.Month;
            int diaQuedada = value.FechaEvento.Day;
            int horaQuedada = value.HoraEvento;
            int minutoQuedada = value.MinutoEvento;

            quedadaGuardado.FechaEvento = new DateTime(anioQuedada,mesQuedada,diaQuedada,horaQuedada,minutoQuedada,0);

            QuedadaService foroService = new QuedadaService();

            if (value.Id == 0)
            { resultado = foroService.CrearQuedada(quedadaGuardado);

                if (value.Fotografias != null)
                {
                    foreach (Fotografia fotografia in value.Fotografias)
                    {
                        fotografia.IdUsuario = value.Id;
                    }
                }

            }
            else
            {
                quedadaGuardado.Id = value.Id;
                resultado = foroService.EditarQuedada(quedadaGuardado);
            }

            FotografiaService fotografiaService = new FotografiaService();
            fotografiaService.AdjuntarFotografiasQuedadas(value.Fotografias);

            return quedadaGuardado.Id;
        }

        [Route("api/Quedada/GetQuedada")]
        [HttpGet]
        public Quedada GetQuedada(int id)
        {
            Quedada quedadaResultado = new Quedada();

            QuedadaService quedadaService = new QuedadaService();
            quedadaResultado = quedadaService.GetQuedada(id);

            return quedadaResultado;
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