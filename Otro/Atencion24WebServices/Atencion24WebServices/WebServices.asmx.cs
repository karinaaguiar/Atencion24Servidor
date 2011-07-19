using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Atencion24WebServices.Atencion24Negocio;

namespace Atencion24WebServices
{
    /// <summary>
    /// Web services para consulta de reportes (Atención 24)
    /// </summary>
    [WebService(Namespace = "http://localhost/Atencion24Servidor")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class OperacionesWebServicesAtencion24 : System.Web.Services.WebService
    {

        [WebMethod(Description = "Inicio de sesión en la aplicación Atencion 24")]
        public String InicioSesion(string usuario_tb, string clave_tb)
        {
            ManejadorXML manej = new ManejadorXML();
            DataSet ds = new DataSet();

            usuario_tb = usuario_tb.Trim();
            clave_tb = clave_tb.Trim();

            //LOGIN USUARIO//

            //Creamos una instancia de usuario con los datos que fueron introducidos por pantalla (Pantalla de Inicio de Sesión)
            Usuario usuarioInput = new Usuario(usuario_tb, clave_tb);

            //Verificamos si el usuario ingresado existe en la base de datos
            ds = usuarioInput.ExisteUsuario(usuarioInput);
            if (ds.Tables[0].Rows.Count == 0)
            {
                //Los datos no corresponden a un usuario de la Base de datos
                return manej.codificarXmlAEnviar(manej.envioMensajeError("1"));
            }
            else
            {
                ds = usuarioInput.ConsultarUsuario(usuarioInput);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    //La contraseña es incorrecta
                    return manej.codificarXmlAEnviar(manej.envioMensajeError("0"));
                }
                
                //Los datos introducidos son correctos
                //Del DataSet devuelto tomo los datos del usuario
                string nombre = ds.Tables[0].Rows[0].ItemArray.ElementAt(0).ToString();
                string apellido = ds.Tables[0].Rows[0].ItemArray.ElementAt(1).ToString();
                string codigo = ds.Tables[0].Rows[0].ItemArray.ElementAt(2).ToString();
                string nombreUsuario = ds.Tables[0].Rows[0].ItemArray.ElementAt(3).ToString();

                return manej.codificarXmlAEnviar(manej.creacionRespuestaInicioSesion(nombre, apellido, codigo, nombreUsuario));
            }
        }

        [WebMethod(Description = "Consultar Estado de Cuenta por antiguedad de saldo")]
        public String edoCtaAntiguedadSaldo(string medico_tb)
        {
            ManejadorXML manej = new ManejadorXML();

            medico_tb = medico_tb.Trim();
          
            //Creamos una instancia de EstadoDeCuenta con los datos de entrada (medico_tb)
            EstadoDeCuenta edoCta = new EstadoDeCuenta(medico_tb);

            //Consultamos el estado de cuenta por antiguedad de saldo
            edoCta.ConsultarEstadoDeCuentaAS();
            if(edoCta.sinDeuda==true)
                return manej.codificarXmlAEnviar(manej.envioMensajeError("0"));
            else
                return manej.codificarXmlAEnviar(manej.creacionRespuestaEdoCtaAS(edoCta));
        }
    }
}

