using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DadisService.Models
{
    public class ApuntadoQuedada
    {
        public ApuntadoQuedada()
        {
           // usuario = new Usuario();
            ApuntadosAdultos = 0;
            ApuntadosNinos = 0;
        }

        public int Id { get; set; }

        public int IdUsuario { get; set; }
        
        public string NombreUsuario { get; set; }

        public string RutaFotoApuntado { get; set; }
        //public Usuario usuario;

        public int ApuntadosAdultos { get; set; }
        public int ApuntadosNinos { get; set; }

        public DateTime FechaAlta { get; set; }
    }
}