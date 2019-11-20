using DadisService.Models.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace DadisService.Service
{
    public class MetadataService
    {
        private static ClaseModel getInformacionControlador(Type tipoControlador)
        {
            ClaseModel elementoClase = new ClaseModel();

            elementoClase.NombreClase = tipoControlador.Name.Replace("Controller", "");

            MetadataAttribute metadataCustomizadaClase = tipoControlador.GetCustomAttribute<MetadataAttribute>();
            if (metadataCustomizadaClase != null) { elementoClase.Descripcion = metadataCustomizadaClase.Descripcion; }

            foreach (MethodInfo m in tipoControlador.GetMethods())
            {
                if ((m.ReturnType == typeof(String) || m.ReturnType.ToString().Trim().ToLower().Contains("list")) && !m.Name.ToLower().Trim().Contains("string"))
                {
                    MetodoModel elementoMetodo = new MetodoModel();
                    elementoMetodo.NombreMetodo = m.Name;

                    MetadataAttribute metadataCustomizadaMetodo = m.GetCustomAttribute<MetadataAttribute>();
                    if (metadataCustomizadaMetodo != null) { elementoMetodo.Descripcion = metadataCustomizadaMetodo.Descripcion; }

                    ParameterInfo[] infoParametros = m.GetParameters();

                    foreach (ParameterInfo itemInfoParametros in infoParametros)
                    {
                        MetadataAttribute metadataCustomizadaParametro = itemInfoParametros.GetCustomAttribute<MetadataAttribute>();
                        elementoMetodo.Parametros.Add(new ParametroMetodoModel(itemInfoParametros.Name, itemInfoParametros.ParameterType.ToString(), "", ((metadataCustomizadaParametro != null) ? metadataCustomizadaParametro.Descripcion : ""), ((metadataCustomizadaParametro != null) ? metadataCustomizadaParametro.Observaciones : ""), ((metadataCustomizadaParametro != null) ? metadataCustomizadaParametro.Obligatorio : false), ((metadataCustomizadaParametro != null) ? metadataCustomizadaParametro.ValorDefecto : "")));
                    }

                    elementoClase.Metodos.Add(elementoMetodo);
                }
            }

            return elementoClase;
        }

        public static MetadataModel getMetaData(List<Type> listaClases)
        {
            MetadataModel modelo = new MetadataModel();

            foreach (Type tipoControlador in listaClases)
            { modelo.Clases.Add(getInformacionControlador(tipoControlador)); }

            return modelo;
        }

        public static string buildWebApiUrl(ClaseModel clase)
        {
            StringBuilder url = new StringBuilder(System.Configuration.ConfigurationManager.AppSettings["urlBaseApp"] + "/" + System.Configuration.ConfigurationManager.AppSettings["complementoUrlWebApi"].ToString());

            url.Append(clase.NombreClase.Replace("Controller", ""));
            url.Append("/");
            url.Append(clase.Metodos[0].NombreMetodo);
            url.Append("?");

            foreach (ParametroMetodoModel parametroModel in clase.Metodos[0].Parametros)
            {
                url.Append(parametroModel.Nombre);
                url.Append("=");
                url.Append(parametroModel.Valor);
                url.Append("&");
            }

            // Como el último caracter de la cadena resultará ser "&", lo retiramos. 
            url.Remove(url.Length - 1, 1);

            return url.ToString();
        }
    }
}