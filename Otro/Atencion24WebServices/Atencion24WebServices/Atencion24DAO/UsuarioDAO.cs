using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Configuration; //Acceder al archivo de configuracion, donde están los datos de la conexion con SQL Server
using System.Data.SqlClient; //Librería para interactuar con SQL SERVER 2008
using Atencion24WebServices.Atencion24Negocio;

namespace Atencion24WebServices.Atencion24DAO  
{
    public class UsuarioDAO : DAO
    {

        public UsuarioDAO() : base()
        {}

        //Función que dice si un usuario existe o no en la base de datos
        public DataSet ExisteUsuarioDAO(Usuario user)
        {
            Cmd.CommandText = QueryAtencion24.ExisteUsuario(user);
            Da.SelectCommand = Cmd;
            Da.Fill(Ds);
            CerrarConexionBd();
            return Ds;
        }

        //Función que devuelve los datos de un usuario conociendo el Login, Password
        public DataSet ConsultarUsuarioDAO(Usuario user)
        {
            Cmd.CommandText = QueryAtencion24.ConsultarUsuario(user);
            Da.SelectCommand = Cmd;
            Da.Fill(Ds);
            CerrarConexionBd();
            return Ds;
        }
    }
}
