using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DadisService.Models.Metadata
{
    public class MetodoModel
    {
        private string nombreMetodo;
        private string descripcion;
        private List<ParametroMetodoModel> parametros;

        public string NombreMetodo
        {
            get { return nombreMetodo; }
            set { nombreMetodo = value; }
        }

        public string Descripcion
        {
            get { return descripcion; }
            set { descripcion = value; }
        }

        public List<ParametroMetodoModel> Parametros
        {
            get { return parametros; }
            set { parametros = value; }
        }

        public MetodoModel()
        { Parametros = new List<ParametroMetodoModel>(); }
    }
}