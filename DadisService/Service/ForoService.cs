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
    public class ForoService
    {
        public List<MensajeForo> GetTemasPrincipales(string param)
        {
            List<MensajeForo> resultado = new List<MensajeForo>();

            string connectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            Engine engine = new Engine(connectionString);

            StringBuilder query = new StringBuilder();

            query.Append("select mensajesForo.Id, mensajesForo.Titulo, mensajesForo.Mensaje, mensajesForo.IdMensajePadre, mensajesForo.FechaAlta ");
            query.Append(" , padre.Titulo as TituloPadre ");
            query.Append(" , autor.Id as IdAutor, autor.Nombres, autor.Apellido1, autor.Apellido2 ");
            query.Append(" , (SELECT MAX(p2.Id) FROM mensajesForo p2 WHERE p2.IdMensajePadre = mensajesForo.Id) as ultimoId ");
            query.Append(" from mensajesForo");
            query.Append(" left join mensajesForo padre on mensajesForo.IdMensajePadre = padre.Id ");
            query.Append(" inner join usuarios autor on mensajesForo.IdUsuarioAlta = autor.Id ");
            query.Append(" where mensajesForo.fechabaja is null  ");
            query.Append(" and mensajesForo.Id = mensajesForo.IdMensajePadre ");
            if (!string.IsNullOrEmpty(param))
            {
                query.Append(" and (titulo like '%" + param + "%' OR mensaje like '%" + param + "%' ");
                query.Append(" or autor.Nombres like '%" + param + "%' or autor.apellido1 like '%" + param + "%' or autor.apellido2 like '%" + param + "%') ");
            }

            DataTable table = engine.Query(query.ToString());

            foreach (DataRow dr in table.Rows)
            {
                MensajeForo mensajeForoFila = new MensajeForo();

                mensajeForoFila.Id = int.Parse(dr["Id"].ToString());
                mensajeForoFila.Titulo = dr["Titulo"].ToString();
                mensajeForoFila.TituloPadre = dr["TituloPadre"].ToString();
                mensajeForoFila.Autor = dr["Nombres"].ToString() + " " + dr["Apellido1"].ToString() + " " + dr["Apellido2"].ToString();
                mensajeForoFila.IdUsuarioAlta = int.Parse(dr["IdAutor"].ToString());
                mensajeForoFila.Mensaje = dr["Mensaje"].ToString();
                mensajeForoFila.IdMensajePadre = int.Parse(dr["IdMensajePadre"].ToString());
                mensajeForoFila.FechaAlta = DateTime.Parse(dr["FechaAlta"].ToString());

                MensajeForo ultimoMensaje = GetMensaje(mensajeForoFila.Id);
                mensajeForoFila.UltimoAutor = ultimoMensaje.Autor;
                mensajeForoFila.FechaUltimaContestacion = ultimoMensaje.FechaAlta;

                resultado.Add(mensajeForoFila);
            }

            return resultado;
        }

        public List<MensajeForo> GetHiloTema(int idTemaForo)
        {
            List<MensajeForo> resultado = new List<MensajeForo>();

            string connectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            Engine engine = new Engine(connectionString);

            StringBuilder query = new StringBuilder();

            query.Append("select mensajesForo.Id, mensajesForo.Titulo, mensajesForo.Mensaje, mensajesForo.IdMensajePadre, mensajesForo.FechaAlta ");
            query.Append(" , padre.Titulo as TituloPadre ");
            query.Append(" , autor.Id as IdAutor, autor.Nombres, autor.Apellido1, autor.Apellido2 ");
            query.Append(" from mensajesForo");
            query.Append(" inner join mensajesForo padre on mensajesForo.IdMensajePadre = padre.Id ");
            query.Append(" inner join usuarios autor on mensajesForo.IdUsuarioAlta = autor.Id ");
            query.Append(" where mensajesForo.IdMensajePadre = " + idTemaForo);
           // query.Append(" and mensajesForo.Id <> mensajesForo.IdMensajePadre ");
            query.Append(" and mensajesForo.FechaBaja is null ");

            DataTable table = engine.Query(query.ToString());

            foreach (DataRow dr in table.Rows)
            {
                MensajeForo mensajeForoFila = new MensajeForo();

                mensajeForoFila.Id = int.Parse(dr["Id"].ToString());
                mensajeForoFila.Titulo = dr["Titulo"].ToString();
                mensajeForoFila.TituloPadre = dr["TituloPadre"].ToString();
                mensajeForoFila.Autor = dr["Nombres"].ToString() + " " + dr["Apellido1"].ToString() + " " + dr["Apellido2"].ToString();
                mensajeForoFila.IdUsuarioAlta = int.Parse(dr["IdAutor"].ToString());
                mensajeForoFila.Mensaje = dr["Mensaje"].ToString();
                mensajeForoFila.FechaAlta = DateTime.Parse(dr["FechaAlta"].ToString());
                mensajeForoFila.IdMensajePadre = int.Parse(dr["IdMensajePadre"].ToString());
                 
                resultado.Add(mensajeForoFila);
            }

            return resultado;
        }

        public MensajeForo GetMensaje(int idMensajeForo)
        {
            MensajeForo resultado = new MensajeForo();

            string connectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            Engine engine = new Engine(connectionString);

            StringBuilder query = new StringBuilder();

            query.Append("select mensajesForo.Id, mensajesForo.Titulo, mensajesForo.Mensaje, mensajesForo.IdMensajePadre, mensajesForo.FechaAlta ");
            query.Append(" , padre.Titulo as TituloPadre ");
            query.Append(" , autor.Id as IdAutor, autor.Nombres, autor.Apellido1, autor.Apellido2 ");
            query.Append(" from mensajesForo");
            query.Append(" inner join mensajesForo padre on mensajesForo.IdMensajePadre = padre.Id ");
            query.Append(" inner join usuarios autor on mensajesForo.IdUsuarioAlta = autor.Id ");
            query.Append("where mensajesForo.Id = " + idMensajeForo);


            DataTable table = engine.Query(query.ToString());

            foreach (DataRow dr in table.Rows)
            {
                resultado.Id = int.Parse(dr["Id"].ToString());
                resultado.Titulo = dr["Titulo"].ToString();
                resultado.TituloPadre = dr["TituloPadre"].ToString();
                resultado.Autor = dr["Nombres"].ToString() + " " + dr["Apellido1"].ToString() + " " + dr["Apellido2"].ToString();
                resultado.IdUsuarioAlta = int.Parse(dr["IdAutor"].ToString());
                resultado.Mensaje = dr["Mensaje"].ToString();
                resultado.IdMensajePadre = int.Parse(dr["IdMensajePadre"].ToString());
                resultado.FechaAlta = DateTime.Parse(dr["FechaAlta"].ToString());
                resultado.TituloPadre = dr["TituloPadre"].ToString();
            }

            return resultado;
        }


        public int CrearMensajeForo(MensajeForo mensaje)
        {
            int idGenerado = 0;

            if (mensaje.IdUsuarioAlta == 0) { mensaje.IdUsuarioAlta = 1; }

            string connectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            Engine engine = new Engine(connectionString);

            int idMensajeNuevo = CommonService.GetLastIdFromTable("mensajesForo") + 1;

            StringBuilder comando = new StringBuilder();
            comando.Append("insert into mensajesForo ");
            comando.Append(" (titulo, mensaje, idusuarioalta, fechaAlta, idmensajepadre) ");
            comando.Append(" values ");
            comando.Append(" ('" + mensaje.Titulo + "','" + mensaje.Mensaje + "', "+ mensaje.IdUsuarioAlta + ", CURDATE(), "+(mensaje.IdMensajePadre != 0 ? mensaje.IdMensajePadre : idMensajeNuevo) + " ) ");

            int resultado = engine.Execute(comando.ToString());

            if (resultado > 0)
            {

                idGenerado = CommonService.GetLastIdFromTable("mensajesforo");
            }

            return idGenerado;
        }
        public int EditarMensajeForo(MensajeForo mensaje)
        {
            string connectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            Engine engine = new Engine(connectionString);

            StringBuilder comando = new StringBuilder();
            comando.Append("update mensajesforo set ");
            comando.Append("titulo = '" + mensaje.Titulo + "'");
            comando.Append(", mensaje = '" + mensaje.Mensaje + "'");
            comando.Append(", fechamodificacion =  CURDATE()");
            comando.Append(", idusuariomodificacion = " + mensaje.IdUsuarioModificacion);
            comando.Append(" where id=" + mensaje.Id);

            int resultado = engine.Execute(comando.ToString());


            return mensaje.Id;
        }

        public int BajaMensajesForo(string[] IdsUsuarios)
        {
            string connectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            Engine engine = new Engine(connectionString);

            StringBuilder comando = new StringBuilder();
            comando.Append("update mensajesforo set ");
            comando.Append("FechaBaja = CURDATE() ");
            comando.Append(" where id in ");
            comando.Append(" (");

            for (int i = 0; i < IdsUsuarios.Length; i++)
            {
                comando.Append(IdsUsuarios[i]);

                if (i != IdsUsuarios.Length - 1)
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