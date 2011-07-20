using System;
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

        public XmlDocument auxiliarHonorariosPagados(XmlDocument documento, Pago pago)
        {
            XmlElement elemento;
            XmlElement elemento1;
            XmlElement elemento2;
            XmlElement elemento3;
            XmlText texto;
            
            elemento = documento.CreateElement("pago");
            
            //MontoLiberado
            elemento1 = documento.CreateElement("montoLiberado");
            String valor = pago.MontoLiberado;
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

            documento.AppendChild(elemento);
            return documento;
        }
        
        public String creacionRespuestaProximoPago(Pago pago) 
        {
            XmlDocument documento;
            
            documento = newDocument();
            documento = auxiliarHonorariosPagados(documento, pago);
            return XMLtoString(documento);
        }

        public String creacionRespuestaHistoricoPagos(HistoricoPagos pagos) 
        {
            XmlDocument documento;
    
            documento = newDocument();

            System.Diagnostics.Debug.WriteLine("Numero de Pagos " + pagos.Pagos.Length);
            for (int i = 0; i < pagos.Pagos.Length; i++)
            {
                Pago pago = pagos.getPago(i);
                if(!string.IsNullOrEmpty(pago.FechaPago))
                    System.Diagnostics.Debug.WriteLine("Fecha del pago " + i + ": " + Convert.ToString(pago.FechaPago));
                documento = auxiliarHonorariosPagados(documento, pago); 
            }
            
            return XMLtoString(documento);
        }
    }
}
