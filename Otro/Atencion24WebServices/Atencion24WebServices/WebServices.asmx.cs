﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data;
using System.Threading;
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

        //Función que permite verificar que el parámetro 'medico' que se envía a través del URL para
        //consumir el servicio web corresponde a algún código de pago del médico al que corresponde 
        //la sesión
        public bool CodValido(String medico)
        {
            bool valido = false;
            if (Session["codigosPago"] != null)
            {
                ArrayList codigosPago = ( ArrayList)Session["codigosPago"];
                foreach (CodigoPago codigoPago in codigosPago)
                {
                    if (codigoPago.Codigo == medico)
                        valido = true;
                }
            }
            return valido;
        }
        
        /// <summary>
        /// Este servicio web permite bloquear a un usuario. 
        /// </summary>
        /// <param name="usuario_tb"> nombre usuario</param>
        /// <returns>XML con respuesta de bloqueo exitoso</returns>
        [WebMethod(Description = "Inicio de sesión en la aplicación Atencion 24", EnableSession = true)]
        public String bloquear(string usuario_tb)
        {
            ManejadorXML manej = new ManejadorXML();

            usuario_tb = usuario_tb.Trim();

            System.Diagnostics.Debug.WriteLine("ESTE es el SessionID " + Session.SessionID);

            //check the IsNewSession value, this will tell us if the session has been reset
            if (Session.IsNewSession)
            {
                //now we know it's a new session, so we check to see if a cookie is present
                string cookie = HttpContext.Current.Request.Headers["Cookie"];
                //now we determine if there is a cookie does it contains what we're looking for
                if ((null != cookie) && (cookie.IndexOf("ASP.NET_SessionId") >= 0))
                {
                    //since it's a new session but a ASP.Net cookie exist we know
                    //the session has expired so we need to redirect them
                    System.Diagnostics.Debug.WriteLine("Error 503");
                    return manej.codificarXmlAEnviar(manej.envioMensajeError("503"));
                }
            }

            Usuario usuarioInput = new Usuario(usuario_tb);
            
            System.Diagnostics.Debug.WriteLine("Error 3");
            //Invocar método que coloque bloqueado = 1 
            usuarioInput.setBloqueadoTrue(usuario_tb);
            //Creamos un hilo que espera a que  
            //que transcurra el tiempo para poner en 0 nuevamente el campo
            //bloqueado
            Thread th1 = new Thread(new ThreadStart(usuarioInput.esperaDesbloquear));
            th1.Start();
            return manej.codificarXmlAEnviar(manej.envioMensajeError("3"));


        }

        /// <summary>
        /// Este servicio web permite iniciar sesión. 
        /// </summary>
        /// <param name="usuario_tb"> nombre usuario</param>
        /// <param name="clave_tb"> contraseña del usuario </param>
        /// <returns>XML con respuesta en cuanto a Inicio de Sesión</returns>
        [WebMethod(Description = "Inicio de sesión en la aplicación Atencion 24", EnableSession = true)]
        public String InicioSesion(string usuario_tb, string clave_tb)
        {
            ManejadorXML manej = new ManejadorXML();

            usuario_tb = usuario_tb.Trim();
            clave_tb = clave_tb.Trim();

            System.Diagnostics.Debug.WriteLine("ESTE es el SessionID " + Session.SessionID);
            
            //check the IsNewSession value, this will tell us if the session has been reset
            if (Session.IsNewSession)
            {
                //now we know it's a new session, so we check to see if a cookie is present
                string cookie = HttpContext.Current.Request.Headers["Cookie"];
                //now we determine if there is a cookie does it contains what we're looking for
                if ((null != cookie) && (cookie.IndexOf("ASP.NET_SessionId") >= 0))
                {
                    //since it's a new session but a ASP.Net cookie exist we know
                    //the session has expired so we need to redirect them
                    System.Diagnostics.Debug.WriteLine("Error 502");
                    return manej.codificarXmlAEnviar(manej.envioMensajeError("502"));
                }
                else
                {
                    Session.Add("Loggedin", "");
                }
            }  

            //Creamos una instancia de usuario con los datos que fueron introducidos por pantalla (Pantalla de Inicio de Sesión)
            Usuario usuarioInput = new Usuario(usuario_tb, clave_tb);

            //Verificamos si la base de datos está disponible
            usuarioInput.DisponibleBD();
            if (usuarioInput.Disponible == false)
                return manej.codificarXmlAEnviar(manej.envioMensajeError("600"));

            //Verificamos si el usuario ingresado existe en la base de datos
            usuarioInput.ExisteUsuario();
            if (usuarioInput.Valido == false)
            {
                System.Diagnostics.Debug.WriteLine("Error 1");
                return manej.codificarXmlAEnviar(manej.envioMensajeError("1"));
            }
            else
            {
                //Verificamos si el usuario está bloqueado
                if (usuarioInput.estaBloqueado(usuario_tb))
                {
                    System.Diagnostics.Debug.WriteLine("Error 4");
                    return manej.codificarXmlAEnviar(manej.envioMensajeError("4"));
                }
                else
                {
                    //Como el usuario es válido creamos su count de intentos fallidos (en caso de que no exista)
                    //if (Session[usuario_tb] == null)
                      //  Session.Add(usuario_tb, 0);
                    //System.Diagnostics.Debug.WriteLine("Count de " + usuario_tb + " " + (int)Session[usuario_tb]);

                    //Verificamos que se introdujo bien la contraseña
                    String codigo = usuarioInput.ConsultarUsuario();
                    if (usuarioInput.Valido == false)
                    {
                        //Session[usuario_tb] = (int)Session[usuario_tb] + 1;
                        //if ((int)Session[usuario_tb] == 3)
                        /*{
                            System.Diagnostics.Debug.WriteLine("Error 3");
                            //Invocar método que coloque bloqueado = 1 
                            usuarioInput.setBloqueadoTrue(usuario_tb);
                            //Creamos un hilo que espera a que  
                            //que transcurra el tiempo para poner en 0 nuevamente el campo
                            //bloqueado
                            Thread th1 = new Thread(new ThreadStart(usuarioInput.esperaDesbloquear));
                            th1.Start();
                            //Reiniciamos el count
                            Session.Remove(usuario_tb);
                            return manej.codificarXmlAEnviar(manej.envioMensajeError("3"));
                        }
                        else
                        {*/
                            System.Diagnostics.Debug.WriteLine("Error 0");
                            return manej.codificarXmlAEnviar(manej.envioMensajeError("0"));
                        //}
                    }
                    else
                    {
                        //Los datos introducidos son correctos. Inicio de sesión exitoso
                        usuarioInput.ConsultarCodigosPago(codigo);
                        usuarioInput.ConsultarFechaAdmin();

                        Session["Loggedin"] = "yes";
                        Session.Add("UltimaConsulta", DateTime.Now);
                        Session["codigosPago"] = usuarioInput.CodigosPago;
                        return manej.codificarXmlAEnviar(manej.creacionRespuestaInicioSesion(usuarioInput));
                    }
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
             
            //check to see if the Session is null (doesnt exist)
            if (Context.Session != null)
            {
                //check the IsNewSession value, this will tell us if the session has been reset
                if (Session.IsNewSession)
                {
                    //now we know it's a new session, so we check to see if a cookie is present
                    string cookie = HttpContext.Current.Request.Headers["Cookie"];
                    //now we determine if there is a cookie does it contains what we're looking for
                    if ((null != cookie) && (cookie.IndexOf("ASP.NET_SessionId") >= 0))
                    {
                        //since it's a new session but a ASP.Net cookie exist we know
                        //the session has expired so we need to redirect them
                        return manej.codificarXmlAEnviar(manej.envioMensajeError("505"));
                    }
                    else
                    {
                        return manej.codificarXmlAEnviar(manej.envioMensajeError("13"));
                    }
                }
                else
                {
                    if (Session["Loggedin"] != null)
                    {
                        if (Session["Loggedin"].Equals("yes"))
                        {
                            
                            if (CodValido(medico_tb))
                            {
                                DateTime x = DateTime.Now;
                                DateTime y = (DateTime)Session["UltimaConsulta"];
                                TimeSpan z = x.Subtract(y);

                                System.Diagnostics.Debug.WriteLine(x.ToString("yyyyMMdd HH:mm:ss"));
                                System.Diagnostics.Debug.WriteLine(y.ToString("yyyyMMdd HH:mm:ss"));
                                System.Diagnostics.Debug.WriteLine("Diferencia " + z.TotalMinutes);

                                if (z.TotalMinutes < 10)
                                {
                                    Session["UltimaConsulta"] = x;
                                   
                                    //Creamos una instancia de EstadoDeCuenta con los datos de entrada (medico_tb)
                                    EstadoDeCuenta edoCta = new EstadoDeCuenta(medico_tb);

                                    //Verificamos si la base de datos está disponible
                                    bool disponible = edoCta.DisponibleBD();
                                    if (disponible == false)
                                        return manej.codificarXmlAEnviar(manej.envioMensajeError("600"));
                                    else
                                    {
                                        //Consultamos el estado de cuenta por antiguedad de saldo
                                        edoCta.ConsultarEstadoDeCuentaAS();
                                        if (edoCta.sinDeuda == true)
                                            return manej.codificarXmlAEnviar(manej.envioMensajeError("0"));
                                        else
                                            return manej.codificarXmlAEnviar(manej.creacionRespuestaEdoCtaAS(edoCta));
                                    }
                                }
                                else
                                {
                                    return manej.codificarXmlAEnviar(manej.envioMensajeError("500"));
                                }
                            }
                            else return manej.codificarXmlAEnviar(manej.envioMensajeError("14"));
                        }
                        else
                            return manej.codificarXmlAEnviar(manej.envioMensajeError("13"));
                    }
                    else
                        return manej.codificarXmlAEnviar(manej.envioMensajeError("13"));
                }
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

            if (Context.Session != null)
            {
                //check the IsNewSession value, this will tell us if the session has been reset
                if (Session.IsNewSession)
                {
                    //now we know it's a new session, so we check to see if a cookie is present
                    string cookie = HttpContext.Current.Request.Headers["Cookie"];
                    //now we determine if there is a cookie does it contains what we're looking for
                    if ((null != cookie) && (cookie.IndexOf("ASP.NET_SessionId") >= 0))
                    {
                        //since it's a new session but a ASP.Net cookie exist we know
                        //the session has expired so we need to redirect them
                        return manej.codificarXmlAEnviar(manej.envioMensajeError("505"));
                    }
                    else
                    {
                        return manej.codificarXmlAEnviar(manej.envioMensajeError("13"));
                    }
                }
                else
                {
                    if (Session["Loggedin"] != null)
                    {
                        if (Session["Loggedin"].Equals("yes"))
                        {
                            if (CodValido(medico_tb))
                            {
                                DateTime x = DateTime.Now;
                                DateTime y = (DateTime)Session["UltimaConsulta"];
                                TimeSpan z = x.Subtract(y);

                                System.Diagnostics.Debug.WriteLine(x.ToString("yyyyMMdd HH:mm:ss"));
                                System.Diagnostics.Debug.WriteLine(y.ToString("yyyyMMdd HH:mm:ss"));
                                System.Diagnostics.Debug.WriteLine("Diferencia " + z.TotalMinutes);

                                if (z.TotalMinutes < 10)
                                {
                                    Session["UltimaConsulta"] = x;
                                    //Creamos una instancia de EstadoDeCuenta con los datos de entrada (medico_tb)
                                    Pago pago = new Pago(medico_tb);

                                    //Verificamos si la base de datos está disponible
                                    bool disponible = pago.DisponibleBD();
                                    if (disponible == false)
                                        return manej.codificarXmlAEnviar(manej.envioMensajeError("600"));
                                    else
                                    {
                                        //Consultamos el estado de cuenta por antiguedad de saldo
                                        pago.consultarProximoPago();
                                        if (pago.sinPago == true)
                                            return manej.codificarXmlAEnviar(manej.envioMensajeError("0"));
                                        else
                                            return manej.codificarXmlAEnviar(manej.creacionRespuestaProximoPago(pago));
                                    }
                                }
                                else
                                {
                                    return manej.codificarXmlAEnviar(manej.envioMensajeError("500"));
                                }
                            }
                            else return manej.codificarXmlAEnviar(manej.envioMensajeError("14"));
                        }
                        else
                            return manej.codificarXmlAEnviar(manej.envioMensajeError("13"));
                    }
                    else
                        return manej.codificarXmlAEnviar(manej.envioMensajeError("13"));
                }
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
            String fechaI = " ";
            String fechaF = " ";
            try
            {
                fechaI = String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(fechaI_tb));
                fechaF = String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(fechaF_tb));
            }   
            catch (System.FormatException)
            {
                //Si la fecha ingresada por el usuario está en un formato inválido
                return manej.codificarXmlAEnviar(manej.envioMensajeError("15"));
            }  
            
            System.Diagnostics.Debug.WriteLine("fechaI: " + fechaI + "fechaF: " + fechaF);

            System.Diagnostics.Debug.WriteLine("En Historico de pagos ESTE es el SessionID " + Session.SessionID);

            if (Context.Session != null)
            {
                //check the IsNewSession value, this will tell us if the session has been reset
                if (Session.IsNewSession)
                {
                    //now we know it's a new session, so we check to see if a cookie is present
                    string cookie = HttpContext.Current.Request.Headers["Cookie"];
                    //now we determine if there is a cookie does it contains what we're looking for
                    if ((null != cookie) && (cookie.IndexOf("ASP.NET_SessionId") >= 0))
                    {
                        //since it's a new session but a ASP.Net cookie exist we know
                        //the session has expired so we need to redirect them
                        return manej.codificarXmlAEnviar(manej.envioMensajeError("505"));
                    }
                    else
                    {
                        return manej.codificarXmlAEnviar(manej.envioMensajeError("13"));
                    }
                }
                else
                {
                    if (Session["Loggedin"] != null)
                    {
                        if (Session["Loggedin"].Equals("yes"))
                        {
                            if (CodValido(medico_tb))
                            {
                                DateTime x = DateTime.Now;
                                DateTime y = (DateTime)Session["UltimaConsulta"];
                                TimeSpan z = x.Subtract(y);

                                System.Diagnostics.Debug.WriteLine(x.ToString("yyyyMMdd HH:mm:ss"));
                                System.Diagnostics.Debug.WriteLine(y.ToString("yyyyMMdd HH:mm:ss"));
                                System.Diagnostics.Debug.WriteLine("Diferencia " + z.TotalMinutes);

                                if (z.TotalMinutes < 10)
                                {
                                    Session["UltimaConsulta"] = x;

                                    //System.Diagnostics.Debug.WriteLine("FECHAS ANTES:" + fechaI_tb + " " + fechaF_tb);
                                    //System.Diagnostics.Debug.WriteLine("FECHAS DESPUES:" + fechaI + " " + fechaF);

                                    //Creamos una instancia de HistoricoPagos con los datos de entrada (medico_tb, fechaI, fechaF)
                                    HistoricoPagos pagos = new HistoricoPagos(medico_tb, fechaI, fechaF);

                                    //Verificamos si la base de datos está disponible
                                    bool disponible = pagos.DisponibleBD();
                                    if (disponible == false)
                                        return manej.codificarXmlAEnviar(manej.envioMensajeError("600"));
                                    else
                                    {
                                        //Consultamos el listado de pagos generados para el médico en el rango de fechas
                                        pagos.consultarHistoricoPagos();
                                        if (pagos.sinPagos == true)
                                            return manej.codificarXmlAEnviar(manej.envioMensajeError("0"));
                                        else
                                        {
                                            if (pagos.Excede == true)
                                                return manej.codificarXmlAEnviar(manej.envioMensajeError("1"));
                                            else
                                                return manej.codificarXmlAEnviar(manej.creacionRespuestaHistoricoPagos(pagos.Pagos));
                                        }
                                    }
                                }
                                else
                                {
                                    return manej.codificarXmlAEnviar(manej.envioMensajeError("500"));
                                }
                            }
                            else
                                return manej.codificarXmlAEnviar(manej.envioMensajeError("14"));
                        }
                        else
                            return manej.codificarXmlAEnviar(manej.envioMensajeError("13"));
                    }
                    else
                        return manej.codificarXmlAEnviar(manej.envioMensajeError("13"));
                }
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
            String fechaI = " ";
            String fechaF = " ";
            try
            {
                fechaI = String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(fechaI_tb));
                fechaF = String.Format("{0:yyyy/MM/dd}", Convert.ToDateTime(fechaF_tb));
            }
            catch (System.FormatException)
            {
                //Si la fecha ingresada por el usuario está en un formato inválido
                return manej.codificarXmlAEnviar(manej.envioMensajeError("15"));
            }    
            System.Diagnostics.Debug.WriteLine("fechaI: " + fechaI + "fechaF: " + fechaF);

            System.Diagnostics.Debug.WriteLine("En Facturado ESTE es el SessionID " + Session.SessionID);

            if (Context.Session != null)
            {
                //check the IsNewSession value, this will tell us if the session has been reset
                if (Session.IsNewSession)
                {
                    //now we know it's a new session, so we check to see if a cookie is present
                    string cookie = HttpContext.Current.Request.Headers["Cookie"];
                    //now we determine if there is a cookie does it contains what we're looking for
                    if ((null != cookie) && (cookie.IndexOf("ASP.NET_SessionId") >= 0))
                    {
                        //since it's a new session but a ASP.Net cookie exist we know
                        //the session has expired so we need to redirect them
                        return manej.codificarXmlAEnviar(manej.envioMensajeError("505"));
                    }
                    else
                    {
                        return manej.codificarXmlAEnviar(manej.envioMensajeError("13"));
                    }
                }
                else
                {
                    if (Session["Loggedin"] !=null) 
                    {
                        if (Session["Loggedin"].Equals("yes"))
                        {
                            if (CodValido(medico_tb))
                            {
                                DateTime x = DateTime.Now;
                                DateTime y = (DateTime)Session["UltimaConsulta"];
                                TimeSpan z = x.Subtract(y);

                                System.Diagnostics.Debug.WriteLine(x.ToString("yyyyMMdd HH:mm:ss"));
                                System.Diagnostics.Debug.WriteLine(y.ToString("yyyyMMdd HH:mm:ss"));
                                System.Diagnostics.Debug.WriteLine("Diferencia " + z.TotalMinutes);

                                if (z.TotalMinutes < 10)
                                {
                                    Session["UltimaConsulta"] = x;

                                    //Creamos una instancia de HistoricoPagos con los datos de entrada (medico_tb, fechaI, fechaF)
                                    FacturadoUDN facturado = new FacturadoUDN(medico_tb, fechaI, fechaF);

                                    //Verificamos si la base de datos está disponible
                                    bool disponible = facturado.DisponibleBD();
                                    if (disponible == false)
                                        return manej.codificarXmlAEnviar(manej.envioMensajeError("600"));
                                    else
                                    {
                                        //Consultamos el listado de pagos generados para el médico en el rango de fechas
                                        facturado.consultarHonorariosFacturados();
                                        if (facturado.SinFacturado == true)
                                            return manej.codificarXmlAEnviar(manej.envioMensajeError("0"));
                                        else
                                            return manej.codificarXmlAEnviar(manej.creacionRespuestaHonorariosFacturados(facturado.FactPorUdn, facturado.MontoTotal));
                                    }
                                }
                                else
                                {
                                    return manej.codificarXmlAEnviar(manej.envioMensajeError("500"));
                                }
                            }
                            else return manej.codificarXmlAEnviar(manej.envioMensajeError("14"));
                        }
                        else
                            return manej.codificarXmlAEnviar(manej.envioMensajeError("13")); 
                    }
                    else
                        return manej.codificarXmlAEnviar(manej.envioMensajeError("13")); 
                }
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

            if (Context.Session != null)
            {
                //check the IsNewSession value, this will tell us if the session has been reset
                if (Session.IsNewSession)
                {
                    //now we know it's a new session, so we check to see if a cookie is present
                    string cookie = HttpContext.Current.Request.Headers["Cookie"];
                    //now we determine if there is a cookie does it contains what we're looking for
                    if ((null != cookie) && (cookie.IndexOf("ASP.NET_SessionId") >= 0))
                    {
                        //since it's a new session but a ASP.Net cookie exist we know
                        //the session has expired so we need to redirect them
                        return manej.codificarXmlAEnviar(manej.envioMensajeError("505"));
                    }
                    else
                    {
                        return manej.codificarXmlAEnviar(manej.envioMensajeError("13"));
                    }
                }
                else
                {
                    if (Session["Loggedin"] != null)
                    {
                        if (Session["Loggedin"].Equals("yes"))
                        {
                            if (CodValido(medico_tb))
                            {
                                DateTime x = DateTime.Now;
                                DateTime y = (DateTime)Session["UltimaConsulta"];
                                TimeSpan z = x.Subtract(y);

                                System.Diagnostics.Debug.WriteLine(x.ToString("yyyyMMdd HH:mm:ss"));
                                System.Diagnostics.Debug.WriteLine(y.ToString("yyyyMMdd HH:mm:ss"));
                                System.Diagnostics.Debug.WriteLine("Diferencia " + z.TotalMinutes);

                                if (z.TotalMinutes < 10)
                                {
                                    Session["UltimaConsulta"] = x;
                                    
                                    //Creamos una instancia de HistoricoPagos con los datos de entrada (medico_tb, fechaI, fechaF)
                                    string[] apellidos = apellido_tb.Split('_');
                                    ListadoCasos casos = new ListadoCasos(medico_tb, apellidos);
                                    
                                    //Verificamos si la base de datos está disponible
                                    bool disponible = casos.DisponibleBD();
                                    if (disponible == false)
                                        return manej.codificarXmlAEnviar(manej.envioMensajeError("600"));
                                    else
                                    {
                                        //Consultamos el listado de pagos generados para el médico en el rango de fechas
                                        casos.ConsultarListadoDeCasos();
                                        if (casos.SinCasos == true)
                                            return manej.codificarXmlAEnviar(manej.envioMensajeError("0"));
                                        else
                                        {
                                            if (casos.Excede == true)
                                                return manej.codificarXmlAEnviar(manej.envioMensajeError("1"));
                                            else
                                                return manej.codificarXmlAEnviar(manej.creacionRespuestaListadoDeCaso(casos.Casos));
                                        }
                                    }
                                }
                                else
                                {
                                    return manej.codificarXmlAEnviar(manej.envioMensajeError("500"));
                                }
                            }
                            else return manej.codificarXmlAEnviar(manej.envioMensajeError("14"));
                        }
                        else
                            return manej.codificarXmlAEnviar(manej.envioMensajeError("13"));
                    }
                    else
                        return manej.codificarXmlAEnviar(manej.envioMensajeError("13"));
                 }
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

            if (Context.Session != null)
            {
                //check the IsNewSession value, this will tell us if the session has been reset
                if (Session.IsNewSession)
                {
                    //now we know it's a new session, so we check to see if a cookie is present
                    string cookie = HttpContext.Current.Request.Headers["Cookie"];
                    //now we determine if there is a cookie does it contains what we're looking for
                    if ((null != cookie) && (cookie.IndexOf("ASP.NET_SessionId") >= 0))
                    {
                        //since it's a new session but a ASP.Net cookie exist we know
                        //the session has expired so we need to redirect them
                        return manej.codificarXmlAEnviar(manej.envioMensajeError("505"));
                    }
                    else
                    {
                        return manej.codificarXmlAEnviar(manej.envioMensajeError("13"));
                    }
                }
                else
                {
                    if (Session["Loggedin"] !=null) 
                    {
                        if (Session["Loggedin"].Equals("yes"))
                        {
                            if (CodValido(medico_tb))
                            {
                                DateTime x = DateTime.Now;
                                DateTime y = (DateTime)Session["UltimaConsulta"];
                                TimeSpan z = x.Subtract(y);

                                System.Diagnostics.Debug.WriteLine(x.ToString("yyyyMMdd HH:mm:ss"));
                                System.Diagnostics.Debug.WriteLine(y.ToString("yyyyMMdd HH:mm:ss"));
                                System.Diagnostics.Debug.WriteLine("Diferencia " + z.TotalMinutes);

                                if (z.TotalMinutes < 10)
                                {
                                    Session["UltimaConsulta"] = x;

                                    //Creamos una instancia de HistoricoPagos con los datos de entrada (medico_tb, fechaI, fechaF)
                                    Caso caso = new Caso(medico_tb, caso_tb, udn_tb);

                                    //Verificamos si la base de datos está disponible
                                    bool disponible = caso.DisponibleBD();
                                    if (disponible == false)
                                        return manej.codificarXmlAEnviar(manej.envioMensajeError("600"));
                                    else
                                    {
                                        //Consultamos el listado de pagos generados para el médico en el rango de fechas
                                        caso.ConsultarDetalleDeCaso();

                                        return manej.codificarXmlAEnviar(manej.creacionRespuestaDetalleDeCaso(caso));
                                    }
                                }
                                else
                                {
                                    return manej.codificarXmlAEnviar(manej.envioMensajeError("500")); 
                                }
                            }
                            else return manej.codificarXmlAEnviar(manej.envioMensajeError("14")); 
                        }
                        else
                            return manej.codificarXmlAEnviar(manej.envioMensajeError("13")); 
                    }
                    else
                        return manej.codificarXmlAEnviar(manej.envioMensajeError("13")); 
                }
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

            if (Context.Session != null)
            {
                //check the IsNewSession value, this will tell us if the session has been reset
                if (Session.IsNewSession)
                {
                    //now we know it's a new session, so we check to see if a cookie is present
                    string cookie = HttpContext.Current.Request.Headers["Cookie"];
                    //now we determine if there is a cookie does it contains what we're looking for
                    if ((null != cookie) && (cookie.IndexOf("ASP.NET_SessionId") >= 0))
                    {
                        //since it's a new session but a ASP.Net cookie exist we know
                        //the session has expired so we need to redirect them
                        return manej.codificarXmlAEnviar(manej.envioMensajeError("505"));
                    }
                    else
                    {
                        return manej.codificarXmlAEnviar(manej.envioMensajeError("13"));
                    }
                }
                else
                {
                    if (Session["Loggedin"] != null)
                    {
                        if (Session["Loggedin"].Equals("yes"))
                        {
                            if (CodValido(medico_tb))
                            {
                                DateTime x = DateTime.Now;
                                DateTime y = (DateTime)Session["UltimaConsulta"];
                                TimeSpan z = x.Subtract(y);

                                System.Diagnostics.Debug.WriteLine(x.ToString("yyyyMMdd HH:mm:ss"));
                                System.Diagnostics.Debug.WriteLine(y.ToString("yyyyMMdd HH:mm:ss"));
                                System.Diagnostics.Debug.WriteLine("Diferencia " + z.TotalMinutes);

                                if (z.TotalMinutes < 10)
                                {
                                    Session["UltimaConsulta"] = x;

                                    //Creamos una instancia de HistoricoPagos con los datos de entrada (medico_tb, fechaI, fechaF)
                                    ListadoFianzas fianzas = new ListadoFianzas(medico_tb);

                                    //Verificamos si la base de datos está disponible
                                    bool disponible = fianzas.DisponibleBD();
                                    if (disponible == false)
                                        return manej.codificarXmlAEnviar(manej.envioMensajeError("600"));
                                    else
                                    {
                                        //Consultamos el listado de pagos generados para el médico en el rango de fechas
                                        fianzas.ConsultarListadoFianzas();

                                        if (fianzas.SinFianzas == true)
                                            return manej.codificarXmlAEnviar(manej.envioMensajeError("0"));
                                        else
                                            return manej.codificarXmlAEnviar(manej.creacionRespuestaListadoFianzas(fianzas.Fianzas));
                                    }

                                }
                                else
                                {
                                    return manej.codificarXmlAEnviar(manej.envioMensajeError("500"));
                                }
                            }
                            else return manej.codificarXmlAEnviar(manej.envioMensajeError("14"));
                        }
                        else
                            return manej.codificarXmlAEnviar(manej.envioMensajeError("13"));
                    }
                    else
                        return manej.codificarXmlAEnviar(manej.envioMensajeError("13"));
                }
            }
            else
                return manej.codificarXmlAEnviar(manej.envioMensajeError("13"));
        }

        /// <summary>
        /// Este servicio web permite cerrar sesion
        /// </summary>
        /// <returns>Cierra la Session</returns>
        [WebMethod(Description = "Cerrar sesión", EnableSession = true)]
        public String cerrarSesion()
        {
            ManejadorXML manej = new ManejadorXML();

            System.Diagnostics.Debug.WriteLine("En cierre de sesion ESTE es el SessionID " + Session.SessionID);

            if (Context.Session != null)
            {
                //check the IsNewSession value, this will tell us if the session has been reset
                if (Session.IsNewSession)
                {
                    //now we know it's a new session, so we check to see if a cookie is present
                    string cookie = HttpContext.Current.Request.Headers["Cookie"];
                    //now we determine if there is a cookie does it contains what we're looking for
                    if ((null != cookie) && (cookie.IndexOf("ASP.NET_SessionId") >= 0))
                    {
                        //since it's a new session but a ASP.Net cookie exist we know
                        //the session has expired so we need to redirect them
                        return manej.codificarXmlAEnviar(manej.envioMensajeError("505"));
                    }
                    else
                    {
                        return manej.codificarXmlAEnviar(manej.envioMensajeError("13"));
                    }
                }
                else
                {
                    if (Session["Loggedin"] != null)
                    {
                        if (Session["Loggedin"].Equals("yes"))
                        {
                            Session.Abandon();
                            System.Diagnostics.Debug.WriteLine("Cerre Sesion");
                        }
                    }
                    return manej.codificarXmlAEnviar(manej.envioMensajeError("13"));
                }
            }
            else
                return manej.codificarXmlAEnviar(manej.envioMensajeError("13"));

        }

    }
}

