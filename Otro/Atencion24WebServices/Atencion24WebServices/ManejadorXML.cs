using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.IO;
using Atencion24WebServices.Atencion24Negocio;

namespace Atencion24WebServices
{
    public class ManejadorXML
    {

        //Metodo que permite crear un documento XML con su declaración 
        public XmlDocument newDocument()
        {
            //Se crea un nuevo documento XML
            XmlDocument documento = new XmlDocument();

            // Se crea la declaracion del documento
            XmlNode nodo = documento.CreateNode(XmlNodeType.XmlDeclaration, "", "");

            // se adiciona la declaración de XML al documento XML
            documento.AppendChild(nodo);

            return documento;
        }

        //Metodo que permite convertir un documentoXML en String
        public String XMLtoString(XmlDocument documentoXML)
        {
            StringWriter sw = new StringWriter();
            XmlTextWriter xw = new XmlTextWriter(sw);
            documentoXML.WriteTo(xw);
            return sw.ToString();
        }

        public String codificarXmlAEnviar(String xml)
        {
            String result;
            result = xml.Replace("<", "[");
            result = result.Replace(">", "]");
            result = result.Replace("version=\"1.0\"", "version=\"1.0\" encoding=\"iso-8859-1\"");
            return result;
        }

        //Método que permite enviar mensajes de error
        public String envioMensajeError(String mensaje)
        {
            XmlDocument documento;
            XmlElement elemento;
            XmlText texto;

            documento = newDocument();

            elemento = documento.CreateElement("error"); // se genera un nodo 
            texto = documento.CreateTextNode(mensaje); // se adiciona el texto del nodo
            elemento.AppendChild(texto); // se adiciona el texto al elemento nombreValido
            documento.AppendChild(elemento);

            return XMLtoString(documento);
        }

        //Método que permite enviar respuesta cuando se solicita iniciar sesión
        public String creacionRespuestaInicioSesion(String nombre, String apellido, String codigo, String nombreUsuario)
        {
            XmlDocument documento;
            XmlElement elemento;
            XmlElement elemento1;
            XmlText texto;

            documento = newDocument();

            elemento = documento.CreateElement("usuario");
            //Nombre del proveedor
            elemento1 = documento.CreateElement("nombre");
            texto = documento.CreateTextNode(nombre);
            elemento1.AppendChild(texto);
            elemento.AppendChild(elemento1);

            //Apellido del proveedor
            elemento1 = documento.CreateElement("apellido");
            texto = documento.CreateTextNode(apellido);
            elemento1.AppendChild(texto);
            elemento.AppendChild(elemento1);

            //Codigo del médico
            elemento1 = documento.CreateElement("CodigoMedico");
            texto = documento.CreateTextNode(codigo);
            elemento1.AppendChild(texto);
            elemento.AppendChild(elemento1);

            //Nombre de usuario
            elemento1 = documento.CreateElement("nombreUsuario");
            texto = documento.CreateTextNode(nombreUsuario);
            elemento1.AppendChild(texto);
            elemento.AppendChild(elemento1);

            documento.AppendChild(elemento);

            return XMLtoString(documento);
        }

        public String creacionRespuestaEdoCtaAS(EstadoDeCuenta estadoCta) 
        {
            XmlDocument documento;
            XmlElement elemento;
            XmlElement elemento1;
            XmlText texto;

            documento = newDocument();
            elemento = documento.CreateElement("estadoCuenta");
            
            //Monto30Dias
            elemento1 = documento.CreateElement("montoA30Dias");
            texto = documento.CreateTextNode((estadoCta.MontoA30Dias).ToString("0.##"));
            elemento1.AppendChild(texto);
            elemento.AppendChild(elemento1);

            //Monto60Dias
            elemento1 = documento.CreateElement("montoA60Dias");
            texto = documento.CreateTextNode((estadoCta.MontoA60Dias).ToString("0.##"));
            elemento1.AppendChild(texto);
            elemento.AppendChild(elemento1);

            //Monto90Dias
            elemento1 = documento.CreateElement("montoA90Dias");
            texto = documento.CreateTextNode((estadoCta.MontoA90Dias).ToString("0.##"));
            elemento1.AppendChild(texto);
            elemento.AppendChild(elemento1);

            //Monto180Dias
            elemento1 = documento.CreateElement("montoA180Dias");
            texto = documento.CreateTextNode((estadoCta.MontoA180Dias).ToString("0.##"));
            elemento1.AppendChild(texto);
            elemento.AppendChild(elemento1);

            //MontoMas180Dias
            elemento1 = documento.CreateElement("montoAMas180Dias");
            texto = documento.CreateTextNode((estadoCta.MontoAMas180Dias).ToString("0.##"));
            elemento1.AppendChild(texto);
            elemento.AppendChild(elemento1);

            //MontoTotal
            elemento1 = documento.CreateElement("montoTotal");
            texto = documento.CreateTextNode((estadoCta.MontoTotal).ToString("0.##"));
            elemento1.AppendChild(texto);
            elemento.AppendChild(elemento1);

            documento.AppendChild(elemento);

            return XMLtoString(documento);
        }

        public XmlElement auxiliarHonorariosPagados(XmlDocument documento, XmlElement elementoPadre, Pago pago)
        {
            XmlElement elemento;
            XmlElement elemento1;
            XmlElement elemento2;
            XmlElement elemento3;
            XmlText texto;
            
            elemento = documento.CreateElement("pago");
            
            //MontoLiberado
            elemento1 = documento.CreateElement("montoLiberado");
            String valor = pago.MontoLiberado.ToString("0.##");
            texto = documento.CreateTextNode(valor);
            elemento1.AppendChild(texto);
            elemento.AppendChild(elemento1);

            //Deducciones
            elemento1 = documento.CreateElement("deducciones");
            //Deducción
            for(int i =0; i < (pago.Deducciones.GetLength(0)); i++) 
            {
                elemento2 = documento.CreateElement("deduccion");
                //Concepto
                elemento3 = documento.CreateElement("concepto");
                texto = documento.CreateTextNode(pago.Deducciones[i,0]);
                elemento3.AppendChild(texto);
                elemento2.AppendChild(elemento3);
                
                //Monto
                elemento3 = documento.CreateElement("monto");
                texto = documento.CreateTextNode(pago.Deducciones[i,1]);
                elemento3.AppendChild(texto);
                elemento2.AppendChild(elemento3);

                elemento1.AppendChild(elemento2);
            }
            elemento.AppendChild(elemento1);

            //MontoNeto 
            elemento1 = documento.CreateElement("montoNeto");
            texto = documento.CreateTextNode((pago.MontoNeto).ToString("0.##"));
            elemento1.AppendChild(texto);
            elemento.AppendChild(elemento1);

            //Fecha Pago
            elemento1 = documento.CreateElement("fechaPago");
            texto = documento.CreateTextNode(pago.FechaPago);
            elemento1.AppendChild(texto);
            elemento.AppendChild(elemento1);

            elementoPadre.AppendChild(elemento);
            
            return elementoPadre;
        }
        
        public String creacionRespuestaProximoPago(Pago pago) 
        {
            XmlDocument documento;
            
            documento = newDocument();
            //documento = auxiliarHonorariosPagados(documento, pago);
            return XMLtoString(documento);
        }
        
        public String creacionRespuestaHistoricoPagos(ArrayList pagos) 
        {
            XmlDocument documento;
            XmlElement elemento;
 
            documento = newDocument();
            elemento = documento.CreateElement("pagos");

            ArrayList listadoPagos = new ArrayList();
            listadoPagos = pagos;

            foreach(Pago pago in listadoPagos)
            {
                elemento = auxiliarHonorariosPagados(documento, elemento, pago); 
            }

            documento.AppendChild(elemento);
            return XMLtoString(documento);
        }

        public String creacionRespuestaHonorariosFacturados(FacturadoUDN facturado) 
        {
            XmlDocument documento;
            XmlElement elemento;
            XmlElement elemento1;
            XmlText texto;

            documento = newDocument();
            elemento = documento.CreateElement("facturado");
            
            //Monto30Dias
            elemento1 = documento.CreateElement("hospitalizacion");
            texto = documento.CreateTextNode(facturado.Hospitalizacion.ToString("0.##"));
            elemento1.AppendChild(texto);
            elemento.AppendChild(elemento1);

            //Monto60Dias
            elemento1 = documento.CreateElement("emergencia");
            texto = documento.CreateTextNode((facturado.Emergencia).ToString("0.##"));
            elemento1.AppendChild(texto);
            elemento.AppendChild(elemento1);

            //Monto90Dias
            elemento1 = documento.CreateElement("cirugia");
            texto = documento.CreateTextNode((facturado.Cirugia).ToString("0.##"));
            elemento1.AppendChild(texto);
            elemento.AppendChild(elemento1);

            //Monto180Dias
            elemento1 = documento.CreateElement("convenios");
            texto = documento.CreateTextNode((facturado.Convenios).ToString("0.##"));
            elemento1.AppendChild(texto);
            elemento.AppendChild(elemento1);

            //MontoMas180Dias
            elemento1 = documento.CreateElement("total");
            texto = documento.CreateTextNode((facturado.MontoTotal).ToString("0.##"));
            elemento1.AppendChild(texto);
            elemento.AppendChild(elemento1);

            documento.AppendChild(elemento);

            return XMLtoString(documento);
        }

        public XmlElement auxiliarDetalleCaso(XmlDocument documento, XmlElement elementoPadre, Caso caso, int tipo)
        {
            XmlElement elemento;
            XmlElement elemento1;
            XmlElement elemento2;
            XmlElement elemento3;
            XmlText texto;

            if (tipo == 0) elemento = elementoPadre;
            else  elemento = documento.CreateElement("caso"); 

            //Nombre del paciente
            elemento1 = documento.CreateElement("nombrePaciente");
            String valor = caso.NombrePaciente;
            texto = documento.CreateTextNode(valor);
            elemento1.AppendChild(texto);
            elemento.AppendChild(elemento1);

            //Fecha de emisión 
            elemento1 = documento.CreateElement("fechaEmision");
            valor = caso.FechaEmision;
            texto = documento.CreateTextNode(valor);
            elemento1.AppendChild(texto);
            elemento.AppendChild(elemento1);

            //Nro de caso 
            elemento1 = documento.CreateElement("nroCaso");
            valor = caso.NroCaso;
            texto = documento.CreateTextNode(valor);
            elemento1.AppendChild(texto);
            elemento.AppendChild(elemento1);

            //Unidad de Negocio  
            elemento1 = documento.CreateElement("unidadNegocio");
            valor = caso.UnidadNegocio;
            texto = documento.CreateTextNode(valor);
            elemento1.AppendChild(texto);
            elemento.AppendChild(elemento1);

            if (caso.Simple == false)
            {
                //Cédula del paciente
                elemento1 = documento.CreateElement("ciPaciente");
                valor = caso.CiPaciente;
                texto = documento.CreateTextNode(valor);
                elemento1.AppendChild(texto);
                elemento.AppendChild(elemento1);

                //Responsable de pago
                elemento1 = documento.CreateElement("responsablePago");
                valor = caso.ResponsablePago;
                texto = documento.CreateTextNode(valor);
                elemento1.AppendChild(texto);
                elemento.AppendChild(elemento1);

                //Monto Facturado
                elemento1 = documento.CreateElement("montoFacturado");
                valor = caso.MontoFacturado.ToString("0.##");
                texto = documento.CreateTextNode(valor);
                elemento1.AppendChild(texto);
                elemento.AppendChild(elemento1);

                //Monto Exonerado
                elemento1 = documento.CreateElement("montoExonerado");
                valor = caso.MontoExonerado.ToString("0.##");
                texto = documento.CreateTextNode(valor);
                elemento1.AppendChild(texto);
                elemento.AppendChild(elemento1);

                //Monto Abonado
                elemento1 = documento.CreateElement("montoAbonado");
                valor = caso.MontoAbonado.ToString("0.##");
                texto = documento.CreateTextNode(valor);
                elemento1.AppendChild(texto);
                elemento.AppendChild(elemento1);

                //Monto Abonado
                elemento1 = documento.CreateElement("totalDeuda");
                valor = caso.TotalDeuda.ToString("0.##");
                texto = documento.CreateTextNode(valor);
                elemento1.AppendChild(texto);
                elemento.AppendChild(elemento1);

                //Honorarios
                elemento1 = documento.CreateElement("honorarios");
                //Honorarios
                if (caso.Honorarios != null)
                {
                    foreach (Honorario honorario in caso.Honorarios)
                    {
                        elemento2 = documento.CreateElement("honorario");

                        //Nombre honorario 
                        elemento3 = documento.CreateElement("nombre");
                        texto = documento.CreateTextNode(honorario.Nombre);
                        elemento3.AppendChild(texto);
                        elemento2.AppendChild(elemento3);

                        //Monto Facturado 
                        elemento3 = documento.CreateElement("facturado");
                        texto = documento.CreateTextNode(honorario.MontoFacturado.ToString("0.##"));
                        elemento3.AppendChild(texto);
                        elemento2.AppendChild(elemento3);

                        //Nombre honorario 
                        elemento3 = documento.CreateElement("exonerado");
                        texto = documento.CreateTextNode(honorario.MontoExonerado.ToString("0.##"));
                        elemento3.AppendChild(texto);
                        elemento2.AppendChild(elemento3);

                        //Nombre honorario 
                        elemento3 = documento.CreateElement("abonado");
                        texto = documento.CreateTextNode(honorario.MontoAbonado.ToString("0.##"));
                        elemento3.AppendChild(texto);
                        elemento2.AppendChild(elemento3);

                        //Nombre honorario 
                        elemento3 = documento.CreateElement("deuda");
                        texto = documento.CreateTextNode(honorario.TotalDeuda.ToString("0.##"));
                        elemento3.AppendChild(texto);
                        elemento2.AppendChild(elemento3);

                        elemento1.AppendChild(elemento2);
                    }
                    elemento.AppendChild(elemento1);
                }
            }

            if (tipo != 0) elementoPadre.AppendChild(elemento);
            else elementoPadre = elemento;

            return elementoPadre;
        }
        
        public String creacionRespuestaListadoDeCaso(ArrayList casos) 
        {
            XmlDocument documento;
            XmlElement elemento;

            documento = newDocument();
            elemento = documento.CreateElement("casos");

            ArrayList listadoCasos = new ArrayList();
            listadoCasos = casos;

            foreach (Caso caso in listadoCasos)
            {
                elemento = auxiliarDetalleCaso(documento, elemento, caso, 1);
            }

            documento.AppendChild(elemento);
            return XMLtoString(documento);
        }

        public String creacionRespuestaDetalleDeCaso(Caso caso) 
        {
            XmlDocument documento;
            XmlElement elemento;

            documento = newDocument();
            elemento = documento.CreateElement("caso");

            Caso casoDet = new Caso();
            casoDet = caso; 

            elemento = auxiliarDetalleCaso(documento, elemento, casoDet, 0);

            documento.AppendChild(elemento);
            return XMLtoString(documento);
        }

        public XmlElement auxiliarListadoFianzas(XmlDocument documento, XmlElement elementoPadre, Fianza fianza)
        {
            XmlElement elemento;
            XmlElement elemento1;
            XmlText texto;

            elemento = documento.CreateElement("fianza");

            //Número de caso 
            elemento1 = documento.CreateElement("caso");
            String valor = fianza.NroCaso;
            texto = documento.CreateTextNode(valor);
            elemento1.AppendChild(texto);
            elemento.AppendChild(elemento1);

            //Fecha emisión
            elemento1 = documento.CreateElement("fechaEmision");
            texto = documento.CreateTextNode(fianza.FechaEmision);
            elemento1.AppendChild(texto);
            elemento.AppendChild(elemento1);

            //Fecha emisión
            elemento1 = documento.CreateElement("paciente");
            texto = documento.CreateTextNode(fianza.Paciente);
            elemento1.AppendChild(texto);
            elemento.AppendChild(elemento1);

            //Moto a cobrar
            elemento1 = documento.CreateElement("montoACobrar");
            texto = documento.CreateTextNode(fianza.MontoACobrar.ToString("0.##"));
            elemento1.AppendChild(texto);
            elemento.AppendChild(elemento1);

            //Moto abonado
            elemento1 = documento.CreateElement("montoAbonado");
            texto = documento.CreateTextNode(fianza.MontoAbonado.ToString("0.##"));
            elemento1.AppendChild(texto);
            elemento.AppendChild(elemento1);

            //Moto reintegro
            elemento1 = documento.CreateElement("montoReintegro");
            texto = documento.CreateTextNode(fianza.MontoReintegro.ToString("0.##"));
            elemento1.AppendChild(texto);
            elemento.AppendChild(elemento1);

            //Moto Notas Crédito
            elemento1 = documento.CreateElement("montoNotasCred");
            texto = documento.CreateTextNode(fianza.MontoNotasCred.ToString("0.##"));
            elemento1.AppendChild(texto);
            elemento.AppendChild(elemento1);

            
            //Moto Notas Débito
            elemento1 = documento.CreateElement("montoNotasDeb");
            texto = documento.CreateTextNode(fianza.MontoNotasDeb.ToString("0.##"));
            elemento1.AppendChild(texto);
            elemento.AppendChild(elemento1);

            //Moto neto
            elemento1 = documento.CreateElement("montoDeuda");
            texto = documento.CreateTextNode(fianza.MontoNeto.ToString("0.##"));
            elemento1.AppendChild(texto);
            elemento.AppendChild(elemento1);

            elementoPadre.AppendChild(elemento);

            return elementoPadre;
        }

        public String creacionRespuestaListadoFianzas(ArrayList fianzas) 
        {
            XmlDocument documento;
            XmlElement elemento;

            documento = newDocument();
            elemento = documento.CreateElement("fianzas");

            ArrayList listadoFianzas = new ArrayList();
            listadoFianzas = fianzas;

            foreach (Fianza fianza in listadoFianzas)
            {
                elemento = auxiliarListadoFianzas(documento, elemento, fianza);
            }

            documento.AppendChild(elemento);
            return XMLtoString(documento);
        }
        
    }
}
