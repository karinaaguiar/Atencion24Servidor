using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Atencion24WebServices.Atencion24Negocio
{
    public class Honorario
    {
        private String nombre = " ";
        private decimal montoFacturado = 0;
        private decimal montoExonerado = 0;
        private decimal montoAbonado = 0;
        private decimal totalDeuda = 0;

        public Honorario() { }

        //Getter y Setters
        public String Nombre
        {
            get { return nombre; }
            set { nombre = value; }
        }

        public decimal MontoFacturado
        {
            get { return montoFacturado; }
            set { montoFacturado = value; }
        }

        public decimal MontoExonerado
        {
            get { return montoExonerado; }
            set { montoExonerado = value; }
        }

        public decimal MontoAbonado
        {
            get { return montoAbonado; }
            set { montoAbonado = value; }
        }

        public decimal TotalDeuda
        {
            get { return totalDeuda; }
            set { totalDeuda = value; }
        }

    }
}
