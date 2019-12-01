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

            StringBuilder comando = new StringBuilder();
            comando.Append("insert into fotos ");
            comando.Append(" (IdUsuario, RutaFoto, idusuarioalta, fechaAlta) ");
            comando.Append(" values ");
            comando.Append(" (" + fotografia.IdUsuario + ",'" + fotografia.RutaFoto + "', " + fotografia.IdUsuarioAlta + ", CURDATE())");

            int resultado = engine.Execute(comando.ToString());

            if (resultado > 0)
            {
                idGenerado = CommonService.GetLastIdFromTable("fotos");
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

        public int BajaFotosUsuario(int idUsuario, int idUsuarioBaja)
        {
            //int idGenerado = 0;

            if (idUsuarioBaja == 0) { idUsuarioBaja = 1; }

            string connectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            Engine engine = new Engine(connectionString);

        //    int idRegistroFotografiaNuevo = CommonService.GetLastIdFromTable("fotos") + 1;

            StringBuilder comando = new StringBuilder();
            comando.Append("update fotos ");
            comando.Append(" set fechabaja = curdate(), idusuariobaja= " + idUsuarioBaja);
            comando.Append(" where idusuario = " + idUsuario);

            int resultado = engine.Execute(comando.ToString());

            return resultado;
        }

        public List<Fotografia> ObtenerFotosUsuario(int idUsuario)
        {
            List<Fotografia> resultado = new List<Fotografia>();


            string connectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            Engine engine = new Engine(connectionString);

            StringBuilder query = new StringBuilder();

            query.Append("select Id, RutaFoto ");
            query.Append(" from fotos ");
            query.Append(" where fechabaja is null  ");
            query.Append(" and IdUsuario = " + idUsuario + " ");

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

    }
}