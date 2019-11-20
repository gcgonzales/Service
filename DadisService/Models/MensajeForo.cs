using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DadisService.Models
{
    public class MensajeForo
    {
        public int Id { get; set; }

        public string Mensaje { get; set; }

        public DateTime FechaAlta { get; set; }

        public int IdUsuarioAlta { get; set; }

        public DateTime FechaModificacion { get; set; }


        public int IdUsuarioModificacion { get; set; }

        public int FechaBaja { get; set; }

        public int IdUsuarioBaja { get; set; }

        public int IdMensajeOriginal { get; set; }



    }
}