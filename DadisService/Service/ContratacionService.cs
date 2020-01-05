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
    public class Contratacioneservice
    {
        public List<Contratacion> GetContrataciones(string param)
        {
            List<Contratacion> resultado = new List<Contratacion>();

            string connectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            Engine engine = new Engine(connectionString);

            StringBuilder query = new StringBuilder();

            query.Append("select Contrataciones.Id, Contrataciones.Titulo, Contrataciones.Descripcion, Contrataciones.MaximoHijos, Contrataciones.Locacion, Contrataciones.PrecioTotal, Contrataciones.IdUsuarioAlta, Contrataciones.FechaAlta ");
            query.Append(" , autor.Id as IdAutor, autor.Nombres, autor.Apellido1, autor.Apellido2 ");
            query.Append(" from Contrataciones");
            query.Append(" inner join usuarios autor on Contrataciones.IdUsuarioAlta = autor.Id ");
            query.Append(" where Contrataciones.fechabaja is null  ");

            if (!string.IsNullOrEmpty(param))
            {
                query.Append(" and (Contrataciones.titulo like '%" + param + "%' OR Contrataciones.resumen like '%" + param + "%' OR Contrataciones.descripcion like '%" + param + "%' ");
            }

            DataTable table = engine.Query(query.ToString());

            foreach (DataRow dr in table.Rows)
            {
                Contratacion ContratacionFila = new Contratacion();

                ContratacionFila.Id = int.Parse(dr["Id"].ToString());
                ContratacionFila.Titulo = dr["Titulo"].ToString();
                ContratacionFila.Descripcion = dr["Descripcion"].ToString();
                ContratacionFila.MaximoHijos = int.Parse(dr["MaximoHijos"].ToString());
                ContratacionFila.Locacion = dr["Locacion"].ToString();
                ContratacionFila.PrecioTotal = dr["PrecioTotal"].ToString();
                ContratacionFila.Autor = dr["Nombres"].ToString() + " " + dr["Apellido1"].ToString() + " " + dr["Apellido2"].ToString();
                ContratacionFila.IdUsuarioAlta = int.Parse(dr["IdUsuarioAlta"].ToString());
                ContratacionFila.FechaAlta = DateTime.Parse(dr["FechaAlta"].ToString());
                ContratacionFila.RutaFotoAutor = new FotografiaService().ObtenerFotoPrincipal(ContratacionFila.IdUsuarioAlta).RutaFoto;
                ContratacionFila.RutaFotoPrincipal = new FotografiaService().ObtenerFotoPrincipalContratacion(ContratacionFila.Id).RutaFoto;
                ContratacionFila.Fotografias = new FotografiaService().ObtenerFotosContrataciones(ContratacionFila.Id);
                ContratacionFila.Apuntados = GetApuntadosContratacion(ContratacionFila.Id);
                resultado.Add(ContratacionFila);
            }

            return resultado;
        }


        public Contratacion GetContratacion(int idContratacion)
        {
            Contratacion resultado = new Contratacion();

            string connectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            Engine engine = new Engine(connectionString);

            StringBuilder query = new StringBuilder();

            query.Append("select Contrataciones.Id, Contrataciones.Titulo, Contrataciones.Descripcion, Contrataciones.Locacion, Contrataciones.PrecioTotal, Contrataciones.MaximoHijos, Contrataciones.FechaContratacion, Contrataciones.FechaAlta ");
            query.Append(" , autor.Id as IdAutor, autor.Nombres, autor.Apellido1, autor.Apellido2 ");
            query.Append(" from Contrataciones");
            query.Append(" inner join usuarios autor on Contrataciones.IdUsuarioAlta = autor.Id ");
            query.Append("where Contrataciones.Id = " + idContratacion);


            DataTable table = engine.Query(query.ToString());

            foreach (DataRow dr in table.Rows)
            {
                resultado.Id = int.Parse(dr["Id"].ToString());
                resultado.Titulo = dr["Titulo"].ToString();
                resultado.Descripcion = dr["Descripcion"].ToString();
                resultado.Locacion = dr["Locacion"].ToString();
                resultado.MaximoHijos = int.Parse(dr["MaximoHijos"].ToString());
                resultado.PrecioTotal = dr["PrecioTotal"].ToString();
                resultado.Autor = dr["Nombres"].ToString() + " " + dr["Apellido1"].ToString() + " " + dr["Apellido2"].ToString();
                resultado.IdUsuarioAlta = int.Parse(dr["IdAutor"].ToString());
                resultado.FechaAlta = DateTime.Parse(dr["FechaAlta"].ToString());
                resultado.FechaContratacion = DateTime.Parse(dr["FechaContratacion"].ToString());
                resultado.Apuntados = GetApuntadosContratacion(resultado.Id);
                FotografiaService fotografiaService = new FotografiaService();
                resultado.Fotografias = fotografiaService.ObtenerFotosContrataciones(resultado.Id);
            }

            return resultado;
        }

        public List<Apuntado> GetApuntadosContratacion(int idContratacion)
        {
            List<Apuntado> resultado = new List<Apuntado>();

            string connectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            Engine engine = new Engine(connectionString);

            StringBuilder query = new StringBuilder();

            query.Append("select registrosusuariosContrataciones.Id, registrosusuariosContrataciones.ApuntadosNinos, registrosusuariosContrataciones.FechaAlta ");
            query.Append(" , autor.Id as IdAutor, autor.Nombres, autor.Apellido1, autor.Apellido2 ");
            query.Append(" from registrosusuariosContrataciones");
            query.Append(" inner join usuarios autor on registrosusuariosContrataciones.IdUsuario = autor.Id ");
            query.Append("where registrosusuariosContrataciones.IdContratacion = " + idContratacion);

            DataTable table = engine.Query(query.ToString());

            foreach (DataRow dr in table.Rows)
            {
                Apuntado filaApuntado = new Apuntado();
                filaApuntado.Id = int.Parse(dr["Id"].ToString());
                filaApuntado.IdUsuario = int.Parse(dr["IdAutor"].ToString());
                filaApuntado.ApuntadosNinos = int.Parse(dr["ApuntadosNinos"].ToString());
                filaApuntado.NombreUsuario = dr["Nombres"].ToString() + " " + dr["Apellido1"].ToString() + " " + dr["Apellido2"].ToString();
                filaApuntado.FechaAlta = DateTime.Parse(dr["FechaAlta"].ToString());

                FotografiaService fotografiaService = new FotografiaService();
                filaApuntado.RutaFotoApuntado = fotografiaService.ObtenerFotoPrincipal(filaApuntado.IdUsuario).RutaFoto;
                resultado.Add(filaApuntado);
            }

            return resultado;
        }

        public int CrearContratacion(Contratacion Contratacion)
        {
            int idGenerado = 0;

            if (Contratacion.IdUsuarioAlta == 0) { Contratacion.IdUsuarioAlta = 1; }

            string connectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            Engine engine = new Engine(connectionString);

            StringBuilder comando = new StringBuilder();
            comando.Append("insert into Contrataciones ");
            comando.Append(" (titulo,  descripcion, locacion, maximohijos, preciototal, fechaContratacion, idusuarioalta, fechaAlta) ");
            comando.Append(" values ");
            comando.Append(" ('" + Contratacion.Titulo + "','" + Contratacion.Descripcion + "', '" + Contratacion.Locacion + "', " + Contratacion.MaximoHijos + ", '" + Contratacion.PrecioTotal + "', '" + Contratacion.FechaContratacion.ToString("yyyy-MM-dd HH:mm:ss") + "', " + Contratacion.IdUsuarioAlta + ", NOW()) ");

            int resultado = engine.Execute(comando.ToString());

            if (resultado > 0)
            {

                idGenerado = CommonService.GetLastIdFromTable("Contrataciones");
            }

            return idGenerado;
        }
        public int EditarContratacion(Contratacion Contratacion)
        {
            string connectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            Engine engine = new Engine(connectionString);

            StringBuilder comando = new StringBuilder();
            comando.Append("update Contrataciones set ");
            comando.Append("titulo = '" + Contratacion.Titulo + "'");
            comando.Append(", descripcion = '" + Contratacion.Descripcion + "'");
            comando.Append(", maximohijos = " + Contratacion.MaximoHijos);
            comando.Append(", preciototal = '" + Contratacion.PrecioTotal + "'");
            comando.Append(", locacion = '" + Contratacion.Locacion + "'");
            comando.Append(", fechaContratacion = '" + Contratacion.FechaContratacion.ToString("yyyy-MM-dd HH:mm:ss") + "'");
            comando.Append(", fechamodificacion =  NOW()");
            comando.Append(", idusuariomodificacion = " + Contratacion.IdUsuarioModificacion);
            comando.Append(" where id=" + Contratacion.Id);

            int resultado = engine.Execute(comando.ToString());

            return Contratacion.Id;
        }

        public int BajaContrataciones(string[] IdsContrataciones)
        {
            string connectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            Engine engine = new Engine(connectionString);

            StringBuilder comando = new StringBuilder();
            comando.Append("update Contrataciones set ");
            comando.Append("FechaBaja = NOW() ");
            comando.Append(" where id in ");
            comando.Append(" (");

            for (int i = 0; i < IdsContrataciones.Length; i++)
            {
                comando.Append(IdsContrataciones[i]);

                if (i != IdsContrataciones.Length - 1)
                {
                    comando.Append(", ");
                }
            }

            comando.Append(") ");

            int resultado = engine.Execute(comando.ToString());

            return resultado;
        }

        public int ApuntarseContratacion(Apuntado apunte)
        {
            int idGenerado = 0;

            string connectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            Engine engine = new Engine(connectionString);

            StringBuilder comando = new StringBuilder();
            comando.Append("insert into registrosusuariosContrataciones ");
            comando.Append(" (idusuario, idContratacion, apuntadosninos, fechaalta, idusuarioalta) ");
            comando.Append(" values ");
            comando.Append(" (" + apunte.IdUsuario + "," + apunte.IdContratacion + ", " + apunte.ApuntadosNinos + ", NOW(), " + apunte.IdUsuario + ") ");

            int resultado = engine.Execute(comando.ToString());

            if (resultado > 0)
            {
                idGenerado = CommonService.GetLastIdFromTable("registrosusuariosContrataciones");
            }

            return idGenerado;
        }

        public int DesapuntarseContratacion(Apuntado apunte)
        {
            string connectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            Engine engine = new Engine(connectionString);

            StringBuilder comando = new StringBuilder();
            comando.Append("update registrosusuariosContrataciones set ");
            comando.Append("FechaBaja = NOW() ");
            comando.Append(" where idContratacion = " + apunte.IdContratacion);
            comando.Append(" and idUsuario = " + apunte.IdUsuario);

            int resultado = engine.Execute(comando.ToString());

            return resultado;
        }
    }
}