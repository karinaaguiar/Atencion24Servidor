using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.IO;

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

        public String creacionRespuestaEdoCta(EstadoDeCuenta edoCta) 
        {
            XmlDocument documento;
            XmlElement elemento;
            XmlElement elemento1;
            XmlText texto;

            documento = newDocument();
            elemento = documento.CreateElement("estadoCuenta");
            //MontoTotal
            elemento1 = documento.CreateElement("montoTotal");
            //texto = documento.CreateTextNode(edoCta.MontoTotal);
            elemento1.AppendChild(documento.CreateTextNode(edoCta.MontoTotal));
            elemento.AppendChild(elemento1);
            /*
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
            elemento.AppendChild(elemento1);*/

            documento.AppendChild(elemento);

            return XMLtoString(documento);
        }
    }
}
