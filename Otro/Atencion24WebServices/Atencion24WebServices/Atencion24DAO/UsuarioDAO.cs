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
        public DataSet InicioSesionExisteUsuario(String login)
        {
            Cmd.CommandText = QueryAtencion24.ExisteUsuario(login);
            Da.SelectCommand = Cmd;
            Da.Fill(Ds);
            CerrarConexionBd();
            return Ds;
        }

        //Función que devuelve los datos de un usuario (cedula) conociendo el Login, Password
        public DataSet InicioSesionConsultarUsuario(String login, String password)
        {
            Cmd.CommandText = QueryAtencion24.ConsultarUsuario(login, password);
            Da.SelectCommand = Cmd;
            Da.Fill(Ds);
            CerrarConexionBd();
            return Ds;
        }

        //Query inicio de sesión. Retorna los códigos de pago asociados a la cédula del usuario 
        public DataSet InicioSesionConsultarCodigosPago(String cedula)
        {
            Cmd.CommandText = QueryAtencion24.ConsultarCodigosPago(cedula);
            Da.SelectCommand = Cmd;
            Da.Fill(Ds);
            CerrarConexionBd();
            return Ds;
        }

        //Query inicio de sesión. Retorna el nombre de los códigos de pago asociados al usuario loggeado
        public DataSet InicioSesionConsultarNombreCodigosPago(String codigo)
        {
            Cmd.CommandText = QueryAtencion24.ConsultarNombreCodigosPago(codigo);
            Da.SelectCommand = Cmd;
            Da.Fill(Ds);
            CerrarConexionBd();
            return Ds;
        }
    }
}
