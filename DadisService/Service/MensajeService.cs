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
    public class MensajeService
    {
        public int CrearMensaje(MensajeUsuario mensajeUsuario)
        {
            int idGenerado = 0;

            string connectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            Engine engine = new Engine(connectionString);

            StringBuilder comando = new StringBuilder();
            comando.Append("insert into mensajes ");
            comando.Append(" (IdUsuarioRemitente, IdUsuarioReceptor, Mensaje, FechaAlta) ");
            comando.Append(" values ");
            comando.Append(" (" + mensajeUsuario.IdUsuarioRemitente + "," + mensajeUsuario.IdUsuarioReceptor + ", '" + mensajeUsuario.Mensaje + "', NOW())");

            int resultado = engine.Execute(comando.ToString());

            if (resultado > 0)
            {
                idGenerado = CommonService.GetLastIdFromTable("mensajes");
            }

            return idGenerado;
        }

        public List<MensajeUsuario> GetMensajes(int idUsuarioRemitente, int idUsuarioReceptor)
        {
            List<MensajeUsuario> resultado = new List<MensajeUsuario>();

            string connectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            Engine engine = new Engine(connectionString);

            StringBuilder query = new StringBuilder();

            query.Append("select mensajes.Id, mensajes.IdUsuarioRemitente, mensajes.IdUsuarioReceptor, mensajes.Mensaje,  mensajes.FechaAlta ");
            query.Append(" from mensajes");
            query.Append(" where mensajes.fechabaja is null  ");
            query.Append(" and mensajes.IdUsuarioRemitente in (" + idUsuarioRemitente + " , " + idUsuarioReceptor + ") ");
            query.Append(" and mensajes.IdUsuarioReceptor in (" + idUsuarioRemitente + " , " + idUsuarioReceptor + ") ");
            query.Append(" order by mensajes.fechaalta ");

            DataTable table = engine.Query(query.ToString());

            foreach (DataRow dr in table.Rows)
            {
                MensajeUsuario mensajeUsuarioFila = new MensajeUsuario();

                mensajeUsuarioFila.Id = int.Parse(dr["Id"].ToString());
                mensajeUsuarioFila.Mensaje = dr["Mensaje"].ToString();
                mensajeUsuarioFila.IdUsuarioRemitente = int.Parse(dr["IdUsuarioRemitente"].ToString());
                mensajeUsuarioFila.IdUsuarioReceptor = int.Parse(dr["IdUsuarioReceptor"].ToString());
                mensajeUsuarioFila.FechaMensaje = DateTime.Parse(dr["FechaAlta"].ToString());

                resultado.Add(mensajeUsuarioFila);
            }

            return resultado;
        }

        public int BajaQuedadas(int idMensaje)
        {
            string connectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            Engine engine = new Engine(connectionString);

            StringBuilder comando = new StringBuilder();
            comando.Append("update mensajes set ");
            comando.Append("FechaBaja = NOW() ");
            comando.Append(" where id = " + idMensaje);
             
            int resultado = engine.Execute(comando.ToString());

            return resultado;
        }

    }
}