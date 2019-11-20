using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DadisService.Models.Metadata
{
    public class ParametroMetodoModel
    {
        private string nombre;
        private string tipo;
        private string valor;
        private string descripcion;
        private string observacion;
        private bool obligatorio;
        private string valorDefecto;


        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        public string Tipo
        {
            get { return tipo; }
            set { tipo = value; }
        }

        public string Valor
        {
            get { return valor; }
            set { valor = value; }
        }

        public string Descripcion
        {
            get
            {
                return descripcion;
            }

            set
            {
                descripcion = value;
            }
        }

        public string Observacion
        {
            get
            {
                return observacion;
            }

            set
            {
                observacion = value;
            }
        }

        public bool Obligatorio
        {
            get
            {
                return obligatorio;
            }

            set
            {
                obligatorio = value;
            }
        }

        public string ValorDefecto
        {
            get
            {
                return valorDefecto;
            }

            set
            {
                valorDefecto = value;
            }
        }

        public ParametroMetodoModel(string paramNombre, string paramTipo, string paramValor, string paramDescripcion, string paramObservaciones, bool paramObligatorio, string paramDefecto)
        {
            this.nombre = paramNombre;
            this.tipo = paramTipo;
            this.valor = paramValor;
            this.descripcion = paramDescripcion;
            this.observacion = paramObservaciones;
            this.obligatorio = paramObligatorio;
            this.valorDefecto = paramDefecto;

            if (obligatorio) { valor = paramDefecto; }

        }

        public ParametroMetodoModel()
        {
            this.obligatorio = false;
        }
    }
}