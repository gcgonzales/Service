using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DadisService.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public string Nombres { get; set; }

        public string ApellidoPrimero { get; set; }

        public string ApellidoSegundo { get; set; }

        public string Email { get; set; }

        public string Telefono { get; set; }

        public List<Imagen> Fotos { get; set; }

        public int IdUsuarioAlta { get; set; }
        public DateTime FechaAlta { get; set; }

        public DateTime FechaModificacion { get; set; }
        
        public DateTime FechaBaja { get; set; }

        public string IncidenciaUsuario { get; set; }


    }
}