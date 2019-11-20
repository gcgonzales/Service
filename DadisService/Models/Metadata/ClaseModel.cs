using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DadisService.Models.Metadata
{
    public class ClaseModel
    {
        private string nombreClase;
        private string descripcion;
        private List<MetodoModel> metodos;

        public string NombreClase
        {
            get { return nombreClase; }
            set { nombreClase = value; }
        }

        public List<MetodoModel> Metodos
        {
            get { return metodos; }
            set { metodos = value; }
        }

        public string Descripcion
        {
            get { return descripcion; }
            set { descripcion = value; }
        }

        public ClaseModel()
        { metodos = new List<MetodoModel>(); }
    }
}