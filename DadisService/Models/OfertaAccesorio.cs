﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DadisService.Models
{
    public class OfertaAccesorio
    {
        public int Id { get; set; }

        public string Titulo { get; set; }

        public string Resumen { get; set; }

        public string Descripcion { get; set; }

        public string Precio { get; set;}

        public string Locacion { get; set; }

        public List<Fotografia> Fotografias { get; set; }

        public string Autor { get; set; }

        public string RutaFotoAutor { get; set; }

        public string RutaFotoPrincipal { get; set; }

        public DateTime FechaAlta { get; set; }

        public int IdUsuarioAlta { get; set; }

        public DateTime FechaModificacion { get; set; }

        public int IdUsuarioModificacion { get; set; }

        public int FechaBaja { get; set; }

        public int IdUsuarioBaja { get; set; }
         
        public List<Apuntado> Apuntados { get; set; }
    }
}