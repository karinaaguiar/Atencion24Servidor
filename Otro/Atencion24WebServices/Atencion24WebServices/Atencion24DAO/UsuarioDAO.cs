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

        //Query inicio de sesión. Retorna el código de pool en caso de que el médico pertenezca a alguno
        public DataSet InicioSesionConsultarPool(String codigoPropio)
        {
            Cmd.CommandText = QueryAtencion24.ConsultarPool(codigoPropio);
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

        //Query para consultar fecha en la que fueron cargados los datos en Atencion24
        public DataSet ConsultarFechaAdmin()
        {
            Cmd.CommandText = QueryAtencion24.ConsultarFechaAdmin();
            Da.SelectCommand = Cmd;
            Da.Fill(Ds);
            CerrarConexionBd();
            return Ds;
        }

        //Query para consultar si el usuario está bloqueado o no
        public DataSet estaBloqueado(string usuario)
        {
            Cmd.CommandText = QueryAtencion24.estaBloqueado(usuario);
            Da.SelectCommand = Cmd;
            Da.Fill(Ds);
            CerrarConexionBd();
            return Ds;
        }

        //Query para colocar bloqueado al usuario 
        public void setBloqueadoTrue(string usuario)
        {
            Cmd.CommandText = QueryAtencion24.setBloqueadoTrue(usuario);
            Da.SelectCommand = Cmd;
            Da.Fill(Ds);
            CerrarConexionBd();
        }

        //Query para desbloquear al usuario 
        public void setBloqueadoFalse(string usuario)
        {
            Cmd.CommandText = QueryAtencion24.setBloqueadoFalse(usuario);
            Da.SelectCommand = Cmd;
            Da.Fill(Ds);
            CerrarConexionBd();
        }
    }
}
