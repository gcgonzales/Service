﻿using System;
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

            query.Append("select quedadas.Id, quedadas.Titulo, quedadas.Resumen, quedadas.Descripcion, quedadas.Locacion, quedadas.IdUsuarioAlta, quedadas.FechaAlta ");
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
                quedadaFila.Locacion = dr["Locacion"].ToString();
                quedadaFila.Autor = dr["Nombres"].ToString() + " " + dr["Apellido1"].ToString() + " " + dr["Apellido2"].ToString();
                quedadaFila.IdUsuarioAlta = int.Parse(dr["IdUsuarioAlta"].ToString());                
                quedadaFila.FechaAlta = DateTime.Parse(dr["FechaAlta"].ToString());
                quedadaFila.RutaFotoAutor = new FotografiaService().ObtenerFotoPrincipal(quedadaFila.IdUsuarioAlta).RutaFoto;
                quedadaFila.RutaFotoPrincipal = new FotografiaService().ObtenerFotoPrincipalQuedada(quedadaFila.Id).RutaFoto;
                quedadaFila.Fotografias = new FotografiaService().ObtenerFotosQuedadas(quedadaFila.Id);
                quedadaFila.Apuntados = GetApuntadosQuedada(quedadaFila.Id);
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

            query.Append("select quedadas.Id, quedadas.Titulo, quedadas.Resumen, quedadas.Descripcion, quedadas.Locacion, quedadas.MaximoAsistentes, quedadas.FechaQuedada, quedadas.FechaAlta ");
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
                resultado.FechaEvento = DateTime.Parse(dr["FechaQuedada"].ToString());
                resultado.Apuntados = GetApuntadosQuedada(resultado.Id);
                FotografiaService fotografiaService = new FotografiaService();
                resultado.Fotografias = fotografiaService.ObtenerFotosQuedadas(resultado.Id);
            }

            return resultado;
        }

        public List<Apuntado> GetApuntadosQuedada(int idQuedada)
        {
            List<Apuntado> resultado = new List<Apuntado>();

            string connectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            Engine engine = new Engine(connectionString);

            StringBuilder query = new StringBuilder();

            query.Append("select registrosusuariosquedadas.Id, registrosusuariosquedadas.ApuntadosAdultos, registrosusuariosquedadas.ApuntadosNinos, registrosusuariosquedadas.FechaAlta ");
            query.Append(" , autor.Id as IdAutor, autor.Nombres, autor.Apellido1, autor.Apellido2 ");
            query.Append(" from registrosusuariosquedadas");
            query.Append(" inner join usuarios autor on registrosusuariosquedadas.IdUsuario = autor.Id ");
            query.Append("where registrosusuariosquedadas.IdQuedada = " + idQuedada);

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

        public int CrearQuedada(Quedada quedada)
        {
            int idGenerado = 0;

            if (quedada.IdUsuarioAlta == 0) { quedada.IdUsuarioAlta = 1; }

            string connectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            Engine engine = new Engine(connectionString);

            StringBuilder comando = new StringBuilder();
            comando.Append("insert into quedadas ");
            comando.Append(" (titulo, resumen, descripcion, locacion, maximoasistentes, fechaquedada, idusuarioalta, fechaAlta) ");
            comando.Append(" values ");
            comando.Append(" ('" + quedada.Titulo + "','" + quedada.Resumen + "','" + quedada.Descripcion + "', '" + quedada.Locacion  + "', " + quedada.MaximoAsistentes + ", '" +  quedada.FechaEvento.ToString("yyyy-MM-dd HH:mm:ss") + "', " + quedada.IdUsuarioAlta + ", NOW()) ");

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
            comando.Append(", locacion = '" + quedada.Locacion + "'");
            comando.Append(", fechaquedada = '" + quedada.FechaEvento.ToString("yyyy-MM-dd HH:mm:ss") + "'");
            comando.Append(", fechamodificacion =  NOW()");
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
            comando.Append("FechaBaja = NOW() ");
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

        public int ApuntarseQuedada(Apuntado apunte)
        {
            int idGenerado = 0;
            
            string connectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            Engine engine = new Engine(connectionString);

            StringBuilder comando = new StringBuilder();
            comando.Append("insert into registrosusuariosquedadas ");
            comando.Append(" (idusuario, idquedada, apuntadosadultos, apuntadosninos, fechaalta, idusuarioalta) ");
            comando.Append(" values ");
            comando.Append(" (" + apunte.IdUsuario + "," + apunte.IdQuedada + "," + apunte.ApuntadosAdultos + ", " + apunte.ApuntadosNinos + ", NOW(), " + apunte.IdUsuario + ") ");

            int resultado = engine.Execute(comando.ToString());

            if (resultado > 0)
            {
                idGenerado = CommonService.GetLastIdFromTable("registrosusuariosquedadas");
            }

            return idGenerado;
        }

        public int DesapuntarseQuedada(Apuntado apunte)
        {
            string connectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            Engine engine = new Engine(connectionString);

            StringBuilder comando = new StringBuilder();
            comando.Append("update registrosusuariosquedadas set ");
            comando.Append("FechaBaja = NOW() ");
            comando.Append(" where idQuedada = " + apunte.IdQuedada);
            comando.Append(" and idUsuario = " + apunte.IdUsuario);

            int resultado = engine.Execute(comando.ToString());

            return resultado;
        }


      

    }
}