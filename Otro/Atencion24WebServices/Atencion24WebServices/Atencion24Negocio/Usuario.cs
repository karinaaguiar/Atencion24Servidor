using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Atencion24WebServices.Atencion24DAO;

namespace Atencion24WebServices.Atencion24Negocio
{
    public class Usuario
    {
        private string login;
        private string password;

        //Constructor
        public Usuario() { }

        //Constructor
        public Usuario(string log, string pass)
        {
            login = log;
            password = pass;
        }

        public string Login
        {
            get { return login; }
            set { login = value; }
        }

        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        //Función que llama a otra función en la capa de datos para decir si un usuario existe o no.
        public DataSet ExisteUsuario(Usuario user)
        {
            DataSet ds = new DataSet();

            UsuarioDAO ud = new UsuarioDAO();

            ds = ud.ExisteUsuarioDAO(user);
            return ds;
        }

        //Función que llama a otra función en la capa de datos para devolver los datos de un usuario
        public DataSet ConsultarUsuario(Usuario user)
        {
            DataSet ds = new DataSet();

            UsuarioDAO ud = new UsuarioDAO();

            ds = ud.ConsultarUsuarioDAO(user);
            return ds;
        }
    }
}
