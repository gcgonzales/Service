using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DadisService.Models
{
    public class MensajeForo
    {
        public int Id { get; set; }

        public string Titulo { get; set; }

        public string TituloPadre { get; set; }

        public string Mensaje { get; set; }

        public DateTime FechaAlta { get; set; }

        public int IdUsuarioAlta { get; set; }

        public DateTime FechaModificacion { get; set; }


        public int IdUsuarioModificacion { get; set; }

        public int FechaBaja { get; set; }

        public int IdUsuarioBaja { get; set; }

        public int IdMensajePadre { get; set; }

        public string Autor { get; set; }

        public string RutaFotoAutor { get; set; }

        public string UltimoAutor { get; set; }

        public DateTime FechaUltimaContestacion { get; set; }
    }
}