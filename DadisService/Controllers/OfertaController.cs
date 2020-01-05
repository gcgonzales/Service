using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DadisService.Models;
using DadisService.Service;

namespace DadisService.Controllers
{
    public class OfertaController : ApiController
    {
        [Route("api/Oferta/GetOfertas")]
        [HttpGet]
        public List<OfertaAccesorio> GetQuedadas(string textoBusqueda)
        {
            List<OfertaAccesorio> resultado = new List<OfertaAccesorio>();

            OfertaAccesoriosService ofertaService = new OfertaAccesoriosService();
            resultado = ofertaService.GetOfertaAccesorios(textoBusqueda);

            return resultado;
        }

        [Route("api/Oferta/Guardar")]
        [HttpPost]
        public int Guardar(OfertaAccesorio value)
        {
            OfertaAccesorio ofertaGuardado = new OfertaAccesorio();

            int resultado = 0;

            ofertaGuardado.Titulo = value.Titulo;
            ofertaGuardado.Resumen = value.Resumen;
            ofertaGuardado.Descripcion = value.Descripcion;
            ofertaGuardado.IdUsuarioAlta = value.IdUsuarioAlta;
            ofertaGuardado.IdUsuarioModificacion = value.IdUsuarioModificacion;
            ofertaGuardado.Locacion = value.Locacion;
            ofertaGuardado.Precio = value.Precio;
            
            OfertaAccesoriosService ofertaService = new OfertaAccesoriosService();

            if (value.Id == 0)
            {
                resultado = ofertaService.CrearOfertaAccesorio(ofertaGuardado);
                value.Id = resultado;

                if (value.Fotografias != null)
                {
                    foreach (Fotografia fotografia in value.Fotografias)
                    {
                        fotografia.IdQuedada = value.Id;
                    }
                }

            }
            else
            {
                ofertaGuardado.Id = value.Id;
                resultado = ofertaService.EditarOfertaAccesorio(ofertaGuardado);
            }

            FotografiaService fotografiaService = new FotografiaService();
            fotografiaService.AdjuntarFotografiasOfertas(value.Fotografias);

            return ofertaGuardado.Id;
        }

        [Route("api/Oferta/Apuntarse")]
        [HttpPost]
        public int Apuntarse(Apuntado value)
        {
            int resultado = 0;

            // usuarioGuardado.Id = int.Parse(DateTime.Now.Ticks.ToString().Substring(0, 5));
            QuedadaService quedadaService = new QuedadaService();
            resultado = quedadaService.ApuntarseQuedada(value);

            return resultado;
        }

        [Route("api/Oferta/Desapuntarse")]
        [HttpPost]
        public int Desapuntarse(Apuntado value)
        {
            int resultado = 0;

            QuedadaService quedadaService = new QuedadaService();
            resultado = quedadaService.DesapuntarseQuedada(value);

            return resultado;
        }

        [Route("api/Oferta/GetOferta")]
        [HttpGet]
        public OfertaAccesorio GetOferta(int id)
        {
            OfertaAccesorio ofertaResultado = new OfertaAccesorio();

            OfertaAccesoriosService ofertaService = new OfertaAccesoriosService();
            ofertaResultado = ofertaService.GetOfertaAccesorio(id);

            return ofertaResultado;
        }
    }
}
