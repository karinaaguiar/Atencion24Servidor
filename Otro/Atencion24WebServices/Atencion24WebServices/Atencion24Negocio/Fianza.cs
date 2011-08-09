using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Atencion24WebServices.Atencion24Negocio
{
    public class Fianza
    {
        private String nroCaso=" ";
        private String fechaEmisionFactura=" ";
        private String paciente=" ";
        private decimal montoACobrar = 0;
        private decimal montoAbonado = 0;
        private decimal montoNeto = 0;

        private decimal montoReintegro = 0;
        private decimal montoNotasCred = 0;
        private decimal montoNotasDeb = 0;

         ///Constructor
        public Fianza() { }

        //Getter y Setters
        public String NroCaso
        {
            get { return nroCaso; }
            set { nroCaso = value; }
        }

        public String FechaEmision
        {
            get { return fechaEmisionFactura; }
            set { fechaEmisionFactura = value; }
        }

        public String Paciente
        {
            get { return paciente; }
            set { paciente = value; }
        }

        public decimal MontoACobrar
        {
            get { return montoACobrar; }
            set { montoACobrar = value; }
        }

        public decimal MontoAbonado
        {
            get { return montoAbonado; }
            set { montoAbonado = value; }
        }

        public decimal MontoNeto
        {
            get { return montoNeto; }
            set { montoNeto = value; }
        }

        public decimal MontoReintegro
        {
            get { return montoReintegro; }
            set { montoReintegro = value; }
        }

        public decimal MontoNotasCred
        {
            get { return montoNotasCred; }
            set { montoNotasCred = value; }
        }

        public decimal MontoNotasDeb
        {
            get { return montoNotasDeb; }
            set { montoNotasDeb = value; }
        }

    }
}
