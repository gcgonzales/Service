using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DadisService.Models
{
    public class Apuntado
    {
        public Apuntado()
        {
           // usuario = new Usuario();
            ApuntadosAdultos = 0;
            ApuntadosNinos = 0;
        }

        public int Id { get; set; }

        public int IdUsuario { get; set; }

        public int IdQuedada { get; set; }

        public int IdContratacion { get; set; }

        public int IdOfertaAccesorio { get; set; }

        public string NombreUsuario { get; set; }

        public string RutaFotoApuntado { get; set; }

        public int ApuntadosAdultos { get; set; }
        public int ApuntadosNinos { get; set; }

        public DateTime FechaAlta { get; set; }
    }
}