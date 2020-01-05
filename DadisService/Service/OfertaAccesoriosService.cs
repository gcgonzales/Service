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
    public class OfertaAccesoriosService
    {
        public List<OfertaAccesorio> GetOfertaAccesorios(string param)
        {
            List<OfertaAccesorio> resultado = new List<OfertaAccesorio>();

            string connectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            Engine engine = new Engine(connectionString);

            StringBuilder query = new StringBuilder();

            query.Append("select OfertaAccesorios.Id, OfertaAccesorios.Titulo, OfertaAccesorios.Resumen, OfertaAccesorios.Descripcion, OfertaAccesorios.Locacion, OfertaAccesorios.UsuarioAlta, OfertaAccesorios.FechaAlta ");
            query.Append(" , autor.Id as IdAutor, autor.Nombres, autor.Apellido1, autor.Apellido2 ");
            query.Append(" from OfertaAccesorios");
            query.Append(" inner join usuarios autor on OfertaAccesorios.UsuarioAlta = autor.Id ");
            query.Append(" where OfertaAccesorios.fechabaja is null  ");

            if (!string.IsNullOrEmpty(param))
            {
                query.Append(" and (OfertaAccesorios.titulo like '%" + param + "%' OR OfertaAccesorios.resumen like '%" + param + "%' OR OfertaAccesorios.descripcion like '%" + param + "%' ");
            }

            DataTable table = engine.Query(query.ToString());

            foreach (DataRow dr in table.Rows)
            {
                OfertaAccesorio OfertaAccesorioFila = new OfertaAccesorio();

                OfertaAccesorioFila.Id = int.Parse(dr["Id"].ToString());
                OfertaAccesorioFila.Titulo = dr["Titulo"].ToString();
                OfertaAccesorioFila.Resumen = dr["Resumen"].ToString();
                OfertaAccesorioFila.Descripcion = dr["Descripcion"].ToString();
                OfertaAccesorioFila.Locacion = dr["Locacion"].ToString();
                OfertaAccesorioFila.Autor = dr["Nombres"].ToString() + " " + dr["Apellido1"].ToString() + " " + dr["Apellido2"].ToString();
                OfertaAccesorioFila.IdUsuarioAlta = int.Parse(dr["UsuarioAlta"].ToString());
                OfertaAccesorioFila.FechaAlta = DateTime.Parse(dr["FechaAlta"].ToString());
                OfertaAccesorioFila.RutaFotoAutor = new FotografiaService().ObtenerFotoPrincipal(OfertaAccesorioFila.IdUsuarioAlta).RutaFoto;
                OfertaAccesorioFila.RutaFotoPrincipal = new FotografiaService().ObtenerFotoPrincipalOferta(OfertaAccesorioFila.Id).RutaFoto;
                OfertaAccesorioFila.Fotografias = new FotografiaService().ObtenerFotosOfertaAccesorio(OfertaAccesorioFila.Id);
              // OfertaAccesorioFila.Apuntados = GetApuntadosOfertaAccesorio(OfertaAccesorioFila.Id);
                resultado.Add(OfertaAccesorioFila);
            }

            return resultado;
        }


        public OfertaAccesorio GetOfertaAccesorio(int idOfertaAccesorio)
        {
            OfertaAccesorio resultado = new OfertaAccesorio();

            string connectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            Engine engine = new Engine(connectionString);

            StringBuilder query = new StringBuilder();

            query.Append("select OfertaAccesorios.Id, OfertaAccesorios.Titulo, OfertaAccesorios.Resumen, OfertaAccesorios.Descripcion, OfertaAccesorios.Locacion, OfertaAccesorios.Precio, OfertaAccesorios.FechaAlta ");
            query.Append(" , autor.Id as IdAutor, autor.Nombres, autor.Apellido1, autor.Apellido2 ");
            query.Append(" from OfertaAccesorios");
            query.Append(" inner join usuarios autor on OfertaAccesorios.UsuarioAlta = autor.Id ");
            query.Append("where OfertaAccesorios.Id = " + idOfertaAccesorio);


            DataTable table = engine.Query(query.ToString());

            foreach (DataRow dr in table.Rows)
            {
                resultado.Id = int.Parse(dr["Id"].ToString());
                resultado.Titulo = dr["Titulo"].ToString();
                resultado.Resumen = dr["Resumen"].ToString();
                resultado.Descripcion = dr["Descripcion"].ToString();
                resultado.Locacion = dr["Locacion"].ToString();
                resultado.Autor = dr["Nombres"].ToString() + " " + dr["Apellido1"].ToString() + " " + dr["Apellido2"].ToString();
                resultado.Precio = dr["Precio"].ToString();
                resultado.IdUsuarioAlta = int.Parse(dr["IdAutor"].ToString());
                resultado.FechaAlta = DateTime.Parse(dr["FechaAlta"].ToString());
                //resultado.Apuntados = GetApuntadosOfertaAccesorio(resultado.Id);
                FotografiaService fotografiaService = new FotografiaService();
                resultado.Fotografias = fotografiaService.ObtenerFotosOfertaAccesorio(resultado.Id);
            }

            return resultado;
        }

        public List<Apuntado> GetApuntadosOfertaAccesorio(int idOfertaAccesorio)
        {
            List<Apuntado> resultado = new List<Apuntado>();

            string connectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            Engine engine = new Engine(connectionString);

            StringBuilder query = new StringBuilder();

            query.Append("select registrosusuariosOfertaAccesorios.Id, registrosusuariosOfertaAccesorios.ApuntadosAdultos, registrosusuariosOfertaAccesorios.ApuntadosNinos, registrosusuariosOfertaAccesorios.FechaAlta ");
            query.Append(" , autor.Id as IdAutor, autor.Nombres, autor.Apellido1, autor.Apellido2 ");
            query.Append(" from registrosusuariosOfertaAccesorios");
            query.Append(" inner join usuarios autor on registrosusuariosOfertaAccesorios.IdUsuario = autor.Id ");
            query.Append("where registrosusuariosOfertaAccesorios.IdOfertaAccesorio = " + idOfertaAccesorio);

            DataTable table = engine.Query(query.ToString());

            foreach (DataRow dr in table.Rows)
            {
                Apuntado filaApuntado = new Apuntado();
                filaApuntado.Id = int.Parse(dr["Id"].ToString());
                filaApuntado.IdUsuario = int.Parse(dr["IdAutor"].ToString());
                filaApuntado.ApuntadosAdultos = int.Parse(dr["ApuntadosAdultos"].ToString());
                filaApuntado.ApuntadosNinos = int.Parse(dr["ApuntadosNinos"].ToString());
                filaApuntado.NombreUsuario = dr["Nombres"].ToString() + " " + dr["Apellido1"].ToString() + " " + dr["Apellido2"].ToString();
                filaApuntado.FechaAlta = DateTime.Parse(dr["FechaAlta"].ToString());

                FotografiaService fotografiaService = new FotografiaService();
                filaApuntado.RutaFotoApuntado = fotografiaService.ObtenerFotoPrincipal(filaApuntado.IdUsuario).RutaFoto;
                resultado.Add(filaApuntado);
            }

            return resultado;
        }

        public int CrearOfertaAccesorio(OfertaAccesorio OfertaAccesorio)
        {
            int idGenerado = 0;

            if (OfertaAccesorio.IdUsuarioAlta == 0) { OfertaAccesorio.IdUsuarioAlta = 1; }

            string connectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            Engine engine = new Engine(connectionString);

            StringBuilder comando = new StringBuilder();
            comando.Append("insert into OfertaAccesorios ");
            comando.Append(" (titulo, resumen, descripcion, locacion, precio, usuarioalta, fechaAlta) ");
            comando.Append(" values ");
            comando.Append(" ('" + OfertaAccesorio.Titulo + "','" + OfertaAccesorio.Resumen + "','" + OfertaAccesorio.Descripcion + "', '" + OfertaAccesorio.Locacion + "', '" + OfertaAccesorio.Precio + "', " + OfertaAccesorio.IdUsuarioAlta + ", NOW()) ");

            int resultado = engine.Execute(comando.ToString());

            if (resultado > 0)
            {

                idGenerado = CommonService.GetLastIdFromTable("OfertaAccesorios");
            }

            return idGenerado;
        }
        public int EditarOfertaAccesorio(OfertaAccesorio OfertaAccesorio)
        {
            string connectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            Engine engine = new Engine(connectionString);

            StringBuilder comando = new StringBuilder();
            comando.Append("update OfertaAccesorios set ");
            comando.Append("titulo = '" + OfertaAccesorio.Titulo + "'");
            comando.Append(", resumen = '" + OfertaAccesorio.Resumen + "'");
            comando.Append(", descripcion = '" + OfertaAccesorio.Descripcion + "'");
            comando.Append(", precio = '" + OfertaAccesorio.Precio + "'");
            comando.Append(", locacion = '" + OfertaAccesorio.Locacion + "'");
            comando.Append(", fechamodificacion =  NOW()");
            comando.Append(", usuariomodificacion = " + OfertaAccesorio.IdUsuarioModificacion);
            comando.Append(" where id=" + OfertaAccesorio.Id);

            int resultado = engine.Execute(comando.ToString());

            return OfertaAccesorio.Id;
        }

        public int BajaOfertaAccesorios(string[] IdsOfertaAccesorios)
        {
            string connectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            Engine engine = new Engine(connectionString);

            StringBuilder comando = new StringBuilder();
            comando.Append("update OfertaAccesorios set ");
            comando.Append("FechaBaja = NOW() ");
            comando.Append(" where id in ");
            comando.Append(" (");

            for (int i = 0; i < IdsOfertaAccesorios.Length; i++)
            {
                comando.Append(IdsOfertaAccesorios[i]);

                if (i != IdsOfertaAccesorios.Length - 1)
                {
                    comando.Append(", ");
                }
            }

            comando.Append(") ");

            int resultado = engine.Execute(comando.ToString());

            return resultado;
        }

        public int ApuntarseOfertaAccesorio(Apuntado apunte)
        {
            int idGenerado = 0;

            string connectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            Engine engine = new Engine(connectionString);

            StringBuilder comando = new StringBuilder();
            comando.Append("insert into registrosusuariosOfertaAccesorios ");
            comando.Append(" (idusuario, idOfertaAccesorio, apuntadosadultos, apuntadosninos, fechaalta, idusuarioalta) ");
            comando.Append(" values ");
            comando.Append(" (" + apunte.IdUsuario + "," + apunte.IdOfertaAccesorio + "," + apunte.ApuntadosAdultos + ", " + apunte.ApuntadosNinos + ", NOW(), " + apunte.IdUsuario + ") ");

            int resultado = engine.Execute(comando.ToString());

            if (resultado > 0)
            {
                idGenerado = CommonService.GetLastIdFromTable("registrosusuariosOfertaAccesorios");
            }

            return idGenerado;
        }

        public int DesapuntarseOfertaAccesorio(Apuntado apunte)
        {
            string connectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            Engine engine = new Engine(connectionString);

            StringBuilder comando = new StringBuilder();
            comando.Append("update registrosusuariosOfertaAccesorios set ");
            comando.Append("FechaBaja = NOW() ");
            comando.Append(" where idOfertaAccesorio = " + apunte.IdOfertaAccesorio);
            comando.Append(" and idUsuario = " + apunte.IdUsuario);

            int resultado = engine.Execute(comando.ToString());

            return resultado;
        }
    }
}