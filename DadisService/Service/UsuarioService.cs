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
    public class UsuarioService
    {
        public List<Usuario> GetUsuarios(string param)
        {
            List<Usuario> resultado = new List<Usuario>();

            string connectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            Engine engine = new Engine(connectionString);

            string query = "";

            if (!string.IsNullOrEmpty(param))
            { query = string.Format("select Id,Nombres,Apellido1,Apellido2,Email,Telefono,Usuario from usuarios where nombres like '%{0}%' OR apellido1 like '%{0}%' OR apellido2 like '%{0}%' and fechaBaja is NULL", param); }
            else
            { query = string.Format("select Id,Nombres,Apellido1,Apellido2,Email,Telefono,Usuario from usuarios where fechabaja is null order by fechaalta desc limit 10"); }

            

            DataTable table = engine.Query(query);

            foreach (DataRow dr in table.Rows)
            {
                Usuario usuarioFila = new Usuario();

                usuarioFila.Id = int.Parse(dr["Id"].ToString());
                usuarioFila.Nombres = dr["Nombres"].ToString();
                usuarioFila.ApellidoPrimero = dr["Apellido1"].ToString();
                usuarioFila.ApellidoSegundo = dr["Apellido2"].ToString();
                usuarioFila.Email = dr["Email"].ToString();
                usuarioFila.Telefono = dr["Telefono"].ToString();

                resultado.Add(usuarioFila);
            }

            return resultado;
        }


        public Usuario GetUsuarioPorId(string id)
        {
            Usuario resultado = new Usuario();

            string connectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            Engine engine = new Engine(connectionString);

            string query = "";

            query = string.Format("select Id,Nombres,Apellido1,Apellido2,Email,Telefono,Usuario from usuarios where Id = {0} and fechaBaja is NULL", id);
          
            DataTable table = engine.Query(query);

            if (table != null && table.Rows != null && table.Rows.Count > 0)
            {
                resultado.Id = int.Parse(table.Rows[0]["Id"].ToString());
                resultado.Nombres = table.Rows[0]["Nombres"].ToString();
                resultado.ApellidoPrimero = table.Rows[0]["Apellido1"].ToString();
                resultado.ApellidoSegundo = table.Rows[0]["Apellido2"].ToString();
                resultado.Email = table.Rows[0]["Email"].ToString();
                resultado.Telefono = table.Rows[0]["Telefono"].ToString();
                resultado.Login = table.Rows[0]["Usuario"].ToString();
            }

            return resultado;
        }

        public int CrearUsuario(Usuario usuario)
        {
            int idGenerado = 0;

            string connectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            Engine engine = new Engine(connectionString);

            StringBuilder comando = new StringBuilder();
            comando.Append("insert into usuarios ");
            comando.Append(" (nombres, apellido1, apellido2, usuario, telefono, email, usuarioAlta, fechaAlta) ");
            comando.Append(" values ");
            comando.Append(" ('"+usuario.Nombres+"','"+usuario.ApellidoPrimero+"','"+usuario.ApellidoSegundo+"','"+usuario.Login+"','"+usuario.Telefono+"','"+usuario.Email+"',1, CURDATE() ) ");

            int resultado = engine.Execute(comando.ToString());

            if (resultado > 0) {

                int idGeneradoUsuario = GetLastIdFromTable("usuarios");
                CrearCredencialesUsuario(idGeneradoUsuario, usuario.Password);
            }
            
            return idGenerado;
        }

        public int CrearCredencialesUsuario(int idUsuario, string password)
        {
            int resultado = 0;

            int idGeneradoHistoricoPassword = CrearHistoricoPassword(idUsuario);

            if (idGeneradoHistoricoPassword > 0)
            {
                resultado = RegistrarPasswordActual(idGeneradoHistoricoPassword,password);
            }


            return resultado;
        }

        public int CrearHistoricoPassword(int idUsuario)
        {
            int resultado = 0;

            string connectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            Engine engine = new Engine(connectionString);

            StringBuilder comando = new StringBuilder();
            comando.Append("insert into historicopasswords ");
            comando.Append(" (idusuario, fechaalta) ");
            comando.Append(" values ");
            comando.Append(" ('" + idUsuario + "', CURDATE())");

            int rowsAffected = engine.Execute(comando.ToString());

            if (rowsAffected > 0)
            {
                resultado = GetLastIdFromTable("historicopasswords");
            }

            return resultado;
        }

        public int RegistrarPasswordActual(int idHistoricoPassword, string password)
        {
            int resultado = 0;

            string passwordHasheado = EncryptionService.MD5Hash(password);

            string connectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            Engine engine = new Engine(connectionString);

            StringBuilder comando = new StringBuilder();
            comando.Append("insert into passwords ");
            comando.Append(" (idHistoricoPassword, password) ");
            comando.Append(" values ");
            comando.Append(" ('" + idHistoricoPassword + "', '"+ passwordHasheado + "')");

            int rowsAffected = engine.Execute(comando.ToString());

            if (rowsAffected > 0)
            {
                resultado = GetLastIdFromTable("passwords");
            }

            return resultado;
        }


        public int GetIdGenerated()
        {
            int resultado = 0;
            string connectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            Engine engine = new Engine(connectionString);
            DataTable dtResult = engine.Query("select LAST_INSERT_ID()");

            if (dtResult.Rows.Count > 0)
            {
                resultado = int.Parse(dtResult.Rows[0][0].ToString());
            }

            return resultado;
        }

        public int GetLastIdFromTable(string nombreTabla)
        {
            int resultado = 0;
            string connectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            Engine engine = new Engine(connectionString);
            DataTable dtResult = engine.Query("select Id from " + nombreTabla + " order by id desc LIMIT 1 ");

            if (dtResult.Rows.Count > 0)
            {
                resultado = int.Parse(dtResult.Rows[0][0].ToString());
            }

            return resultado;
        }


        public int EditarUsuario(Usuario usuario)
        {
            string connectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            Engine engine = new Engine(connectionString);

            StringBuilder comando = new StringBuilder();
            comando.Append("update usuarios set ");
            comando.Append("nombres = '" + usuario.Nombres + "'");
            comando.Append(", apellido1 = '" + usuario.ApellidoPrimero + "'");
            comando.Append(", apellido2 = '" + usuario.ApellidoSegundo + "'");
            comando.Append(", telefono = '" + usuario.Telefono + "'");
            comando.Append(", email = '" + usuario.Email + "'");
            comando.Append(", usuario = '" + usuario.Login + "'");
            comando.Append(" where id=" + usuario.Id);

            int resultado = engine.Execute(comando.ToString());


            return usuario.Id;
        }

        public int BajaUsuarios(string[] IdsUsuarios)
        {
            string connectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            Engine engine = new Engine(connectionString);

            StringBuilder comando = new StringBuilder();
            comando.Append("update usuarios set ");
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

        public Usuario GetUsuarioAutenticado(string login, string password)
        {
            Usuario resultado = new Usuario();
            resultado.Id = 0;

            string passwordHash = EncryptionService.MD5Hash(password);

            string connectionString = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            Engine engine = new Engine(connectionString);

            StringBuilder query = new StringBuilder();
            query.Append("select ");
            query.Append(" usuarios.id, usuarios.usuario, usuarios.nombres, usuarios.apellido1, usuarios.apellido2, usuarios.telefono, usuarios.email, usuarios.fechaBaja, usuarios.idperfilusuario  ");
            query.Append(" ,passwords.Password ");
            query.Append(" from usuarios ");
            query.Append(" inner join historicopasswords ");
            query.Append(" on usuarios.id = historicopasswords.idusuario ");
            query.Append(" inner join passwords ");
            query.Append(" on historicopasswords.id = passwords.idhistoricopassword ");
            query.Append(" where ");
            query.Append(" usuarios.Usuario = '" + login.ToLower().Trim() + "' ");

            DataTable table = engine.Query(query.ToString());

            if (table != null && table.Rows != null && table.Rows.Count > 0)
            {
                bool passwordVigente = false;
                foreach (DataRow fila in table.Rows)
                {
                    if (fila["fechabaja"].ToString().ToLower().Equals("") || fila["fechabaja"].ToString().ToLower().Equals("null"))
                    {
                        passwordVigente = true;

                        if (fila["Password"].ToString().Trim().Equals(passwordHash))
                        {
                            resultado.Id = int.Parse(fila["Id"].ToString());
                            resultado.Nombres = fila["Nombres"].ToString();
                            resultado.ApellidoPrimero = fila["Apellido1"].ToString();
                            resultado.ApellidoSegundo = fila["Apellido2"].ToString();
                            resultado.Login = fila["Usuario"].ToString();
                            resultado.Email = fila["Email"].ToString();
                            resultado.Telefono = fila["Telefono"].ToString();
                        }
                        else
                        {
                            resultado.IncidenciaUsuario = "Password incorrecto.";
                        }
                    }
                }

                if (!passwordVigente)
                { resultado.IncidenciaUsuario = "La cuenta tiene contraseña caducada. Contactar con el administrador."; }

            }
            else
            {
                resultado.IncidenciaUsuario = "No se ha encontrado Usuario con ese nombre de login.";
            }

            return resultado;
        }
    }
}