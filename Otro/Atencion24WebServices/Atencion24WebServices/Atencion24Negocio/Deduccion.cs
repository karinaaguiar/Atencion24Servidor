using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Atencion24WebServices.Atencion24Negocio
{
    public class Deduccion
    {
        private string concepto = " ";
        private decimal monto = 0;

        ///Constructor
        public Deduccion() {}

        //Getter y Setters
        public string Concepto
        {
            get { return concepto; }
            set { concepto = value; }
        }

        public decimal Monto
        {
            get { return monto; }
            set { monto = value; }
        }
    }
}
