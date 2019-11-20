using DadisService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DadisService.Controllers
{
    public class ForoController : ApiController
    {
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        public List<MensajeForo> GetMensajes(string param)
        {
            List<MensajeForo> resultado = new List<MensajeForo>();
            
            return resultado;
        }


        [HttpPost]
        public  MensajeForo GetMensaje(int id)
        {
            MensajeForo MensajeResultado = new MensajeForo();



            return MensajeResultado;
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
