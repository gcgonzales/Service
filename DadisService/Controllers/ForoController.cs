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
    public class ForoController : ApiController
    {
        public string Get(int id)
        {
            return "value";
        }

       

        [Route("api/Foro/GetMensajesPrincipales")]
        [HttpGet]
        public List<MensajeForo> GetMensajesPrincipales(string textoBusqueda)
        {
            List<MensajeForo> resultado = new List<MensajeForo>();

            ForoService foroService = new ForoService();
            resultado = foroService.GetTemasPrincipales(textoBusqueda); 
            
            return resultado;
        }

        [Route("api/Foro/GetMensaje")]
        [HttpGet]
        public  MensajeForo GetMensaje(int id)
        {
            MensajeForo MensajeResultado = new MensajeForo();

            ForoService foroService = new ForoService();
            MensajeResultado = foroService.GetMensaje(id);

            return MensajeResultado;
        }

        [Route("api/Foro/GetHiloTema")]
        [HttpGet]
        public List<MensajeForo> GetHiloTema(int idMensajePadre)
        {
            List<MensajeForo> resultado = new List<MensajeForo>();

            ForoService foroService = new ForoService();
            resultado = foroService.GetHiloTema(idMensajePadre);

            return resultado;
        }

        [Route("api/Foro/Guardar")]
        [HttpPost]
        public int Guardar(MensajeForo value)
        {
            MensajeForo mensajeGuardado = new MensajeForo();

            int resultado = 0;

            // usuarioGuardado.Id = int.Parse(DateTime.Now.Ticks.ToString().Substring(0, 5));
            mensajeGuardado.Titulo = value.Titulo;
            mensajeGuardado.Mensaje = value.Mensaje;
            mensajeGuardado.IdUsuarioAlta = value.IdUsuarioAlta;
            mensajeGuardado.IdUsuarioModificacion = value.IdUsuarioModificacion;
            mensajeGuardado.IdMensajePadre = value.IdMensajePadre;

            ForoService foroService = new ForoService();

            if (value.Id == 0)
            { resultado = foroService.CrearMensajeForo(mensajeGuardado); }
            else
            {
                mensajeGuardado.Id = value.Id;
                resultado = foroService.EditarMensajeForo(mensajeGuardado);

            }


            return mensajeGuardado.Id;
        }

        [Route("api/Foro/BajaMensajesForo")]
        [HttpPost]
        public int BajaMensajesForo(string[] idsMensajes)
        {
            Usuario usuarioGuardado = new Usuario();

            int resultado = 0;

            ForoService foroService = new ForoService();
            resultado = foroService.BajaMensajesForo(idsMensajes);

            return usuarioGuardado.Id;
        }

        [HttpPost]
        public int Crear(MensajeForo value)
        {
            MensajeForo nuevoMensaje = new MensajeForo();

            nuevoMensaje.Id = int.Parse(DateTime.Now.Ticks.ToString().Substring(0, 5));
            return nuevoMensaje.Id;
        }

        [HttpPost]
        public int Editar(MensajeForo value)
        {
            MensajeForo nuevoMensaje = new MensajeForo();

            nuevoMensaje.Id = int.Parse(DateTime.Now.Ticks.ToString().Substring(0, 5));
            return nuevoMensaje.Id;
        }

        [HttpPost]
        public int Baja(int idUsuario, int idUsuarioResponsable)
        {
            MensajeForo nuevoMensaje = new MensajeForo();

            nuevoMensaje.Id = int.Parse(DateTime.Now.Ticks.ToString().Substring(0, 5));
            return nuevoMensaje.Id;
        }



        // POST api/foro
        public void Post([FromBody]string value)
        {
        }

        // PUT api/foro/5
        public void Put(int id, [FromBody]string value)
        {
        }

       

        // DELETE api/foro/5
        public void Delete(int id)
        {
        }
    }
}
