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
    public class ContratacionController : ApiController
    {
        [Route("api/Contratacion/GetContrataciones")]
        [HttpGet]
        public List<Contratacion> GetContrataciones(string textoBusqueda)
        {
            List<Contratacion> resultado = new List<Contratacion>();

            Contratacioneservice Contratacioneservice = new Contratacioneservice();
            resultado = Contratacioneservice.GetContrataciones(textoBusqueda);

            return resultado;
        }

        [Route("api/Contratacion/Guardar")]
        [HttpPost]
        public int Guardar(Contratacion value)
        {
            Contratacion ContratacionGuardado = new Contratacion();

            int resultado = 0;

            // usuarioGuardado.Id = int.Parse(DateTime.Now.Ticks.ToString().Substring(0, 5));
            ContratacionGuardado.Titulo = value.Titulo;
            ContratacionGuardado.Descripcion = value.Descripcion;
            ContratacionGuardado.IdUsuarioAlta = value.IdUsuarioAlta;
            ContratacionGuardado.IdUsuarioModificacion = value.IdUsuarioModificacion;
            ContratacionGuardado.Locacion = value.Locacion;
            ContratacionGuardado.MaximoHijos = value.MaximoHijos;
            ContratacionGuardado.PrecioTotal = value.PrecioTotal;
            int anioContratacion = value.FechaContratacion.Year;
            int mesContratacion = value.FechaContratacion.Month;
            int diaContratacion = value.FechaContratacion.Day;
            int horaContratacion = value.HoraContratacion;
            int minutoContratacion = value.MinutoContratacion;

            ContratacionGuardado.FechaContratacion = new DateTime(anioContratacion, mesContratacion, diaContratacion, horaContratacion, minutoContratacion, 0);

            Contratacioneservice foroService = new Contratacioneservice();

            if (value.Id == 0)
            {
                resultado = foroService.CrearContratacion(ContratacionGuardado);
                value.Id = resultado;

                if (value.Fotografias != null)
                {
                    foreach (Fotografia fotografia in value.Fotografias)
                    {
                        fotografia.IdContratacion = value.Id;
                    }
                }

            }
            else
            {
                ContratacionGuardado.Id = value.Id;
                resultado = foroService.EditarContratacion(ContratacionGuardado);
            }

            FotografiaService fotografiaService = new FotografiaService();
            fotografiaService.AdjuntarFotografiasContrataciones(value.Fotografias);

            return ContratacionGuardado.Id;
        }

        [Route("api/Contratacion/Apuntarse")]
        [HttpPost]
        public int Apuntarse(Apuntado value)
        {
            int resultado = 0;

            // usuarioGuardado.Id = int.Parse(DateTime.Now.Ticks.ToString().Substring(0, 5));
            Contratacioneservice Contratacioneservice = new Contratacioneservice();
            resultado = Contratacioneservice.ApuntarseContratacion(value);

            return resultado;
        }

        [Route("api/Contratacion/Desapuntarse")]
        [HttpPost]
        public int Desapuntarse(Apuntado value)
        {
            int resultado = 0;

            Contratacioneservice Contratacioneservice = new Contratacioneservice();
            resultado = Contratacioneservice.DesapuntarseContratacion(value);

            return resultado;
        }

        [Route("api/Contratacion/GetContratacion")]
        [HttpGet]
        public Contratacion GetContratacion(int id)
        {
            Contratacion ContratacionResultado = new Contratacion();

            Contratacioneservice Contratacioneservice = new Contratacioneservice();
            ContratacionResultado = Contratacioneservice.GetContratacion(id);

            return ContratacionResultado;
        }

    }
}
