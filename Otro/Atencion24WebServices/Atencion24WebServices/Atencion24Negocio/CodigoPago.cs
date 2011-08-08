using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Atencion24WebServices.Atencion24Negocio
{
    public class CodigoPago
    {
        private string codigo = "";
        private string nombre = "";

        ///Constructor
        public CodigoPago() { }

        public CodigoPago(String nombre, String codigo)
        {
            Codigo = codigo;
            Nombre = nombre;
        }

        //Getter y Setters
        public string Codigo
        {
            get { return codigo; }
            set { codigo = value; }
        }

        public string Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }
    }
}
