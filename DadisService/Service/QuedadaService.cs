using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DadisService.Models;
using DadisConnectorDb;
using System.Configuration;
using System.Data;
using System.Text;

namespace DadisService.Service
{
    public class QuedadaService
    {
        public List<Quedada> GetQuedadas(string param)
        {
            List<Quedada> resultado = new List<Quedada>();

            string connectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            Engine engine = new Engine(connectionString);

            StringBuilder query = new StringBuilder();

            query.Append("select quedadas.Id, quedadas.Titulo, quedadas.Resumen, quedadas.Descripcion, quedadas.IdUsuarioAlta, quedadas.FechaAlta ");
            query.Append(" , autor.Id as IdAutor, autor.Nombres, autor.Apellido1, autor.Apellido2 ");
            query.Append(" from quedadas");
            query.Append(" inner join usuarios autor on quedadas.IdUsuarioAlta = autor.Id ");
            query.Append(" where quedadas.fechabaja is null  ");
             
            if (!string.IsNullOrEmpty(param))
            {
                query.Append(" and (quedadas.titulo like '%" + param + "%' OR quedadas.resumen like '%" + param + "%' OR quedadas.descripcion like '%" + param + "%' ");
            }

            DataTable table = engine.Query(query.ToString());

            foreach (DataRow dr in table.Rows)
            {
                Quedada quedadaFila = new Quedada();

                quedadaFila.Id = int.Parse(dr["Id"].ToString());
                quedadaFila.Titulo = dr["Titulo"].ToString();
                quedadaFila.Resumen = dr["Resumen"].ToString();
                quedadaFila.Descripcion = dr["Descripcion"].ToString();
                quedadaFila.Autor = dr["Nombres"].ToString() + " " + dr["Apellido1"].ToString() + " " + dr["Apellido2"].ToString();
                quedadaFila.IdUsuarioAlta = int.Parse(dr["IdUsuarioAlta"].ToString());                
                quedadaFila.FechaAlta = DateTime.Parse(dr["FechaAlta"].ToString());
                quedadaFila.RutaFotoAutor = new FotografiaService().ObtenerFotoPrincipal(quedadaFila.IdUsuarioAlta).RutaFoto;
                quedadaFila.RutaFotoPrincipal = new FotografiaService().ObtenerFotoPrincipalQuedada(quedadaFila.Id).RutaFoto;
                resultado.Add(quedadaFila);
            }

            return resultado;
        }


        public Quedada GetQuedada(int idQuedada)
        {
            Quedada resultado = new Quedada();

            string connectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            Engine engine = new Engine(connectionString);

            StringBuilder query = new StringBuilder();

            query.Append("select quedadas.Id, quedadas.Titulo, quedadas.Resumen, quedadas.Descripcion, quedadas.Locacion, quedadas.MaximoAsistentes, quedadas.IdMensajePadre, quedadas.FechaAlta ");
            query.Append(" , autor.Id as IdAutor, autor.Nombres, autor.Apellido1, autor.Apellido2 ");
            query.Append(" from quedadas");
            query.Append(" inner join usuarios autor on quedadas.IdUsuarioAlta = autor.Id ");
            query.Append("where quedadas.Id = " + idQuedada);


            DataTable table = engine.Query(query.ToString());

            foreach (DataRow dr in table.Rows)
            {
                resultado.Id = int.Parse(dr["Id"].ToString());
                resultado.Titulo = dr["Titulo"].ToString();
                resultado.Resumen = dr["Resumen"].ToString();
                resultado.Descripcion = dr["Descripcion"].ToString();
                resultado.Locacion = dr["Locacion"].ToString();
                resultado.MaximoAsistentes = int.Parse(dr["MaximoAsistentes"].ToString());
                resultado.Autor = dr["Nombres"].ToString() + " " + dr["Apellido1"].ToString() + " " + dr["Apellido2"].ToString();
                resultado.Descripcion = dr["Descripcion"].ToString();
                resultado.IdUsuarioAlta = int.Parse(dr["IdAutor"].ToString());
                resultado.FechaAlta = DateTime.Parse(dr["FechaAlta"].ToString());

                FotografiaService fotografiaService = new FotografiaService();
                resultado.Fotografias = fotografiaService.ObtenerFotosQuedadas(resultado.Id);
            }

            return resultado;
        }


        public int CrearQuedada(Quedada quedada)
        {
            int idGenerado = 0;

            if (quedada.IdUsuarioAlta == 0) { quedada.IdUsuarioAlta = 1; }

            string connectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            Engine engine = new Engine(connectionString);

            int idMensajeNuevo = CommonService.GetLastIdFromTable("quedadas") + 1;

            StringBuilder comando = new StringBuilder();
            comando.Append("insert into quedadas ");
            comando.Append(" (titulo, resumen, descripcion, locacion, maximoasistentes, fechaquedada, idusuarioalta, fechaAlta) ");
            comando.Append(" values ");
            comando.Append(" ('" + quedada.Titulo + "','" + quedada.Resumen + "','" + quedada.Descripcion + "', '" + quedada.Locacion  + "', " + quedada.MaximoAsistentes + ", '" +  quedada.FechaEvento + "', " + quedada.IdUsuarioAlta + ", CURDATE()) ");

            int resultado = engine.Execute(comando.ToString());

            if (resultado > 0)
            {

                idGenerado = CommonService.GetLastIdFromTable("quedadas");
            }

            return idGenerado;
        }
        public int EditarQuedada(Quedada quedada)
        {
            string connectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            Engine engine = new Engine(connectionString);

            StringBuilder comando = new StringBuilder();
            comando.Append("update quedadas set ");
            comando.Append("titulo = '" + quedada.Titulo + "'");
            comando.Append(", resumen = '" + quedada.Resumen + "'");
            comando.Append(", descripcion = '" + quedada.Descripcion + "'");
            comando.Append(", fechamodificacion =  CURDATE()");
            comando.Append(", idusuariomodificacion = " + quedada.IdUsuarioModificacion);
            comando.Append(" where id=" + quedada.Id);

            int resultado = engine.Execute(comando.ToString());

            return quedada.Id;
        }

        public int BajaQuedadas(string[] IdsQuedadas)
        {
            string connectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            Engine engine = new Engine(connectionString);

            StringBuilder comando = new StringBuilder();
            comando.Append("update quedadas set ");
            comando.Append("FechaBaja = CURDATE() ");
            comando.Append(" where id in ");
            comando.Append(" (");

            for (int i = 0; i < IdsQuedadas.Length; i++)
            {
                comando.Append(IdsQuedadas[i]);

                if (i != IdsQuedadas.Length - 1)
                {
                    comando.Append(", ");
                }
            }

            comando.Append(") ");

            int resultado = engine.Execute(comando.ToString());

            return resultado;
        }
    }
}