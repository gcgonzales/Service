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
    public class FotografiaService
    {

        public int RegistrarFoto(Fotografia fotografia)
        {
            int idGenerado = 0;

            if (fotografia.IdUsuarioAlta == 0) { fotografia.IdUsuarioAlta = 1; }

            string connectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            Engine engine = new Engine(connectionString);

            int idRegistroFotografiaNuevo = CommonService.GetLastIdFromTable("fotos") + 1;

            int valorEsPrincipal = 0;
            if (fotografia.EsPrincipal) valorEsPrincipal = 1; 

            StringBuilder comando = new StringBuilder();
            comando.Append("insert into fotos ");
            comando.Append(" (IdUsuario, RutaFoto, idusuarioalta, fechaAlta, esprincipal) ");
            comando.Append(" values ");
            comando.Append(" (" + fotografia.IdUsuario + ",'" + fotografia.RutaFoto + "', " + fotografia.IdUsuarioAlta + ", CURDATE()," + valorEsPrincipal + ")");

            int resultado = engine.Execute(comando.ToString());

            if (resultado > 0)
            {
                idGenerado = CommonService.GetLastIdFromTable("fotos");
            }

            return idGenerado;
        }

        public int RegistrarFotoQuedada(Fotografia fotografia)
        {
            int idGenerado = 0;

            if (fotografia.IdUsuarioAlta == 0) { fotografia.IdUsuarioAlta = 1; }

            string connectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            Engine engine = new Engine(connectionString);

            int idRegistroFotografiaNuevo = CommonService.GetLastIdFromTable("fotos") + 1;

            int valorEsPrincipal = 0;
            if (fotografia.EsPrincipal) valorEsPrincipal = 1;

            StringBuilder comando = new StringBuilder();
            comando.Append("insert into fotosquedadas ");
            comando.Append(" (IdQuedada, RutaFoto, idusuarioalta, fechaAlta, esprincipal) ");
            comando.Append(" values ");
            comando.Append(" (" + fotografia.IdQuedada + ",'" + fotografia.RutaFoto + "', " + fotografia.IdUsuarioAlta + ", CURDATE()," + valorEsPrincipal + ")");

            int resultado = engine.Execute(comando.ToString());

            if (resultado > 0)
            {
                idGenerado = CommonService.GetLastIdFromTable("fotosquedadas");
            }

            return idGenerado;
        }

        public int BajaFoto(Fotografia fotografia)
        {
            //int idGenerado = 0;

            if (fotografia.IdUsuarioBaja == 0) { fotografia.IdUsuarioBaja = 1; }

            string connectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            Engine engine = new Engine(connectionString);

           // int idRegistroFotografiaNuevo = CommonService.GetLastIdFromTable("fotos") + 1;

            StringBuilder comando = new StringBuilder();
            comando.Append("update fotos ");
            comando.Append(" set fechabaja = curdate(), idusuariobaja= " + fotografia.IdUsuarioBaja);
            comando.Append(" where id = " + fotografia.Id);

            int resultado = engine.Execute(comando.ToString());

            return resultado;
        }


        public int BajaFotoQuedada(Fotografia fotografia)
        {
            //int idGenerado = 0;

            if (fotografia.IdUsuarioBaja == 0) { fotografia.IdUsuarioBaja = 1; }

            string connectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            Engine engine = new Engine(connectionString);

            // int idRegistroFotografiaNuevo = CommonService.GetLastIdFromTable("fotos") + 1;

            StringBuilder comando = new StringBuilder();
            comando.Append("update fotosquedadas ");
            comando.Append(" set fechabaja = curdate(), idusuariobaja= " + fotografia.IdUsuarioBaja);
            comando.Append(" where id = " + fotografia.Id);

            int resultado = engine.Execute(comando.ToString());

            return resultado;
        }


      

        public List<Fotografia> ObtenerFotosUsuario(int idUsuario)
        {
            List<Fotografia> resultado = new List<Fotografia>();


            string connectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            Engine engine = new Engine(connectionString);

            StringBuilder query = new StringBuilder();

            query.Append("select Id, RutaFoto, EsPrincipal ");
            query.Append(" from fotos ");
            query.Append(" where fechabaja is null  ");
            query.Append(" and IdUsuario = " + idUsuario);
            query.Append(" order by EsPrincipal desc ");

            DataTable table = engine.Query(query.ToString());

            foreach (DataRow dr in table.Rows)
            {
                Fotografia fotografia = new Fotografia();

                fotografia.Id = int.Parse(dr["Id"].ToString());
                fotografia.RutaFoto = dr["RutaFoto"].ToString();
                fotografia.EsPrincipal = (dr["RutaFoto"].ToString().Equals("1") ? true : false);
                resultado.Add(fotografia);
            }

            return resultado;
        }

        public Fotografia ObtenerFotoPrincipal(int idUsuario)
        {
           Fotografia resultado = new Fotografia();


            string connectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            Engine engine = new Engine(connectionString);

            StringBuilder query = new StringBuilder();

            query.Append("select Id, RutaFoto, EsPrincipal ");
            query.Append(" from fotos ");
            query.Append(" where fechabaja is null  ");
            query.Append(" and IdUsuario = " + idUsuario + " ");
            query.Append(" and EsPrincipal = 1");

            DataTable table = engine.Query(query.ToString());

            foreach (DataRow dr in table.Rows)
            {
                resultado.Id = int.Parse(dr["Id"].ToString());
                resultado.RutaFoto = dr["RutaFoto"].ToString();
                resultado.EsPrincipal = (dr["RutaFoto"].ToString().Equals("1") ? true : false);   
            }

            return resultado;
        }

        public Fotografia ObtenerFotoPrincipalQuedada(int idQuedada)
        {
            Fotografia resultado = new Fotografia();


            string connectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            Engine engine = new Engine(connectionString);

            StringBuilder query = new StringBuilder();

            query.Append("select Id, RutaFoto, EsPrincipal ");
            query.Append(" from fotosquedadas ");
            query.Append(" where fechabaja is null  ");
            query.Append(" and IdQuedada = " + idQuedada + " ");
            query.Append(" and EsPrincipal = 1");

            DataTable table = engine.Query(query.ToString());

            foreach (DataRow dr in table.Rows)
            {
                resultado.Id = int.Parse(dr["Id"].ToString());
                resultado.RutaFoto = dr["RutaFoto"].ToString();
                resultado.EsPrincipal = (dr["RutaFoto"].ToString().Equals("1") ? true : false);
            }

            return resultado;
        }

        public List<Fotografia> ObtenerFotosQuedadas(int idQuedada)
        {
            List<Fotografia> resultado = new List<Fotografia>();


            string connectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            Engine engine = new Engine(connectionString);

            StringBuilder query = new StringBuilder();

            query.Append("select Id, RutaFoto ");
            query.Append(" from fotosquedadas ");
            query.Append(" where fechabaja is null  ");
            query.Append(" and IdQuedada = " + idQuedada);

            DataTable table = engine.Query(query.ToString());

            foreach (DataRow dr in table.Rows)
            {
                Fotografia fotografia = new Fotografia();

                fotografia.Id = int.Parse(dr["Id"].ToString());
                fotografia.RutaFoto = dr["RutaFoto"].ToString();

                resultado.Add(fotografia);
            }

            return resultado;
        }

        public int AdjuntarFotografias(List<Fotografia> fotografias) {
            int resultado = 0;

            foreach (Fotografia foto in fotografias)
            {
                if (foto.Id == 0)
                {
                    RegistrarFoto(foto);
                }
                else if (foto.Baja)
                {
                    BajaFoto(foto);
                }
            }

            return resultado;
        }

        public int AdjuntarFotografiasQuedadas(List<Fotografia> fotografias)
        {
            int resultado = 0;

            foreach (Fotografia foto in fotografias)
            {
                if (foto.Id == 0)
                {
                    RegistrarFotoQuedada(foto);
                }
                else if (foto.Baja)
                {
                    BajaFotoQuedada(foto);
                }
            }

            return resultado;
        }

    }
}