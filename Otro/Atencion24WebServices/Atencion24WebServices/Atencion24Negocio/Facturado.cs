using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Atencion24WebServices.Atencion24Negocio
{
    public class Facturado
    {
        private decimal monto = 0;
        private string udn = " ";

        ///Constructor
        public Facturado() { }

        public Facturado(String udn, decimal monto)
        {
            Monto = monto;
            Udn = udn;
        }

        //Getter y Setters
        public decimal Monto
        {
            get { return monto; }
            set { monto = value; }
        }

        public string Udn
        {
            get { return udn; }
            set { udn = value; }
        }
    }
}
