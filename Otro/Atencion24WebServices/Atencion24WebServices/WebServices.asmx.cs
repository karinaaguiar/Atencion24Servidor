using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using Atencion24WebServices.Atencion24Negocio;

namespace Atencion24WebServices
{
    /// <summary>
    /// Servicios Web para aplicación Atención 24 móvil
    /// </summary>
   
    [WebService(Namespace = "http://localhost/Atencion24Servidor")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    
        
    public class OperacionesWebServicesAtencion24 : System.Web.Services.WebService
    {

        [WebMethod(Description = "Inicio de sesión en la aplicación Atencion 24", EnableSession = true)]
        public String InicioSesion(string usuario_tb, string clave_tb)
        {
            ManejadorXML manej = new ManejadorXML();

            usuario_tb = usuario_tb.Trim();
            clave_tb = clave_tb.Trim();

            System.Diagnostics.Debug.WriteLine("ESTE es el SessionID " + Session.SessionID);

            //Creamos una instancia de usuario con los datos que fueron introducidos por pantalla (Pantalla de Inicio de Sesión)
            Usuario usuarioInput = new Usuario(usuario_tb, clave_tb);

            //Verificamos si el usuario ingresado existe en la base de datos
            usuarioInput.ExisteUsuario();
            if (usuarioInput.Valido == false)
                return manej.codificarXmlAEnviar(manej.envioMensajeError("1"));
            else
            { 
                //Verificamos que se introdujo bien la contraseña
                String cedula = usuarioInput.ConsultarUsuario();
                if (usuarioInput.Valido == false)
                    return manej.codificarXmlAEnviar(manej.envioMensajeError("0"));
                else
                {
                    //Los datos introducidos son correctos. Inicio de sesión exitoso
                    usuarioInput.ConsultarCodigosPago(cedula);
                    Session.Add("Loggedin", "");
                    Session["Loggedin"] = "yes";
                    return manej.codificarXmlAEnviar(manej.creacionRespuestaInicioSesion(usuarioInput));
                }
            }
            
            /*VERSION VIEJA
            ManejadorXML manej = new ManejadorXML();
            DataSet ds = new DataSet();

            usuario_tb = usuario_tb.Trim();
            clave_tb = clave_tb.Trim();
           
            //LOGIN USUARIO//
            System.Diagnostics.Debug.WriteLine("ESTE es el SessionID " + Session.SessionID); 
            Session.Add("Loggedin", "");

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
                else
                {   
                    //Los datos introducidos son correctos
                    //Del DataSet devuelto tomo los datos del usuario
                    string nombre = ds.Tables[0].Rows[0].ItemArray.ElementAt(0).ToString();
                    string apellido = ds.Tables[0].Rows[0].ItemArray.ElementAt(1).ToString();
                    string codigo = ds.Tables[0].Rows[0].ItemArray.ElementAt(2).ToString();
                    string nombreUsuario = ds.Tables[0].Rows[0].ItemArray.ElementAt(3).ToString();
                    Session["Loggedin"] = "yes";
                    return manej.codificarXmlAEnviar(manej.creacionRespuestaInicioSesion(nombre, apellido, codigo, nombreUsuario));
                }
            }*/
        }

        /// <summary>
        /// Este servicio web permite consultar el estado de 
        /// cuenta de un médico por antiguedad de saldo
        /// </summary>
        /// <param name="medico_tb"> codigo de pago del médico cuyo  
        /// estado de cuenta se quiere conocer.</param>
        /// <returns>XML con el estado de cuenta</returns>
        [WebMethod(Description = "Consultar Estado de Cuenta por antiguedad de saldo", EnableSession = true)]
        public String edoCtaAntiguedadSaldo(string medico_tb)
        {
            ManejadorXML manej = new ManejadorXML();

            medico_tb = medico_tb.Trim();

            System.Diagnostics.Debug.WriteLine("En Estado de cuenta ESTE es el SessionID " + Session.SessionID);
             
            if (Session["Loggedin"] !=null) 
            {
                if (Session["Loggedin"].Equals("yes"))
                {
                    //Creamos una instancia de EstadoDeCuenta con los datos de entrada (medico_tb)
                    EstadoDeCuenta edoCta = new EstadoDeCuenta(medico_tb);

                    //Consultamos el estado de cuenta por antiguedad de saldo
                    edoCta.ConsultarEstadoDeCuentaAS();
                    if (edoCta.sinDeuda == true)
                        return manej.codificarXmlAEnviar(manej.envioMensajeError("0"));
                    else
                        return manej.codificarXmlAEnviar(manej.creacionRespuestaEdoCtaAS(edoCta));
                }
                else
                    return manej.codificarXmlAEnviar(manej.envioMensajeError("13")); 
            }
            else
                return manej.codificarXmlAEnviar(manej.envioMensajeError("13")); 
        }

        /// <summary>
        /// Este servicio web permite consultar el monto a cobrar en  
        /// en próximo pago a realizarse. 
        /// </summary>
        /// <param name="medico_tb"> codigo de pago del médico cuyo  
        /// próximo pago se quiere conocer.</param>
        /// <returns>XML con la información del próximo pago </returns>
        [WebMethod(Description = "Consultar Honorarios pagados. Pago en proceso", EnableSession = true)]
        public String ConsultarProximoPago(string medico_tb)
        {
            ManejadorXML manej = new ManejadorXML();

            medico_tb = medico_tb.Trim();

            System.Diagnostics.Debug.WriteLine("En proximo pago ESTE es el SessionID " + Session.SessionID);

            if (Session["Loggedin"] !=null) 
            {
                if (Session["Loggedin"].Equals("yes"))
                {
                    //Creamos una instancia de EstadoDeCuenta con los datos de entrada (medico_tb)
                    Pago pago = new Pago(medico_tb);

                    //Consultamos el estado de cuenta por antiguedad de saldo
                    pago.consultarProximoPago();
                    if (pago.sinPago == true)
                        return manej.codificarXmlAEnviar(manej.envioMensajeError("0"));
                    else
                        return manej.codificarXmlAEnviar(manej.creacionRespuestaProximoPago(pago));
                }
                else
                    return manej.codificarXmlAEnviar(manej.envioMensajeError("13")); 
            }
            else
                return manej.codificarXmlAEnviar(manej.envioMensajeError("13")); 
        }

        /// <summary>
        /// Este servicio web permite consultar los pagos que se han realizado a un
        /// médico en el rango de fechas indicado.
        /// </summary>
        /// <param name="medico_tb">codigo de pago del médico</param>
        /// <param name="fechaI_tb">fecha inicio consulta</param>
        /// <param name="fechaF_tb">fecha fin consulta</param>
        /// <returns>XML con la información de los pagos hechos en el rango de fechas</returns>
        [WebMethod(Description = "Consultar Honorarios pagados. Histórico de pagos", EnableSession = true)]
        public String ConsultarHistoricoPagos(string medico_tb, string fechaI_tb, string fechaF_tb)
        {
            ManejadorXML manej = new ManejadorXML();
            medico_tb = medico_tb.Trim();
            String fechaI = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(fechaI_tb));
            String fechaF = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(fechaF_tb));

            System.Diagnostics.Debug.WriteLine("En Historico de pagos ESTE es el SessionID " + Session.SessionID);

            if (Session["Loggedin"] !=null) 
            {
                if (Session["Loggedin"].Equals("yes"))
                {
                    //System.Diagnostics.Debug.WriteLine("FECHAS ANTES:" + fechaI_tb + " " + fechaF_tb);
                    //System.Diagnostics.Debug.WriteLine("FECHAS DESPUES:" + fechaI + " " + fechaF);

                    //Creamos una instancia de HistoricoPagos con los datos de entrada (medico_tb, fechaI, fechaF)
                    HistoricoPagos pagos = new HistoricoPagos(medico_tb, fechaI, fechaF);

                    //Consultamos el listado de pagos generados para el médico en el rango de fechas
                    pagos.consultarHistoricoPagos();
                    if (pagos.sinPagos == true)
                        return manej.codificarXmlAEnviar(manej.envioMensajeError("0"));
                    else
                        return manej.codificarXmlAEnviar(manej.creacionRespuestaHistoricoPagos(pagos.Pagos));
                }
                else
                    return manej.codificarXmlAEnviar(manej.envioMensajeError("13")); 
            }
            else
                return manej.codificarXmlAEnviar(manej.envioMensajeError("13")); 
       
        }
       
        /// <summary>
        /// Este servicio web permite consultar los honorarios facturados por un médico
        /// en el rango de fechas indicado.
        /// </summary>
        /// <param name="medico_tb">codigo de pago del médico</param>
        /// <param name="fechaI_tb">fecha inicio consulta</param>
        /// <param name="fechaF_tb">fecha fin consulta</param>
        /// <returns>XML con la información de lo facturado por el médico en el rango de fechas</returns>
        [WebMethod(Description = "Consultar Honorarios facturados ", EnableSession = true)]
        public String ConsultarHonorariosFacturados(string medico_tb, string fechaI_tb, string fechaF_tb)
        {
            ManejadorXML manej = new ManejadorXML();
            medico_tb = medico_tb.Trim();
            String fechaI = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(fechaI_tb));
            String fechaF = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(fechaF_tb));

            System.Diagnostics.Debug.WriteLine("En Facturado ESTE es el SessionID " + Session.SessionID);

            if (Session["Loggedin"] !=null) 
            {
                if (Session["Loggedin"].Equals("yes"))
                {
                    //Creamos una instancia de HistoricoPagos con los datos de entrada (medico_tb, fechaI, fechaF)
                    FacturadoUDN facturado = new FacturadoUDN(medico_tb, fechaI, fechaF);

                    //Consultamos el listado de pagos generados para el médico en el rango de fechas
                    facturado.consultarHonorariosFacturados();
                    if (facturado.SinFacturado == true)
                        return manej.codificarXmlAEnviar(manej.envioMensajeError("0"));
                    else
                        return manej.codificarXmlAEnviar(manej.creacionRespuestaHonorariosFacturados(facturado));
                }
                else
                    return manej.codificarXmlAEnviar(manej.envioMensajeError("13")); 
            }
            else
                return manej.codificarXmlAEnviar(manej.envioMensajeError("13")); 

        }

        /// <summary>
        /// Este servicio web permite consultar el listado de casos atendidos por un médico 
        /// por apellido de paciente
        /// </summary>
        /// <param name="medico_tb">médico</param>
        /// <param name="apellido_tb">apellido del paciente</param>
        /// <returns>XML con el listado de casos atendidos por el médico con pacientes por el apellido ingresado</returns>
        [WebMethod(Description = "Consultar Listado de casos atendidos por un médico por apellido del paciente", EnableSession = true)]
        public String consultarListadoDeCaso(string medico_tb, string apellido_tb)
        {
            ManejadorXML manej = new ManejadorXML();
            medico_tb = medico_tb.Trim();

            System.Diagnostics.Debug.WriteLine("En Listado de casos ESTE es el SessionID " + Session.SessionID);

            if (Session["Loggedin"] !=null) 
            {
                if (Session["Loggedin"].Equals("yes"))
                {
                    //Creamos una instancia de HistoricoPagos con los datos de entrada (medico_tb, fechaI, fechaF)
                    ListadoCasos casos = new ListadoCasos(medico_tb, apellido_tb);

                    //Consultamos el listado de pagos generados para el médico en el rango de fechas
                    casos.ConsultarListadoDeCasos();
                    if (casos.SinCasos == true)
                        return manej.codificarXmlAEnviar(manej.envioMensajeError("0"));
                    else
                        return manej.codificarXmlAEnviar(manej.creacionRespuestaListadoDeCaso(casos.Casos));
                }
                else
                    return manej.codificarXmlAEnviar(manej.envioMensajeError("13")); 
            }
            else 
                return manej.codificarXmlAEnviar(manej.envioMensajeError("13")); 

        }

        /// <summary>
        /// Este servicio web permite consultar el detalle de un caso.
        /// </summary>
        /// <param name="medico_tb">médico que prestó algún servicio médico en el caso</param>
        /// <param name="caso_tb">identificador del caso</param>
        /// <param name="udn_tb">unidad de negocio en la cual ingresó el caso</param>
        /// <returns>XML con el detalle del caso</returns>
        [WebMethod(Description = "Consultar detalle de un caso", EnableSession = true)]
        public String consultarCaso(string medico_tb, string caso_tb, string udn_tb)
        {
            ManejadorXML manej = new ManejadorXML();
            medico_tb = medico_tb.Trim();
            caso_tb = caso_tb.Trim();
            udn_tb = udn_tb.Trim();

            System.Diagnostics.Debug.WriteLine("En Detalle de caso ESTE es el SessionID " + Session.SessionID);

            if (Session["Loggedin"] !=null) 
            {
                if (Session["Loggedin"].Equals("yes"))
                {
                    //Creamos una instancia de HistoricoPagos con los datos de entrada (medico_tb, fechaI, fechaF)
                    Caso caso = new Caso(medico_tb, caso_tb, udn_tb);

                    //Consultamos el listado de pagos generados para el médico en el rango de fechas
                    caso.ConsultarDetalleDeCaso();

                    return manej.codificarXmlAEnviar(manej.creacionRespuestaDetalleDeCaso(caso));
                }
                else
                    return manej.codificarXmlAEnviar(manej.envioMensajeError("13")); 
            }
            else
                return manej.codificarXmlAEnviar(manej.envioMensajeError("13")); 

        }

        /// <summary>
        /// Este servicio web permite consultar el listado de fianzas pendientes de un médico
        /// </summary>
        /// <param name="medico_tb">codigo de pago del médico</param>
        /// <returns>XML con el listado de fianzas pendientes del médico</returns>
        [WebMethod(Description = "Consultar listado de fianzas pendientes", EnableSession = true)]
        public String listFianzas(string medico_tb)
        {
            ManejadorXML manej = new ManejadorXML();
            medico_tb = medico_tb.Trim();

            System.Diagnostics.Debug.WriteLine("En listado de fianzas ESTE es el SessionID " + Session.SessionID);

           if (Session["Loggedin"] !=null) 
            {
                if (Session["Loggedin"].Equals("yes"))
                {
                    //Creamos una instancia de HistoricoPagos con los datos de entrada (medico_tb, fechaI, fechaF)
                    ListadoFianzas fianzas = new ListadoFianzas(medico_tb);

                    //Consultamos el listado de pagos generados para el médico en el rango de fechas
                    fianzas.ConsultarListadoFianzas();

                    if (fianzas.SinFianzas == true)
                        return manej.codificarXmlAEnviar(manej.envioMensajeError("0"));
                    else
                        return manej.codificarXmlAEnviar(manej.creacionRespuestaListadoFianzas(fianzas.Fianzas));
                }
                else
                    return manej.codificarXmlAEnviar(manej.envioMensajeError("13")); 
            }
            else
                return manej.codificarXmlAEnviar(manej.envioMensajeError("13")); 

        }

        /// <summary>
        /// Este servicio web permite cerrar sesion
        /// </summary>
        /// <returns>Cierra la Session</returns>
        [WebMethod(Description = "Cerrar sesión", EnableSession = true)]
        public void cerrarSesion()
        {
            ManejadorXML manej = new ManejadorXML();

            System.Diagnostics.Debug.WriteLine("En cierre de sesion ESTE es el SessionID " + Session.SessionID);

            if (Session["Loggedin"] != null)
            {
                if (Session["Loggedin"].Equals("yes"))
                {
                    Session.Abandon();
                }
            }

        }

    }
}

