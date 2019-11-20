using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DadisService.Models.Metadata
{
    [AttributeUsage(AttributeTargets.All)]
    public class MetadataAttribute : Attribute
    {
        private string descripcion;
        private string observaciones;
        private bool obligatorio;
        private string valorDefecto;

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

        public string Observaciones
        {
            get
            {
                return observaciones;
            }

            set
            {
                observaciones = value;
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

        public MetadataAttribute()
        {
            descripcion = "";
            observaciones = "";
            obligatorio = false;
        }
    }
}