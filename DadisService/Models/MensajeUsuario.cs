using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DadisService.Models
{
    public class MensajeUsuario
    {

        public int Id { get; set; }

        public int IdUsuarioRemitente { get; set; }

        public int IdUsuarioReceptor { get; set; }

        public DateTime FechaMensaje { get; set; }

        public DateTime FechaBaja { get; set; }

        public string Mensaje { get; set; }

    }
}