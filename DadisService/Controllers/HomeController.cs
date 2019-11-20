using DadisService.Models.Metadata;
using DadisService.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Http.Cors;

namespace DadisService.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            MetadataModel modeloMetaData = new MetadataModel();
            List<Type> listaTiposControlador = new List<Type>();

            // Añadimos los controladores de tipo Web Api
            Assembly asm = Assembly.GetExecutingAssembly();

            foreach (Type tipo in asm.GetTypes().Where(x => x.IsClass).Where(x => x.FullName.ToLower().Contains("controller")).Where(x => x.BaseType.Name.ToLower().Contains("apicontroller")))
            { listaTiposControlador.Add(tipo); }

            modeloMetaData = MetadataService.getMetaData(listaTiposControlador);
            return View(modeloMetaData);
        }

        public ActionResult TestWebApi(MetadataModel modeloListaController, string botonSubmit)
        {
            if (modeloListaController != null && modeloListaController.Clases != null && modeloListaController.Clases.Count > 0)
            { ViewBag.url = MetadataService.buildWebApiUrl(modeloListaController.Clases.Where(x => x.NombreClase == botonSubmit.Split('_')[1]).FirstOrDefault()); }

            return View();
        }
    }
}
