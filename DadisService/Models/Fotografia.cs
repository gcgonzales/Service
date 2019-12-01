using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DadisService.Models
{
    public class Fotografia
    {
        public int Id { get; set; }

        public int IdUsuario { get; set; }

        public string RutaFoto { get; set; }

        public DateTime FechaAlta { get; set; }

        public int IdUsuarioAlta { get; set; }

        public DateTime FechaBaja { get; set; }

        public int IdUsuarioBaja { get; set; }

        public bool Baja { get; set; }
    }
}