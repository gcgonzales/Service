﻿using DadisService.Models;
using DadisService.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;
using System.Configuration;

namespace DadisService.Controllers
{
    //[EnableCors(origins: "*", headers: "*", methods: "*")]
    public class UsuariosController : ApiController

    {
        public string Get(int id)
        {
            return "value";
        }

        [Route("api/Usuarios/GetListaUsuarios")]
        [HttpGet]
        public List<Usuario> GetListaUsuarios(string textoBusqueda)
        {
            List<Usuario> resultado = new List<Usuario>();

            UsuarioService usuarioService = new UsuarioService();
            resultado = usuarioService.GetUsuarios(textoBusqueda);

            return resultado;
        }

        [Route("api/Usuarios/GetUsuario")]
        [HttpGet]
        public Usuario GetUsuario(string id)
        {
            Usuario usuarioResultado = new Usuario();

            if (id != "0")
            {
                UsuarioService usuarioService = new UsuarioService();
                usuarioResultado = usuarioService.GetUsuarioPorId(id);
            }
            else
            {
                usuarioResultado.Nombres = "Giancarlo Andre";
                usuarioResultado.ApellidoPrimero = "Gonzales";
                usuarioResultado.ApellidoSegundo = "Vargas";
                usuarioResultado.Fotos = new List<Imagen>();
                Imagen fotoPrincipal = new Imagen();
                fotoPrincipal.Extension = "png";
                fotoPrincipal.NombreFichero = "https://cdn.icon-icons.com/icons2/1123/PNG/512/happysmilingemoticonface_79597.";

                usuarioResultado.Fotos.Add(fotoPrincipal);


            }
           
            return usuarioResultado;
        }

       
        //[HttpPost]
        //public int Crear(Usuario value)
        //{
        //    Usuario nuevoUsuario = new Usuario();

        //    nuevoUsuario.Id = int.Parse(DateTime.Now.Ticks.ToString().Substring(0,5));
        //    nuevoUsuario.Nombres = value.Nombres;
        //    nuevoUsuario.ApellidoPrimero = value.ApellidoPrimero;
        //    nuevoUsuario.ApellidoSegundo = value.ApellidoSegundo;
        //    nuevoUsuario.Telefono = value.Telefono;
        //    nuevoUsuario.Email = value.Email;



        //    return nuevoUsuario.Id;
        //}

        [HttpPost]
        public int Editar(Usuario value)
        {
            Usuario nuevoUsuario = new Usuario();

            nuevoUsuario.Id = int.Parse(DateTime.Now.Ticks.ToString().Substring(0, 5));
            return nuevoUsuario.Id;
        }

        [HttpPost]
        public int Baja(int idUsuario, int idUsuarioResponsable)
        {
            Usuario nuevoUsuario = new Usuario();

            nuevoUsuario.Id = int.Parse(DateTime.Now.Ticks.ToString().Substring(0, 5));
            return nuevoUsuario.Id;
        }


        [HttpPost]
        public int BloquearUsuario(int idUsuarioBloqueado, int idUsuarioBloqueante)
        {
            Usuario nuevoUsuario = new Usuario();

            nuevoUsuario.Id = int.Parse(DateTime.Now.Ticks.ToString().Substring(0, 5));
            return nuevoUsuario.Id;
        }

        [Route("api/Usuarios/EnviarMensaje")]
        [HttpPost]
        public int EnviarMensaje(MensajeUsuario mensaje)
        {
            int resultado = 0;
            MensajeService mensajeService = new MensajeService();
            resultado = mensajeService.CrearMensaje(mensaje);
            return resultado;
        }


        [Route("api/Usuarios/GetMensajes")]
        [HttpGet]
        public List<MensajeUsuario> GetMensajes(int idUsuarioSesion, int idUsuario)
        {
            List<MensajeUsuario> mensajes = new List<MensajeUsuario>();

            MensajeService mensajeService = new MensajeService();
            mensajes = mensajeService.GetMensajes(idUsuarioSesion, idUsuario);

            return mensajes;
        }

        [Route("api/Usuarios/Autenticarse")]
        [HttpPost]
        public Usuario Autenticarse (Credencial credencial)
        {
            Usuario usuario = new Usuario();

            UsuarioService usuarioService = new UsuarioService();
            usuario = usuarioService.GetUsuarioAutenticado(credencial.Login, credencial.Password);
            
            return usuario;
        }

        [Route("api/Usuarios/ReiniciarPassword")]
        [HttpGet]
        public Usuario ReiniciarPassword (string email)
        {
            UsuarioService usuarioService = new UsuarioService();
            Usuario resultado = usuarioService.ReiniciarPassword(email);

            return resultado;
        }

        [Route("api/Usuarios/GetAdminKey")]
        [HttpGet]
        public string GetAdminKey()
        {
            return ConfigurationManager.AppSettings["adminKey"].ToString();
        }


        [Route("api/Usuarios/Guardar")]
        [HttpPost]
        public int Guardar(Usuario value)
        {
            int resultado = 0;
            bool validacionToken = CommonService.ProcessTokenHeader(Request);

            if (validacionToken)
            {
                Usuario usuarioGuardado = new Usuario();
                usuarioGuardado.Nombres = value.Nombres;
                usuarioGuardado.ApellidoPrimero = value.ApellidoPrimero;
                usuarioGuardado.ApellidoSegundo = value.ApellidoSegundo;
                usuarioGuardado.Telefono = value.Telefono;
                usuarioGuardado.Email = value.Email;
                usuarioGuardado.Login = value.Login;
                usuarioGuardado.IdUsuarioAlta = value.IdUsuarioAlta;
                usuarioGuardado.Password = value.Password;

                UsuarioService usuarioService = new UsuarioService();

                if (value.Id == 0)
                {
                    resultado = usuarioService.CrearUsuario(usuarioGuardado);
                    value.Id = resultado;

                    if (value.Fotografias != null)
                    {
                        foreach (Fotografia fotografia in value.Fotografias)
                        {
                            fotografia.IdUsuario = value.Id;
                        }
                    }
                }
                else
                {
                    usuarioGuardado.Id = value.Id;
                    resultado = usuarioService.EditarUsuario(usuarioGuardado);

                }

                FotografiaService fotografiaService = new FotografiaService();
                fotografiaService.AdjuntarFotografias(value.Fotografias);

            }

            return resultado;
        }


        [Route("api/Usuarios/BajaUsuarios")]
        [HttpPost]
        public int BajaUsuarios(string [] idsUsuarios)
        {
            Usuario usuarioGuardado = new Usuario();

            int resultado = 0;

            UsuarioService usuarioService = new UsuarioService();
            resultado = usuarioService.BajaUsuarios(idsUsuarios);

            return usuarioGuardado.Id;
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}