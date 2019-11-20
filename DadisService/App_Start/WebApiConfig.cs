using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace DadisService
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {

            // Configuración y servicios de API web
            var cors = new EnableCorsAttribute("*", "*", "*");
             config.EnableCors(cors);
            //config.EnableCors();

            // Rutas de API web
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
               name: "Usuarios",
               routeTemplate: "api/Usuarios/{action}" +
               "/{id}",
               defaults: new { id = RouteParameter.Optional }
               );

           config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}" +
                "/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
