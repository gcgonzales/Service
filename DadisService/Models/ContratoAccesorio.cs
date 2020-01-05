using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DadisService.Models
{
    public class ContratoAccesorio
    {
        public int Id { get; set; }

        public int IdUsuario { get; set; }

        public int IdOfertaAccesorio { get; set; }

        public DateTime FechaOferta { get; set; }




    }
}